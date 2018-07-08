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

        private IEnumerable<SentimentAnalysisDataModel> getSentimentAnalysisFileData(string dataFileLocation)
        {
            return File.ReadAllLines(dataFileLocation)
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
                var modelMetrics = modelBuilder.Evaluate(trainedModel);

                var inputModels = classificationRequest.ClassificationInput
                    .Select(p => new SentimentAnalysisDataModel
                    {
                        SentimentText = p.TextForAnalysis
                    })
                    .ToList();
                var outputModels = modelBuilder.Predict(trainedModel, inputModels);
                var classificationOutput = outputModels
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
