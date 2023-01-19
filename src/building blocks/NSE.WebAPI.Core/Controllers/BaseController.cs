using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSE.Core.Comunication;
using System.Collections.Generic;
using System.Linq;

namespace NSE.WebAPI.Core.Controllers
{
    [ApiController]
    public abstract class BaseController : Controller
    {
        protected ICollection<string> Errors = new List<string>();
        protected ActionResult CustomResponse(object result = null)
        {
            if (ValidOperation())
            {
                return Ok(result);
            }

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                {"Messages", Errors.ToArray() }
            }));
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(e => e.Errors);
            foreach (var error in errors)
            {
                AddProcessingError(error.ErrorMessage);
            }

            return CustomResponse();
        }

        protected ActionResult CustomResponse(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                AddProcessingError(error.ErrorMessage);
            }

            return CustomResponse();
        }

        protected ActionResult CustomResponse(ResponseResult responseResult)
        {
            ResponseHasErrors(responseResult);

            return CustomResponse();
        }

        protected bool ResponseHasErrors(ResponseResult responseResult)
        {
            if (responseResult == null || responseResult.Errors.Messages.Any()) return false;

            foreach (var messages in responseResult.Errors.Messages)
            {
                AddProcessingError(messages);
            }

            return true;
        }

        protected bool ValidOperation()
        {
            return !Errors.Any();
        }

        protected void AddProcessingError(string erro)
        {
            Errors.Add(erro);
        }

        protected void CelarProcessingError()
        {
            Errors.Clear();
        }
    }
}
