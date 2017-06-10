using System.Collections.Generic;
using System.Linq;
using AdventuresInGrythia.Engine.Objects;

namespace AdventuresInGrythia.Engine.Locations
{
    public class AiGPortal : AiGEntity
    {
        public List<AiGPortalEntry> Entries {get;}
        public AiGPortal(int id, string name) : base(id, name)
        {
            Entries = new List<AiGPortalEntry>();
        }

        public void AddEntry(AiGPortalEntry entry)
        {
            if (!Entries.Any(x => x.Direction == entry.Direction))
                return;
            Entries.Add(entry);
        }

        public bool HasEntriesWithRoom(int roomId)
        {
            return Entries.Any(x => x.StartRoom == roomId || x.EndRoom == roomId);
        }
    }
}