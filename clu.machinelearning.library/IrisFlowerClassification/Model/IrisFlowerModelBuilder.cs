using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Models;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;

using System;
using System.Collections.Generic;

namespace clu.machinelearning.library
{
    /// <summary>
    /// Class to build, train and evaluate iris flower prediction model.
    /// </summary>
    internal class IrisFlowerModelBuilder
    {
        /// <summary>
        /// Builds pipeline and trains prediction model for iris flower classification.
        /// </summary>
        /// <returns>Trained prediction model for iris flower classification.</returns>
        public PredictionModel<IrisFlowerDataModel, IrisFlowerPredictionModel> Train()
        {
            var learningPipeline = new LearningPipeline
            {
                // 1) Load training data.
                // Create a pipeline and load training data.
                new TextLoader(IrisFlowerClassificationConstants.TrainingDataFileLocation).CreateFrom<IrisFlowerDataModel>(useHeader: true, separator: ','),

                // 2) Transform training data.
                // Assign numeric values to text in "Label" column, because only numbers can be processed during model training.
                new Dictionarizer("Label"),

                // Puts all features into a vector.
                new ColumnConcatenator("Features", "SepalLength", "SepalWidth", "PetalLength", "PetalWidth"),

                // 3) Add learner for training data.
                // Add a learning algorithm to pipeline for classification. 
                new StochasticDualCoordinateAscentClassifier(),

                // Convert text in "Label" column back into value of step 2.
                new PredictedLabelColumnOriginalValueConverter() { PredictedLabelColumn = "PredictedLabel" }
            };

            // 4) Train model based on pipeline.
            var trainedModel = learningPipeline.Train<IrisFlowerDataModel, IrisFlowerPredictionModel>();

            return trainedModel;
        }

        /// <summary>
        /// Evaluates trained prediction model for iris flower classification.
        /// </summary>
        /// <param name="trainedModel">Trained prediction model for iris flower classification.</param>
        /// <returns>Overall metrics of trained prediction model for iris flower classification.</returns>
        public ClassificationMetrics Evaluate(PredictionModel<IrisFlowerDataModel, IrisFlowerPredictionModel> trainedModel)
        {
            // 1) Load test data.
            var testModels = new TextLoader(IrisFlowerClassificationConstants.TestDataFileLocation)
                .CreateFrom<IrisFlowerDataModel>(useHeader: true, separator: ',');

            // 2) Evaluate trained model.
            var modelMetrics = new ClassificationEvaluator().Evaluate(trainedModel, testModels);

            Console.WriteLine($"*************************************************");
            Console.WriteLine($"*      Accuracy of prediction model: {modelMetrics.AccuracyMacro}%       *");
            Console.WriteLine($"*************************************************");

            return modelMetrics;
        }

        private IrisFlowerPredictionModel Predict(PredictionModel<IrisFlowerDataModel, IrisFlowerPredictionModel> trainedModel, IrisFlowerDataModel dataModel)
        {
            IrisFlowerPredictionModel predictionModel = trainedModel.Predict(dataModel);

            Console.WriteLine($"Predicted species: {predictionModel.PredictedLabels}");
            Console.WriteLine($"Actual species:    {dataModel.Label}");
            Console.WriteLine($"-------------------------------------------------");

            return predictionModel;
        }

        /// <summary>
        /// Predicts iris flower classification based trained model and input.
        /// </summary>
        /// <param name="trainedModel">Trained prediction model for iris flower classification.</param>
        /// <param name="dataModels">Input data for iris flower classification.</param>
        /// <returns>Predicition models from iris flower classification.</returns>
        public List<IrisFlowerPredictionModel> Predict(PredictionModel<IrisFlowerDataModel, IrisFlowerPredictionModel> trainedModel, List<IrisFlowerDataModel> dataModels)
        {
            var predictionModels = new List<IrisFlowerPredictionModel>();

            foreach (var dataModel in dataModels)
            {
                var predictionModel = trainedModel.Predict(dataModel);

                predictionModels.Add(predictionModel);
            }

            return predictionModels;
        }
    }
}
