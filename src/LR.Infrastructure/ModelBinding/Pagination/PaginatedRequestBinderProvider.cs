using LR.Application.Requests;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace LR.Infrastructure.ModelBinding.Pagination
{
    public class PaginatedRequestBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(PaginatedRequest))
                return new BinderTypeModelBinder(typeof(PaginatedRequestBinder));

            return null;
        }
    }
}
