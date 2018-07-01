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

        private static async Task runIrisFlowerClassificationAsync()
        {
            var classificationInput = new IrisFlowerClassificationInput
            {
                SepalLength = 3.3f, //getIrisFlowerInputValue(IrisFlowerInputType.SepalLength),
                SepalWidth = 1.6f, //getIrisFlowerInputValue(IrisFlowerInputType.SepalWidth),
                PetalLength = 0.2f, //getIrisFlowerInputValue(IrisFlowerInputType.PetalLength),
                PetalWidth = 5.1f //getIrisFlowerInputValue(IrisFlowerInputType.PetalWidth)
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

        private static async Task runSentimentAnalysisClassificationAsync()
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
                ClassificationInput = classificationInput
            };

            var classificationResponse = await SentimentAnalysisClassificationRunner.Instance.RunClassificationAsync(classificationRequest);
            if (!classificationResponse.Success)
            {
                Console.WriteLine($"Sentiment analysis classification failed: {classificationResponse.Message}");
            }
        }

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
