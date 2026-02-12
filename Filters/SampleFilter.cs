using Microsoft.AspNetCore.Mvc.Filters;

namespace IntroductionToAPI.Filters
{
    public class SampleFilter : ActionFilterAttribute
    {

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("Sample Filter Executed...");
        }
    }
}
