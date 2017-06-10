using System.Collections.Generic;

namespace AdventuresInGrythia.Domain.Models
{
    public class Entity : EntityBase
    {
        public string Name {get; set;}
        public List<Trait> Traits {get; set;}
        public List<EntityComponent> Components {get; set;}
        public int? ParentId {get; set;}
        public Entity Parent {get; set;}
        public List<Entity> Children {get; set;}
    }
}