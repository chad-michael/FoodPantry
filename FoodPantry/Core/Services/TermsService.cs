using FoodPantry.Core.Repositories;
using FoodPantry.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace FoodPantry.Core.Services
{
    /// <summary>
    /// This service assists in managing the terms repositories.
    /// </summary>
    public class TermsService
    {
        /// <summary>
        /// The terms repository to get informatino from.
        /// </summary>
        private ITermsRespository termRepository;

        /// <summary>
        /// The parent controller to access the session.
        /// </summary>
        private Controller ParentController;

        /// <summary>
        /// Get the terms repository.
        /// </summary>
        internal ITermsRespository TermRepository
        {
            get
            {
                if (termRepository != null)
                {
                    return termRepository;
                }
                var serviceUrl = WebConfigurationManager.AppSettings["ColleagueWebServiceUrl"];
                var serivceSessionToken = (string)ParentController.Session["ColleagueWebserviceSessionToken"];
                if (string.IsNullOrEmpty(serivceSessionToken))
                {
                    var username = WebConfigurationManager.AppSettings["ColleagueWebServiceUsername"];
                    var password = WebConfigurationManager.AppSettings["ColleagueWebServicePassword"];
                    termRepository = new ColleagueWsTermsRepository(serviceUrl, username, password);
                    ParentController.Session["ColleagueWebserviceSessionToken"] = ((ColleagueWsRepository)termRepository).SessionToken;
                }
                else
                {
                    termRepository = new ColleagueWsTermsRepository(serviceUrl, serivceSessionToken);
                }
                return termRepository;
            }
            set
            {
                termRepository = value;
            }
        }

        /// <summary>
        /// Build the terms service with the parent controller.
        /// </summary>
        /// <param name="parentController"></param>
        public TermsService(Controller parentController)
        {
            ParentController = parentController;
        }
    }
}