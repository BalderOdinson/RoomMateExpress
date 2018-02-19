using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FirebaseCloudMessagingApi.Exceptions;
using FirebaseCloudMessagingApi.Models;
using FirebaseCloudMessagingApi.Models.Core;
using Google.Apis.Auth.OAuth2;
using Refit;
using Google.Apis.Services;
using Google.Apis.Util;
using Newtonsoft.Json;

namespace FirebaseCloudMessagingApi.Services
{
    public class FcmApi
    {
        private readonly IFcmApi _fcmApi;
        private readonly string _projectId;
        private readonly ServiceAccountCredential _serviceAccountCredential;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId">It contains the Firebase project id (i.e. the unique identifier for your Firebase project). </param>
        /// <param name="serviceAccountEmail">Firebase service account.</param>
        /// <param name="privateKey">Firebase project private key.</param>
        public FcmApi(string projectId, string serviceAccountEmail, string privateKey)
        {
            _projectId = projectId;
            _fcmApi = RestService.For<IFcmApi>(new HttpClient(new AuthenticatedHttpClientHandler(GetToken))
            {
                BaseAddress = new Uri(FirebaseCloudMessagingApiConstants.FcmSeverUrl)
            }, new RefitSettings
            {
                JsonSerializerSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }
            });
            _serviceAccountCredential = new ServiceAccountCredential(
                new ServiceAccountCredential.Initializer(serviceAccountEmail)
                {
                    Scopes = new [] { FirebaseCloudMessagingApiConstants.FcmScope }
                }.FromPrivateKey(privateKey));
        }

        /// <summary>
        /// Send a message to specified target a registration token.
        /// </summary>
        /// <param name="isTest">If true, won't send the message to target. It'll just validate it.</param>
        /// <param name="message"></param>
        /// <returns>If successful, the response body contains an instance of T(MassageBase).</returns>
        public async Task<TokenMessage> SendMessage(bool isTest, TokenMessage message)
        {
            var result = await _fcmApi.SendHttpRequestToTokenAsync(_projectId, new SendRequestBody<TokenMessage>
            {
                ValidateOnly = isTest,
                Message = message
            });
            message.Name = result.Name;
            return message;
        }

        /// <summary>
        /// Send a message to specified target topic.
        /// </summary>
        /// <param name="isTest">If true, won't send the message to target. It'll just validate it.</param>
        /// <param name="message"></param>
        /// <returns>If successful, the response body contains an instance of T(MassageBase).</returns>
        public async Task<TopicMessage> SendMessage(bool isTest, TopicMessage message)
        {
            var result = await _fcmApi.SendHttpRequestToTopicAsync(_projectId, new SendRequestBody<TopicMessage>
            {
                ValidateOnly = isTest,
                Message = message
            });
            message.Name = result.Name;
            return message;
        }

        /// <summary>
        /// Send a message to specified target condition.
        /// </summary>
        /// <param name="isTest">If true, won't send the message to target. It'll just validate it.</param>
        /// <param name="message"></param>
        /// <returns>If successful, the response body contains an instance of T(MassageBase).</returns>
        public async Task<ConditionMessage> SendMessage(bool isTest, ConditionMessage message)
        {
            var result = await _fcmApi.SendHttpRequestToConditionAsync(_projectId, new SendRequestBody<ConditionMessage>
            {
                ValidateOnly = isTest,
                Message = message
            });
            message.Name = result.Name;
            return message;
        }

        private async Task<string> GetToken()
        {
            if (_serviceAccountCredential.Token != null &&
                !_serviceAccountCredential.Token.IsExpired(_serviceAccountCredential.Clock))
                return _serviceAccountCredential.Token.AccessToken;
            if (await _serviceAccountCredential.RequestAccessTokenAsync(new CancellationToken()))
                return _serviceAccountCredential.Token.AccessToken;
            throw new FcmApiException(FirebaseCloudMessagingApiConstants.TokenError);
        }
    }
}
