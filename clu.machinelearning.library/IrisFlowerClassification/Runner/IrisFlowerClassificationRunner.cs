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

        public IrisFlowerClassificationResponse RunClassification(IrisFlowerClassificationRequest classificationRequest)
        {
            try
            {
                var modelBuilder = new IrisFlowerModelBuilder();
                var trainedModel = modelBuilder.Train();

                if (classificationRequest.ClassificationType == IrisFlowerClassificationType.Dataset)
                {
                    if (classificationRequest.ClassificationInput != null && classificationRequest.ClassificationInput.Any())
                    {
                        throw new InvalidOperationException($"Do not provide individual input when classification type is set to dataset.");
                    }

                    var modelMetrics = modelBuilder.Evaluate(trainedModel);

                    var dataModels = getIrisFlowerTestData()
                        .Select(p => new IrisFlowerDataModel
                        {
                            SepalLength = p.SepalLength,
                            SepalWidth = p.SepalWidth,
                            PetalLength = p.PetalLength,
                            PetalWidth = p.PetalWidth
                        }).ToList();
                    var predictionModels = modelBuilder.Predict(trainedModel, dataModels);
                    var classificationOutput = predictionModels
                        .Select(p => new IrisFlowerClassificationOutput
                        {
                            PredictedSpecies = p.PredictedLabels
                        }).ToList();

                    return new IrisFlowerClassificationResponse
                    {
                        Success = true,
                        ClassificationOutput = classificationOutput
                    };
                }
                else
                {
                    if (classificationRequest.ClassificationInput == null || !classificationRequest.ClassificationInput.Any())
                    {
                        throw new InvalidOperationException($"Please provide any input when classification type is set to individual.");
                    }

                    var dataModels = classificationRequest.ClassificationInput
                        .Select(p => new IrisFlowerDataModel
                        {
                            SepalLength = p.SepalLength,
                            SepalWidth = p.SepalWidth,
                            PetalLength = p.PetalLength,
                            PetalWidth = p.PetalWidth
                        })
                        .ToList();
                    var predictionModels = modelBuilder.Predict(trainedModel, dataModels);
                    var classificationOutput = predictionModels
                        .Select(p => new IrisFlowerClassificationOutput
                        {
                            PredictedSpecies = p.PredictedLabels
                        })
                        .ToList();

                    return new IrisFlowerClassificationResponse
                    {
                        Success = true,
                        ClassificationOutput = classificationOutput
                    };
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
