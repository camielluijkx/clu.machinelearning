using Microsoft.ML.Runtime.Api;

namespace clu.machinelearning.library.classification.speciesdetermination
{
    internal class DataModel
    {
        /// <summary>
        /// Sepal length of iris flower.
        /// </summary>
        [Column("0")]
        public float SepalLength;

        /// <summary>
        /// Sepal width of iris flower.
        /// </summary>
        [Column("1")]
        public float SepalWidth;

        /// <summary>
        /// Petal length of iris flower.
        /// </summary>
        [Column("2")]
        public float PetalLength;

        /// <summary>
        /// Petal width of iris flower.
        /// </summary>
        [Column("3")]
        public float PetalWidth;

        /// <summary>
        /// Iris flower species.
        /// </summary>
        [Column("4")]
        [ColumnName("Label")]
        public string Label;
    }
}
