using clu.machinelearning.library;

using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace clu.machinelearning.api
{
    [Route("api/[controller]")]
    public class ClassificationController : Controller
    {
        // GET api/classification/IrisFlower/Dataset
        [HttpGet]
        [Route("IrisFlower/Dataset")]
        public async Task<IActionResult> RunDatasetClassificationAsync()
        {
            await IrisFlowerModelRunner.Instance.RunDatasetClassificationAsync();

            return Ok();
        }
    }
}
