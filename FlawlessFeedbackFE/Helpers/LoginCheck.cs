using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Net.Http;

namespace FlawlessFeedbackFE.Helpers
{
    public class LoginCheck
    {
        /// <summary>
        /// Method to check whether a token is valid, therefore if the user is logged in
        /// </summary>
        /// <param name="_client">HttpClient that is in use</param>
        /// <param name="_context">Current Http Sesson</param>
        /// <returns>
        /// Bool
        /// - True if the token is valid
        /// - False if the token is not valid
        /// </returns>
        public bool IsLoggedIn(HttpClient _client, HttpContext _context)
        {
            var token = _context.Session.GetString("Token");


            if (token != null)
            {
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token.ToString());
                return true;
            }

            return false;
        }
    }
}