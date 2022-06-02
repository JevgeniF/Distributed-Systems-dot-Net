using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Helpers.WebApp;

public class CustomFloatingPointBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (context.Metadata.ModelType == typeof(decimal) ||
            context.Metadata.ModelType == typeof(float) ||
            context.Metadata.ModelType == typeof(double))
        {
            var loggerFactory = context.Services.GetRequiredService<ILoggerFactory>();
            return new FloatingPointModelBinder(loggerFactory, context.Metadata.ModelType);
        }

        return null;
    }
}