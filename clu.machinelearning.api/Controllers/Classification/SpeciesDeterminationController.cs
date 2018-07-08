using clu.machinelearning.library.classification.speciesdetermination;

using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace clu.machinelearning.api
{
    [Route("api/Classification/SpeciesDetermination")]
    public class SpeciesDeterminationController : Controller
    {
        /// <summary>
        /// Runs iris flower species determination based on input and returns predicted output.
        /// </summary>
        /// <param name="modelInput">Input needed for iris flower species determination.</param>
        /// <remarks>Uses ML.NET to predict iris flower species based on input an trained model.</remarks>
        /// <returns>Iris flower species prediction result.</returns>
        /// <response code="200">Iris flower species prediction is returned.</response>
        /// <response code="500">Error occurred during prediction of iris flower species.</response>
        [HttpPost]
        [Route("IrisFlower")]
        [ProducesResponseType(typeof(ModelOutput), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RunClassificationAsync([FromBody] ModelInput modelInput)
        {
            var runnerRequest = new RunnerRequest
            {
                ModelInput = new List<ModelInput>() { modelInput }
            };

            var runnerResponse = await ModelRunner.Instance.RunClassificationAsync(runnerRequest);
            if (!runnerResponse.Success)
            {
                return StatusCode(500, runnerResponse.Message);
            }
            return Ok(runnerResponse);
        }
    }
}
