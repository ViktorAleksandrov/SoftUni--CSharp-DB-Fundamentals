namespace PetClinic.DataProcessor.Dto.Import
{
    using System.ComponentModel.DataAnnotations;

    public class AnimalAidDto
    {
        [Required]
        [MinLength(3), MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Price { get; set; }
    }
}
