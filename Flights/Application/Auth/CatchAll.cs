using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Flights.Application.Auth
{
    public class CatchAll : BaseModel
    {
        #region Handlers

        public IActionResult GetOrPost()
        {
            try 
            { 
                Response.StatusCode = (int)HttpStatusCode.NotFound; 
                return View(); 
            }
            catch 
            { 
                return LocalRedirect("/error.htm"); 
            }
        }

        #endregion
    }
}
