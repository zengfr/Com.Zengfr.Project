using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Zfrong.Framework.Core.DataContract;
namespace Zfrong.Framework.Mvc.Binder
{
    public class DataFilterItemModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var filter = (ExtFilterItem)base.BindModel(controllerContext, bindingContext);

            var field = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + "[field]");
            if (field != null)
            {
                filter.Field = field.AttemptedValue;
            }
            var type = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + "[data][type]");
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + "[data][value]");
            var comparison = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + "[data][comparison]");
            if (filter.Data == null)
            {
                filter.Data = new ExtFilterData();
            }
            if (type != null)
            {
                filter.Data.Type = type.AttemptedValue;
            }
            if (value != null)
            {
                filter.Data.Value = value.AttemptedValue;
            }
            if (comparison != null)
            {
                filter.Data.Comparison = comparison.AttemptedValue;
            }
            return filter;
        }
    }
}
