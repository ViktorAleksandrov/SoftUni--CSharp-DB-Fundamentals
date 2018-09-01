using System.Xml.Serialization;

namespace ProductShop.App.Dto.Export
{
    [XmlRoot("users")]
    public class UsersDto
    {
        [XmlAttribute("count")]
        public int UsersCount { get; set; }

        [XmlElement("user")]
        public UserInfoDto[] Users { get; set; }
    }
}
