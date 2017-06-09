using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AdventuresInGrythia.Domain.Models
{
    public class User : IdentityUser<int>
    {
        public int AccountId { get; set; }
        public Account Account { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
