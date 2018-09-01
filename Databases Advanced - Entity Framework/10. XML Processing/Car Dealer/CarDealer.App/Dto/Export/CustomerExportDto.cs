using System.Xml.Serialization;

namespace CarDealer.App.Dto.Export
{
    [XmlType("customer")]
    public class CustomerExportDto
    {
        [XmlAttribute("full-name")]
        public string Name { get; set; }

        [XmlAttribute("bought-cars")]
        public int BoughtCarsCount { get; set; }

        [XmlAttribute("spent-money")]
        public decimal SpentMoney { get; set; }

    }
}
