namespace SoftJail.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    public class MailDto
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public string Sender { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z\d\s]*str\.$")]
        public string Address { get; set; }
    }
}
