using System;
using System.Collections.Generic;
using System.Linq;
using InterviewTask.TestData;
using Newtonsoft.Json;
using RestSharp;

namespace InterviewTask.Helpers
{
    /// <summary>
    /// Contains helper methods for communicating with the server.
    /// </summary>
    public class TestHelper
    {
        private readonly RestClient _baseUrl = new RestClient("https://cci-gwp-adonis-api.herokuapp.com");

        /// <summary>
        /// Gets all users from server.
        /// </summary>
        /// <param name="response"></param>
        /// <returns>A list of users.</returns>
        public List<User> GetUsers(out IRestResponse response)
        {
            var request = new RestRequest("user/get", Method.GET);
            response = _baseUrl.Execute(request);

            return JsonConvert.DeserializeObject<List<User>>(response.Content);
        }

        /// <summary>
        /// Gets user by username.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>The found user. Null if there is no user found. Throws an exception if more than 1 user is found.</returns>
        public User GetUserByUsername(string userName)
        {
            var request = new RestRequest($"user/get?username={userName}", Method.GET);
            var response = _baseUrl.Execute(request);
            var users = JsonConvert.DeserializeObject<List<User>>(response.Content);

            return users.Count switch
            {
                0 => null,
                1 => users[0],
                _ => throw new ArgumentOutOfRangeException(nameof(userName), "Found more than 1 user."),
            };
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="user">User to register.</param>
        /// <param name="response">Server response after attempting to register a user.</param>
        public void RegisterUser(User user, out IRestResponse response)
        {
            var request = new RestRequest("user/create", Method.POST);

            request.AddJsonBody(JsonConvert.SerializeObject(user));
            response = _baseUrl.Execute(request);
        }

        /// <summary>
        /// Checks for the necessary headers in the server response.
        /// Please add the necessary headers.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public bool IsNecessaryHeadersInResponse(IRestResponse response)
        {
            var connection = response.Headers
                .Where(x => x.Name == "Connection")
                .Select(x => x.Value)
                .FirstOrDefault()
                ?.ToString();

            var contentType = response.Headers
                .Where(x => x.Name == "Content-Type")
                .Select(x => x.Value)
                .FirstOrDefault()
                ?.ToString();

            return (connection != null && connection.Equals("keep-alive")) && (contentType != null && contentType.Equals("application/json; charset=utf-8"));
        }

        /// <summary>
        /// Checks for potential dangerous headers in the server response.
        /// Please add the necessary headers.
        /// In case of a positive result, recheck manually content of the headers.
        /// </summary>
        /// <param name="response"></param>
        /// <returns>True if the server response contains potentially dangerous headers. False if not.</returns>
        public bool IsDangerousHeadersInResponse(IRestResponse response)
        {
            var isServerHeaderInResponse = response.Headers
                .FirstOrDefault(x => x.Name == "Server");

            var isViaHeaderInResponse = response.Headers
                .FirstOrDefault(x => x.Name == "Via");

            return (isServerHeaderInResponse != null || isViaHeaderInResponse != null);
        }
    }
}
