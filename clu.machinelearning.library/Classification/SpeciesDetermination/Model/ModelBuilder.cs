using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Models;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace clu.machinelearning.library.classification.speciesdetermination
{
    /// <summary>
    /// Class to build, train and evaluate machine learning model for iris flower species determination.
    /// </summary>
    internal class ModelBuilder
    {
        /// <summary>
        /// Builds pipeline and trains machine learning model for iris flower species determination.
        /// </summary>
        /// <returns>Trained prediction model for iris flower species determination.</returns>
        public async Task<PredictionModel<DataModel, PredictionModel>> TrainAsync()
        {
            // 1) Create pipeline for machine learning.
            var learningPipeline = new LearningPipeline
            {
                // Load training data.
                new TextLoader(Constants.TrainingDataFileLocation).CreateFrom<DataModel>(useHeader: true, separator: ','),

                // Assign numeric values to text in "Label" column, because only numbers can be processed during model training.
                new Dictionarizer("Label"),

                // Puts all features into a vector.
                new ColumnConcatenator("Features", "SepalLength", "SepalWidth", "PetalLength", "PetalWidth"),

                // Add a learning algorithm to pipeline for classification. 
                new StochasticDualCoordinateAscentClassifier(),

                // Convert text in "Label" column back into value of step 2.
                new PredictedLabelColumnOriginalValueConverter() { PredictedLabelColumn = "PredictedLabel" }
            };

            // 2) Train model based on pipeline.
            var trainedModel = learningPipeline.Train<DataModel, PredictionModel>();

            // 3) Save trained model to file.
            await trainedModel.WriteAsync(Constants.TrainingModelSaveFileLocation);

            return trainedModel;
        }

        /// <summary>
        /// Evaluates trained machine learning model for iris flower species determination.
        /// </summary>
        /// <param name="trainedModel">Trained machine learning model for iris flower species determination.</param>
        /// <returns>Overall metrics of trained machine learning model for iris flower species determination.</returns>
        public ClassificationMetrics Evaluate(PredictionModel<DataModel, PredictionModel> trainedModel)
        {
            // 1) Load test data.
            var testModels = new TextLoader(Constants.TestDataFileLocation)
                .CreateFrom<DataModel>(useHeader: true, separator: ',');

            // 2) Evaluate trained model.
            var modelMetrics = new ClassificationEvaluator().Evaluate(trainedModel, testModels);

            Console.WriteLine($"*************************************************");
            Console.WriteLine("Prediction model quality metrics after evaluation");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine($"*      Accuracy of prediction model: {modelMetrics.AccuracyMacro * 100}%       *");
            Console.WriteLine($"*************************************************");

            return modelMetrics;
        }

        private PredictionModel Predict(PredictionModel<DataModel, PredictionModel> trainedModel, DataModel inputModel)
        {
            var outputModel = trainedModel.Predict(inputModel);

            Console.WriteLine($"Predicted species: {outputModel.PredictedLabels}");
            Console.WriteLine($"-------------------------------------------------");

            return outputModel;
        }

        /// <summary>
        /// Predicts iris flower species based on trained machine learning model and input.
        /// </summary>
        /// <param name="trainedModel">Trained machine learning model for iris flower species predicition.</param>
        /// <param name="inputModels">Input data for iris flower species prediction.</param>
        /// <returns>Output data of iris flower species prediction.</returns>
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
