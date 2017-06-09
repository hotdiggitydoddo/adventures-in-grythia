namespace AdventuresInGrythia.Domain.Models
{
    public class EntityCommand : EntityBase
    {
        public int EntityId { get; set; }
        public string Command { get; set; }
    }
}