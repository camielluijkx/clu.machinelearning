using clu.machinelearning.library;
using static clu.machinelearning.library.ConsoleHelper;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace clu.machinelearning.console
{
    class Program
    {
        /// <summary>
        /// Location of test data file.
        /// </summary>
        private const string TestDataFileLocation = @"D:\Workspace\clu.machinelearning\clu.machinelearning.library\IrisFlowerClassification\Data\iris-data_test.csv"; // [TODO] load from datasource, create internal datafile

        internal enum IrisFlowerInputType
        {
            [Display(Name = "sepal lenth of iris flower (for example 3.3)")]
            SepalLength,

            [Display(Name = "sepal width of iris flower (for example 1.6)")]
            SepalWidth,

            [Display(Name = "petal lenth of iris flower (for example 0.2)")]
            PetalLength,

            [Display(Name = "petal width of iris flower (for example 5.1)")]
            PetalWidth
        }

        private static float getValue(IrisFlowerInputType inputType)
        {
            float returnValue = 0.0f;

            Console.WriteLine($"Provide a value for {EnumHelper<IrisFlowerInputType>.GetDisplayValue(inputType)}.");

            bool correctInput = false;
            while (!correctInput)
            {
                string inputValue = Console.ReadLine();
                if (float.TryParse(inputValue, out returnValue))
                {
                    correctInput = true;
                }
                else
                {
                    correctInput = false;
                }

                if (!correctInput)
                {
                    Console.WriteLine($"Incorrect value for {EnumHelper<IrisFlowerInputType>.GetDisplayValue(inputType)}.");
                }
            }

            return returnValue;
        }

        private static void runClassification(IrisFlowerClassificationRequest classificationRequest)
        {
            var classificationResponse = IrisFlowerModelRunner.Instance.RunClassification(classificationRequest);
            if (!classificationResponse.Success)
            {
                Console.WriteLine($"Iris flower classification failed: {classificationResponse.Message}");
            }
        }

        private static void runDatasetClassification()
        {
            var classificationRequest = new IrisFlowerClassificationRequest
            {
                ClassificationType = IrisFlowerClassificationType.Dataset,
                ClassificationInputFileLocation = TestDataFileLocation
            };

            runClassification(classificationRequest);
        }

        private static void runIndividualClassification()
        {
            var classificationInput = new IrisFlowerClassificationInput
            {
                Id = Guid.NewGuid(),
                SepalLength = getValue(IrisFlowerInputType.SepalLength),
                SepalWidth = getValue(IrisFlowerInputType.SepalWidth),
                PetalLength = getValue(IrisFlowerInputType.PetalLength),
                PetalWidth = getValue(IrisFlowerInputType.PetalWidth)
            };

            var classificationRequest = new IrisFlowerClassificationRequest
            {
                ClassificationType = IrisFlowerClassificationType.Individual,
                ClassificationInput = new List<IrisFlowerClassificationInput> { classificationInput }
            };

            runClassification(classificationRequest);
        }

        static void Main(string[] args)
        {
            Initialize();

            ShowMenu(
                new List<MenuItem>
                {
                    new MenuItem(1, "Iris flower classification (dataset)", runDatasetClassification),
                    new MenuItem(2, "Iris flower classification (individual)", runIndividualClassification)
                });
        }
    }
}
