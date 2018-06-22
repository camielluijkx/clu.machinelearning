using Microsoft.ML;

using System;
using System.Threading.Tasks;

namespace clu.machinelearning.library
{
    /// <summary>
    /// Class to run iris flower prediction models. 
    /// </summary>
    public class IrisFlowerModelRunner // [TODO] refactor models
    {
        private static readonly Lazy<IrisFlowerModelRunner> instance =
            new Lazy<IrisFlowerModelRunner>(() => new IrisFlowerModelRunner());

        private IrisFlowerPredictionModel runPrediction(PredictionModel<IrisFlowerDataModel, IrisFlowerPredictionModel> predictionModel, IrisFlowerDataModel testData)
        {
            var prediction = predictionModel.Predict(testData);

            Console.WriteLine($"Predicted type: {prediction.PredictedLabels}");
            Console.WriteLine($"Actual type:    {testData.Label}");
            Console.WriteLine($"-------------------------------------------------");

            return prediction;
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

        public IrisFlowerClassificationResponse RunIndividualClassification(IrisFlowerClassificationRequest classificationRequest)
        {
            var modelBuilder = new IrisFlowerModelBuilder();
            var predictionModel = modelBuilder.BuildAndTrain();

            var testData = new IrisFlowerDataModel
            {
                SepalLength = classificationRequest.SepalLength,
                SepalWidth = classificationRequest.SepalWidth,
                PetalLength = classificationRequest.PetalLength,
                PetalWidth = classificationRequest.PetalWidth
            };

            var prediction = runPrediction(predictionModel, testData);

            var classificationResponse = new IrisFlowerClassificationResponse
            {
                Species = prediction.PredictedLabels
            };

            return classificationResponse;
        }

        public async Task<IrisFlowerClassificationResponse> RunIndividualClassificationAsync(IrisFlowerClassificationRequest classificationRequest)
        {
            var classificationResponse = RunIndividualClassification(classificationRequest);

            return await Task.FromResult(classificationResponse);
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
