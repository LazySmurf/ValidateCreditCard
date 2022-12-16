using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ValidateCreditCard.Models
{
    //This is the model of the Credit Card object which is then used for validation within the ValidateCC Controller.
    public class CreditCard
    {
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Name can only contain letters and spaces.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Cardholder must be between 1 and 100 characters.")]
        [Required]
        public string CardHolder { get; set; } = "John Smith";
        /* ----------   End Property    ---------- */


        [RegularExpression(@"^[0-9 ]{16,19}$", ErrorMessage = "Card number must contain exactly 16 numbers, with up to 3 spaces between the number groups.")]
        [Required]
        public string CardNumber { get; set; } = "0000000000000000";
        /* ----------   End Property    ---------- */


        [RegularExpression(@"^([1-9]|1[012])$", ErrorMessage = "Month must be a number between 1 and 12.")]
        [Required]
        public int ExpMonth { get; set; } = 1;
        /* ----------   End Property    ---------- */


        [RegularExpression(@"^[2-9][0-9]\d{2}$", ErrorMessage = "Year must be a number between 2000 and 9999.")]
        [Required]
        //Since any year beginning with 1 is already passed and therefore invalid, we will check for years beginning with 2 up to 9,
        //then check that the following digit is between 0-9, and then check that there are two more digits afterwards.
        //This should allow any year to be accepted from 2000 up to the year 9999. This means we shouldn't have an issue similar to the
        //Millenium bug for another ~7,900 years. This could, at that point, be expanded so that the first RegEx match allows a 1, and the last
        //match allows 3 digits. Doing this would then allow it to go from year 10,000 to 99,999. For this reason, this type of RegEx check is versatile.
        public int ExpYear { get; set; } = DateTime.Now.Year + 1;
        /* ----------   End Property    ---------- */


        [RegularExpression(@"^[0-9]{3,4}$", ErrorMessage = "CVV/CVC must be exactly 3 or 4 digits, depending on your card issuer.")]
        [Required]
        public string SecurityCode { get; set; } = "000";
        /* ----------   End Property    ---------- */
    }
}