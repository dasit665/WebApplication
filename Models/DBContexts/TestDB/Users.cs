using System;
using System.Collections.Generic;

namespace WebApplication
{
    public partial class Users
    {
        public Users()
        {
            UserRoles = new HashSet<UserRoles>();
        }

        public int Id { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual ICollection<UserRoles> UserRoles { get; set; }
    }
}
