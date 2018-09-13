namespace SoftJail.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using ExportDto;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners
                .Where(p => ids.Contains(p.Id))
                .Select(p => new
                {
                    p.Id,
                    Name = p.FullName,
                    p.Cell.CellNumber,
                    Officers = p.PrisonerOfficers
                        .Select(po => new
                        {
                            OfficerName = po.Officer.FullName,
                            Department = po.Officer.Department.Name
                        })
                        .OrderBy(o => o.OfficerName)
                        .ToArray(),
                    TotalOfficerSalary = Math.Round(p.PrisonerOfficers.Sum(op => op.Officer.Salary), 2)
                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToArray();

            string json = JsonConvert.SerializeObject(prisoners, Formatting.Indented);

            return json;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            string[] names = prisonersNames.Split(',');

            PrisonerExportDto[] prisoners = context.Prisoners
                .Where(p => names.Contains(p.FullName))
                .Select(p => new PrisonerExportDto
                {
                    Id = p.Id,
                    Name = p.FullName,
                    IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    EncryptedMessages = p.Mails
                        .Select(m => new MessageDto
                        {
                            Description = Reverse(m.Description)
                        })
                        .ToArray()
                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToArray();

            var serializer = new XmlSerializer(typeof(PrisonerExportDto[]), new XmlRootAttribute("Prisoners"));

            var sb = new StringBuilder();
            var xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), prisoners, xmlNamespaces);

            return sb.ToString();
        }

        private static string Reverse(string text)
        {
            char[] charArray = text.ToCharArray();

            Array.Reverse(charArray);

            return new string(charArray);
        }
    }
}