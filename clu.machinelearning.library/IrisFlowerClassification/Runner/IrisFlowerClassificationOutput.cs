using Newtonsoft.Json;

using System;

namespace clu.machinelearning.library
{
    public class IrisFlowerClassificationOutput
    {
        /// <summary>
        /// Iris flower identification.
        /// </summary>
        [JsonIgnore]
        public Guid Id { get; set; }

        /// <summary>
        /// Iris flower predicted species.
        /// </summary>
        public string PredictedSpecies { get; set; }
    }
}
