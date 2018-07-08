using System.Collections.Generic;
    
namespace clu.machinelearning.library.classification.speciesdetermination
{
    public class RunnerResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public List<ModelOutput> ModelOutput { get; set; }
    }
}
