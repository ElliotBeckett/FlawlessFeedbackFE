using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlawlessFeedbackFE.Models.ViewModel
{
    public class UserRoleViewModel
    {
        [Display(Name = "Role ID")]
        public string UserRoleID { get; set; }

        public List<SelectListItem> ListOfRoles { get; set; }
    }
}