using clu.machinelearning.library;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Reflection;

namespace clu.machinelearning.api
{
    [AllowAnonymous]
    [Route("")]
    public class RootController : RootApiController
    {
        public RootController()
            : base(Assembly.GetExecutingAssembly())
        {

        }

        /// <summary>
        /// Returns API version.
        /// </summary>
        /// <returns>API version.</returns>
        [Route("")]
        [HttpGet]
        public override IActionResult Get()
        {
            return base.Get();
        }
    }
}