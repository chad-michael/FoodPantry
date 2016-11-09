using FoodPantry.Core.Repositories;
using FoodPantry.Data.Repositories;
using System.Web.Configuration;
using System.Web.Mvc;

namespace FoodPantry.Core.Services
{
    /// <summary>
    /// This service assists in managing the terms repositories.
    /// </summary>
    public class TermsService
    {
        private ITermsRespository _termRepository;

        private readonly Controller _parentController;

        internal ITermsRespository TermRepository
        {
            get
            {
                if (_termRepository != null)
                {
                    return _termRepository;
                }
                var serviceUrl = WebConfigurationManager.AppSettings["ColleagueWebServiceUrl"];
                var serivceSessionToken = (string)_parentController.Session["ColleagueWebserviceSessionToken"];
                if (string.IsNullOrEmpty(serivceSessionToken))
                {
                    var username = WebConfigurationManager.AppSettings["ColleagueWebServiceUsername"];
                    var password = WebConfigurationManager.AppSettings["ColleagueWebServicePassword"];
                    _termRepository = new ColleagueWsTermsRepository(serviceUrl, username, password);
                    _parentController.Session["ColleagueWebserviceSessionToken"] = ((ColleagueWsRepository)_termRepository).SessionToken;
                }
                else
                {
                    _termRepository = new ColleagueWsTermsRepository(serviceUrl, serivceSessionToken);
                }
                return _termRepository;
            }
            set
            {
                _termRepository = value;
            }
        }

        public TermsService(Controller parentController)
        {
            _parentController = parentController;
        }
    }
}