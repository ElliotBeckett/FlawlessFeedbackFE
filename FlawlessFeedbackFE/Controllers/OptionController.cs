using FlawlessFeedbackFE.Helpers;
using FlawlessFeedbackFE.Models;
using FlawlessFeedbackFE.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Net.Http;

namespace FlawlessFeedbackFE.Controllers
{
    public class OptionController : Controller
    {
        #region Setup + CTOR

        private HttpClient _client;
        private LoginCheck _check;
        private readonly string optionController = "Option";

        public OptionController(IHttpClientFactory httpClientFactory, LoginCheck check)
        {
            _client = httpClientFactory.CreateClient("FlawlessFeedbackHttpClient");
            _check = check;
        }

        #endregion Setup + CTOR

        // GET: OptionController
        public ActionResult Index()
        {
            var optionsList = ApiRequest<Option>.GetEnumerable(_client, optionController).ToList();

            return View(optionsList);
        }

        // GET: OptionController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var option = ApiRequest<Option>.GetSingleRecord(_client, optionController, id);

                return View(option);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        // GET: OptionController/Create
        public ActionResult Create()
        {
            GenerateQuestionDDL();
            return View();
        }

        // POST: OptionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Option option)
        {
            try
            {
                ApiRequest<Option>.Post(_client, optionController, option);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        /// <summary>
        /// Creates a new option and redirects the user to the newly created option
        /// </summary>
        /// <param name="option">Option to create</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAndRedirect(Option option)
        {
            try
            {
                var result = ApiRequest<Option>.Post(_client, optionController, option);

                return RedirectToAction("Details", "Option", new { id = result.OptionID });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: OptionController/Edit/5
        public ActionResult Edit(int id)
        {
            if (_check.IsLoggedIn(_client, HttpContext))
            {
                var option = ApiRequest<Option>.GetSingleRecord(_client, optionController, id);

                return View(option);
            }

            /* TODO implement correct UnAuthorized page*/
            return RedirectToAction("Login", "Home");
        }

        // POST: OptionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Option option)
        {
            try
            {
                if (_check.IsLoggedIn(_client, HttpContext))
                {
                    ApiRequest<Option>.Put(_client, optionController, id, option);
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction("Login", "Home");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: OptionController/Delete/5
        public ActionResult Delete(int id)
        {
            if (_check.IsLoggedIn(_client, HttpContext))
            {
                var option = ApiRequest<Option>.GetSingleRecord(_client, optionController, id);

                return View(option);
            }

            return RedirectToAction("Login", "Home");
        }

        // POST: OptionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Option option)
        {
            try
            {
                if (_check.IsLoggedIn(_client, HttpContext))
                {
                    ApiRequest<Option>.Delete(_client, optionController, id);

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
        /// Method to populate a viewbag model for the question drop down list when creating an option
        /// </summary>
        /// <returns></returns>
        public ActionResult GenerateQuestionDDL()
        {
            QuestionViewModel model = new QuestionViewModel();

            var svModel = ApiRequest<Question>.GetEnumerable(_client, "Question").ToList();

            model.ListOfQuestions = svModel.Select(x => new SelectListItem { Value = x.QuestionID.ToString(), Text = x.QuestionText }).ToList();

            ViewBag.ListOfQuestions = model.ListOfQuestions;
            return View();

        }

        #endregion
    }
}