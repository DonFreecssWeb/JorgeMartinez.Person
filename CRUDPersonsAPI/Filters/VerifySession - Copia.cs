using CRUDPersonsAPI.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDPersonsAPI.Filters
{
    public class VerifySession2 : IActionFilter
    {
        private readonly ILogger<VerifySession2> _logger;
        public VerifySession2(ILogger<VerifySession2> logger,string a) {
            _logger = logger;   
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
            _logger.LogInformation("{FilterName}.{MethodName}",nameof(VerifySession2),nameof(OnActionExecuted));

            PersonController personControlle = (PersonController)context.Controller;
           

          



        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("{FilterName}.{MethodName}", nameof(VerifySession2), nameof(OnActionExecuting));
             
            
        }
    }
}
