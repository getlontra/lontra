using Lontra.Core.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Lontra.Web;

public class IdentifierBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (typeof(IIdentifier).IsAssignableFrom(context.Metadata.ModelType))
        {
            return new BinderTypeModelBinder(typeof(IdentifierModelBinder));
        }

        return null;
    }
}
