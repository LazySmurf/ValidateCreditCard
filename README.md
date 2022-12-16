
# Riverty Credit Card Validation API

This project is an API created for Riverty, as a homework assignment.

The API has a single endpoint which accepts a POST request containing the following data:



```http
  POST /api/ValidateCard
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `cardHolder`      | `string` | **Required**. Name of Cardholder |
| `cardNumber`      | `string` | **Required**. 16-Digit Card Number (May be separated by up to 3 spaces) |
| `expMonth`      | `int` | **Required**. Expiry Month (without leading zero) |
| `expYear`      | `int` | **Required**. Expiry Year (as full 4-digit year) |
| `securityCode`      | `string` | **Required**. CVV2/CVC2 Code (3 or 4 digits, based on card issuer) |


Given these values, the API then processes the data against a double-redundant error handler to verify that the given card information is valid.

---

## Deployment

To run this project, open Visual Studio 2022 and select the solution file.

The ```CreditCard``` model is located in ```Models\CreditCard.cs``` which is used to define the CreditCard object used by the controller. It also contains some of the validation logic in the form of property attributes.

The ```ValidateCard``` controller is located in ```Controllers\ValidateCard.cs``` which handles the majority of the validation logic.

Once the project is opened in Visual Studio, press Ctrl+F5 to start without debugging. Once done, the API console window will open, and Swagger should open in your default browser automatically.

When the API is launched and Swagger is loaded, you can easily test the API in the browser.

---
## Developed For

- [Riverty](https://www.riverty.com/en/)

