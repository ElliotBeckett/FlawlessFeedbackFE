using System.ComponentModel.DataAnnotations;

namespace FlawlessFeedbackFE.Models
{
    public class UserInfo
    {
        [Display(Name ="User name")]
        public string UserName { get; set; }
        [Display(Name = "Password")]
        public string UserPass { get; set; }
    }
}