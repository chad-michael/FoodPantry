using System;
using System.Net;

namespace FoodPantry.Core.Repositories
{
    /// <summary>
    /// Base repository for connecting with the Colleague Web Service.
    /// </summary>
    public class ColleagueWsRepository
    {
        /// <summary>
        /// Username to access the webservice.
        /// </summary>
        public string Username;

        /// <summary>
        /// Password for the user.
        /// </summary>
        public string Password;

        /// <summary>
        /// Webservice URL.
        /// </summary>
        public string ServiceUrl;

        /// <summary>
        /// Session token used make requests after a successful login.
        /// </summary>
        public string SessionToken;

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public ColleagueWsRepository() { }

        /// <summary>
        /// Constructor to login to webservice.
        /// </summary>
        /// <param name="serviceUrl">Colleague webservice url.</param>
        /// <param name="username">Username to access the webservice.</param>
        /// <param name="password">Password for the user.</param>
        public ColleagueWsRepository(string serviceUrl, string username, string password)
        {
            ServiceUrl = serviceUrl;
            Username = username;
            Password = password;
            SessionToken = Login();
        }

        /// <summary>
        /// Use this when you have a session token.
        /// </summary>
        /// <param name="serviceUrl">Colleague webservice url.</param>
        /// <param name="sessionToken">Webservice authentication session token.</param>
        public ColleagueWsRepository(string serviceUrl, string sessionToken)
        {
            ServiceUrl = serviceUrl;
            SessionToken = sessionToken;
        }

        /// <summary>
        /// Login to the colleague webservice and returns a session token if successful.
        /// </summary>
        /// <returns>The session token.</returns>
        public string Login()
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.Headers.Add("Accept: application/vnd.ellucian.v1+json");
                    client.Headers.Add("Content-Type: application/json");
                    string headers = "{ 'UserId': '" + Username + "','Password': '" + Password + "'}";
                    return client.UploadString(ServiceUrl + "session/login", headers);
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Send a GET request to a specific service.
        /// </summary>
        /// <param name="service">To request information from.</param>
        /// <returns>A JSON response.</returns>
        public string GetService(string service)
        {
            if (string.IsNullOrEmpty(SessionToken))
            {
                return "{}";
            }
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.Headers.Add("Accept: application/vnd.ellucian.v1+json");
                    client.Headers.Add("Content-Type: application/json");
                    client.Headers.Add("X-CustomCredentials: " + SessionToken);
                    return client.DownloadString(ServiceUrl + service);
                }
                catch (Exception)
                {
                    return "{}";
                }
            }
        }
    }
}