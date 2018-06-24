using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Models;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;

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
        /// <param name="trainingDataFileLocation">Location of training data file.</param>
        /// <returns>Trained prediction model for iris flower classification.</returns>
        public PredictionModel<IrisFlowerDataModel, IrisFlowerPredictionModel> BuildAndTrain(string trainingDataFileLocation)
        {
            var pipeline = new LearningPipeline
            {
                // 1) Load data
                // Create a pipeline and load training data.
                new TextLoader(trainingDataFileLocation).CreateFrom<IrisFlowerDataModel>(useHeader: true, separator: ','),

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

        /// <summary>
        /// Evaluates trained prediction model for iris flower classification.
        /// </summary>
        /// <param name="trainingModel">Trained prediction model for iris flower classification.</param>
        /// <param name="testDataFileLocation">Location of test data file.</param>
        /// <returns>Accuracy of trained prediction model for iris flower classification.</returns>
        public double Evaluate(PredictionModel<IrisFlowerDataModel, IrisFlowerPredictionModel> trainingModel, string testDataFileLocation)
        {
            // 1) Load data
            var testDataLoader = new TextLoader(testDataFileLocation)
                .CreateFrom<IrisFlowerDataModel>(useHeader: true, separator: ',');

            // 2) Evaluate model
            var classificationMetrics = new ClassificationEvaluator().Evaluate(trainingModel, testDataLoader);

            return classificationMetrics.AccuracyMacro;
        }
    }
}
