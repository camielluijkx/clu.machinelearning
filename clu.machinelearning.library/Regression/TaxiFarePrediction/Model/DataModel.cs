using Microsoft.ML.Runtime.Api;

namespace clu.machinelearning.library.regression.taxifareprediction
{
    public class DataModel
    {
        /// <summary>
        /// The ID of the taxi vendor.
        /// </summary>
        [Column("0")]
        public string VendorId;

        /// <summary>
        /// The rate type of the taxi trip.
        /// </summary>
        [Column("1")]
        public string RateCode;

        /// <summary>
        /// The number of passengers on the trip.
        /// </summary>
        [Column("2")]
        public float PassengerCount;

        /// <summary>
        /// The amount of time the trip took. 
        /// </summary>
        [Column("3")]
        public float TripTime;

        /// <summary>
        /// The distance of a trip.
        /// </summary>
        [Column("4")]
        public float TripDistance;

        /// <summary>
        /// The payment method (cash or credit card).
        /// </summary>
        [Column("5")]
        public string PaymentType;

        /// <summary>
        /// The total taxi fare paid.
        /// </summary>
        [Column("6")]
        public float FareAmount;
    }
}
