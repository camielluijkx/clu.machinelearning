using clu.machinelearning.library.extensions;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace clu.machinelearning.library.classification.sentimentanalysis
{
    /// <summary>
    /// Class to run text sentiment analysis.
    /// </summary>
    public class ModelRunner
    {
        private static readonly Lazy<ModelRunner> instance =
            new Lazy<ModelRunner>(() => new ModelRunner());

        private IEnumerable<DataModel> getSentimentAnalysisFileData(string dataFileLocation)
        {
            return File.ReadAllLines(dataFileLocation)
                .Skip(1)
                .Select(x => x.Split('\t'))
                .Select(x => new DataModel
                {
                    Sentiment = float.Parse(x[0]),
                    SentimentText = x[1]
                });
        }

        /// <summary>
        /// Uses ML.NET to predict positive or negative sentiment based on input and trained model.
        /// </summary>
        /// <param name="runnerRequest">Request with input needed for text sentiment analysis.</param>
        /// <returns>Text sentiment prediction result.</returns>
        public async Task<RunnerResponse> RunClassificationAsync(RunnerRequest runnerRequest)
        {
            try
            {
                var modelBuilder = new ModelBuilder();
                var trainedModel = await modelBuilder.TrainAsync();
                var modelMetrics = modelBuilder.Evaluate(trainedModel);

                var modelInput = runnerRequest.ModelInput
                    .Select(p => new DataModel
                    {
                        SentimentText = p.TextForAnalysis
                    })
                    .ToList();
                var modelOutput = modelBuilder.Predict(trainedModel, modelInput)                    
                    .Select(p => new ModelOutput
                    {
                        PredictedSentiment = p.Sentiment
                    })
                    .ToList();

                return new RunnerResponse
                {
                    Success = true,
                    ModelOutput = modelOutput
                };
            }
            catch (Exception ex)
            {
                return new RunnerResponse
                {
                    Success = false,
                    Message = ex.ToExceptionMessage()
                };
            }
        }

        private ModelRunner()
        {
        }

        /// <summary>
        /// Instance of the <see cref="ModelRunner"/> being used.
        /// </summary>
        public static ModelRunner Instance
        {
            get
            {
                return instance.Value;
            }
        }
    }
}
