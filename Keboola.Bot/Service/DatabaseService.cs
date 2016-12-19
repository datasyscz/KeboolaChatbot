using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Keboola.Bot.Controllers;
using Keboola.Shared.Models;

namespace Keboola.Bot.Service
{
    public class DatabaseService
    {
        private IDatabaseContext _context;

        public DatabaseService(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<bool> TokenExistAsync(string token)
        {
            return await _context.KeboolaUser.AnyAsync(a => a.Token.Value == token);
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
    }
}