using System.Xml.Serialization;

namespace CarDealer.App.Dto.Export
{
    [XmlType("car")]
    public class CarFerrariDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

        [XmlAttribute("travelled-distance")]
        public long TravelledDistance { get; set; }
    }
}
