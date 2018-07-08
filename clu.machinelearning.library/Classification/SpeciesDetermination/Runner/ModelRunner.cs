using clu.machinelearning.library.extensions;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace clu.machinelearning.library.classification.speciesdetermination
{
    /// <summary>
    /// Class to run iris flower species determination. 
    /// </summary>
    public class ModelRunner
    {
        private static readonly Lazy<ModelRunner> instance =
            new Lazy<ModelRunner>(() => new ModelRunner());

        private IEnumerable<DataModel> getIrisFlowerFileData(string dataFileLocation)
        {
            return File.ReadAllLines(dataFileLocation)
                .Skip(1)
                .Select(x => x.Split(','))
                .Select(x => new DataModel
                {
                    SepalLength = float.Parse(x[0]),
                    SepalWidth = float.Parse(x[1]),
                    PetalLength = float.Parse(x[2]),
                    PetalWidth = float.Parse(x[3]),
                    Label = x[4]
                });
        }

        /// <summary>
        /// Uses ML.NET to predict iris flower species based on input an trained model.
        /// </summary>
        /// <param name="runnerRequest">Request with input needed for iris flower species determination.</param>
        /// <returns>Iris flower species prediction result.</returns>
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
                        SepalLength = p.SepalLength,
                        SepalWidth = p.SepalWidth,
                        PetalLength = p.PetalLength,
                        PetalWidth = p.PetalWidth
                    })
                    .ToList();
                var modelOutput = modelBuilder.Predict(trainedModel, modelInput)
                    .Select(p => new ModelOutput
                    {
                        PredictedSpecies = p.PredictedLabels
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
