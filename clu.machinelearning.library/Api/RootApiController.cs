using Microsoft.AspNetCore.Mvc;

using System;
using System.Reflection;

namespace clu.machinelearning.library.api
{
    public abstract class RootApiController : Controller
    {
        private readonly Assembly exposingAssembly;

        protected RootApiController()
        {
            exposingAssembly = Assembly.GetCallingAssembly();
        }

        protected RootApiController(Assembly exposingAssembly)
        {
            this.exposingAssembly = exposingAssembly;
        }

        public virtual IActionResult Get()
        {
            string applicationName = exposingAssembly.GetName().Name;
            string environmentName = Environment.MachineName;
            string buildnumber = exposingAssembly.GetName().Version.ToString();

            string version = $"{applicationName} {environmentName} - Build {buildnumber}";

            return Ok(version);
        }
    }
}
