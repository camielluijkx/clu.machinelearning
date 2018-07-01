using System.Collections.Generic;

namespace clu.machinelearning.library
{
    public class SentimentAnalysisClassificationRequest
    {
        public SentimentAnalysisClassificationType ClassificationType { get; set; }

        public List<SentimentAnalysisClassificationInput> ClassificationInput { get; set; }
    }
}
