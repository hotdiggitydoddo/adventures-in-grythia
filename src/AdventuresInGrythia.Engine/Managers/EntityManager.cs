using AdventuresInGrythia.Domain.Contracts;
using AdventuresInGrythia.Domain.Models;

namespace AdventuresInGrythia.Engine.Managers
{
    public class EntityManager
    {
        readonly IRepository<Entity> _entities;
        readonly IRepository<Trait> _traits;
        readonly IRepository<EntityCommand> _entityCommands;
        readonly IRepository<EntityComponent> _entityComponents;

        public EntityManager(IRepository<Entity> entities, IRepository<Trait> traits,
            IRepository<EntityCommand> entityCommands, IRepository<EntityComponent> entityComponents)
        {
            _entities = entities;
            _traits = traits;
            _entityCommands = entityCommands;
            _entityComponents = entityComponents;
        }

        
    }
}