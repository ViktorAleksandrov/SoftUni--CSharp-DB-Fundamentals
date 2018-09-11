namespace PetClinic.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    using AutoMapper;
    using Newtonsoft.Json;

    using PetClinic.Data;
    using PetClinic.DataProcessor.Dto.Import;
    using PetClinic.Models;

    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Error: Invalid data.";

        public static string ImportAnimalAids(PetClinicContext context, string jsonString)
        {
            AnimalAidDto[] deserializedAnimalAids = JsonConvert.DeserializeObject<AnimalAidDto[]>(jsonString);

            var sb = new StringBuilder();

            var validAnimalAids = new List<AnimalAid>();

            foreach (AnimalAidDto animalAidDto in deserializedAnimalAids)
            {
                bool animalAidExists = validAnimalAids.Any(ai => ai.Name == animalAidDto.Name);

                if (!IsValid(animalAidDto) || animalAidExists)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                AnimalAid animalAid = Mapper.Map<AnimalAid>(animalAidDto);

                validAnimalAids.Add(animalAid);

                sb.AppendLine($"Record {animalAid.Name} successfully imported.");
            }

            context.AnimalAids.AddRange(validAnimalAids);

            context.SaveChanges();

            string result = sb.ToString().TrimEnd();

            return result;
        }

        public static string ImportAnimals(PetClinicContext context, string jsonString)
        {
            AnimalDto[] deserializedAnimals = JsonConvert.DeserializeObject<AnimalDto[]>(jsonString);

            var sb = new StringBuilder();

            var validAnimals = new List<Animal>();

            foreach (AnimalDto animalDto in deserializedAnimals)
            {
                bool pasportExists = validAnimals.Any(a => a.Passport.SerialNumber == animalDto.Passport.SerialNumber);

                if (!IsValid(animalDto) || !IsValid(animalDto.Passport) || pasportExists)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var registrationDate = DateTime.ParseExact(
                    animalDto.Passport.RegistrationDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                var animal = new Animal
                {
                    Name = animalDto.Name,
                    Type = animalDto.Type,
                    Age = animalDto.Age,
                    Passport = new Passport
                    {
                        SerialNumber = animalDto.Passport.SerialNumber,
                        OwnerPhoneNumber = animalDto.Passport.OwnerPhoneNumber,
                        OwnerName = animalDto.Passport.OwnerName,
                        RegistrationDate = registrationDate
                    }
                };

                validAnimals.Add(animal);

                sb.AppendLine(
                    $"Record {animal.Name} Passport №: {animal.Passport.SerialNumber} successfully imported.");
            }

            context.Animals.AddRange(validAnimals);

            context.SaveChanges();

            string result = sb.ToString().TrimEnd();

            return result;
        }

        public static string ImportVets(PetClinicContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(VetDto[]), new XmlRootAttribute("Vets"));

            var deserializedVets = (VetDto[])serializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();

            var validVets = new List<Vet>();

            foreach (VetDto vetDto in deserializedVets)
            {
                bool phoneNumberExists = validVets.Any(v => v.PhoneNumber == vetDto.PhoneNumber);

                if (!IsValid(vetDto) || phoneNumberExists)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Vet vet = Mapper.Map<Vet>(vetDto);

                validVets.Add(vet);

                sb.AppendLine($"Record {vet.Name} successfully imported.");
            }

            context.Vets.AddRange(validVets);

            context.SaveChanges();

            string result = sb.ToString().TrimEnd();

            return result;
        }

        public static string ImportProcedures(PetClinicContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ProcedureDto[]), new XmlRootAttribute("Procedures"));

            var deserializedProcedures = (ProcedureDto[])serializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();

            var validProcedures = new List<Procedure>();

            foreach (ProcedureDto procedureDto in deserializedProcedures)
            {
                Vet vet = context.Vets.SingleOrDefault(v => v.Name == procedureDto.Vet);
                Animal animal = context.Animals.SingleOrDefault(a => a.PassportSerialNumber == procedureDto.Animal);

                if (!IsValid(procedureDto) || vet == null || animal == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var dateTime = DateTime.ParseExact(procedureDto.DateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                var procedure = new Procedure
                {
                    Animal = animal,
                    Vet = vet,
                    DateTime = dateTime
                };

                bool allAnimalAidsExist = true;

                var validProcedureAnimalAids = new List<ProcedureAnimalAid>();

                foreach (ProcedureAnimalAidDto procedureAnimalAidDto in procedureDto.AnimalAids)
                {
                    AnimalAid animalAid = context.AnimalAids
                        .SingleOrDefault(aa => aa.Name == procedureAnimalAidDto.Name);

                    if (animalAid == null
                        || validProcedureAnimalAids.Any(pa => pa.AnimalAid.Name == procedureAnimalAidDto.Name))
                    {
                        allAnimalAidsExist = false;
                        break;
                    }

                    var procedureAnimalAid = new ProcedureAnimalAid
                    {
                        Procedure = procedure,
                        AnimalAid = animalAid
                    };

                    validProcedureAnimalAids.Add(procedureAnimalAid);
                }

                if (!allAnimalAidsExist)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                procedure.ProcedureAnimalAids = validProcedureAnimalAids;

                validProcedures.Add(procedure);

                sb.AppendLine("Record successfully imported.");
            }

            context.Procedures.AddRange(validProcedures);

            context.SaveChanges();

            string result = sb.ToString().TrimEnd();

            return result;
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);

            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}
