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

        public AiGEntity SaveEntity(AiGEntity entity)
        {
            var existing = _entities.GetById(entity.Id, x => x.Traits);
            existing.Traits.Clear();

             foreach (var trait in entity.Traits.GetAll())
                 existing.Traits.Add(new Trait { EntityId = entity.Id, Name = trait.Name, Value = trait.Value });
            
            existing.Components = entity.Components.GetAll()
                .Select(x => new EntityComponent { EntityId = entity.Id, Component = x.Name }).ToList();
            
            _entities.Update(existing);
        }

        
    }
}