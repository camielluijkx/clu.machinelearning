using Microsoft.ML;

using System;
using System.Threading.Tasks;

namespace clu.machinelearning.library
{
    /// <summary>
    /// Class to run iris flower prediction models. 
    /// </summary>
    public class IrisFlowerModelRunner
    {
        private static readonly Lazy<IrisFlowerModelRunner> instance =
            new Lazy<IrisFlowerModelRunner>(() => new IrisFlowerModelRunner());

        private void runPrediction(PredictionModel<IrisFlowerDataModel, IrisFlowerPredictionModel> predictionModel, IrisFlowerDataModel testData)
        {
            var prediction = predictionModel.Predict(testData);

            Console.WriteLine($"Predicted type: {prediction.PredictedLabels}");
            Console.WriteLine($"Actual type:    {testData.Label}");
            Console.WriteLine($"-------------------------------------------------");
        }

        public void RunDatasetClassification()
        {
            var modelBuilder = new IrisFlowerModelBuilder();
            var predictionModel = modelBuilder.BuildAndTrain();
            var modelAccuracy = modelBuilder.Evaluate(predictionModel);

            Console.WriteLine($"*************************************************");
            Console.WriteLine($"*      Accuracy of prediction model: {modelAccuracy * 100}%       *");
            Console.WriteLine($"*************************************************");

            modelBuilder.IrisFlowerTestData.ForEach(testData => runPrediction(predictionModel, testData));
        }

        public async Task RunDatasetClassificationAsync()
        {
            RunDatasetClassification();

            await Task.FromResult(0);
        }

        public void RunIndividualClassification()
        {
            var modelBuilder = new IrisFlowerModelBuilder();
            var predictionModel = modelBuilder.BuildAndTrain();

            var testData = new IrisFlowerDataModel
            {
                SepalLength = 3.3f,
                SepalWidth = 1.6f,
                PetalLength = 0.2f,
                PetalWidth = 5.1f,
                Label = "Iris-virginica"
            };

            runPrediction(predictionModel, testData);
        }

        private IrisFlowerModelRunner()
        {
        }

        public static IrisFlowerModelRunner Instance
        {
            get
            {
                return instance.Value;
            }
        }
    }
}
