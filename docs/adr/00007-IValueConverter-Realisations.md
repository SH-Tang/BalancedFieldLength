# Ambiguous IValueConverter behaviour when defining realizations

## Context and Problem Statement

During the realization of the `IValueConverter` interface, an ambiguity was encountered. The [documentation](https://docs.microsoft.com/en-us/dotnet/api/system.windows.data.ivalueconverter?view=netframework-4.8) states the following: 

### For the [Convert()](https://docs.microsoft.com/en-us/dotnet/api/system.windows.data.ivalueconverter.convert?view=netframework-4.8) 
> ### Returns
> Object
>
> A converted value. If the method returns `null`, the valid `null` value is used.
>
> ## Remarks
>
> The data binding engine calls this method when it propagates a value from the binding source to the binding target.
> 
> The data binding engine does not catch exceptions that are thrown by a user-supplied converter. Any exception that is thrown by the Convert method, or any uncaught exceptions that are thrown by methods that the Convert method calls, are treated as run-time errors. Handle anticipated problems by returning `DependencyProperty.UnsetValue`.
>
> A return value of `DependencyProperty.UnsetValue` indicates that the converter produced no value and that the binding uses the `FallbackValue`, if available, or the default value instead.

> A return value of `Binding.DoNothing` indicates that the binding does not transfer the value or use the `FallbackValue` or default value.

### For the [ConvertBack()](https://docs.microsoft.com/en-us/dotnet/api/system.windows.data.ivalueconverter.convertback?view=netframework-4.8) 
> ### Returns
> Object
>
> A converted value. If the method returns `null`, the valid `null` value is used.
>
> ## Remarks
> 
> The data binding engine calls this method when it propagates a value from the binding source to the binding target.

> The data binding engine does not catch exceptions that are thrown by a user-supplied converter. Any exception that is thrown by the Convert method, or any uncaught exceptions that are thrown by methods that the Convert method calls, are treated as run-time errors. Handle anticipated problems by returning `DependencyProperty.UnsetValue`.
> 
> A return value of `DependencyProperty.UnsetValue` indicates that the converter produced no value and that the binding uses the `FallbackValue`, if available, or the default value instead.
> 
> A return value of `Binding.DoNothing` indicates that the binding does not transfer the value or use the `FallbackValue` or default value.

The issue here is that it is unclear what should happen when the converter: 

* Gets a value type that is unsupported (for example converting a reference type to an integer would not make any sense).
* Gets a target Type that is not supported by the conversion.

## Considered options
The following options are considered with the following pro's and cons:

### Follow the interface definition. 
Basically, this means that the realization should not throw any exception, as that is breaking with the Liskov Substitution Principle. 

The conversion could be returning `null` in this situation when the conversion is not supported and tries to bind the null value to the object. This leads into a warning (but not a critical exception) in the application along the lines that " 'Value' could not be converted." Alternatively, depending on the situation, it might opt for returning one of the `Binding` objects. The user can set a value in the XAML file about what value it should use when it reaches this state. See also [here](https://docs.microsoft.com/en-us/dotnet/api/system.windows.data.bindingbase.fallbackvalue?view=netframework-4.8).

### Throw a `NotSupportedException`
The throwback option will break with the Liskov Substitution principle, but it follows through with the 'Fail Fast' methodology. When the interface definition is followed, a developer does not have feedback on whether a right converter was used. Instead, by returning a `Binding` or a `null` object, the error is simply masked, making the debugging potentially more complex. However, the exception will cause a  critical exception if it is not handled correctly.

Do note that this method does not necesarily limit the use of returning a `BindingBase` object if it's useful for the situation.

## Decision Outcome
Because the original interface definition masks potential faulty configurations, it is opted to throw a `NotSupportedException` in the case that the conversion is not supported or incompatible. This situation only arises when a developer uses a converter in the wrong context. When the situation is appropriate, return one of the `Binding` objects for the conversion methods.

This exception will be *explicitly* documented by using a foregoing `</inheritdoc>` statement in the XMLDoc. 


