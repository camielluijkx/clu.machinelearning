using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Models;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace clu.machinelearning.library
{
    public class SentimentAnalysisModelBuilder
    {
        /// <summary>
        /// Builds pipeline and trains prediction model for sentiment analysis classification.
        /// </summary>
        /// <returns>Trained prediction model for sentiment analysis classification.</returns>
        public async Task<PredictionModel<SentimentAnalysisDataModel, SentimentAnalysisPredictionModel>> TrainAsync()
        {
            var learningPipeline = new LearningPipeline
            {
                // 1) Load training data.
                // Create a pipeline and load training data.
                new TextLoader(SentimentAnalysisClassificationConstants.TrainingDataFileLocation).CreateFrom<SentimentAnalysisDataModel>(),

                // 2) Transform training data.
                // Convert "SentimentText" column into a numeric vector called Features used by machine learning algorithm.
                new TextFeaturizer("Features", "SentimentText"),

                // 3) Add learner for training data.
                // Add a learning algorithm to pipeline for classification. 
                new FastTreeBinaryClassifier() { NumLeaves = 5, NumTrees = 5, MinDocumentsInLeafs = 2 }
            };

            // 4) Train model based on pipeline.
            var trainedModel = learningPipeline.Train<SentimentAnalysisDataModel, SentimentAnalysisPredictionModel>();

            // Save trained model to file.
            await trainedModel.WriteAsync(SentimentAnalysisClassificationConstants.TrainingModelSaveFileLocation);

            return trainedModel;
        }

        /// <summary>
        /// Evaluates trained prediction model for sentiment analysis classification.
        /// </summary>
        /// <param name="trainedModel">Trained prediction model for sentiment analysis classification.</param>
        /// <returns>Overall metrics of trained prediction model for sentiment analysis classification.</returns>
        public BinaryClassificationMetrics Evaluate(PredictionModel<SentimentAnalysisDataModel, SentimentAnalysisPredictionModel> trainedModel)
        {
            // 1) Load test data.
            var testModels = new TextLoader(SentimentAnalysisClassificationConstants.TestDataFileLocation).CreateFrom<SentimentAnalysisDataModel>();

            // 2) Evaluate trained model.
            var modelMetrics = new BinaryClassificationEvaluator().Evaluate(trainedModel, testModels);

            return modelMetrics;
        }

        /// <summary>
        /// Predicts sentiment analysis classification based trained model and input.
        /// </summary>
        /// <param name="trainedModel">Trained prediction model for sentiment analysis classification.</param>
        /// <param name="dataModels">Input data for sentiment analysis classification.</param>
        /// <returns>Predicition models from sentiment analysis classification.</returns>
        public IEnumerable<SentimentAnalysisPredictionModel> Predict(PredictionModel<SentimentAnalysisDataModel, SentimentAnalysisPredictionModel> trainedModel, IEnumerable<SentimentAnalysisDataModel> dataModels)
        {
            IEnumerable<SentimentAnalysisPredictionModel> predictionModels = trainedModel.Predict(dataModels);

            return predictionModels;
        }
    }
}
