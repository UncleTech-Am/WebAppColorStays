using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace WebAppColorStays.Models
{
    public class NoHtmlModelBinder : IModelBinder
    {
        private static readonly Regex HtmlTagRegex = new Regex("<.*?>", RegexOptions.Compiled);

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // Retrieve the value of the field
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult != ValueProviderResult.None)
            {
                string value = valueProviderResult.FirstValue;

                // Get the property name and container type from the model metadata
                var propertyName = bindingContext.ModelMetadata.PropertyName;
                var containerType = bindingContext.ModelMetadata.ContainerType;

                if (containerType != null && !string.IsNullOrEmpty(propertyName))
                {
                    // Retrieve the PropertyInfo for the property being bound
                    var propertyInfo = containerType.GetProperty(propertyName);

                    if (propertyInfo != null)
                    {
                        // Check if the property has the [AllowHtml] attribute
                        var allowHtmlAttribute = propertyInfo.GetCustomAttribute<AllowHtmlAttribute>();

                        if (allowHtmlAttribute == null)
                        {
                            // Check if HTML tags are present
                            if (HtmlTagRegex.IsMatch(value))
                            {
                                // Add a validation error to the ModelState
                                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "HTML tags are not allowed.");
                            }
                        }
                    }
                }
                // Set empty string inputs to null
                if (string.IsNullOrWhiteSpace(value))
                {
                    value = null;
                }
                // Set the result regardless of validation
                bindingContext.Result = ModelBindingResult.Success(value);
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Success(null);
            }

            return Task.CompletedTask;
        }
    }
}



