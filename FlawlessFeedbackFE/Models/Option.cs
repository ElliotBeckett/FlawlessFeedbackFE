using System.ComponentModel.DataAnnotations;

namespace FlawlessFeedbackFE.Models
{
    public class Option
    {
        [Display(Name = "Option ID")]
        public int OptionID { get; set; }

        [Display(Name = "Option Order")]
        [Required]
        public int OptionOrder { get; set; }

        [Display(Name = "Option Text")]
        [Required]
        public string OptionText { get; set; }

        [Display(Name = "Question ID")]
        [Required]
        public int QuestionID { get; set; }
    }
}