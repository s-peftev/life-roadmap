using LR.Application.Requests;
using LR.Infrastructure.Constants;
using LR.Infrastructure.Options;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace LR.Infrastructure.ModelBinding.Pagination
{
    public class PaginatedRequestBinder(IOptions<PaginationOptions> opts) : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var vp = bindingContext.ValueProvider;

            int page = TryGet(vp, QueryParameters.PaginationPageNumber);
            int size = TryGet(vp, QueryParameters.PaginationPageSize);

            page = page > 0 ? page : opts.Value.DefaultPageNumber;

            size = size > 0
                ? Math.Min(size, opts.Value.MaxPageSize)
                : opts.Value.DefaultPageSize;

            bindingContext.Result = ModelBindingResult.Success(new PaginatedRequest
            {
                PageNumber = page,
                PageSize = size
            });

            return Task.CompletedTask;
        }

        private static int TryGet(IValueProvider vp, string key)
        {
            var val = vp.GetValue(key).FirstValue;
            return int.TryParse(val, out var i) ? i : -1;
        }
    }
}
