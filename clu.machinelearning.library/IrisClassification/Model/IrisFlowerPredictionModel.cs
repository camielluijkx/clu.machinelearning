using Microsoft.ML.Runtime.Api;

namespace clu.machinelearning.library
{
    /// <summary>
    /// Class to represent result returned from iris flower prediction.
    /// </summary>
    public class IrisFlowerPredictionModel
    {
        /// <summary>
        /// Predicted iris flower species.
        /// </summary>
        [ColumnName("PredictedLabel")]
        public string PredictedLabels;
    }
}
