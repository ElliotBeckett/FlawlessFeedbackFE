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
using System.Threading.Tasks;

namespace FlawlessFeedbackFE.Controllers
{
    public class UserController : Controller
    {
        #region Setup + CTOR

        private HttpClient _client;
        private readonly string userController = "User";
        private LoginCheck _check;

        public UserController(IHttpClientFactory httpClientFactory, LoginCheck check)
        {
            _client = httpClientFactory.CreateClient("FlawlessFeedbackHttpClient");
            _check = check;
        }

        #endregion Setup + CTOR

        // GET: UserController
        public ActionResult Index()
        {
            var userList = ApiRequest<UserInfo>.GetEnumerable(_client, $"{userController}/GetAllWithUserRole").ToList();

            return View(userList);
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var user = ApiRequest<UserInfo>.GetSingleRecord(_client, $"{userController}/GetSingleUserWithRole", id);
                return View(user);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            GenerateUserRoleDDL();
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        #region Custom Methods

        public ActionResult GenerateUserRoleDDL()
        {
            UserRoleViewModel model = new UserRoleViewModel();

            var svModel = ApiRequest<UserRole>.GetEnumerable(_client, "Role").ToList();

            model.ListOfRoles = svModel.Select(x => new SelectListItem { Value = x.UserRoleID.ToString(), Text = x.UserRoleTitle }).ToList();

            ViewBag.ListOfRoles = model.ListOfRoles;

            return View();
        }

        #endregion Custom Methods
    }
}