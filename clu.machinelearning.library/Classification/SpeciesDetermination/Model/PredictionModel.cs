using Microsoft.ML.Runtime.Api;

namespace clu.machinelearning.library.classification.speciesdetermination
{
    internal class PredictionModel
    {
        /// <summary>
        /// Predicted species of iris flower.
        /// </summary>
        [ColumnName("PredictedLabel")]
        public string PredictedLabels;
    }
}
