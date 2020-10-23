using System.Collections.Generic;

namespace KayakBackend.RequestModels
{
    public class PredictRequestModel
    {
        public List<ThreeAxisTemporalData> Observations { get; set; }
    }
    
    public class ThreeAxisTemporalData
    {
        public long TimeStamp { get; set; }
        public double xAxis { get; set; }
        public double yAxis { get; set; }
        public double zAxis { get; set; }
    }
}