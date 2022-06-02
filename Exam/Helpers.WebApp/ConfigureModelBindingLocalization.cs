using Base.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Helpers.WebApp;

public class ConfigureModelBindingLocalization : IConfigureOptions<MvcOptions>
{
    public void Configure(MvcOptions options)
    {
        options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) =>
            string.Format(Common.ErrorMessage_AttemptedValueIsInvalid, x, y));

        options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(x =>
            string.Format(Common.ErrorMessage_MissingBindRequiredValue, x));

        // localizer["A value for the '{0}' parameter or property was not provided.", x]);

        options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() =>
            Common.ErrorMessage_MissingKeyOrValue);

        // localizer["A value is required."]);

        options.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() =>
            Common.ErrorMessage_MissingRequestBodyRequiredValue);

        // localizer["A non-empty request body is required."]);

        options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(x =>
            string.Format(Common.ErrorMessage_NonPropertyAttemptedValueIsInvalid, x));
        // localizer["The value '{0}' is not valid.", x]);

        options.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() =>
            Common.ErrorMessage_NonPropertyUnknownValueIsInvalid);
        // localizer["The supplied value is invalid."]);

        options.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() =>
            Common.ErrorMessage_NonPropertyValueMustBeANumber);
        // localizer["The field must be a number."]);

        options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor(x =>
            string.Format(Common.ErrorMessage_UnknownValueIsInvalid, x));
        //  localizer["The supplied value is invalid for {0}.", x]);

        options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(x =>
            string.Format(Common.ErrorMessage_ValueIsInvalid, x));
        //  localizer["The value '{0}' is invalid.", x]);

        options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(x =>
            string.Format(Common.ErrorMessage_ValueMustBeANumber, x));
        //  localizer["The field {0} must be a number.", x]);

        options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x =>
            string.Format(Common.ErrorMessage_ValueMustNotBeNull, x));
        //  localizer["The value '{0}' is invalid.", x]);
    }
}