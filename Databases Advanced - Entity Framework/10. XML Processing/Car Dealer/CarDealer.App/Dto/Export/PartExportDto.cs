using System.Xml.Serialization;

namespace CarDealer.App.Dto.Export
{
    [XmlType("part")]
    public class PartExportDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("price")]
        public decimal Price { get; set; }
    }
}
