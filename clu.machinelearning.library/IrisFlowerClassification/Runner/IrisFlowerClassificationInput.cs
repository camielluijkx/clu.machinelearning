﻿using Newtonsoft.Json;

using System;

namespace clu.machinelearning.library
{
    public class IrisFlowerClassificationInput
    {
        /// <summary>
        /// Iris flower identification.
        /// </summary>
        [JsonIgnore]
        public Guid Id { get; set; }

        /// <summary>
        /// Sepal length of iris flower (for example 3.3).
        /// </summary>
        public float SepalLength { get; set; }

        /// <summary>
        /// Sepal width of iris flower (for example 1.6).
        /// </summary>
        public float SepalWidth { get; set; }

        /// <summary>
        /// Petal length of iris flower (for example 0.2).
        /// </summary>
        public float PetalLength { get; set; }

        /// <summary>
        /// Petal width of iris flower (for example 5.1).
        /// </summary>
        public float PetalWidth { get; set; }

        /// <summary>
        /// Iris flower actual species.
        /// </summary>
        public string ActualSpecies { get; set; }
    }
}