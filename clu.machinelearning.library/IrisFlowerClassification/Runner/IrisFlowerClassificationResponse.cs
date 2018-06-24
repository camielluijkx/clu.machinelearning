using System.Collections.Generic;
    
namespace clu.machinelearning.library
{
    public class IrisFlowerClassificationResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public List<IrisFlowerClassificationOutput> ClassificationOutput { get; set; }
    }
}
