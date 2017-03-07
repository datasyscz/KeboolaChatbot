using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Keboola.Bot.Controllers;
using log4net;
using Microsoft.Bot.Connector;

namespace Keboola.Bot.Service
{
    [Serializable]
    public class DatabaseService
    {
        private static IDatabaseContext _context;
        public static TimeSpan TokenExpiration = new TimeSpan(30, 0, 0);
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public DatabaseService(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<bool> TokenExistAsync(string token)
        {
            return await _context.KeboolaUser.AnyAsync(a => a.Token.Value == token);
        }

        public async Task<bool> KeboolaUserExist(int id)
        {
            return await _context.KeboolaUser.AnyAsync(a => a.KeboolaId == id);
        }

        public async Task<KeboolaUser> KeboolaUserFind(int? id)
        {
            return await _context.KeboolaUser.FirstOrDefaultAsync(a => a.KeboolaId == id);
        }

        public async Task<bool> UserIsAtivated(string token)
        {
            return await _context.KeboolaUser.AnyAsync(a => a.Token.Value == token && a.Active);
        }

        public async Task<ConversationExt> FindConversationAsync(
            IMessageActivity activity)
        {
            try
            {
                return
                    await _context.Conversation
                        .FirstOrDefaultAsync(
                            a =>
                                a.FrameworkId == activity.Conversation.Id
                                &&
                                a.BaseUri == activity.ServiceUrl
                            //Need check service url too, ConversationID is unique only for serviceUrl 
                        );
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return null;
            }
        }


        public async Task<KeboolaUser> AddUserAndToken(StateModel state)
        {
            var newToken = new KeboolaToken
            {
                Value = state.Token,
                Expiration = DateTime.Now + TokenExpiration
            };

            var newUser = new KeboolaUser
            {
                Token = newToken,
                Active = state.Active == true,
                KeboolaId = (int) state.Id
            };

            //Add new user
            _context.KeboolaUser.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }

        public async Task<KeboolaUser> GetKeboolaUserByTokenAsync(string token)
        {
            return await _context.KeboolaUser.FirstOrDefaultAsync(a => a.Token.Value == token);
        }

        public async Task<KeboolaUser> GetKeboolaUserByIdAsync(int id)
        {
            return await _context.KeboolaUser.FirstOrDefaultAsync(a => a.KeboolaId == id);
        }

        public List<KeboolaUser> GetExpiredUsersTokens()
        {
            var now = DateTime.Now;
            return
                _context.KeboolaUser.Where(o => EntityFunctions.TruncateTime(o.Token.Expiration) <= now && o.Active)
                    .ToList();
        }

        public async Task<string> GetIntentAsync(string id)
        {
            var intent = await _context.IntentAnswer.FirstOrDefaultAsync(a => a.Name == id);
            if (intent != null)
                return intent.Answer;
            return id;
        }

        public async Task<bool> UpdateToken(KeboolaUser user, string token)
        {
            if (user.Token.Value != token)
                if (!await TokenExistAsync(token))
                {
                    var oldToke = user.Token;
                    user.InactiveTokens.Add(oldToke);
                    user.Token = new KeboolaToken
                    {
                        Expiration = DateTime.Now + TokenExpiration,
                        Value = token
                    };
                    return true;
                }
            return false;
        }

        public async Task<UserExt> GetUserAsync(KeboolaUser keboolaUser)
        {
            return await _context.Customer.FirstOrDefaultAsync(a => a.KeboolaUser.KeboolaId == keboolaUser.KeboolaId);
        }

        public async Task<ConversationExt> GetConversationAsync(KeboolaUser keboolaUser)
        {
            return
                await _context.Conversation.Where(a => a.User.KeboolaUser.KeboolaId == keboolaUser.KeboolaId)
                    .FirstOrDefaultAsync();
        }
    }
}