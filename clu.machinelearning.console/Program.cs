using clu.machinelearning.library;
using static clu.machinelearning.library.ConsoleHelper;

using System.Collections.Generic;

namespace clu.machinelearning.console
{
    class Program
    {
        private static void runDatasetClassification()
        {
            IrisFlowerModelRunner.Instance.RunDatasetClassification();
        }

        private static void runIndividualClassification()
        {
            // [TODO] collect input

            var classificationRequest = new IrisFlowerClassificationRequest
            {
                SepalLength = 3.3f,
                SepalWidth = 1.6f,
                PetalLength = 0.2f,
                PetalWidth = 5.1f
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
