using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace OnlineStoreForWoman.Entities
{
    public class Role : IdentityRole<int>
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
