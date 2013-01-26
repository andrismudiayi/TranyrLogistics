using System;
using System.Web.Mvc;

namespace TranyrLogistics.Models.CustomModelBinders
{
    public class CustomerModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            if (modelType.Equals(typeof(Customer)))
            {
                // If this value is null on the model then we know this is not a company. This is
                // because as per spec, individuals have no vat numbers.
                var testValue = controllerContext.Controller.ValueProvider.GetValue("VatNumber");

                Type instantiationType;
                if (testValue == null)
                {
                    instantiationType = typeof(Individual);
                }
                else
                {
                    instantiationType = typeof(Company);
                }
                
                var objectInstance = Activator.CreateInstance(instantiationType);
                bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, instantiationType);
                bindingContext.ModelMetadata.Model = objectInstance;
                return objectInstance;
            }

            return base.CreateModel(controllerContext, bindingContext, modelType);
        }
    }
}