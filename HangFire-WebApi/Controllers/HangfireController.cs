using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangFire_WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HangfireController : ControllerBase
    {
      [HttpGet]
      public IActionResult Get()
      {
         return Ok("Hello from hangfire!!");
      }

      #region Fire and Forget Job
      [HttpPost]
      [Route("[action]") ]
      public IActionResult Welcome()
      {
            var jobId = BackgroundJob.Enqueue(() => SendWelcomeEmail("Welcome to our App"));
            return Ok($"JobId :{jobId}. Welcome email sent to the user");
      }
        public void SendWelcomeEmail(string text)//private will throw exception
        {
            Console.WriteLine(text);
        }
        #endregion

      #region Delay Job
        [HttpPost]
        [Route("[action]")]
        public IActionResult Discount()
        {
            var timeInSeconds = 60;
            var jobId = BackgroundJob.Schedule(() => SendDiscountEmail("Welcome to our App"), TimeSpan.FromSeconds(timeInSeconds));
            return Ok($"JobId :{jobId}. Welcome email sent to the user");
        }
        public void SendDiscountEmail(string text)//private will throw exception
        {
            Console.WriteLine(text);
        }
        #endregion

      #region Recurring Jobs
        [HttpPost]
        [Route("[action]")]
        public IActionResult DatabaseUpdate()
        {
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Database updated!"),Cron.Minutely);
            return Ok("Database check job initiated!");
        }

        #endregion

      #region Continious Jobs
        [HttpPost]
        [Route("[action]")]
        public IActionResult Confirm()
        {
            var timeInSeconds = 30;
            var parentJobId = BackgroundJob.Schedule(() => SendUnsubscribeEmail("You asked to be unsubscibed!"), TimeSpan.FromSeconds(timeInSeconds));
            BackgroundJob.ContinueJobWith(parentJobId, () => Console.WriteLine("You were Unsubscribed!"));
            return Ok("Confirmation job created!");
        }
        public void SendUnsubscribeEmail(string text)//private will throw exception
        {
            Console.WriteLine(text);
        }
        #endregion
    }
}
