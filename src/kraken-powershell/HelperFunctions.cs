﻿using System;
using System.IO;
using kraken.powershell.Model;
using SeaMist.Http;
using SeaMist.Model;

namespace kraken.powershell
{
    internal static class HelperFunctions
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

        public static string BuildAzurePath(string url, bool keepPath, string azurePath)
        {
            if (keepPath)
            {
                return azurePath + new Uri(url).LocalPath.Replace("//", "/");
            }

            return azurePath + Path.GetFileName(new Uri(url).LocalPath);
        }
    }
}