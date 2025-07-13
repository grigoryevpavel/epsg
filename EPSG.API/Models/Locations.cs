using System.Collections.Generic;

namespace EPSG.API.Models
{
    public class Locations
    {
        public int Id { get; set; }
        public double[] Center { get; set; }
        public List<LocationPoint> Polygon { get; set; }
    }
}
