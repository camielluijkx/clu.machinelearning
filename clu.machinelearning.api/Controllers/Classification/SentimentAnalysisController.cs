using clu.machinelearning.library.classification.sentimentanalysis;

using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace clu.machinelearning.api
{
    [Route("api/Classification/SentimentAnalysis")]
    public class ClassificationController : Controller
    {
        /// <summary>
        /// Runs text sentiment analysis based on input and returns predicted output.
        /// </summary>
        /// <param name="modelInput">Model input needed for text sentiment analysis.</param>
        /// <remarks>Uses ML.NET to predict positive or negative sentiment based on input and trained model.</remarks>
        /// <returns>Text sentiment prediction result.</returns>
        /// <response code="200">Text sentiment prediction is returned.</response>
        /// <response code="500">Error occurred during prediction of text sentiment.</response>
        [HttpPost]
        [Route("Text")]
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
