namespace PetClinic.DataProcessor.Dto.Export
{
    using System.Xml.Serialization;

    [XmlType("AnimalAid")]
    public class AnimalAidExportDto
    {
        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
