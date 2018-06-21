using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Models;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace clu.machinelearning.irisclassification
{
    /// <summary>
    /// Class to build, train and evaluate iris flower prediction model.
    /// </summary>
    public class IrisFlowerModelBuilder
    {
        /// <summary>
        /// Location of training data file.
        /// </summary>
        private const string TrainingDataFileLocation = @"IrisClassification/Data/iris-data_training.csv";

        /// <summary>
        /// Location of test data file.
        /// </summary>
        private const string TestDataFileLocation = @"IrisClassification/Data/iris-data_test.csv";

        private IEnumerable<IrisFlowerDataModel> getIrisFlowerDataFromCsv(string dataFileLocation)
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
                });
        }

        /// <summary>
        /// Returns iris flower data from test file.
        /// </summary>
        /// <returns>Iris flower data from test file.</returns>
        public IEnumerable<IrisFlowerDataModel> GetIrisFlowerTestData()
        {
            return getIrisFlowerDataFromCsv(TestDataFileLocation);
        }

        /// <summary>
        /// Returns iris flower data from training file.
        /// </summary>
        /// <returns>Iris flower data from training file.</returns>
        public IEnumerable<IrisFlowerDataModel> GetIrisFlowerTrainingData()
        {
            return getIrisFlowerDataFromCsv(TrainingDataFileLocation);
        }

        /// <summary>
        /// Builds machine learning pipeline and trains model.
        /// </summary>
        /// <returns>Trained machine learning model.</returns>
        public PredictionModel<IrisFlowerDataModel, IrisFlowerPredictionModel> BuildAndTrain()
        {
            var pipeline = new LearningPipeline
            {
                // 1) Create a pipeline and load data
                new TextLoader(TrainingDataFileLocation).CreateFrom<IrisFlowerDataModel>(useHeader: true, separator: ','),

                // 2) Transform data
                // Assign numeric values to text in "Label" column, because only numbers can be processed during model training
                new Dictionarizer("Label"),

                // Puts all features into a vector
                new ColumnConcatenator("Features", "SepalLength", "SepalWidth", "PetalLength", "PetalWidth"),

                // 3) Add learner
                // Add a learning algorithm to the pipeline. 
                // This is a classification scenario (What type of iris is this?)
                new StochasticDualCoordinateAscentClassifier(),

                // Convert the Label back into original text(after converting to number in step 2)
                new PredictedLabelColumnOriginalValueConverter() { PredictedLabelColumn = "PredictedLabel" }
            };

            // 4) Train your model based on the data set
            var model = pipeline.Train<IrisFlowerDataModel, IrisFlowerPredictionModel>();

            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Returns accuracy of iris flower prediction model.</returns>
        public double Evaluate(PredictionModel<IrisFlowerDataModel, IrisFlowerPredictionModel> model)
        {
            var testData = new TextLoader(TestDataFileLocation).CreateFrom<IrisFlowerDataModel>(useHeader: true, separator: ',');
            var metrics = new ClassificationEvaluator().Evaluate(model, testData);

            return metrics.AccuracyMacro;
        }
    }
}
