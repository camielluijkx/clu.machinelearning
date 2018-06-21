using System;

namespace clu.machinelearning.irisclassification
{
    public class IrisFlowerModelRunner
    {
        private static readonly Lazy<IrisFlowerModelRunner> instance =
            new Lazy<IrisFlowerModelRunner>(() => new IrisFlowerModelRunner());

        public void RunDatasetClassification()
        {
            var modelBuilder = new IrisFlowerModelBuilder();
            var predictionModel = modelBuilder.BuildAndTrain();
            var modelAccuracy = modelBuilder.Evaluate(predictionModel);

            Console.WriteLine($"*************************************************");
            Console.WriteLine($"*      Accuracy of prediction model: {modelAccuracy * 100}%       *");
            Console.WriteLine($"*************************************************");

            var irisFlowerTestData = modelBuilder.GetIrisFlowerTestData();
            foreach (var irisFlowerData in irisFlowerTestData)
            {
                var prediction = predictionModel.Predict(irisFlowerData);

                Console.WriteLine($"Predicted type: {prediction.PredictedLabels}");
                Console.WriteLine($"Actual type:    {irisFlowerData.Label}");
                Console.WriteLine($"-------------------------------------------------");
            }
        }

        public void RunIndividualClassification()
        {
            var modelBuilder = new IrisFlowerModelBuilder();
            var predictionModel = modelBuilder.BuildAndTrain();

            var irisFlowerData = new IrisFlowerDataModel
            {
                SepalLength = 3.3f,
                SepalWidth = 1.6f,
                PetalLength = 0.2f,
                PetalWidth = 5.1f,
                Label = "Iris-virginica"
            };

            var prediction = predictionModel.Predict(irisFlowerData);

            Console.WriteLine($"Predicted type : {prediction.PredictedLabels}");
            Console.WriteLine($"Actual type :    {irisFlowerData.Label}");
            Console.WriteLine($"-------------------------------------------------");
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
