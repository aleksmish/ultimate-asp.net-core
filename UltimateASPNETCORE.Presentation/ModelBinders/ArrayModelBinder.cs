using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace UltimateASPNETCORE.Presentation.ModelBinders
{
    public class ArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            var providedValue = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName)
                .ToString();

            if (string.IsNullOrEmpty(providedValue))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }

            var genericType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
            var converter = TypeDescriptor.GetConverter(genericType);

            var objectArray = providedValue.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(v => converter.ConvertFromString(v.Trim())).ToArray();

            var guidArray = Array.CreateInstance(genericType, objectArray.Length);

            objectArray.CopyTo(guidArray, 0);

            bindingContext.Model = guidArray;
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);

            return Task.CompletedTask;
        }
    }
}
