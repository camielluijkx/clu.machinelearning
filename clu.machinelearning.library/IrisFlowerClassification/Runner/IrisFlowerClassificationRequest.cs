using System.Collections.Generic;

namespace clu.machinelearning.library
{
    public class IrisFlowerClassificationRequest
    {
        public IrisFlowerClassificationType ClassificationType { get; set; }

        public List<IrisFlowerClassificationInput> ClassificationInput { get; set; }
    }
}
