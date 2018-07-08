using clu.machinelearning.library.extensions;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace clu.machinelearning.library.regression.taxifareprediction
{
    /// <summary>
    /// Class to run taxi fare prediction. 
    /// </summary>
    public class ModelRunner
    {
        private static readonly Lazy<ModelRunner> instance =
            new Lazy<ModelRunner>(() => new ModelRunner());

        private IEnumerable<DataModel> getTaxiFareFileData(string dataFileLocation)
        {
            return File.ReadAllLines(dataFileLocation)
                .Skip(1)
                .Select(x => x.Split(','))
                .Select(x => new DataModel
                {
                    VendorId = x[0],
                    RateCode = x[1],
                    PassengerCount = float.Parse(x[2]),
                    TripDistance = float.Parse(x[4]),
                    PaymentType = x[5]
                });
        }

        /// <summary>
        /// Uses ML.NET to predict taxi fare based on input an trained model.
        /// </summary>
        /// <param name="runnerRequest">Request with input needed for taxi fare prediction.</param>
        /// <returns>Taxi fare prediction result.</returns>
        public async Task<RunnerResponse> RunClassificationAsync(RunnerRequest runnerRequest)
        {
            try
            {
                var modelBuilder = new ModelBuilder();
                var trainedModel = await modelBuilder.TrainAsync();
                var modelMetrics = modelBuilder.Evaluate(trainedModel);

                var modelInput = runnerRequest.ModelInput
                    .Select(p => new DataModel
                    {
                        VendorId = p.VendorId,
                        RateCode = p.RateCode,
                        PassengerCount = p.PassengerCount,
                        TripDistance = p.TripDistance,
                        PaymentType = p.PaymentType
                    })
                    .ToList();
                var modelOutput = modelBuilder.Predict(trainedModel, modelInput)
                    .Select(p => new ModelOutput
                    {
                        PredictedFareAmount = p.FareAmount
                    })
                    .ToList();

                return new RunnerResponse
                {
                    Success = true,
                    ModelOutput = modelOutput
                };
            }
            catch (Exception ex)
            {
                return new RunnerResponse
                {
                    Success = false,
                    Message = ex.ToExceptionMessage()
                };
            }
        }

        private ModelRunner()
        {
        }

        /// <summary>
        /// Instance of the <see cref="ModelRunner"/> being used.
        /// </summary>
        public static ModelRunner Instance
        {
            get
            {
                return instance.Value;
            }
        }
    }
}
