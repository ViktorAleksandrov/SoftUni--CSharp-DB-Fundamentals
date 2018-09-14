namespace VaporStore.DataProcessor.Dto.Import
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class UserDto
    {
        [Required]
        [RegularExpression("^[A-Z][a-zA-Z]* [A-Z][a-zA-Z]*$")]
        public string FullName { get; set; }

        [Required]
        [MinLength(3), MaxLength(20)]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [Range(3, 103)]
        public int Age { get; set; }

        [MinLength(1)]
        public ICollection<CardDto> Cards { get; set; }
    }
}
