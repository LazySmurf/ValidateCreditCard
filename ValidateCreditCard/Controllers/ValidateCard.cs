using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace ValidateCreditCard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidateCard : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<List<Models.CreditCard>>> ValidateCardInfo(Models.CreditCard creditCard) //Async in case you wanted to implement database interactions with EF.
        {

            //Remove any spaces from the credit card number before processing, so we can accept entire numbers or space separated numbers.
            string CardNumber = creditCard.CardNumber.Replace(" ", string.Empty);
            //Check that, after removing spaces, the card number is exactly 16 characters in length. (We allow up to 19 characters if spaces are provided.)
            if (CardNumber.Length != 16)
            {
                return BadRequest("Card number must contain 16 digits");
            }

            //Ensure that the card information isn't null
            if (creditCard == null)
            {
                return BadRequest("Invalid credit card information entered.");
            }

            // Validate the credit card number using the Luhn algorithm
            if (!IsValidLuhnNumber(CardNumber))
            {
                return BadRequest("Invalid credit card number.");
            }

            // Validate the expiration date
            try
            {
                DateTime ExpirationDate = DateTime.ParseExact(creditCard.ExpMonth + "/" + creditCard.ExpYear, "M/yyyy", CultureInfo.InvariantCulture);
                if (ExpirationDate < DateTime.Now)
                {
                    return BadRequest("Credit card has expired.");
                }
            }
            catch (Exception)
            {
                return BadRequest("Expiry date is in the wrong format.");
                //throw;
            }

            // Validate the CVV/CVC code is not empty
            if (string.IsNullOrEmpty(creditCard.SecurityCode.ToString()))
            {
                return BadRequest("CVV/CVC code is required.");
            }
            else    //If it is not empty, ensure it is the correct value according to card type.
            {       //Since the CreditCard model only accepts CVVs with 3 or 4 digits, we can simply check the card type, and ensure it's length is what it should be.

                string InvalidCodeMsg = "CVV/CVC code is invalid for this type of card.";

                //Check that American Express cards have a 4-digit code
                if (GetCardIssuer(CardNumber) == "American Express" && creditCard.SecurityCode.ToString().Length == 3)
                {
                    return BadRequest(InvalidCodeMsg);
                }

                //Check that Visa cards have a 3-digit code
                if (GetCardIssuer(CardNumber) == "Visa" && creditCard.SecurityCode.ToString().Length == 4)
                {
                    return BadRequest(InvalidCodeMsg);
                }

                //Check that MasterCards have a 3-digit code
                if (GetCardIssuer(CardNumber) == "MasterCard" && creditCard.SecurityCode.ToString().Length == 4)
                {
                    return BadRequest(InvalidCodeMsg);
                }
            }

            if (string.IsNullOrEmpty(creditCard.CardHolder))
            {
                return BadRequest("Card holder is not valid.");
            }

            return Ok(GetCardIssuer(CardNumber));
        }

        //---------------------------------------------------------------------\\
        //                          Supporting Methods                         \\
        //---------------------------------------------------------------------\\

        //Check is valid Luhn number to assist in validation of card number
        [NonAction]
        private bool IsValidLuhnNumber(string number)
        {
            // Check if the number is null or empty
            if (string.IsNullOrEmpty(number))
            {
                return false;
            }

            // Reverse the number and convert it to a character array
            char[] digits = number.ToCharArray();
            Array.Reverse(digits);

            int sum = 0;
            bool isSecond = false;
            foreach (char digit in digits)
            {
                if (!char.IsDigit(digit))
                {
                    // The string contains non-numeric characters
                    return false;
                }

                int digitValue = (digit - '0');
                if (isSecond)
                {
                    digitValue *= 2;
                }

                sum += digitValue / 10;
                sum += digitValue % 10;

                isSecond = !isSecond;
            }

            return (sum % 10 == 0);
        }

        //Method to check Card Issuer
        [NonAction]
        private string GetCardIssuer(string cardNumber)
        {
            //First, ensure the card number passed isn't Null or empty.
            if (string.IsNullOrEmpty(cardNumber))
            {
                return "Empty";
            }

            // Get the first six digits of the card number
            string prefix = cardNumber.Substring(0, 4);
            if (prefix.StartsWith("4"))
            {
                return "Visa";
            }
            else if (prefix.StartsWith("34") || prefix.StartsWith("37"))
            {
                return "American Express";
            }
            else if (prefix.StartsWith("51") || prefix.StartsWith("52") || prefix.StartsWith("53") || prefix.StartsWith("54") || prefix.StartsWith("55"))
            {
                return "MasterCard";
            }
            /*
             * As the assignment explicitly specified to only validate Visa, Mastercard, and American Express cards, I have commented Discover out.
             * However, adding/removing other card issuers can be done here in this check.
             * 
            else if (prefix.StartsWith("6011"))
            {
                return "Discover";
            }
            */
            else
            {
                return "Unknown";
            }
        }
    }
}