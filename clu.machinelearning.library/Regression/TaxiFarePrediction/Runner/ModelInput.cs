namespace clu.machinelearning.library.regression.taxifareprediction
{
    public class ModelInput
    {
        /// <summary>
        /// The ID of the taxi vendor.
        /// </summary>
        public string VendorId;

        /// <summary>
        /// The rate type of the taxi trip.
        /// </summary>
        public string RateCode;

        /// <summary>
        /// The number of passengers on the trip.
        /// </summary>
        public float PassengerCount;

        /// <summary>
        /// The distance of a trip.
        /// </summary>
        public float TripDistance;

        /// <summary>
        /// The payment method (cash or credit card).
        /// </summary>
        public string PaymentType;
    }
}
