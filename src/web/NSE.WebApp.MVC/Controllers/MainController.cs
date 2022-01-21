using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using System.Linq;

namespace NSE.WebApp.MVC.Controllers
{
    public class MainController: Controller
    {
        protected bool HasErrorsResponse(ResponseResult responseResult)
        {
            if(responseResult != null && responseResult.Errors.Messages.Any())
            {
                foreach (var message in responseResult.Errors.Messages)
                {
                    ModelState.AddModelError(key: string.Empty, errorMessage: message);
                }

                return true;
            }

            return false;
        }
    }
}
