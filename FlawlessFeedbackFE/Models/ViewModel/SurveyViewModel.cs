using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FlawlessFeedbackFE.Models.ViewModel
{
    public class SurveyViewModel
    {
        [DisplayName("Survey")]
        public string SurveyID { get; set; }
        public List<SelectListItem> ListOfSurveys { get; set; }

    }
}
