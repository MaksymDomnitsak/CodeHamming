using System.ComponentModel.DataAnnotations;

namespace HammingCode.Models
{
    public class HammingModel
    {
        /*[StringLength(7, ErrorMessage = "Length of code - 7 symbols",MinimumLength = 7)]
        public string? Code { get; set; }*/

        [StringLength(1, ErrorMessage = "Must be 1 symbol")]
        public string? Symbol { get; set; }

        [StringLength(11, ErrorMessage = "Length of code - 11 symbols",MinimumLength = 11)]
        public string? Decode { get; set; }
    }
}
