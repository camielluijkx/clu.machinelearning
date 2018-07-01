using clu.machinelearning.library;

using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace clu.machinelearning.api
{
    [Route("api/[controller]")]
    public class ClassificationController : Controller
    {
        /// <summary>
        /// Runs iris flower classification based on properties and returns prediction result.
        /// </summary>
        /// <param name="classificationInput">Input needed for iris flower classification.</param>
        /// <remarks>Uses ML.NET to predict iris flower species based on input.</remarks>
        /// <returns>Iris flower classification result.</returns>
        /// <response code="200">Classification is predicted and returned.</response>
        /// <response code="500">Error occurred during classification.</response>
        [HttpPost]
        [Route("IrisFlower")]
        [ProducesResponseType(typeof(IrisFlowerClassificationOutput), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RunClassificationAsync([FromBody] IrisFlowerClassificationInput classificationInput)
        {
            var classificationRequest = new IrisFlowerClassificationRequest
            {
                ClassificationInput = new List<IrisFlowerClassificationInput>() { classificationInput }
            };

            var classificationResponse = await IrisFlowerClassificationRunner.Instance.RunClassificationAsync(classificationRequest);
            if (!classificationResponse.Success)
            {
                return StatusCode(500, classificationResponse.Message);
            }
            return Ok(classificationResponse);
        }
    }
}
