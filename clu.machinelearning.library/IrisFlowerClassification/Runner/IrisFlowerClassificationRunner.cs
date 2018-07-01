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
    public class IrisFlowerClassificationRunner
    {
        private static readonly Lazy<IrisFlowerClassificationRunner> instance =
            new Lazy<IrisFlowerClassificationRunner>(() => new IrisFlowerClassificationRunner());

        private IEnumerable<IrisFlowerDataModel> getIrisFlowerTestData()
        {
            return File.ReadAllLines(IrisFlowerClassificationConstants.TestDataFileLocation)
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
            PredictionModel<IrisFlowerDataModel, IrisFlowerPredictionModel> predictionModel)
        {
            var classificationInput = getIrisFlowerTestData()
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
                var classificationModel = new IrisFlowerModelBuilder();
                var predictionModel = classificationModel.Train();

                if (classificationRequest.ClassificationType == IrisFlowerClassificationType.Dataset)
                {
                    if (classificationRequest.ClassificationInput != null && classificationRequest.ClassificationInput.Any())
                    {
                        throw new InvalidOperationException($"Do not provide individual input when classification type is set to dataset.");
                    }

                    var classificationMetrics = classificationModel.Evaluate(predictionModel);

                    Console.WriteLine($"*************************************************");
                    Console.WriteLine($"*      Accuracy of prediction model: {classificationMetrics.AccuracyMacro}%       *");
                    Console.WriteLine($"*************************************************");

                    return runDatasetClassification(predictionModel);
                }
                else
                {
                    if (classificationRequest.ClassificationInput == null || !classificationRequest.ClassificationInput.Any())
                    {
                        throw new InvalidOperationException($"Please provide any input when classification type is set to individual.");
                    }

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

        public async Task<IrisFlowerClassificationResponse> RunClassificationAsync(IrisFlowerClassificationRequest classificationRequest)
        {
            var classificationResponse = RunClassification(classificationRequest);

            return await Task.FromResult(classificationResponse);
        }

        private IrisFlowerClassificationRunner()
        {
        }

        public static IrisFlowerClassificationRunner Instance
        {
            get
            {
                return instance.Value;
            }
        }
    }
}
