using clu.machinelearning.library.helpers;
using static clu.machinelearning.library.helpers.ConsoleHelper;
using SentimentAnalysis = clu.machinelearning.library.classification.sentimentanalysis;
using SpeciesDetermination = clu.machinelearning.library.classification.speciesdetermination;
using TaxiFarePrediction = clu.machinelearning.library.regression.taxifareprediction;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace clu.machinelearning.console
{
    class Program
    {
        #region Iris flower species determination

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

        private static float getIrisFlowerInputFloatValue(IrisFlowerInputType inputType)
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

        private static async Task runIrisFlowerSpeciesDeterminationAsync()
        {
            var modelInput = new SpeciesDetermination.ModelInput
            {
                SepalLength = getIrisFlowerInputFloatValue(IrisFlowerInputType.SepalLength),
                SepalWidth = getIrisFlowerInputFloatValue(IrisFlowerInputType.SepalWidth),
                PetalLength = getIrisFlowerInputFloatValue(IrisFlowerInputType.PetalLength),
                PetalWidth = getIrisFlowerInputFloatValue(IrisFlowerInputType.PetalWidth)
            };

            var runnerRequest = new SpeciesDetermination.RunnerRequest
            {
                ModelInput = new List<SpeciesDetermination.ModelInput> { modelInput }
            };

            var runnerResponse = await SpeciesDetermination.ModelRunner.Instance.RunClassificationAsync(runnerRequest);
            if (!runnerResponse.Success)
            {
                Console.WriteLine($"Iris flower species determination failed: {runnerResponse.Message}");
            }
        }

        #endregion

        #region Text sentiment analysis

        private static string getTextInputStringValue()
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

        private static async Task runTextSentimentAnalysisAsync()
        {
            var modelInput = new List<SentimentAnalysis.ModelInput>
            {
                new SentimentAnalysis.ModelInput
                {
                    TextForAnalysis = getTextInputStringValue()
                }
            };

            var runnerRequest = new SentimentAnalysis.RunnerRequest
            {
                ModelInput = modelInput
            };

            var runnerResponse = await SentimentAnalysis.ModelRunner.Instance.RunClassificationAsync(runnerRequest);
            if (!runnerResponse.Success)
            {
                Console.WriteLine($"Text sentiment analysis failed: {runnerResponse.Message}");
            }
        }

        #endregion

        #region Taxi Fare Prediction

        internal enum TaxiFareInputType
        {
            [Display(Name = "ID of taxi vendor (CMT or VTS)")]
            VendorId,

            [Display(Name = "rate type of taxi trip (for example 1)")]
            RateCode,

            [Display(Name = "number of passengers on taxi trip (for example 1)")]
            PassengerCount,

            [Display(Name = "distance of taxi trip (for example 10.33)")]
            TripDistance,

            [Display(Name = "payment type of taxi trip (CSH or CRD)")]
            PaymentType,
        }

        private static float getTaxiFareInputFloatValue(TaxiFareInputType inputType)
        {
            var returnValue = 0.0f;

            Console.WriteLine($"Provide a value for {EnumHelper<TaxiFareInputType>.GetDisplayValue(inputType)}.");

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
                    Console.WriteLine($"Incorrect value for {EnumHelper<TaxiFareInputType>.GetDisplayValue(inputType)}.");
                }
            }

            return returnValue;
        }

        private static string getTaxiFareInputStringValue(TaxiFareInputType inputType)
        {
            var returnValue = string.Empty;

            Console.WriteLine($"Provide a value for {EnumHelper<TaxiFareInputType>.GetDisplayValue(inputType)}.");

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
                    Console.WriteLine($"Incorrect value for {EnumHelper<TaxiFareInputType>.GetDisplayValue(inputType)}.");
                }
            }

            return returnValue;
        }
        private static async Task runTaxiFarePredictionAsync()
        {
            var modelInput = new List<TaxiFarePrediction.ModelInput>
            {
                new TaxiFarePrediction.ModelInput
                {
                    VendorId = getTaxiFareInputStringValue(TaxiFareInputType.VendorId),
                    RateCode = getTaxiFareInputStringValue(TaxiFareInputType.RateCode),
                    PassengerCount = getTaxiFareInputFloatValue(TaxiFareInputType.PassengerCount),
                    TripDistance = getTaxiFareInputFloatValue(TaxiFareInputType.TripDistance),
                    PaymentType= getTaxiFareInputStringValue(TaxiFareInputType.PaymentType)
                }
            };

            var runnerRequest = new TaxiFarePrediction.RunnerRequest
            {
                ModelInput = modelInput
            };

            var runnerResponse = await TaxiFarePrediction.ModelRunner.Instance.RunClassificationAsync(runnerRequest);
            if (!runnerResponse.Success)
            {
                Console.WriteLine($"Text sentiment analysis failed: {runnerResponse.Message}");
            }
        }

        #endregion

        static async Task Main(string[] args)
        {
            Initialize();

            ShowMenu(
                new List<MenuItem>
                {
                    new MenuItem(1, "Iris flower species determination", async () => await runIrisFlowerSpeciesDeterminationAsync()),
                    new MenuItem(2, "Text sentiment analysis", async () => await runTextSentimentAnalysisAsync()),
                    new MenuItem(3, "Taxi fare prediction", async () => await runTaxiFarePredictionAsync())
                });

            await Task.FromResult(0);
        }
    }
}
