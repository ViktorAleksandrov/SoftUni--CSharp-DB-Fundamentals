namespace PetClinic.DataProcessor.Dto.Import
{
    using System.Xml.Serialization;

    [XmlType("AnimalAid")]
    public class ProcedureAnimalAidDto
    {
        public string Name { get; set; }
    }
}
