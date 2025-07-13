namespace EPSG.API.Models
{
    public class Field
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public Locations Locations { get; set; }
    }
}
