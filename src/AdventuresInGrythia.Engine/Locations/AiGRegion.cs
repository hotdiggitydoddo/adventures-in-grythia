using System.Collections.Generic;
using AdventuresInGrythia.Engine.Objects;

namespace AdventuresInGrythia.Engine.Locations
{
    public class AiGRegion : AiGEntity
    {
        public int ZoneId { get; }
        public List<int> Rooms { get; }
        public AiGRegion(int regionId, int id, string name) : base(id, name)
        {
            Rooms = new List<int>();
        }
    }
}