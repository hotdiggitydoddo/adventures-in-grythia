using System.Collections.Generic;
using AdventuresInGrythia.Engine.Containers;

namespace AdventuresInGrythia.Engine.Objects
{
    public class AiGEntity
    {
        public int Id { get; }
        public string Name { get; set; }
        public TraitSet Traits { get; }
        public ComponentSet Components { get; }
        public List<int> Children {get;}
        public int? Parent {get; set;}
        public AiGEntity(int id, string name = null, int? parent = null)
        {
            Name = name ?? "-none-";
            Id = id;
            Traits = new TraitSet(id);
            Components = new ComponentSet(id);
            Children = new List<int>();
            Parent = parent;
        }
    }
}