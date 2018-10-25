using Microsoft.ML.Legacy;
using Microsoft.ML.Legacy.Data;
using Microsoft.ML.Legacy.Models;
using Microsoft.ML.Legacy.Trainers;
using Microsoft.ML.Legacy.Transforms;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace clu.machinelearning.library.regression.taxifareprediction
{
    /// <summary>
    /// Class to build, train and evaluate machine learning model for taxi fare prediction.
    /// </summary>
    public class ModelBuilder
    {
        /// <summary>
        /// Builds pipeline and trains machine learning model for taxi fare prediction.
        /// </summary>
        /// <returns>Trained prediction model for taxi fare prediction.</returns>
        public async Task<PredictionModel<DataModel, PredictionModel>> TrainAsync()
        {
            // 1) Create pipeline for machine learning.
            var pipeline = new LearningPipeline
            {
                // Load training data.
                new TextLoader(Constants.TrainingDataFileLocation).CreateFrom<DataModel>(useHeader: true, separator: ','),

                // 2) Copy values from "FareAmount" to "Label" column considered as correct values to be predicted.
                new ColumnCopier(("FareAmount", "Label")),

                // 3) Transform values of categorical data into numbers.
                new CategoricalOneHotVectorizer("VendorId", "RateCode", "PaymentType"),

                // 4) Arrange ;earner processes fields from "Features" column.
                new ColumnConcatenator("Features", "VendorId", "RateCode", "PassengerCount", "TripDistance", "PaymentType"),

                // 5) Add a learning algorithm which utilizes gradient boosting for regression problems.
                new FastTreeRegressor()
            };

            // 2) Train model based on pipeline.
            PredictionModel<DataModel, PredictionModel> trainedModel = pipeline.Train<DataModel, PredictionModel>();

            // 3) Save trained model to file.
            await trainedModel.WriteAsync(Constants.TrainingModelSaveFileLocation);

            return trainedModel;
        }

        /// <summary>
        /// Evaluates trained machine learning model for taxi fare prediction.
        /// </summary>
        /// <param name="trainedModel">Trained machine learning model for taxi fare prediction.</param>
        /// <returns>Overall metrics of trained machine learning model for taxi fare prediction.</returns>
        public RegressionMetrics Evaluate(PredictionModel<DataModel, PredictionModel> trainedModel)
        {
            // 1) Load test data.
            var testModels = new TextLoader(Constants.TestDataFileLocation).CreateFrom<DataModel>(useHeader: true, separator: ',');

            // 2) Evaluate trained model.
            var modelMetrics = new RegressionEvaluator().Evaluate(trainedModel, testModels);

            Console.WriteLine($"*************************************************");
            Console.WriteLine("Prediction model quality metrics after evaluation");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine($"Rms = {modelMetrics.Rms}");
            Console.WriteLine($"RSquared = {modelMetrics.RSquared}");
            Console.WriteLine($"*************************************************");

            return modelMetrics;
        }

        private PredictionModel Predict(PredictionModel<DataModel, PredictionModel> trainedModel, DataModel inputModel)
        {
            var outputModel = trainedModel.Predict(inputModel);

            Console.WriteLine($"Predicted taxi fare: {outputModel.FareAmount}");
            Console.WriteLine($"-------------------------------------------------");

            return outputModel;
        }

        /// <summary>
        /// Predicts taxi fare based on trained machine learning model and input.
        /// </summary>
        /// <param name="trainedModel">Trained machine learning model for taxi fare prediction.</param>
        /// <param name="inputModels">Input data for taxi fare prediction.</param>
        /// <returns>Output data of taxi fare prediction.</returns>
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
