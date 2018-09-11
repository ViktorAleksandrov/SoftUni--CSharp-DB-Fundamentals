namespace PetClinic.DataProcessor
{
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    using PetClinic.Data;
    using PetClinic.DataProcessor.Dto.Export;

    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportAnimalsByOwnerPhoneNumber(PetClinicContext context, string phoneNumber)
        {
            var animals = context.Animals
                .Where(a => a.Passport.OwnerPhoneNumber == phoneNumber)
                .Select(a => new
                {
                    a.Passport.OwnerName,
                    AnimalName = a.Name,
                    a.Age,
                    SerialNumber = a.PassportSerialNumber,
                    RegisteredOn = a.Passport.RegistrationDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)
                })
                .OrderBy(a => a.Age)
                .ThenBy(a => a.SerialNumber)
                .ToArray();

            string json = JsonConvert.SerializeObject(animals, Formatting.Indented);

            return json;
        }

        public static string ExportAllProcedures(PetClinicContext context)
        {
            ProcedureExportDto[] procedures = context.Procedures
                .OrderBy(p => p.DateTime)
                .ThenBy(p => p.Animal.PassportSerialNumber)
                .Select(p => new ProcedureExportDto
                {
                    Passport = p.Animal.PassportSerialNumber,
                    OwnerNumber = p.Animal.Passport.OwnerPhoneNumber,
                    DateTime = p.DateTime.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                    AnimalAids = p.ProcedureAnimalAids
                        .Select(paa => new AnimalAidExportDto
                        {
                            Name = paa.AnimalAid.Name,
                            Price = paa.AnimalAid.Price
                        })
                        .ToArray(),
                    TotalPrice = p.Cost
                })
                .ToArray();

            var serializer = new XmlSerializer(typeof(ProcedureExportDto[]), new XmlRootAttribute("Procedures"));

            var sb = new StringBuilder();
            var xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), procedures, xmlNamespaces);

            string result = sb.ToString();

            return result;
        }
    }
}
