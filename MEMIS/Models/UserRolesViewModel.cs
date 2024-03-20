using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MEMIS.Models
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public string MobileNo { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }

    public class ManageUserRolesViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool Selected { get; set; }
    }
}
