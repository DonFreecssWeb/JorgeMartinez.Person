using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDPersonsAPI.Filters
{
    public class VerifySession : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {            
            context.HttpContext.Response.StatusCode = 100;

            base.OnActionExecuted(context);
        }
    }
}
