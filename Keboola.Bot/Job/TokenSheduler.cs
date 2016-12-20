using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Keboola.Bot.Keboola;
using Keboola.Bot.Service;
using Microsoft.Bot.Connector;
using Quartz;

namespace Keboola.Bot.Job
{

    public class TokenShedulerJob : IJob
    {
        private IDatabaseContext _context;
        private IKeboolaClient _client;
        private DatabaseService _service;
        private int _deadLineDays = 30;

        public TokenShedulerJob()
        {
            _context = new DatabaseContext();
            _client = new KeboolaClient((WebConfigurationManager.AppSettings["KeboolaBaseUrl"]));
            _deadLineDays = Int32.Parse(WebConfigurationManager.AppSettings["TokenExpirationDays"]);
            _service = new DatabaseService(_context);
        }

        public TokenShedulerJob(IDatabaseContext context, IKeboolaClient client)
        {
            this._client = client;
            this._context = context;
            _service = new DatabaseService(context);
        }

        public void Execute(IJobExecutionContext context)
        {
            var needRefresh = _service.GetExpiredUsersTokens();
            foreach (var keboolaUser in needRefresh)
            {
                try
                {
                    string newToken = Task.Run(() => _client.RefreshTokenAsync(keboolaUser.Token.Value)).Result;
                    keboolaUser.Token.Value = newToken;
                }
                catch (Exception ex)
                {
                    Debug.Fail(ex.Message);
                }
            }
            _context.SaveChanges();
        }
    }
}