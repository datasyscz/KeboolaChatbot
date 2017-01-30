using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Configuration;
using Keboola.Bot.Keboola;
using Keboola.Bot.Service;
using Quartz;

namespace Keboola.Bot.Job
{
    public class TokenShedulerJob : IJob
    {
        private readonly IKeboolaClient _client;
        private readonly IDatabaseContext _context;
        private int _deadLineDays = 30;
        private readonly DatabaseService _service;

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
                    var verifyResponse = Task.Run(() => _client.VerifyTokenAsync(keboolaUser.Token.Value)).Result;
                    if (verifyResponse != null)
                    {
                        var newToken =
                            Task.Run(() => _client.RefreshTokenAsync(verifyResponse.token, int.Parse(verifyResponse.id)))
                                .Result;
                        keboolaUser.Token.Value = newToken;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Fail(ex.Message);
                }
            _context.SaveChanges();
        }
    }
}