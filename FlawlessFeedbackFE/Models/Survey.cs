using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlawlessFeedbackFE.Models
{
    public class Survey
    {
        [Display(Name = "Survey ID")]
        public int SurveyID { get; set; }

        [Display(Name = "Title")]
        [Required]
        public string SurveyTitle { get; set; }

        [Display(Name = "Topic")]
        [Required]
        public string SurveyTopic { get; set; }

        [Display(Name = "Author")]
        [Required]
        public string SurveyAuthor { get; set; }

        [Display(Name = "Date Created")]
        [Required]
        public DateTime SurveyDateCreated { get; set; }

        [Display(Name = "Comments")]
        public string SurveyComments { get; set; }

        [Display(Name = "Image")]
        public string SurveyImageURL { get; set; }

        //Navigation Properties

        public virtual ICollection<Question> Questions { get; set; }
    }
}