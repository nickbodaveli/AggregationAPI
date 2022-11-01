namespace AggregationAPI.Models
{
    public class Dataset
    {
        public int Id { get; set; }
        public string Region { get; set; }
        public string ObjType { get; set; }
        public string ObjName { get; set; }
        public int ObjNumber { get; set; }
        public double PPlus { get; set; }
        public double PMinus { get; set; }
        public DateTime PlTime { get; set; }
    }
}
