using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebAppColorStays.Models;

namespace WebAppColorStays.Models
{
    public class NoHtmlModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            // Apply only to string properties
            if (context.Metadata.ModelType == typeof(string))
            {
                return new BinderTypeModelBinder(typeof(NoHtmlModelBinder));
            }

            return null;
        }
    }
}