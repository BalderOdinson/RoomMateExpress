using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Platform;
using Refit;
using RoomMateExpress.Core.ApiModels;
using RoomMateExpress.Core.Services;

namespace RoomMateExpress.Core.Helpers
{
    public static class ApiRequestHelper
    {
        private static ILocalizationService _localizationService;
        private static IAuthService _authService;
        private static IToastSerivce _toastSerivce;

        private static void Initialize()
        {
            if (_localizationService == null)
                _localizationService = Mvx.Resolve<ILocalizationService>();
            if (_authService == null)
                _authService = Mvx.Resolve<IAuthService>();
            if (_toastSerivce == null)
                _toastSerivce = Mvx.Resolve<IToastSerivce>();
        }

        public static async Task<ApiResult<TResult>> HandlApiRequest<TResult>(Func<Task<ApiResult<TResult>>> request)
        {
            Initialize();
            try
            {
                return await request();
            }
            catch (ApiException apiException)
            {
                var error = _localizationService.GetResourceString(apiException.Content.Replace("\"", ""));
                if (apiException.StatusCode == HttpStatusCode.Unauthorized ||
                    apiException.StatusCode == HttpStatusCode.Forbidden)
                    return await RetryApiRequest(request);
                if (apiException.StatusCode == HttpStatusCode.InternalServerError)
                    error = _localizationService.GetResourceString(Constants.Errors.OperationError);
                return new ApiResult<TResult>(error ?? apiException.Content, false, default(TResult));
            }
            catch (HttpRequestException)
            {
                var error = _localizationService.GetResourceString("noInternet");
                return new ApiResult<TResult>(error, false, default(TResult));
            }
        }

        private static async Task<ApiResult<TResult>> RetryApiRequest<TResult>(Func<Task<ApiResult<TResult>>> request)
        {
            try
            {
                await _authService.RefreshToken();
                return await request();
            }
            catch (ApiException apiException)
            {
                var error = _localizationService.GetResourceString(apiException.Content.Replace("\"", ""));
                if (apiException.StatusCode == HttpStatusCode.Unauthorized ||
                    apiException.StatusCode == HttpStatusCode.Forbidden ||
                    apiException.Content.Equals(Constants.Errors.RefreshTokenExpired))
                    error = Constants.Errors.LoginRequired;
                if (apiException.StatusCode == HttpStatusCode.InternalServerError)
                    error = _localizationService.GetResourceString(Constants.Errors.OperationError);
                return new ApiResult<TResult>(error ?? apiException.Content, false, default(TResult));
            }
            catch (HttpRequestException)
            {
                var error = _localizationService.GetResourceString("noInternet");
                return new ApiResult<TResult>(error, false, default(TResult));
            }
        }

        public static async Task<ApiResult> HandlApiRequest(Func<Task<ApiResult>> request)
        {
            Initialize();
            try
            {
                return await request();
            }
            catch (ApiException apiException)
            {
                var error = _localizationService.GetResourceString(apiException.Content.Replace("\"", ""));
                if (apiException.StatusCode == HttpStatusCode.Unauthorized ||
                    apiException.StatusCode == HttpStatusCode.Forbidden)
                    return await RetryApiRequest(request);
                if (apiException.StatusCode == HttpStatusCode.InternalServerError)
                    error = _localizationService.GetResourceString(Constants.Errors.OperationError);
                return new ApiResult(error ?? apiException.Content, false);
            }
            catch (HttpRequestException)
            {
                var error = _localizationService.GetResourceString("noInternet");
                return new ApiResult(error, false);
            }
        }

        private static async Task<ApiResult> RetryApiRequest(Func<Task<ApiResult>> request)
        {
            try
            {
                await _authService.RefreshToken();
                return await request();
            }
            catch (ApiException apiException)
            {
                var error = _localizationService.GetResourceString(apiException.Content.Replace("\"", ""));
                if (apiException.StatusCode == HttpStatusCode.Unauthorized ||
                    apiException.StatusCode == HttpStatusCode.Forbidden ||
                    apiException.Content.Equals(Constants.Errors.RefreshTokenExpired))
                    error = Constants.Errors.LoginRequired;
                if (apiException.StatusCode == HttpStatusCode.InternalServerError)
                    error = _localizationService.GetResourceString(Constants.Errors.OperationError);
                return new ApiResult(error ?? apiException.Content, false);
            }
            catch (HttpRequestException)
            {
                var error = _localizationService.GetResourceString("noInternet");
                return new ApiResult(error, false);
            }
        }

