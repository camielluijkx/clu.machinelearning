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

        public async Task<SentimentAnalysisClassificationResponse> RunClassificationAsync(SentimentAnalysisClassificationRequest classificationRequest)
        {
            try
            {
                var modelBuilder = new SentimentAnalysisModelBuilder();
                var trainedModel = await modelBuilder.TrainAsync();

                if (classificationRequest.ClassificationType == SentimentAnalysisClassificationType.Dataset)
                {
                    if (classificationRequest.ClassificationInput != null && classificationRequest.ClassificationInput.Any())
                    {
                        throw new InvalidOperationException($"Do not provide individual input when classification type is set to dataset.");
                    }

                    var modelMetrics = modelBuilder.Evaluate(trainedModel);

                    var dataModels = getSentimentAnalysisTestData()
                        .Select(p => new SentimentAnalysisDataModel
                        {
                            SentimentText = p.SentimentText
                        })
                        .ToList();
                    var predictionModels = modelBuilder.Predict(trainedModel, dataModels);
                    var classificationOutput = predictionModels
                        .Select(p => new SentimentAnalysisClassificationOutput
                        {
                            PredictedSentiment = p.Sentiment
                        })
                        .ToList();

                    return new SentimentAnalysisClassificationResponse
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
                        .Select(p => new SentimentAnalysisDataModel
                        {
                            SentimentText = p.TextForAnalysis
                        })
                        .ToList();
                    var predictionModels = modelBuilder.Predict(trainedModel, dataModels);
                    var classificationOutput = predictionModels
                        .Select(p => new SentimentAnalysisClassificationOutput
                        {
                            PredictedSentiment = p.Sentiment
                        })
                        .ToList();

                    return new SentimentAnalysisClassificationResponse
                    {
                        Success = true,
                        ClassificationOutput = classificationOutput
                    };
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
