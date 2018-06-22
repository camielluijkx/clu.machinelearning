using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Models;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace clu.machinelearning.library
{
    /// <summary>
    /// Class to build, train and evaluate iris flower prediction model.
    /// </summary>
    public class IrisFlowerModelBuilder // [TODO] application settings
    {
        /// <summary>
        /// Location of training data file.
        /// </summary>
        private const string TrainingDataFileLocation = @"D:\Workspace\clu.machinelearning\clu.machinelearning.library\IrisFlowerClassification\Data\iris-data_training.csv";

        /// <summary>
        /// Location of test data file.
        /// </summary>
        private const string TestDataFileLocation = @"D:\Workspace\clu.machinelearning\clu.machinelearning.library\IrisFlowerClassification\Data\iris-data_test.csv";

        private List<IrisFlowerDataModel> getIrisFlowerDataFromCsv(string dataFileLocation)
        {
            return File.ReadAllLines(TestDataFileLocation)
                .Skip(1)
                .Select(x => x.Split(','))
                .Select(x => new IrisFlowerDataModel
                {
                    SepalLength = float.Parse(x[0]),
                    SepalWidth = float.Parse(x[1]),
                    PetalLength = float.Parse(x[2]),
                    PetalWidth = float.Parse(x[3]),
                    Label = x[4]
                })
                .ToList();
        }

        /// <summary>
        /// Returns iris flower data from test file.
        /// </summary>
        /// <returns>Iris flower data from test file.</returns>
        public List<IrisFlowerDataModel> IrisFlowerTestData
        {
            get
            {
                return getIrisFlowerDataFromCsv(TestDataFileLocation);
            }
        }

        /// <summary>
        /// Returns iris flower data from training file.
        /// </summary>
        public List<IrisFlowerDataModel> GetIrisFlowerTrainingData
        {
            get
            {
                return getIrisFlowerDataFromCsv(TrainingDataFileLocation);
            }
        }

        /// <summary>
        /// Builds pipeline and trains prediction model for iris flower classification.
        /// </summary>
        /// <returns>Trained prediction model for iris flower classification.</returns>
        public PredictionModel<IrisFlowerDataModel, IrisFlowerPredictionModel> BuildAndTrain()
        {
            try
            {
                var pipeline = new LearningPipeline
                {
                    // 1) Load data
                    // Create a pipeline and load training data.
                    new TextLoader(TrainingDataFileLocation).CreateFrom<IrisFlowerDataModel>(useHeader: true, separator: ','),

                    // 2) Transform data
                    // Assign numeric values to text in "Label" column, because only numbers can be processed during model training.
                    new Dictionarizer("Label"),

                    // Puts all features into a vector.
                    new ColumnConcatenator("Features", "SepalLength", "SepalWidth", "PetalLength", "PetalWidth"),

                    // 3) Add learner
                    // Add a learning algorithm to the pipeline for classification. 
                    new StochasticDualCoordinateAscentClassifier(),

                    // Convert text in "Label" column back into value of step 2.
                    new PredictedLabelColumnOriginalValueConverter() { PredictedLabelColumn = "PredictedLabel" }
                };

                // 4) Train your model based on the data set.
                var trainingModel = pipeline.Train<IrisFlowerDataModel, IrisFlowerPredictionModel>();

                return trainingModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Evaluates trained prediction model for iris flower classification.
        /// </summary>
        /// <param name="trainingModel">Trained prediction model for iris flower classification.</param>
        /// <returns>Accuracy of trained prediction model for iris flower classification.</returns>
        public double Evaluate(PredictionModel<IrisFlowerDataModel, IrisFlowerPredictionModel> trainingModel)
        {
            try
            {
                // 1) Load data
                var testData = new TextLoader(TestDataFileLocation).CreateFrom<IrisFlowerDataModel>(useHeader: true, separator: ',');

                // 2) Evaluate model
                var classificationMetrics = new ClassificationEvaluator().Evaluate(trainingModel, testData);

                return classificationMetrics.AccuracyMacro;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
