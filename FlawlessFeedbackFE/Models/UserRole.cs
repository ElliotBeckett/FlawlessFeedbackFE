using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FlawlessFeedbackFE.Models
{
    public class UserRole
    {
        [Display(Name = "Role ID")]
        public int UserRoleID { get; set; }

        [Display(Name = "Role Title")]
        public string UserRoleTitle { get; set; }
    }
}