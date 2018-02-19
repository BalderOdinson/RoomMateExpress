using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FirebaseCloudMessagingApi.Models;
using FirebaseCloudMessagingApi.Models.Core;
using Refit;

namespace FirebaseCloudMessagingApi.Services
{
    /// <summary>
    /// Interface for sending Http request to https://fcm.googleapis.com
    /// </summary>
    public interface IFcmApi
    {
        /// <summary>
        /// Send a message to specified target a registration token. Http POST request.
        /// </summary>
        /// <param name="projectId">Required. 
        /// It contains the Firebase project id (i.e. the unique identifier for your Firebase project), in the format of projects/{project_id}. 
        /// For legacy support, the numeric project number with no padding is also supported in the format of projects/{project_number}.</param>
        /// <param name="requestBody">The request body.</param>
        /// <returns>If successful, the response body contains an instance of T(MassageBase).</returns>
        [Post("/v1/projects/{project_id}/messages:send")]
        Task<TokenMessage> SendHttpRequestToTokenAsync([AliasAs("project_id")]string projectId,
            [Body] SendRequestBody<TokenMessage> requestBody);

        /// <summary>
        /// Send a message to specified target topic. Http POST request.
        /// </summary>
        /// <param name="projectId">Required. 
        /// It contains the Firebase project id (i.e. the unique identifier for your Firebase project), in the format of projects/{project_id}. 
        /// For legacy support, the numeric project number with no padding is also supported in the format of projects/{project_number}.</param>
        /// <param name="requestBody">The request body.</param>
        /// <returns>If successful, the response body contains an instance of T(MassageBase).</returns>
        [Post("/v1/projects/{project_id}/messages:send")]
        Task<TopicMessage> SendHttpRequestToTopicAsync([AliasAs("project_id")]string projectId,
            [Body] SendRequestBody<TopicMessage> requestBody);

        /// <summary>
        /// Send a message to specified target condition. Http POST request.
        /// </summary>
        /// <param name="projectId">Required. 
        /// It contains the Firebase project id (i.e. the unique identifier for your Firebase project), in the format of projects/{project_id}. 
        /// For legacy support, the numeric project number with no padding is also supported in the format of projects/{project_number}.</param>
        /// <param name="requestBody">The request body.</param>
        /// <returns>If successful, the response body contains an instance of T(MassageBase).</returns>
        [Post("/v1/projects/{project_id}/messages:send")]
        Task<ConditionMessage> SendHttpRequestToConditionAsync([AliasAs("project_id")]string projectId,
            [Body] SendRequestBody<ConditionMessage> requestBody);
    }
}
