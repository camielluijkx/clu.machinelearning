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
        #region Iris Flower Classification

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
            var returnValue = 0.0f;

            Console.WriteLine($"Provide a value for {EnumHelper<IrisFlowerInputType>.GetDisplayValue(inputType)}.");

            var correctInput = false;
            while (!correctInput)
            {
                var inputValue = Console.ReadLine();
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

        private static async Task runIrisFlowerClassificationAsync()
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
                ClassificationInput = new List<IrisFlowerClassificationInput> { classificationInput }
            };

            var classificationResponse = await IrisFlowerClassificationRunner.Instance.RunClassificationAsync(classificationRequest);
            if (!classificationResponse.Success)
            {
                Console.WriteLine($"Iris flower classification failed: {classificationResponse.Message}");
            }
        }

        #endregion

        #region Sentiment Analysis Classification

        private static string getSentimentAnalysisValue()
        {
            var returnValue = string.Empty;

            Console.WriteLine($"Provide a text with or without sentiment.");

            var correctInput = false;
            while (!correctInput)
            {
                var inputValue = Console.ReadLine();
                if (!string.IsNullOrEmpty(inputValue))
                {
                    correctInput = true;
                }
                else
                {
                    correctInput = false;
                }

                if (!correctInput)
                {
                    Console.WriteLine($"Empty text provided.");
                }
            }

            return returnValue;
        }

        private static async Task runSentimentAnalysisClassificationAsync()
        {
            var classificationInput = new List<SentimentAnalysisClassificationInput>
            {
                new SentimentAnalysisClassificationInput
                {
                    TextForAnalysis = getSentimentAnalysisValue()
                }
            };

            var classificationRequest = new SentimentAnalysisClassificationRequest
            {
                ClassificationInput = classificationInput
            };

            var classificationResponse = await SentimentAnalysisClassificationRunner.Instance.RunClassificationAsync(classificationRequest);
            if (!classificationResponse.Success)
            {
                Console.WriteLine($"Sentiment analysis classification failed: {classificationResponse.Message}");
            }
        }

        #endregion

        static async Task Main(string[] args)
        {
            Initialize();

            ShowMenu(
                new List<MenuItem>
                {
                    new MenuItem(1, "Iris flower classification", async () => await runIrisFlowerClassificationAsync()),
                    new MenuItem(2, "Sentiment analysis classification", async () => await runSentimentAnalysisClassificationAsync()),
                });

            await Task.FromResult(0);
        }
    }
}
