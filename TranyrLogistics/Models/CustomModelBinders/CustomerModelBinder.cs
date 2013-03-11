using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TranyrLogistics.Models.Customers;
using TranyrLogistics.Models.Groups;

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
                if (testValue != null)
                {
                    instantiationType = typeof(Company);
                }
                else
                {
                    instantiationType = typeof(Individual);
                }
                
                var objectInstance = Activator.CreateInstance(instantiationType);
                bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, instantiationType);
                bindingContext.ModelMetadata.Model = objectInstance;
                return objectInstance;
            }

            if (modelType.Equals(typeof(Group)))
            {
                // If this value is null on the model then we know this is not a company. This is
                // because as per spec, individuals have no vat numbers.
                var testValue = controllerContext.Controller.ValueProvider.GetValue("GroupType");

                Type instantiationType;
                if (testValue != null)
                {
                    instantiationType = typeof(CustomerGroup);
                }
                else
                {
                    instantiationType = typeof(ServiceProviderGroup);
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