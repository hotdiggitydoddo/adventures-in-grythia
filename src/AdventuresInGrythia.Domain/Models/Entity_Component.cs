namespace AdventuresInGrythia.Domain.Models
{
    public class Entity_Component : EntityBase
    {
        public int EntityId { get; set; }
        public string Component { get; set; }

        public Entity Entity {get; set;}
    }
}