using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlawlessFeedbackFE.Models
{
    public class Question
    {
        [Display (Name = "Question ID")]
        public int QuestionID { get; set; }

        [Display (Name = "Question")]
        [Required]
        public string QuestionText { get; set; }

        [Display (Name = "Survey ID")]
        [Required]
        public int SurveyID { get; set; }

        //Navigation Variables
        public virtual ICollection<Option> Options { get; set; }
    }
}