using System.Collections.Generic;

namespace AdventuresInGrythia.Domain.Models
{
    public class Entity : EntityBase
    {
        public string Name {get; set;}
        public List<Trait> Traits {get; set;}
    }
}