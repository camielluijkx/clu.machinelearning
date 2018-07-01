using Microsoft.ML;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace clu.machinelearning.library
{
    public class SentimentAnalysisClassificationRunner
    {
        private static readonly Lazy<SentimentAnalysisClassificationRunner> instance =
            new Lazy<SentimentAnalysisClassificationRunner>(() => new SentimentAnalysisClassificationRunner());

        private IEnumerable<SentimentAnalysisDataModel> getSentimentAnalysisTestData()
        {
            return File.ReadAllLines(SentimentAnalysisClassificationConstants.TestDataFileLocation)
                .Skip(1)
                .Select(x => x.Split('\t'))
                .Select(x => new SentimentAnalysisDataModel
                {
                    Sentiment = float.Parse(x[0]),
                    SentimentText = x[1]
                });
        }

        private SentimentAnalysisPredictionModel runPrediction(PredictionModel<SentimentAnalysisDataModel, SentimentAnalysisPredictionModel> predictionModel, SentimentAnalysisDataModel dataModel)
        {
            var prediction = predictionModel.Predict(dataModel);

            Console.WriteLine($"Predicted sentiment: {prediction.Sentiment}");
            Console.WriteLine($"Actual sentiment:    {dataModel.Sentiment}");
            Console.WriteLine($"-------------------------------------------------");

            return prediction;
        }

        private SentimentAnalysisClassificationOutput runClassification(PredictionModel<SentimentAnalysisDataModel, SentimentAnalysisPredictionModel> predictionModel, SentimentAnalysisClassificationInput classificationInput)
        {
            if (classificationInput.Id == Guid.Empty)
            {
                classificationInput.Id = Guid.NewGuid();
            }

            var dataModel = new SentimentAnalysisDataModel
            {
                Sentiment = classificationInput.ActualSentiment,
                SentimentText = classificationInput.TextForAnalysis
            };

            var prediction = runPrediction(predictionModel, dataModel);

            return new SentimentAnalysisClassificationOutput
            {
                Id = classificationInput.Id,
                PredictedSentiment = prediction.Sentiment
            };
        }

        private SentimentAnalysisClassificationResponse runDatasetClassification(
            PredictionModel<SentimentAnalysisDataModel, SentimentAnalysisPredictionModel> predictionModel)
        {
            var classificationInput = getSentimentAnalysisTestData()
                .Select(p => new SentimentAnalysisClassificationInput
                {
                    Id = Guid.NewGuid(),
                    ActualSentiment = p.Sentiment,
                    TextForAnalysis = p.SentimentText
                })
                .ToList();

            var classificationOutput = new List<SentimentAnalysisClassificationOutput>();
            classificationInput.ForEach(ci => classificationOutput.Add(runClassification(predictionModel, ci)));

            var classificationResponse = new SentimentAnalysisClassificationResponse
            {
                Success = true,
                ClassificationOutput = classificationOutput
            };

            return classificationResponse;
        }

        private SentimentAnalysisClassificationResponse runIndividualClassification(PredictionModel<SentimentAnalysisDataModel, SentimentAnalysisPredictionModel> predictionModel, List<SentimentAnalysisClassificationInput> classificationInput)
        {
            var classificationOutput = new List<SentimentAnalysisClassificationOutput>();

            classificationInput.ForEach(ci => classificationOutput.Add(runClassification(predictionModel, ci)));

            var classificationResponse = new SentimentAnalysisClassificationResponse
            {
                Success = true,
                ClassificationOutput = classificationOutput
            };

            return classificationResponse;
        }

        public async Task<SentimentAnalysisClassificationResponse> RunClassificationAsync(SentimentAnalysisClassificationRequest classificationRequest)
        {
            try
            {
                var classificationModel = new SentimentAnalysisModelBuilder();
                var predictionModel = await classificationModel.TrainAsync();

                if (classificationRequest.ClassificationType == SentimentAnalysisClassificationType.Dataset)
                {
                    if (classificationRequest.ClassificationInput != null && classificationRequest.ClassificationInput.Any())
                    {
                        throw new InvalidOperationException($"Do not provide individual input when classification type is set to dataset.");
                    }

                    var classificationMetrics = classificationModel.Evaluate(predictionModel);

                    Console.WriteLine($"*************************************************");
                    Console.WriteLine("Prediction model quality metrics evaluation");
                    Console.WriteLine("------------------------------------------");
                    Console.WriteLine($"Accuracy: {classificationMetrics.Accuracy:P2}");
                    Console.WriteLine($"Auc: {classificationMetrics.Auc:P2}");
                    Console.WriteLine($"F1Score: {classificationMetrics.F1Score:P2}");
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
                return new SentimentAnalysisClassificationResponse
                {
                    Success = false,
                    Message = ex.ToExceptionMessage()
                };
            }
        }

        private SentimentAnalysisClassificationRunner()
        {
        }

        public static SentimentAnalysisClassificationRunner Instance
        {
            get
            {
                return instance.Value;
            }
        }
    }
}
