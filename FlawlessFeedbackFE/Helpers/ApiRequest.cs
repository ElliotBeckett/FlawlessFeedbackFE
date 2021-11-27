using System.Collections.Generic;
using System.Net.Http;

namespace FlawlessFeedbackFE.Helpers
{
    /// <summary>
    /// Static class that accepts Generic inputs for common API requests
    /// </summary>
    /// <typeparam name="T">Object returned</typeparam>
    public static class ApiRequest<T>
    {
        /// <summary>
        /// Returns a single record of the specified type
        /// </summary>
        /// <param name="_client">Http client to use</param>
        /// <param name="apiController">apiController endpoint to use</param>
        /// <param name="id">ID of the record to access</param>
        /// <returns></returns>
        public static T GetSingleRecord(HttpClient _client, string apiController, int id)
        {
            HttpResponseMessage response = _client.GetAsync($"{apiController}/{id}").Result;
            response.EnsureSuccessStatusCode();
            var record = response.Content.ReadAsAsync<T>().Result;
            return record;
        }

        /// <summary>
        /// Returns an IEnumerable collection of records of the specified type
        /// </summary>
        /// <param name="_client">Http Client to use</param>
        /// <param name="apiController">apiController endpoint to use</param>
        /// <returns></returns>
        public static IEnumerable<T> GetEnumerable(HttpClient _client, string apiController)
        {
            HttpResponseMessage response = _client.GetAsync(apiController).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsAsync<IEnumerable<T>>().Result;
        }

        /// <summary>
        /// Creates a new entry based upon the details provided
        /// </summary>
        /// <param name="_client">Http Client to use</param>
        /// <param name="apiController">apiController endpoint to use</param>
        /// <param name="entity">Class/Type of entity to update</param>
        /// <returns></returns>
        public static T Post(HttpClient _client, string apiController, T entity)
        {
            HttpResponseMessage message = _client.PostAsJsonAsync($"{apiController}", entity).Result;
#if DEBUG
            message.EnsureSuccessStatusCode();
#endif
            return message.Content.ReadAsAsync<T>().Result;
        }

        /// <summary>
        /// Updates the provided entity and returns the updated entity
        /// </summary>
        /// <param name="_client">Http Client to use</param>
        /// <param name="apiController">apiController endpoint to use</param>
        /// <param name="id">ID of the entity to update</param>
        /// <param name="entity">Type of entity to update</param>
        /// <returns>Updated entity</returns>
        public static T Put(HttpClient _client, string apiController, int id, T entity)
        {
            HttpResponseMessage message = _client.PutAsJsonAsync($"{apiController}/{id}", entity).Result;
#if DEBUG
            message.EnsureSuccessStatusCode();
#endif
            return message.Content.ReadAsAsync<T>().Result;
        }

        /// <summary>
        /// Deletes a single entry based upon the details provided
        /// </summary>
        /// <param name="_client">Http Client to use</param>
        /// <param name="apiController">apiController endpoint to use</param>
        /// <param name="id">ID of the entity to delete</param>
        /// <returns></returns>
        public static T Delete(HttpClient _client, string apiController, int id)
        {
            HttpResponseMessage response = _client.DeleteAsync($"{apiController}/{id}").Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsAsync<T>().Result;
        }
    }
}