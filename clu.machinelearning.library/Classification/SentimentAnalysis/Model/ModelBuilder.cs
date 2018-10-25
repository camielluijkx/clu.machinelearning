using Microsoft.ML.Legacy;
using Microsoft.ML.Legacy.Data;
using Microsoft.ML.Legacy.Models;
using Microsoft.ML.Legacy.Trainers;
using Microsoft.ML.Legacy.Transforms;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace clu.machinelearning.library.classification.sentimentanalysis
{
    /// <summary>
    /// Class to build, train and evaluate machine learning model for text sentiment analysis.
    /// </summary>
    public class ModelBuilder
    {
        /// <summary>
        /// Builds pipeline and trains machine learning model for text sentiment analysis.
        /// </summary>
        /// <returns>Trained prediction model for text sentiment analysis.</returns>
        public async Task<PredictionModel<DataModel, PredictionModel>> TrainAsync()
        {
            // 1) Create pipeline for machine learning.
            var learningPipeline = new LearningPipeline
            {
                // Load training data.
                new TextLoader(Constants.TrainingDataFileLocation).CreateFrom<DataModel>(),

                // Convert "SentimentText" column into a numeric vector called Features used by machine learning algorithm.
                new TextFeaturizer("Features", "SentimentText"),

                // Add a learning algorithm to pipeline for classification. 
                new FastTreeBinaryClassifier() { NumLeaves = 5, NumTrees = 5, MinDocumentsInLeafs = 2 }
            };

            // 2) Train model based on pipeline.
            var trainedModel = learningPipeline.Train<DataModel, PredictionModel>();

            // 3) Save trained model to file.
            await trainedModel.WriteAsync(Constants.TrainingModelSaveFileLocation);

            return trainedModel;
        }

        /// <summary>
        /// Evaluates trained machine learning model for text sentiment analysis.
        /// </summary>
        /// <param name="trainedModel">Trained machine learning model for text sentiment analysis.</param>
        /// <returns>Overall metrics of trained machine learning model for text sentiment analysis.</returns>
        public BinaryClassificationMetrics Evaluate(PredictionModel<DataModel, PredictionModel> trainedModel)
        {
            // 1) Load test data.
            var testModels = new TextLoader(Constants.TestDataFileLocation).CreateFrom<DataModel>();

            // 2) Evaluate trained model.
            var modelMetrics = new BinaryClassificationEvaluator().Evaluate(trainedModel, testModels);

            Console.WriteLine($"*************************************************");
            Console.WriteLine("Prediction model quality metrics after evaluation");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine($"Accuracy: {modelMetrics.Accuracy:P2}");
            Console.WriteLine($"Auc: {modelMetrics.Auc:P2}");
            Console.WriteLine($"F1Score: {modelMetrics.F1Score:P2}");
            Console.WriteLine($"*************************************************");

            return modelMetrics;
        }

        private PredictionModel Predict(PredictionModel<DataModel, PredictionModel> trainedModel, DataModel inputModel)
        {
            var outputModel = trainedModel.Predict(inputModel);

            Console.WriteLine($"Predicted sentiment: {outputModel.Sentiment}");
            Console.WriteLine($"-------------------------------------------------");

            return outputModel;
        }

        /// <summary>
        /// Predicts text sentiment based on trained machine learning model and input.
        /// </summary>
        /// <param name="trainedModel">Trained machine learning model for text sentiment analysis.</param>
        /// <param name="inputModels">Input data for text sentiment analysis.</param>
        /// <returns>Output data of text sentiment analysis.</returns>
        public List<PredictionModel> Predict(PredictionModel<DataModel, PredictionModel> trainedModel, List<DataModel> inputModels)
        {
            var outputModels = new List<PredictionModel>();

            foreach (var inputModel in inputModels)
            {
                var outputModel = Predict(trainedModel, inputModel);

                outputModels.Add(outputModel);
            }

            return outputModels;
        }
    }
}
