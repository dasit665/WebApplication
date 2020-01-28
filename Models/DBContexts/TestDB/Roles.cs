using System;
using System.Collections.Generic;

namespace WebApplication
{
    public partial class Roles
    {
        public Roles()
        {
            UserRoles = new HashSet<UserRoles>();
        }

        public int Id { get; set; }
        public string Role { get; set; }

        public virtual ICollection<UserRoles> UserRoles { get; set; }
    }
}
