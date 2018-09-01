using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ProductShop.App.Dto.Import
{
    [XmlType("category")]
    public class CategoryDto
    {
        [XmlElement("name")]
        [MinLength(3), MaxLength(15)]
        public string Name { get; set; }
    }
}
