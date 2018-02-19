using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;

namespace RoomMateExpressWebApi.EF.Services
{
    public interface IUserReportService
    {
        /// <summary>
        /// Gets all UserReports.
        /// </summary>
        /// <returns> </returns>
        Task<List<UserReport>> GetAllUserReports();

        /// <summary>
        /// Gets all UserReports with pagination options.
        /// </summary>
        /// <param name="oldestReport"></param>
        /// <param name="numberToTake">How many reports to take.</param>
        /// <returns></returns>
        Task<List<UserReport>> GetAllUserReports(DateTimeOffset oldestReport, int numberToTake);

        /// <summary>
        /// Gets all UserReports that are in process.
        /// </summary>
        /// <returns></returns>
        Task<List<UserReport>> GetAllInProcessUserReports();

        /// <summary>
        /// Gets all UserReports that are in process with pagination options.
        /// </summary>
        /// <param name="oldestReport"></param>
        /// <param name="numberToTake">How many reports to take.</param>
        /// <returns></returns>
        Task<List<UserReport>> GetAllInProcessUserReports(DateTimeOffset oldestReport, int numberToTake);

        /// <summary>
        /// Gets all UserReports that are processed.
        /// </summary>
        /// <returns></returns>
        Task<List<UserReport>> GetAllProcessedUserReports();

        /// <summary>
        /// Gets all UserReports that are processed with pagination options.
        /// </summary>
        /// <param name="oldestReport"></param>
        /// <param name="numberToTake">How many reports to take.</param>
        /// <returns></returns>
        Task<List<UserReport>> GetAllProcessedUserReports(DateTimeOffset oldestReport, int numberToTake);

        /// <summary>
        /// Gets all UserReports for given Admin.
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        Task<List<UserReport>> GetUserReports(Guid adminId);

        /// <summary>
        /// Gets all UserReports for given Admin with pagination options.
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="oldestReport"></param>
        /// <param name="numberToTake">How many reports to take.</param>
        /// <returns></returns>
        Task<List<UserReport>> GetUserReports(Guid adminId, DateTimeOffset oldestReport, int numberToTake);

        /// <summary>
        /// Gets all UserReports that are in process for given Admin.
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        Task<List<UserReport>> GetInProcessUserReports(Guid adminId);

        /// <summary>
        /// Gets all UserReports that are in process for given Admin with pagination options.
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="oldestReport"></param>
        /// <param name="numberToTake">How many reports to take.</param>
        /// <returns></returns>
        Task<List<UserReport>> GetInProcessUserReports(Guid adminId, DateTimeOffset oldestReport, int numberToTake);

        /// <summary>
        /// Gets all UserReports that are processed for given Admin.
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        Task<List<UserReport>> GetProcessedUserReports(Guid adminId);

        /// <summary>
        /// Gets all UserReports that are processed for given Admin with pagination options.
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="oldestReport"></param>
        /// <param name="numberToTake">How many reports to take.</param>
        /// <returns></returns>
        Task<List<UserReport>> GetProcessedUserReports(Guid adminId, DateTimeOffset oldestReport, int numberToTake);

        /// <summary>
        /// Gets all UserReports in which is given user reported. 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<UserReport>> GetUserReportsHistory(Guid userId);

        /// <summary>
        /// Gets all UserReports in which is given user reported with pagination options.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oldestReport"></param>
        /// <param name="numberToTake">How many reports to take.</param>
        /// <returns></returns>
        Task<List<UserReport>> GetUserReportsHistory(Guid userId, DateTimeOffset oldestReport, int numberToTake);

        /// <summary>
        /// Gets all UserReports in which given user reported someone.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<UserReport>> GetUserReportingHistory(Guid userId);

        /// <summary>
        /// Gets all UserReports in which given user reported someone with pagination options.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oldestReport"></param>
        /// <param name="numberToTake">How many reports to take.</param>
        /// <returns></returns>
        Task<List<UserReport>> GetUserReportingHistory(Guid userId, DateTimeOffset oldestReport, int numberToTake);

        /// <summary>
        /// Reports a user.
        /// </summary>
        /// <param name="userToReportId"></param>
        /// <param name="userReportingId"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        Task<UserReport> ReportUser(Guid userToReportId, Guid userReportingId, string text);

        /// <summary>
        /// Processes a report.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="decision"></param>
        /// <returns></returns>
        Task<UserReport> ProcessReport(Guid id, string decision);

        /// <summary>
        /// Sets report active again.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserReport> SetInProcessReport(Guid id);
    }
}
