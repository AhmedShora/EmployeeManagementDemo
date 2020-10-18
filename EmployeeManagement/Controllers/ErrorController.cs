using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "sorry, page could not be found";
                    break;
            }
            return View("NotFound");
        }

        [Route("Error")]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeatur =
                HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            //ViewBag.ExceptionPath = exceptionHandlerPathFeatur.Path;
            //ViewBag.ExceptionMessage = exceptionHandlerPathFeatur.Error.Message;
            //ViewBag.StackTrace = exceptionHandlerPathFeatur.Error.StackTrace;
            logger.LogError($"The path {exceptionHandlerPathFeatur.Path} " +
                            $"threw an exception {exceptionHandlerPathFeatur.Error}");
            return View("Error");
        }
    }
}
