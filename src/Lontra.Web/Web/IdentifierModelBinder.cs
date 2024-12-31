using Lontra.Core;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Lontra.Web;

public class IdentifierModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var modelName = bindingContext.ModelName;

        // Try to fetch the value of the argument by name
        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

        // Read value as string

        var value = valueProviderResult.FirstValue;

        if (value == null)
        {
            return Task.CompletedTask;
        }

        if (typeof(Identifier<long>).IsAssignableFrom(bindingContext.ModelType) && long.TryParse(value, out var idLong))
        {
            var constructor = bindingContext.ModelType.GetConstructor([typeof(long)]);
            var identifier = constructor!.Invoke([idLong]);

            bindingContext.Result = ModelBindingResult.Success(identifier);
            return Task.CompletedTask;
        }

        if (typeof(Identifier<int>).IsAssignableFrom(bindingContext.ModelType) && int.TryParse(value, out var idInt))
        {
            var constructor = bindingContext.ModelType.GetConstructor([typeof(int)]);
            var identifier = constructor!.Invoke([idInt]);

            bindingContext.Result = ModelBindingResult.Success(identifier);
            return Task.CompletedTask;
        }

        if (typeof(Identifier<string>).IsAssignableFrom(bindingContext.ModelType))
        {
            var constructor = bindingContext.ModelType.GetConstructor([typeof(string)]);
            var identifier = constructor!.Invoke([value]);

            bindingContext.Result = ModelBindingResult.Success(identifier);
            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }
}
