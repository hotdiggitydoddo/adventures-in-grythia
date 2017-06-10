using System.Collections.Generic;
using System.Linq;
using AdventuresInGrythia.Domain.Contracts;
using AdventuresInGrythia.Domain.Models;
using AdventuresInGrythia.Engine.Objects;

namespace AdventuresInGrythia.Engine.Managers
{
    public interface IEntityManager
    {

    }
    public class EntityManager : IEntityManager
    {
        readonly IRepository<Entity> _entities;
        readonly IRepository<Trait> _traits;
        
        readonly IRepository<EntityComponent> _entityComponents;

        public EntityManager(IRepository<Entity> entities, IRepository<Trait> traits,
             IRepository<EntityComponent> entityComponents)
        {
            _entities = entities;
            _traits = traits;
            _entityComponents = entityComponents;
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
            existing.ParentId = entity.Parent;
            existing.Traits = entity.Traits.GetAll()
                .Select(x => new Trait { EntityId = entity.Id, Name = x.Name, Value = x.Value }).ToList();
            existing.Components = entity.Components.GetAll()
                .Select(x => new EntityComponent { EntityId = entity.Id, Component = x.Name }).ToList();
            existing.Children = _entities.Find(e => entity.Children.Contains(e.Id)).ToList();
            _entities.Update(existing);

            return entity;
        }
    }
}