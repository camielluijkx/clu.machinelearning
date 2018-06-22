using clu.machinelearning.library;

using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace clu.machinelearning.api
{
    [Route("api/[controller]")]
    public class ClassificationController : Controller // [TODO] api documentation
    {
        /// <summary>
        /// Runs iris flower classification based on properties and returns prediction result.
        /// </summary>
        /// <param name="classificationRequest">Properties of iris flower needed for classification.</param>
        /// <remarks>Uses ML.NET to predict iris flower classification.</remarks>
        /// <returns>Iris flower classification result.</returns>
        /// <response code="200">Classification is predicted and returned.</response>
        /// <response code="500">Error occurred during classification.</response>
        [HttpPost]
        [Route("IrisFlower")]
        [ProducesResponseType(typeof(IrisFlowerClassificationResponse), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RunClassificationAsync([FromBody] IrisFlowerClassificationRequest classificationRequest)
        {
            var classificationResponse = await IrisFlowerModelRunner.Instance.RunIndividualClassificationAsync(classificationRequest);

            return Ok(classificationResponse);
        }
    }
}
