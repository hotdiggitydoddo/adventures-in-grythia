using System.Collections.Generic;
using AdventuresInGrythia.Engine.Objects;

namespace AdventuresInGrythia.Engine.Locations
{
    public class AiGRoom : AiGEntity
    {
        public int RegionId { get; }
        public List<int> Entities { get; }
        public AiGRoom(int id, string name, int regionId) : base(id, name)
        {
            Entities = new List<int>();
        }
    }
}