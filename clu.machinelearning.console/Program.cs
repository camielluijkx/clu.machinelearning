using clu.machinelearning.library;
using static clu.machinelearning.library.ConsoleHelper;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace clu.machinelearning.console
{
    class Program
    {
        private static void runDatasetClassification()
        {
            IrisFlowerModelRunner.Instance.RunDatasetClassification();
        }

        private enum IrisFlowerInputType
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

        private static void runIndividualClassification()
        {
            var classificationRequest = new IrisFlowerClassificationRequest
            {
                SepalLength = getValue(IrisFlowerInputType.SepalLength),
                SepalWidth = getValue(IrisFlowerInputType.SepalWidth),
                PetalLength = getValue(IrisFlowerInputType.PetalLength),
                PetalWidth = getValue(IrisFlowerInputType.PetalWidth)
            };

            IrisFlowerModelRunner.Instance.RunIndividualClassification(classificationRequest);
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
