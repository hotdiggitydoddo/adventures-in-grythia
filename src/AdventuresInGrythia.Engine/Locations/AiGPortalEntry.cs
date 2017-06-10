using AdventuresInGrythia.Engine.Objects;

namespace AdventuresInGrythia.Engine.Locations
{
    public class AiGPortalEntry
    {
        public int Id {get;}
        public int StartRoom {get;}   
        public int EndRoom {get;}      
        public string Direction {get;}

         public AiGPortalEntry(int id, int startRoom, int endRoom, string direction)
         {
            Id = id;
            StartRoom = startRoom;
            EndRoom = endRoom;
            Direction = direction;
         }
    }

}