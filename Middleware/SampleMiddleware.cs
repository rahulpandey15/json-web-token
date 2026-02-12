namespace IntroductionToAPI.Middleware
{
    public class SampleMiddleware
    {
        private readonly RequestDelegate _next;

        public SampleMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke(HttpContext context)
        {
            Console.WriteLine("Executing SampleMiddleware...");

            await _next(context);

            Console.WriteLine("Sample Middleware Code Executed");
        }
    }

}
