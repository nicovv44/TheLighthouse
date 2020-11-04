
namespace TheLighthouse
{
    /// <summary>
    /// Public class regrouping all the constants used in the software
    /// </summary>
    public static class Constant
    {
        /// <summary>
        /// Time to wait for a new result before considering that the connection is lost (millisecond)
        /// </summary>
        public static int TimeoutFreshnessMs => 1000;

        /// <summary>
        /// Time between two GET requests (millisecond)
        /// </summary>
        public static int RequestPeriod => 300;

        /// <summary>
        /// Default URI to use when no URI is saved in the ressources
        /// </summary>
        public static string DefaultUri => "http://192.168.45.118";
    }
}
