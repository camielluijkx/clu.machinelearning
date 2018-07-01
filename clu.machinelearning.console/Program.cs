using clu.machinelearning.library;
using static clu.machinelearning.library.ConsoleHelper;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace clu.machinelearning.console
{
    class Program
    {
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

        private static float getIrisFlowerInputValue(IrisFlowerInputType inputType)
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

        private static async Task runIrisFlowerClassificationAsync(IrisFlowerClassificationRequest classificationRequest)
        {
            var classificationResponse = await IrisFlowerClassificationRunner.Instance.RunClassificationAsync(classificationRequest);
            if (!classificationResponse.Success)
            {
                Console.WriteLine($"Iris flower classification failed: {classificationResponse.Message}");
            }
        }

        private static async Task runIrisFlowerDatasetClassificationAsync()
        {
            var classificationRequest = new IrisFlowerClassificationRequest
            {
                ClassificationType = IrisFlowerClassificationType.Dataset
            };

            await runIrisFlowerClassificationAsync(classificationRequest);
        }

        private static async Task runIrisFlowerIndividualClassificationAsync()
        {
            var classificationInput = new IrisFlowerClassificationInput
            {
                SepalLength = getIrisFlowerInputValue(IrisFlowerInputType.SepalLength),
                SepalWidth = getIrisFlowerInputValue(IrisFlowerInputType.SepalWidth),
                PetalLength = getIrisFlowerInputValue(IrisFlowerInputType.PetalLength),
                PetalWidth = getIrisFlowerInputValue(IrisFlowerInputType.PetalWidth)
            };

            var classificationRequest = new IrisFlowerClassificationRequest
            {
                ClassificationType = IrisFlowerClassificationType.Individual,
                ClassificationInput = new List<IrisFlowerClassificationInput> { classificationInput }
            };

            await runIrisFlowerClassificationAsync(classificationRequest);
        }

        private static async Task runSentimentAnalysisClassificationAsync(SentimentAnalysisClassificationRequest classificationRequest)
        {
            var classificationResponse = await SentimentAnalysisClassificationRunner.Instance.RunClassificationAsync(classificationRequest);
            if (!classificationResponse.Success)
            {
                Console.WriteLine($"Sentiment analysis classification failed: {classificationResponse.Message}");
            }
        }

        private static async Task runSentimentAnalysisDatasetClassificationAsync()
        {
            var classificationRequest = new SentimentAnalysisClassificationRequest
            {
                ClassificationType = SentimentAnalysisClassificationType.Dataset
            };

            await runSentimentAnalysisClassificationAsync(classificationRequest);
        }

        private static async Task runSentimentAnalysisIndividualClassificationAsync()
        {
            var classificationInput = new List<SentimentAnalysisClassificationInput>
            {
                new SentimentAnalysisClassificationInput
                {
                    TextForAnalysis = "Please refrain from adding nonsense to Wikipedia."
                },
                new SentimentAnalysisClassificationInput
                {
                    TextForAnalysis = "He is the best, and the article should say that."
                },
            };

            var classificationRequest = new SentimentAnalysisClassificationRequest
            {
                ClassificationType = SentimentAnalysisClassificationType.Individual,
                ClassificationInput = classificationInput
            };

            await runSentimentAnalysisClassificationAsync(classificationRequest);
        }

        static async Task Main(string[] args)
        {
            Initialize();

            ShowMenu(
                new List<MenuItem>
                {
                    new MenuItem(1, "Iris flower classification (dataset)", async () => await runIrisFlowerDatasetClassificationAsync()),
                    new MenuItem(2, "Iris flower classification (individual)", async () => await runIrisFlowerIndividualClassificationAsync()),
                    new MenuItem(3, "Sentiment analysis classification (dataset)", async () => await runSentimentAnalysisDatasetClassificationAsync()),
                    new MenuItem(4, "Sentiment analysis classification (individual)", async () => await runSentimentAnalysisIndividualClassificationAsync())
                });

            await Task.FromResult(0);
        }
    }
}
