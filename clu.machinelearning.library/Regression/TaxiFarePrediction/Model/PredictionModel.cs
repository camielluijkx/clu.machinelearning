using Microsoft.ML.Runtime.Api;

namespace clu.machinelearning.library.regression.taxifareprediction
{
    public class PredictionModel
    {
        /// <summary>
        /// Predicted taxi fare.
        /// </summary>
        [ColumnName("Score")]
        public float FareAmount;
    }
}
