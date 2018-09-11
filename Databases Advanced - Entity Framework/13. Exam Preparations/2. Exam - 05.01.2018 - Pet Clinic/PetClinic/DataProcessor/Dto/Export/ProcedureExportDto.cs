namespace PetClinic.DataProcessor.Dto.Export
{
    using System.Xml.Serialization;

    [XmlType("Procedure")]
    public class ProcedureExportDto
    {
        public string Passport { get; set; }

        public string OwnerNumber { get; set; }

        public string DateTime { get; set; }

        public AnimalAidExportDto[] AnimalAids { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
