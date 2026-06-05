using BusinessLayer.Contracks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;

namespace ApiLayer.Filters
{
    public class IdempotencyAttribute : ActionFilterAttribute
    {
        private const string IdempotencyKeyHeader = "Idempotency-Key";
        private const string InProgressStatus = "InProgress";

        private class IdempotencyCashedRecord
        {
            public int StatusCode { get; set; }
            public object? Result { get; set; }
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //if donot have IdempotencyKeyHeader then return bad request
            if (!context.HttpContext.Request.Headers.TryGetValue(IdempotencyKeyHeader, out var idempotencyKey) || string.IsNullOrEmpty(idempotencyKey))
            {
                context.Result = new BadRequestObjectResult("Idempotency-Key header is required");
                return;
            }

            var cashKey = $"idempotency:{idempotencyKey}";
            var cashService = context.HttpContext.RequestServices.GetRequiredService<IDistributedCache>();
            var cashedRecordString = await cashService.GetStringAsync(cashKey);

            if (!string.IsNullOrEmpty(cashedRecordString))
            {
                //if the record is in progress then return conflict
                if (cashedRecordString == InProgressStatus)
                {
                    context.Result = new ConflictObjectResult("Request is already in progress");
                    return;

                }

                var cashedRecord = System.Text.Json.JsonSerializer.Deserialize<IdempotencyCashedRecord>(cashedRecordString);
                if (cashedRecord != null)
                {
                    context.Result = new ObjectResult(cashedRecord.Result)
                    {
                        StatusCode = cashedRecord.StatusCode
                    };
                    return;
                }
            }

            if (string.IsNullOrEmpty(cashedRecordString))
            {
                //mark the request as in progress
                await cashService.SetStringAsync(cashKey, InProgressStatus, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5)
                });

                var executedContext = await next();

                //if the Result is objectResult cast it t oobjectResult (بتتاكد انه نفس النوع وبتحول في نفس الوقت)
                if (executedContext.Result is ObjectResult objectResult)
                {
                    if (objectResult.StatusCode >= 200 && objectResult.StatusCode < 300)
                    {
                        var cashedRecord = new IdempotencyCashedRecord
                        {
                            StatusCode = objectResult.StatusCode ?? 200,
                            Result = objectResult.Value
                        };

                        var newCashedRecordString = System.Text.Json.JsonSerializer.Serialize(cashedRecord);
                        await cashService.SetStringAsync(cashKey, newCashedRecordString, new DistributedCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                        });
                    }
                    else
                    {
                        //if the request is failed then remove the in progress record
                        await cashService.RemoveAsync(cashKey);
                    }
                }

            }

        }
    }
}

