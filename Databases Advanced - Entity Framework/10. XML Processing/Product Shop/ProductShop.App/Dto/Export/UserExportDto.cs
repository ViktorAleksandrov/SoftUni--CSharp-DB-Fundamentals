using System.Xml.Serialization;
using ProductShop.App.Dto.Import;

namespace ProductShop.App.Dto.Export
{
    [XmlType("user")]
    public class UserExportDto
    {
        [XmlAttribute("first-name")]
        public string FirstName { get; set; }

        [XmlAttribute("last-name")]
        public string LastName { get; set; }

        [XmlArray("sold-products")]
        public ProductDto[] SoldProducts { get; set; }
    }
}