        public static async Task HandlApiRequest(Func<Task> request, Action<string> errorHandler)
        {
            Initialize();
            try
            {
                await request();
            }
            catch (ApiException apiException)
            {
                var error = _localizationService.GetResourceString(apiException.Content.Replace("\"", ""));
                if (apiException.StatusCode == HttpStatusCode.Unauthorized ||
                    apiException.StatusCode == HttpStatusCode.Forbidden)
                {
                    await RetryApiRequest(request, errorHandler);
                    return;
                }
                if (apiException.StatusCode == HttpStatusCode.InternalServerError)
                    error = _localizationService.GetResourceString(Constants.Errors.OperationError);
                errorHandler(error ?? apiException.Content);
            }
            catch (HttpRequestException)
            {
                var error = _localizationService.GetResourceString("noInternet");
                errorHandler(error);
            }
        }

        private static async Task RetryApiRequest(Func<Task> request, Action<string> errorHandler)
        {
            try
            {
                await _authService.RefreshToken();
                await request();
            }
            catch (ApiException apiException)
            {
                var error = _localizationService.GetResourceString(apiException.Content.Replace("\"", ""));
                if (apiException.StatusCode == HttpStatusCode.Unauthorized ||
                    apiException.StatusCode == HttpStatusCode.Forbidden ||
                    apiException.Content.Equals(Constants.Errors.RefreshTokenExpired))
                    error = Constants.Errors.LoginRequired;
                if (apiException.StatusCode == HttpStatusCode.InternalServerError)
                    error = _localizationService.GetResourceString(Constants.Errors.OperationError);
                errorHandler(error ?? apiException.Content);
            }
            catch (HttpRequestException)
            {
                var error = _localizationService.GetResourceString("noInternet");
                errorHandler(error);
            }
        }

        public static async Task HandleApiResult<TResult>(Func<Task<ApiResult<TResult>>> request, Action<TResult> onSuccess)
        {
            var result = await request();
            if (result.Success)
            {
                onSuccess(result.Result);
                return;
            }

            if (result.Error.Equals(Constants.Errors.LoginRequired))
            {
                await RequestLoginHelper.RequestLogin(async () =>
                {
                    result = await request();
                    if (result.Success)
                    {
                        onSuccess(result.Result);
                        return;
                    }
                    _toastSerivce.ShowByValue(result.Error);
                });
            }
            else _toastSerivce.ShowByValue(result.Error);
        }

        public static async Task HandleApiResult(Func<Task<ApiResult>> request)
        {
            var result = await request();
            if (result.Success)
            {
                return;
            }

            if (result.Error.Equals(Constants.Errors.LoginRequired))
            {
                await RequestLoginHelper.RequestLogin(async () =>
                {
                    result = await request();
                    if (result.Success)
                    {
                        return;
                    }
                    _toastSerivce.ShowByValue(result.Error);
                });
            }
            else _toastSerivce.ShowByValue(result.Error);
        }

        public static async Task HandleApiResult(Func<Task<ApiResult>> request, Action onSuccess)
        {
            var result = await request();
            if (result.Success)
            {
                onSuccess();
                return;
            }

            if (result.Error.Equals(Constants.Errors.LoginRequired))
            {
                await RequestLoginHelper.RequestLogin(async () =>
                {
                    result = await request();
                    if (result.Success)
                    {
                        onSuccess();
                        return;
                    }
                    _toastSerivce.ShowByValue(result.Error);
                });
            }
            else _toastSerivce.ShowByValue(result.Error);
        }

        public static async Task<TResult> HandleApiResult<TResult>(Func<Task<ApiResult<TResult>>> request)
        {
            var result = await request();
            if (result.Success)
            {
                return result.Result;
            }

            if (result.Error.Equals(Constants.Errors.LoginRequired))
            {
                await RequestLoginHelper.RequestLogin(async () =>
                {
                    result = await request();
                    if (result.Success)
                    {
                        return;
                    }
                    _toastSerivce.ShowByValue(result.Error);
                });
                if (result.Success)
                    return result.Result;
            }
            else _toastSerivce.ShowByValue(result.Error);

            return default(TResult);
        }
    }
}
