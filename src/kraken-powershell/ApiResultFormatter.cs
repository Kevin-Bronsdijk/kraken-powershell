using Kraken.Powershell.Model;
using Kraken.Http;
using Kraken.Model;

namespace Kraken.Powershell
{
    internal class ApiResultFormatter : IResultFormatter
    {
        public object FormatObject(object data)
        {
            // Return data using a custom model.
            if (data is IApiResponse<OptimizeResult>)
            {
                var dataTyped = data as IApiResponse<OptimizeResult>;
                data = FormatOptimizeResult(dataTyped);
            }
            if (data is IApiResponse<OptimizeWaitResult>)
            {
                var dataTyped = data as IApiResponse<OptimizeWaitResult>;
                data = FormatOptimizeWaitResult(dataTyped);
            }

            return data;
        }

        private static object FormatOptimizeResult(IApiResponse<OptimizeResult> response)
        {
            if (response.Success)
            {
                return new ResultCallback
                {
                    Id = response.Body.Id,
                    Success = response.Success,
                    StatusCode = (int)response.StatusCode
                };
            }
            else
            {
                return new ResultCallback
                {
                    Id = null,
                    Success = response.Success,
                    StatusCode = (int)response.StatusCode,
                    Error = response.Error
                };
            }
        }

        private static object FormatOptimizeWaitResult(IApiResponse<OptimizeWaitResult> response)
        {
            if (response.Success)
            {
                return new ResultWait
                {
                    Success = response.Success,
                    FileName = response.Body.FileName,
                    OriginalSize = response.Body.OriginalSize,
                    KrakedSize = response.Body.KrakedSize,
                    SavedBytes = response.Body.SavedBytes,
                    KrakedUrl = response.Body.KrakedUrl,
                    StatusCode = (int)response.StatusCode,
                };
            }
            else
            {
                return new ResultWait
                {
                    Success = response.Success,
                    StatusCode = (int)response.StatusCode,
                    Error = response.Error
                };
            }
        }
    }
}
