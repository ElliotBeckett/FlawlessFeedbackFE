using ChartJSCore.Models;
using FlawlessFeedbackFE.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FlawlessFeedbackFE.Controllers
{
    public class ReportController : Controller
    {
        #region Setup + CTOR

        private HttpClient _client;
        private readonly string reportController = "Report";

        public ReportController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("FlawlessFeedbackHttpClient");
        }

        #endregion

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// End point to generate a chart based upon number of questions attached to a survey
        /// </summary>
        /// <returns></returns>
        public IActionResult SurveyQuestionsCountReport()
        {
            // Send a request to the correct endpoint to return a list of SurveyQuestionsCountReport
            HttpResponseMessage response = _client.GetAsync($"{reportController}/NumberOfQuestionsPerSurvey").Result;

            // Convert the response into an actual List
            var sqReportViewModel = response.Content.ReadAsAsync<List<SurveyQuestionViewModel>>().Result;

            // Convert our list into values that will work with ChartJS / ChartJSCore

            // populate a list of strings for labels
            var labels = sqReportViewModel.Select(c => c.SurveyTitle).ToList();

            // populate a list of nullable doubles for the values
            var chartData = sqReportViewModel.Select(d => (double?)d.NumberOfQuestions).ToList();

            // Creating the chart
            Chart chart = new Chart();

            // Setting the chart type
            chart.Type = Enums.ChartType.Bar;

            ChartJSCore.Models.Data data = new ChartJSCore.Models.Data();

            // pass in our pregenerated labels
            data.Labels = labels;

            // Create the actual Dataset (of type bar - with our values)
            BarDataset dataset = new BarDataset()
            {
                Label = "Number of Questions per Survey",
                Data = chartData,

            };

            // Create an empty list to hold our data sets (we only have one)
            data.Datasets = new List<Dataset>();

            // Add our dataset to the list of datasets
            data.Datasets.Add(dataset);

            // Assign this configured data object to the chart object
            chart.Data = data;

            // Creates an ViewData object and assigns the chart to it
            ViewData["SurveyQuestionChart"] = chart;

            return View(chart);
        }
    }
}
