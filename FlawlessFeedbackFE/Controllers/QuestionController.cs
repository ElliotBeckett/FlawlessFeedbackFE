using FlawlessFeedbackFE.Helpers;
using FlawlessFeedbackFE.Models;
using FlawlessFeedbackFE.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace FlawlessFeedbackFE.Controllers
{
    public class QuestionController : Controller
    {
        #region Setup + CTOR

        private HttpClient _client;
        private readonly string questionController = "Question";
        private LoginCheck _check;

        public QuestionController(IHttpClientFactory httpClientFactory, LoginCheck check)
        {
            _client = httpClientFactory.CreateClient("FlawlessFeedbackHttpClient");
            _check = check;
        }

        #endregion Setup + CTOR

        #region Get Methods

        // GET: QuestionController
        public ActionResult Index()
        {
            var questionList = ApiRequest<Question>.GetEnumerable(_client, questionController).ToList();

            return View(questionList);
        }

        // GET: QuestionController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var question = ApiRequest<Question>.GetSingleRecord(_client, questionController, id);
                return View(question);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        // GET: QuestionController/Create
        public ActionResult Create()
        {
            GenerateSurveyDDL();
            return View();
        }

        #endregion Get Methods

        // POST: QuestionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Question question)
        {
            try
            {
                ApiRequest<Question>.Post(_client, questionController, question);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// Creates a new question and redirects the user to the newly created question
        /// </summary>
        /// <param name="question">question to create</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAndRedirect(Question question)
        {
            try
            {
                var result = ApiRequest<Question>.Post(_client, questionController, question);

                return RedirectToAction("DetailsWithOptions", "Question", new { id = result.QuestionID });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: QuestionController/Edit/5
        public ActionResult Edit(int id)
        {
            if (_check.IsLoggedIn(_client, HttpContext))
            {
                var question = ApiRequest<Question>.GetSingleRecord(_client, questionController, id);

                return View(question);
            }

            /* TODO implement correct UnAuthorized page*/
            return RedirectToAction("Login", "Home");
        }

        // POST: QuestionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Question question)
        {
            try
            {
                if (_check.IsLoggedIn(_client, HttpContext))
                {
                    ApiRequest<Question>.Put(_client, questionController, id, question);

                    return RedirectToAction(nameof(Index));
                }

                /* TODO implement correct UnAuthorized page*/
                return RedirectToAction("Login", "Home");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: QuestionController/Delete/5
        public ActionResult Delete(int id)
        {
            if (_check.IsLoggedIn(_client, HttpContext))
            {
                var question = ApiRequest<Question>.GetSingleRecord(_client, questionController, id);

                return View(question);
            }
            return RedirectToAction("Login", "Home");
        }

        // POST: QuestionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Question question)
        {
            try
            {
                if (_check.IsLoggedIn(_client, HttpContext))
                {
                    ApiRequest<Question>.Delete(_client, questionController, id);

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

        // GET: QuestionController/DetailsWithOptions/5
        /// <summary>
        /// GET method to retrieve a list of quesion details with their associated options
        /// </summary>
        /// <param name="id">Question ID</param>
        /// <returns>View of questions with their options</returns>
        public ActionResult DetailsWithOptions(int id)
        {
            try
            {
                Question question = ApiRequest<Question>.GetSingleRecord(_client, questionController + "/GetQuestionsWithOptions", id);

                return View(question);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Method to populate a viewbag model for the survey drop down list when creating a question
        /// </summary>
        /// <returns></returns>
        public ActionResult GenerateSurveyDDL() 
        {
            SurveyViewModel model = new SurveyViewModel();
            
            var svModel = ApiRequest<Survey>.GetEnumerable(_client, "Survey").ToList();

            model.ListOfSurveys = svModel.Select(x => new SelectListItem { Value = x.SurveyID.ToString(), Text = x.SurveyTitle }).ToList();

            ViewBag.ListOfSurveys = model.ListOfSurveys;
            return View();
        
        }

        #endregion Custom Methods
    }
}