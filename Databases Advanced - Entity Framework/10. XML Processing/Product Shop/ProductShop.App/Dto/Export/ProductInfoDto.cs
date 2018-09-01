using System.Xml.Serialization;

namespace ProductShop.App.Dto.Export
{
    [XmlType("sold-products")]
    public class ProductInfoDto
    {
        [XmlAttribute("count")]
        public int ProductsCount { get; set; }

        [XmlElement("product")]
        public SoldProductDto[] SoldProducts { get; set; }
    }
}
