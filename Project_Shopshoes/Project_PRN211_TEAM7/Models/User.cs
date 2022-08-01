using System;
using System.Collections.Generic;

#nullable disable

namespace Project_PRN211_TEAM7.Models
{
    public partial class User
    {
        public User()
        {
            Oders = new HashSet<Oder>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }

        public virtual ICollection<Oder> Oders { get; set; }
    }
}
