﻿using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Configuration;
using Keboola.Bot.Keboola;
using Keboola.Bot.Service;
using log4net;
using Quartz;

namespace Keboola.Bot.Job
{
    public class TokenShedulerJob : IJob
    {
        private readonly IKeboolaClient _client;
        private readonly IDatabaseContext _context;
        private readonly DatabaseService _service;
        public readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly int _deadLineDays = 30;

        public TokenShedulerJob()
        {
            _context = new DatabaseContext();
            _client = new KeboolaClient(WebConfigurationManager.AppSettings["KeboolaBaseUrl"]);
            _deadLineDays = int.Parse(WebConfigurationManager.AppSettings["TokenExpirationDays"]);
            _service = new DatabaseService(_context);
        }

        public TokenShedulerJob(IDatabaseContext context, IKeboolaClient client)
        {
            _client = client;
            _context = context;
            _service = new DatabaseService(context);
        }

        public void Execute(IJobExecutionContext context)
        {
            var needRefresh = _service.GetExpiredUsersTokens();
            foreach (var keboolaUser in needRefresh)
                try
                {
                    RefreshToken(keboolaUser);
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    try
                    {
                        RefreshToken(keboolaUser);
                    }
                    catch (Exception ex2)
                    {
                        logger.Error("Cant refresh token.", ex2);
                        Debug.Fail(ex2.Message);
                    }
                    Debug.Fail(ex.Message);
                }
        }

        private void RefreshToken(KeboolaUser keboolaUser)
        {
            var verifyResponse = Task.Run(() => _client.VerifyTokenAsync(keboolaUser.Token.Value)).Result;
            if (verifyResponse != null)
            {
                var newToken =
                    Task.Run(() => _client.RefreshTokenAsync(verifyResponse.token, int.Parse(verifyResponse.id)))
                        .Result;
                keboolaUser.Token.Value = newToken;
                keboolaUser.Token.Expiration = DateTime.Now.AddDays(_deadLineDays - 1);
                _context.SaveChanges();
            }
        }
    }
}