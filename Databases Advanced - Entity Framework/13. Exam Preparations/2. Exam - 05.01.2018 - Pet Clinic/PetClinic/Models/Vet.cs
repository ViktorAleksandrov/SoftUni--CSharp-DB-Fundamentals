namespace PetClinic.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Vet
    {
        public Vet()
        {
            this.Procedures = new HashSet<Procedure>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(3), MaxLength(40)]
        public string Name { get; set; }

        [Required]
        [MinLength(3), MaxLength(50)]
        public string Profession { get; set; }

        [Required]
        [Range(22, 65)]
        public int Age { get; set; }

        [Required]
        [RegularExpression(@"^(\+359|0)[\d]{9}$")]
        public string PhoneNumber { get; set; }

        public ICollection<Procedure> Procedures { get; set; }
    }
}
