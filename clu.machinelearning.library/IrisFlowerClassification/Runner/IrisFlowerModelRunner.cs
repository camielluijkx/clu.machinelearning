using Microsoft.ML;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        /// <summary>
        /// Location of training data file.
        /// </summary>
        private const string TrainingDataFileLocation = @"D:\Workspace\clu.machinelearning\clu.machinelearning.library\IrisFlowerClassification\Data\iris-data_training.csv"; // [TODO] load from datasource, create internal datafile

        private IEnumerable<IrisFlowerDataModel> getIrisFlowerDataFromCsv(string dataFileLocation)
        {
            return File.ReadAllLines(dataFileLocation)
                .Skip(1)
                .Select(x => x.Split(','))
                .Select(x => new IrisFlowerDataModel
                {
                    SepalLength = float.Parse(x[0]),
                    SepalWidth = float.Parse(x[1]),
                    PetalLength = float.Parse(x[2]),
                    PetalWidth = float.Parse(x[3]),
                    Label = x[4]
                });
        }

        private IrisFlowerPredictionModel runPrediction(PredictionModel<IrisFlowerDataModel, IrisFlowerPredictionModel> predictionModel, IrisFlowerDataModel dataModel)
        {
            var prediction = predictionModel.Predict(dataModel);

            Console.WriteLine($"Predicted species: {prediction.PredictedLabels}");
            Console.WriteLine($"Actual species:    {dataModel.Label}");
            Console.WriteLine($"-------------------------------------------------");

            return prediction;
        }

        private IrisFlowerClassificationOutput runClassification(PredictionModel<IrisFlowerDataModel, IrisFlowerPredictionModel> predictionModel, IrisFlowerClassificationInput classificationInput)
        {
            if (classificationInput.Id == Guid.Empty)
            {
                classificationInput.Id = Guid.NewGuid();
            }

            var dataModel = new IrisFlowerDataModel
            {
                SepalLength = classificationInput.SepalLength,
                SepalWidth = classificationInput.SepalWidth,
                PetalLength = classificationInput.PetalLength,
                PetalWidth = classificationInput.PetalWidth,
                Label = classificationInput.ActualSpecies
            };

            var prediction = runPrediction(predictionModel, dataModel);

            return new IrisFlowerClassificationOutput
            {
                Id = classificationInput.Id,
                PredictedSpecies = prediction.PredictedLabels
            };
        }

        private IrisFlowerClassificationResponse runDatasetClassification(
            PredictionModel<IrisFlowerDataModel, IrisFlowerPredictionModel> predictionModel, string inputDataFileLocation)
        {
            var inputData = getIrisFlowerDataFromCsv(inputDataFileLocation);
            var classificationInput = inputData
                .Select(p => new IrisFlowerClassificationInput
                {
                    Id = Guid.NewGuid(),
                    SepalLength = p.SepalLength,
                    SepalWidth = p.SepalWidth,
                    PetalLength = p.PetalLength,
                    PetalWidth = p.PetalWidth,
                    ActualSpecies = p.Label
                })
                .ToList();

            var classificationOutput = new List<IrisFlowerClassificationOutput>();
            classificationInput.ForEach(ci => classificationOutput.Add(runClassification(predictionModel, ci)));

            var classificationResponse = new IrisFlowerClassificationResponse
            {
                Success = true,
                ClassificationOutput = classificationOutput
            };

            return classificationResponse;
        }

        private IrisFlowerClassificationResponse runIndividualClassification(PredictionModel<IrisFlowerDataModel, IrisFlowerPredictionModel> predictionModel, List<IrisFlowerClassificationInput> classificationInput)
        {
            var classificationOutput = new List<IrisFlowerClassificationOutput>();

            classificationInput.ForEach(ci => classificationOutput.Add(runClassification(predictionModel, ci)));

            var classificationResponse = new IrisFlowerClassificationResponse
            {
                Success = true,
                ClassificationOutput = classificationOutput
            };

            return classificationResponse;
        }

        public IrisFlowerClassificationResponse RunClassification(IrisFlowerClassificationRequest classificationRequest)
        {
            try
            {
                var modelBuilder = new IrisFlowerModelBuilder();
                var predictionModel = modelBuilder.BuildAndTrain(TrainingDataFileLocation);

                if (classificationRequest.ClassificationType == IrisFlowerClassificationType.Dataset)
                {
                    if (string.IsNullOrEmpty(classificationRequest.ClassificationInputFileLocation))
                    {
                        throw new ArgumentNullException(nameof(classificationRequest.ClassificationInputFileLocation));
                    }

                    if (classificationRequest.ClassificationInput != null && classificationRequest.ClassificationInput.Any())
                    {
                        throw new InvalidOperationException($"Do not provide individual input when classification type is set to dataset. Provide a classification input file location instead.");
                    }

                    var inputDataFileLocation = classificationRequest.ClassificationInputFileLocation;
                    var modelAccuracy = modelBuilder.Evaluate(predictionModel, inputDataFileLocation);

                    Console.WriteLine($"*************************************************");
                    Console.WriteLine($"*      Accuracy of prediction model: {modelAccuracy * 100}%       *");
                    Console.WriteLine($"*************************************************");

                    return runDatasetClassification(predictionModel, inputDataFileLocation);
                }
                else
                {
                    return runIndividualClassification(predictionModel, classificationRequest.ClassificationInput);
                }
            }
            catch (Exception ex)
            {
                return new IrisFlowerClassificationResponse
                {
                    Success = false,
                    Message = ex.ToExceptionMessage()
                };
            }
        }

        public async Task<IrisFlowerClassificationResponse> RunIndividualClassificationAsync(IrisFlowerClassificationRequest classificationRequest)
        {
            var classificationResponse = RunClassification(classificationRequest);

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
