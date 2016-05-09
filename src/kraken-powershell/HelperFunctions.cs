using System;
using System.IO;
using kraken.powershell.Model;
using SeaMist.Http;
using SeaMist.Model;

namespace kraken.powershell
{
    public static class HelperFunctions
    {
        public static object CreateReturnObject(IApiResponse<OptimizeResult> response)
        {
            object resultCallback;

            if (response.Success)
            {
                resultCallback = new ResultCallback
                {
                    Id = response.Body.Id,
                    Success = response.Success,
                    StatusCode = (int) response.StatusCode
                };
            }
            else
            {
                resultCallback = new ResultCallback
                {
                    Id = null,
                    Success = response.Success,
                    StatusCode = (int) response.StatusCode
                };
            }

            return resultCallback;
        }

        public static object CreateReturnObject(IApiResponse<OptimizeWaitResult> response)
        {
            object resultCallback;

            if (response.Success)
            {
                resultCallback = new ResultWait
                {
                    Success = response.Success,
                    FileName = response.Body.FileName,
                    OriginalSize = response.Body.OriginalSize,
                    KrakedSize = response.Body.KrakedSize,
                    SavedBytes = response.Body.SavedBytes,
                    KrakedUrl = response.Body.KrakedUrl,
                    StatusCode = (int) response.StatusCode
                };
            }
            else
            {
                resultCallback = new ResultWait
                {
                    Success = response.Success,
                    StatusCode = (int) response.StatusCode
                };
            }

            return resultCallback;
        }

        public static string BuildAzurePath(string url, bool keepPath, string azurePath, string azureContainer)
        {
            var uri = new Uri(url);

            if (keepPath)
            {
                if (DestinationIsSameAzureContainer(uri, azureContainer))
                {
                    var location = uri.LocalPath.Remove(0, azureContainer.Length + 1);
                    return UrlCleanUp($"{azurePath}{location}");
                }

                return UrlCleanUp($"{azurePath}{uri.LocalPath}");
            }

            return UrlCleanUp($"/{azurePath}/{Path.GetFileName(uri.LocalPath)}");
        }

        public static bool DestinationIsSameAzureContainer(Uri url, string azureContainer)
        {
            return url.AbsoluteUri.ToLower().Contains("blob.core.windows.net") && url.LocalPath.StartsWith("/" + azureContainer);
        }

        public static string UrlCleanUp(string url)
        {
            return url.Replace("//", "/");
        }
    }
}