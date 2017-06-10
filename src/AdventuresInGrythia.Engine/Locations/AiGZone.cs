using System.Collections.Generic;
using AdventuresInGrythia.Engine.Objects;

namespace AdventuresInGrythia.Engine.Locations
{
    public class AiGZone : AiGEntity
    {
        public List<int> Regions {get; private set;}
        public AiGZone(int id, string name = null) : base(id, name)
        {
            Regions = new List<int>();
        }
    }
}