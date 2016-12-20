﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Keboola.Bot.Controllers;
using Keboola.Shared;
using Keboola.Shared.Models;
using Microsoft.Bot.Connector;

namespace Keboola.Bot.Service
{
    [Serializable]
    public class DatabaseService
    {
        private static IDatabaseContext _context;

        public DatabaseService(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<bool> TokenExistAsync(string token)
        {
            return await _context.KeboolaUser.AnyAsync(a => a.Token.Value == token);
        }

        public async Task<bool> UserIsAtivated(string token)
        {
            return await _context.KeboolaUser.AnyAsync(a => a.Token.Value == token && a.Active);
        }

        public async Task<Shared.Conversation> FindConversationAsync(
           IMessageActivity activity)
        {
            return
                await _context.Conversation
                    .FirstOrDefaultAsync(
                        a =>
                            (a.FrameworkId == activity.Conversation.Id)
                            &&
                            (a.BaseUri == activity.ServiceUrl)
                        //Need check service url too, ConversationID is unique only for serviceUrl 
                    );
        }


        public async Task AddUserAndToken(StateModel state, int expirationDays)
        {
            KeboolaToken newToken = new KeboolaToken()
            {
                Value = state.Token,
                Expiration = DateTime.Now + TimeSpan.FromDays(expirationDays - 1)
            };

            //newToken = _context.KeboolaToken.Attach(newToken);
            KeboolaUser newUser = new KeboolaUser()
            {
                Token = newToken,
                Active = state.Active
            };

            //Add new user
            _context.KeboolaUser.Add(newUser);
            await _context.SaveChangesAsync();
        }

        public async Task<KeboolaUser> GetKeboolaUserByTokenAsync(string token)
        {
            return await _context.KeboolaUser.FirstOrDefaultAsync(a => a.Token.Value == token);
        }

        public List<KeboolaUser> GetExpiredUsersTokens()
        {
            DateTime now = DateTime.Now;
            return _context.KeboolaUser.Where(o => EntityFunctions.TruncateTime(o.Token.Expiration) <= now).ToList();
        }

        public async Task<string> GetIntentAsync(string id)
        {
            var intent = await _context.IntentAnswer.FirstOrDefaultAsync(a => a.Name == id);
            if (intent != null)
                return intent.Answer;
            return id;
        }
    }
}