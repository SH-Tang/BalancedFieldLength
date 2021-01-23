# Validation of user input

## Context and Problem Statement

The user is currently allowed to enter every value in the input fields. Even though the calculation kernel is able to prevent the application from crashing (due to some validation rules and preventing uncaught exceptions going through the application), the user does not get a lot of information from the generated log messages. The log messages trigger on each individual field, while for the workflow it is more useful to generate all the log messages regarding invalid values entered in the input fields.

## Considered Options

### Validation on the data model
The data model is responsible for determining which values are allowed as input and are then passed to the kernel for the calculation. Validation will happen by means of throwing exceptions when a value is invalid. 

Advantages:
* The validation is located at one central spot which is beneficial in case an import function will be implemented. It will also guarantee that the data is always valid during runtime.
* The data is always valid when it is being sent to the kernel, therefore the kernel does not need to validate on the input data anymore on the user input. However, it still needs to validate on (invalid) combinations on user input. 

Disadvantages:
* It will not retain the invalid entered value. The exception basically prevents the view from committing the value. As a consequence, the last (valid) value will be shown. 
* The focus will remain on the control element. The user cannot change the control unless they correct the wrong value. 

### Using the interface `INotifyDataErrorInfo`
The view model becomes responsible for validating the values with this choice. WPF supports this approach and it basically means that the ViewModel will contain the  logic to verify whether the data is valid. It is not recommended to implement this interface on the domain data, because that will introduce a dependency of the domain data to GUI components. (which is undesirable)

Advantages:
* If the datamodel is kept simple and does not have any validation present, the ViewModel will be fully responsible for validating and committing the individual values to the datamodel. As a functional advantage, the wrongly entered value by the user can be retained and displayed, including an error message.
* A property can be validated with multiple conditions and generating multiple error messages to be displayed. The validation also allows a potential cross validation between properties. 
* Hypothetically, all the validation rules could be rerun on the view model and trigger the `RaiseErrorsChanged` event when the validation needs to be executed all at once.

Disadvantages:
* By introducing validation rules within the ViewModel, the business logic is potentially at two places. In case an import functionality is to be implemented, the domain model has to guarantee that the data passed around in the application is valid with the business logic satisfied. The introduction of the validation rules might cause this redundancy as it needs to be present in the ViewModel as well. Alternatively, this could be refactored in a helper class that validates the data. (Drawback of this approach is that the helper needs to be called whenever the data is involved)

### Using the abstract class `ValidationRule` 
The abstract class `ValidationRule` enables a generic validation rule to be executed before the value is being bound. This basically means that it will intercept the user input before the value is being committed to the underlying datamodel in the ViewModel. 

However, the `ValidationRule` is rather generic and is extremely basic in containing business logic. The behaviour makes it more suitable to validate user input independent of the bussiness logic. 

## Decision Outcome

Based on the foregoing, a combination between the options of "Validation on the data model" and the "ValidationRule" will be implemented. It is beneficial to have the bussiness logic (and thus validation) at a centralised place, namely on the Data model itself. This design also has the advantage that the data is always valid when passed around in the application (except when it is being instantiated). However, it does mean that a separate validation routine needs to be implemented to provide the user with information which fields are invalid when the calculation is started. 

Additionally, the `ValidationRule` can provide the user with more information when they enter a text (instead of a numeric) value in a text box designated for numbers.  

## References 
* [Data validation options in WPF](https://blog.magnusmontin.net/2013/08/26/data-validation-in-wpf/)