using System;
using System.Collections.Generic;
using System.Linq;
using AdventuresInGrythia.Domain.Contracts;
using AdventuresInGrythia.Domain.Models;
using AdventuresInGrythia.Engine.Objects;

namespace AdventuresInGrythia.Engine.Managers
{
    public interface IEntityManager
    {
        AiGEntity CreateEntity(string name = null, Dictionary<string, string> traits = null, int? parent = null);
        AiGEntity SaveEntity(AiGEntity entity);
        AiGEntity LoadEntityById(int id);
        void DeleteEntity(int id);
        List<AiGEntity> LoadCharactersFromAccount(int accountId);
        AiGEntity CreatePlayerCharacter(int accountId, AiGEntity model);
    }
    
    public class EntityManager : IEntityManager
    {
        readonly IRepository<Entity> _entities;
        readonly IRepository<Trait> _traits;
        readonly IRepository<EntityComponent> _entityComponents;
        readonly IRepository<Account> _accounts;

        public EntityManager(IRepository<Entity> entities, IRepository<Trait> traits,
             IRepository<EntityComponent> entityComponents, IRepository<Account> accounts)
        {
            _entities = entities;
            _traits = traits;
            _entityComponents = entityComponents;
            _accounts = accounts;
        }

        public AiGEntity CreateEntity(string name = null, Dictionary<string, string> traits = null, int? parent = null)
        {
            var e = new Entity { Name = name, Traits = new List<Trait>(), ParentId = parent };
            e.Id = _entities.Add(e);

            var dto = new AiGEntity(e.Id, e.Name);

            if (traits == null)
                return dto;

            foreach (var t in traits)
            {
                e.Traits.Add(new Trait { EntityId = e.Id, Name = t.Key, Value = t.Value });
                dto.Traits.Add(t.Key, t.Value);
            }

            _entities.Update(e);

            return dto;
        }

        public AiGEntity SaveEntity(AiGEntity entity)
        {
            var existing = _entities.GetById(entity.Id, x => x.Traits);
            if (existing == null)
                throw new ArgumentNullException("Entity not found.  Be sure to CREATE it prior to saving it.");
            existing.ParentId = entity.Parent;
            existing.Traits = entity.Traits.GetAll()
                .Select(x => new Trait { EntityId = entity.Id, Name = x.Name, Value = x.Value }).ToList();
            existing.Components = entity.Components.GetAll()
                .Select(x => new EntityComponent { EntityId = entity.Id, Component = x.Name }).ToList();
            existing.Children = _entities.Find(e => entity.Children.Contains(e.Id)).ToList();
            _entities.Update(existing);

            return entity;
        }

        public AiGEntity LoadEntityById(int id)
        {
            var e = _entities.GetById(id, x => x.Traits, x => x.Children, x => x.Components);
            var retVal = new AiGEntity(e.Id, e.Name, e.ParentId);

            foreach (var t in e.Traits)
                retVal.Traits.Add(t.Name, t.Value);
            foreach (var c in e.Components.Select(x => x.Component))
                ComponentManager.Instance.AssignComponent(retVal, c);
            retVal.Children.AddRange(e.Children.Select(x => x.Id));

            return retVal;
        }

        public void DeleteEntity(int id)
        {
            _entities.Remove(id);
        }

        public List<AiGEntity> LoadCharactersFromAccount(int accountId)
        {
            var account = _accounts.GetById(accountId, x => x.Characters);
            var existing = _entities.Find(x => account.Characters.Select(c => c.EntityId).Contains(x.Id), o => o.Traits, o => o.Components, o => o.Children).ToList();
            var retVal = new List<AiGEntity>();

            foreach (var entity in existing)
            {
                var e = new AiGEntity(entity.Id, entity.Name);

                foreach (var trait in entity.Traits)
                    e.Traits.Add(trait.Name, trait.Value);
                foreach (var cmp in entity.Components)
                    ComponentManager.Instance.AssignComponent(e, cmp.Component);
                e.Children.AddRange(entity.Children.Select(x => x.Id));

                if (entity.Traits.Count != e.Traits.Count)
                    SaveEntity(e);

                retVal.Add(e);
            }
            return retVal;
        }

        public AiGEntity CreatePlayerCharacter(int accountId, AiGEntity model)
        {
            var e = new Entity
            {
                Name = model.Name,
                Traits = new List<Trait>(model.Traits.GetAll().Select(t => new Trait { Name = t.Name, Value = t.Value }))
            };

            var children = new List<Entity>();
            foreach(var c in model.Children)
                children.Add(_entities.GetById(c));
            
            e.Children.AddRange(children);
            e.Id = _entities.Add(e);

            var account = _accounts.GetById(accountId);
            if (account.Characters == null)
                account.Characters = new List<Account_Entity> { new Account_Entity { AccountId = accountId, EntityId = e.Id } };
            else
            {
                account.Characters.Add(new Account_Entity { AccountId = accountId, EntityId = e.Id });
            }
            _accounts.Update(account);

            var newChar = new AiGEntity(e.Id, e.Name);
            foreach (var t in e.Traits)
                newChar.Traits.Add(t.Name, t.Value);

            return newChar;
        }
    }
}