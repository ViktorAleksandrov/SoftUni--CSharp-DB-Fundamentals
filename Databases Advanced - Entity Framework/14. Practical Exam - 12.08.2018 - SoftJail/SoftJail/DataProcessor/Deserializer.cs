namespace SoftJail.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data";

        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            DepartmentDto[] deserializedDepartments = JsonConvert.DeserializeObject<DepartmentDto[]>(jsonString);

            var sb = new StringBuilder();

            var departments = new List<Department>();

            foreach (DepartmentDto departmentDto in deserializedDepartments)
            {
                if (!IsValid(departmentDto) || departmentDto.Cells.Any(c => !IsValid(c)))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var department = new Department
                {
                    Name = departmentDto.Name,
                    Cells = departmentDto.Cells
                        .Select(c => new Cell
                        {
                            CellNumber = c.CellNumber,
                            HasWindow = c.HasWindow
                        })
                        .ToArray()
                };

                departments.Add(department);

                sb.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
            }

            context.Departments.AddRange(departments);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            PrisonerDto[] deserializedPrisoners = JsonConvert.DeserializeObject<PrisonerDto[]>(jsonString);

            var sb = new StringBuilder();

            var prisoners = new List<Prisoner>();

            foreach (PrisonerDto prisonerDto in deserializedPrisoners)
            {
                if (!IsValid(prisonerDto) || prisonerDto.Mails.Any(m => !IsValid(m)))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var prisoner = new Prisoner
                {
                    FullName = prisonerDto.FullName,
                    Nickname = prisonerDto.Nickname,
                    Age = prisonerDto.Age,
                    IncarcerationDate = DateTime.ParseExact(
                        prisonerDto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Bail = prisonerDto.Bail ?? 0,
                    CellId = prisonerDto.CellId,
                    Mails = prisonerDto.Mails
                        .Select(m => new Mail
                        {
                            Description = m.Description,
                            Sender = m.Sender,
                            Address = m.Address
                        })
                        .ToArray()
                };

                if (prisonerDto.ReleaseDate != null)
                {
                    var releaseDate = DateTime.ParseExact(
                        prisonerDto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    prisoner.ReleaseDate = releaseDate;
                }

                prisoners.Add(prisoner);

                sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }

            context.Prisoners.AddRange(prisoners);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(OfficerDto[]), new XmlRootAttribute("Officers"));

            var deserializedOfficers = (OfficerDto[])serializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();

            var officers = new List<Officer>();

            foreach (OfficerDto officerDto in deserializedOfficers)
            {
                bool isPositionValid = Enum.TryParse(officerDto.Position, out Position position);
                bool isWeaponValid = Enum.TryParse(officerDto.Weapon, out Weapon weapon);

                if (!IsValid(officerDto) || !isPositionValid || !isWeaponValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var officer = new Officer
                {
                    FullName = officerDto.Name,
                    Salary = officerDto.Money,
                    Position = position,
                    Weapon = weapon,
                    DepartmentId = officerDto.DepartmentId,
                    OfficerPrisoners = officerDto.Prisoners
                        .Select(p => new OfficerPrisoner
                        {
                            PrisonerId = p.PrisonerId
                        })
                        .ToArray()
                };

                officers.Add(officer);

                sb.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count} prisoners)");
            }

            context.Officers.AddRange(officers);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);

            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}