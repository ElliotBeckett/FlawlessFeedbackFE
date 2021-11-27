using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FlawlessFeedbackFE.Models.ViewModel
{
    public class QuestionViewModel
    {
        [DisplayName("Question")]
        public string QuestionID { get; set; }
        public List<SelectListItem> ListOfQuestions { get; set; }


    }
}
