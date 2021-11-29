using ChartJSCore.Helpers;
using ChartJSCore.Models;
using CsvHelper;
using FlawlessFeedbackFE.Helpers;
using FlawlessFeedbackFE.Models;
using FlawlessFeedbackFE.Models.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FlawlessFeedbackFE.Controllers
{
    public class SurveyController : Controller
    {
        #region Setup + CTOR

        private HttpClient _client;
        private readonly string surveyController = "Survey";
        private LoginCheck _check;
        private IWebHostEnvironment _env;

        public SurveyController(IHttpClientFactory httpClientFactory, LoginCheck check, IWebHostEnvironment env)
        {
            _client = httpClientFactory.CreateClient("FlawlessFeedbackHttpClient");
            _check = check;
            _env = env; // Gives access to the root of the application in a web environment
        }

        #endregion Setup + CTOR

        // GET: SurveyController
        public ActionResult Index()
        {
            var surveyList = ApiRequest<Survey>.GetEnumerable(_client, surveyController).ToList();

            return View(surveyList);
        }

        // GET: SurveyController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                Survey survey = ApiRequest<Survey>.GetSingleRecord(_client, surveyController, id);

                return View(survey);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        // GET: SurveyController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SurveyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Survey survey)
        {
            try
            {
                if (_check.IsLoggedIn(_client, HttpContext))
                {
                    ApiRequest<Survey>.Post(_client, surveyController, survey);
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction("Login", "Home");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Creates a new survey and redirects the user to the newly created survey
        /// </summary>
        /// <param name="survey">questsurveyion to create</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAndRedirect(Survey survey)
        {
            try
            {
                if (_check.IsLoggedIn(_client, HttpContext))
                {
                    var result = ApiRequest<Survey>.Post(_client, surveyController, survey);

                    return RedirectToAction("DetailsWithQuestions", "Survey", new { id = result.SurveyID });
                }
                return RedirectToAction("Login", "Home");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: SurveyController/Edit/5
        public ActionResult Edit(int id)
        {
            if (_check.IsLoggedIn(_client, HttpContext))
            {
                var survey = ApiRequest<Survey>.GetSingleRecord(_client, surveyController, id);

                return View(survey);
            }

            /* TODO implement correct UnAuthorized page*/
            return RedirectToAction("Login", "Home");
        }

        // POST: SurveyController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Survey survey)
        {
            try
            {
                if (_check.IsLoggedIn(_client, HttpContext))
                {
                    ApiRequest<Survey>.Put(_client, surveyController, id, survey);
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction("Login", "Home");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: SurveyController/Delete/5
        public ActionResult Delete(int id)
        {
            if (_check.IsLoggedIn(_client, HttpContext))
            {
                var survey = ApiRequest<Survey>.GetSingleRecord(_client, surveyController, id);

                return View(survey);
            }
            return RedirectToAction("Login", "Home");
        }

        // POST: SurveyController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Survey survey)
        {
            try
            {
                if (_check.IsLoggedIn(_client, HttpContext))
                {
                    ApiRequest<Survey>.Delete(_client, surveyController, id);

                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction("Login", "Home");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #region Custom Methods

        /// <summary>
        /// Method to allow users to upload picture for surveys. Works in partnership with JavaScript code on the view
        /// </summary>
        /// <param name="file">file to upload</param>
        /// <returns>Status of file upload</returns>
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    string folderRoot = Path.Combine(_env.ContentRootPath, "wwwroot\\Uploads");
                    // Added Guid to ensure filename uniqueness
                    string fileName = file.FileName;
                    Debug.WriteLine("Method Name: " + fileName);
                    string filePath = Path.Combine(folderRoot, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return Ok(new { success = true, response = "File Uploaded" });
                }

                return BadRequest(new { success = false, response = "No file found" });
            }
            catch (Exception e)
            {
                return BadRequest(new { success = false, response = e.Message });
            }
        }

        /// <summary>
        /// GET method to retrieve a list of survey details with their associated questions
        /// </summary>
        /// <param name="id">Survey ID</param>
        /// <returns>View of surveys with their questions</returns>
        public ActionResult DetailsWithQuestions(int id)
        {
            try
            {
                Survey survey = ApiRequest<Survey>.GetSingleRecord(_client, surveyController + "/GetSurveyWithQuestions", id);

                return View(survey);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Generates a Chart based upon questions per survey
        /// </summary>
        /// <returns>Chart</returns>
        public IActionResult SurveyQuestionsCountReport()
        {
            // Send a request to the correct endpoint to return a list of SurveyQuestionsCountReport
            HttpResponseMessage response = _client.GetAsync($"Report/NumberOfQuestionsPerSurvey").Result;

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

            Data data = new Data();

            // pass in our pregenerated labels
            data.Labels = labels;

            // Making our chart colourful
            List<ChartColor> barColour = new List<ChartColor>()
            {
                ChartColor.CreateRandomChartColor(true),
                ChartColor.CreateRandomChartColor(true),
                ChartColor.CreateRandomChartColor(true),
                ChartColor.CreateRandomChartColor(true)
            };

            // Create the actual Dataset (of type bar - with our values)
            BarDataset dataset = new BarDataset()
            {
                Label = "Number of Questions per Survey",
                Data = chartData,
                BackgroundColor = barColour
            };

            // Create an empty list to hold our data sets (we only have one)
            data.Datasets = new List<Dataset>();

            // Add our dataset to the list of datasets
            data.Datasets.Add(dataset);

            // Assign this configured data object to the chart object
            chart.Data = data;

            // Creates an ViewData object and assigns the chart to it
            ViewData["chart"] = chart;

            var test = ViewData["chart"];

            return View(chart);
        }

        /// <summary>
        /// Allows users to download/export the chart results to CSV file format
        /// </summary>
        /// <returns>CSV file</returns>
        public IActionResult ExportCSV()
        {
            // Send a request to the correct endpoint to return a list of SurveyQuestionsCountReport
            HttpResponseMessage response = _client.GetAsync($"Report/NumberOfQuestionsPerSurvey").Result;

            // Convert the response into an actual List
            var sqReportViewModel = response.Content.ReadAsAsync<List<SurveyQuestionViewModel>>().Result;

            // create a new memory stream to use
            var stream = new MemoryStream();

            // Sets up a stream, makes the connection but don't close the sream until everything is done
            using (var writeFile = new StreamWriter(stream, leaveOpen: true))
            {
                // Use a package to generate a CSV
                // CultureInfo sets the language to match where the user is
                var csv = new CsvWriter(writeFile, culture: CultureInfo.CurrentCulture, true);

                csv.WriteRecords(sqReportViewModel);
            }

            // Resets the stream position, to prevent it getting stuck in a loop
            stream.Position = 0;

            return File(stream, "application/octet-stream", $"ReportData_{DateTime.Now.ToString("ddMMM_HHmmss")}.csv");
        }

        #endregion Custom Methods
    }
}