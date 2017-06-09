using System.Collections.Generic;

namespace AdventuresInGrythia.Domain.Models
{
    public class Account : EntityBase
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Account_Entity> Characters {get; set;}
        public bool IsBanned { get; set; }
    }
}