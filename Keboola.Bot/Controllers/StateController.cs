using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Description;
using Keboola.Shared.Models;

namespace Keboola.Bot.Controllers
{
    public class StateModel
    {
        [Required]
        public bool Active { get; set; }

        [Required]
        public string Token { get; set; }
    }

    /// <summary>
    /// State controller manage acces from keboola backend to chatbota backend.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/State")]
    public class StateController : ApiController
    {
        private IDatabaseContext db = new DatabaseContext();
        private int tokenExpirationDays = 0;// int.Parse(WebConfigurationManager.AppSettings["KeboolaTokenExpirationDays"]);

        public StateController()
        {
        }

        public StateController(IDatabaseContext db, int tokenExpirationDays = 30)
        {
            this.db = db;
            this.tokenExpirationDays = tokenExpirationDays;
        }

        // POST api/state
        [ResponseType(typeof(void))]
        public IHttpActionResult Post([FromBody] StateModel state)
        {
            //Validate input
            if (!ModelState.IsValid)
                return StatusCode(HttpStatusCode.BadRequest);
            try
            {
                //If token exist
                if (db.KeboolaToken.Any(a => a.Value == state.Token))
                    return StatusCode(HttpStatusCode.Conflict);
                KeboolaToken newToken = new KeboolaToken()
                {
                    Value = state.Token,
                    Expiration = DateTime.Now + TimeSpan.FromDays(tokenExpirationDays-1)
                };

                //Add token
                db.KeboolaToken.Add(newToken);
                KeboolaUser newUser = new KeboolaUser()
                {
                    Token = newToken,
                    Active = state.Active
                };

                //Add new user
                db.KeboolaUser.Add(newUser);
                db.SaveChanges();
                return StatusCode(HttpStatusCode.OK);
            }
            catch (DbEntityValidationException)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
        }

        //PUT api/state/654dfs6sd54f
        [ResponseType(typeof(void))]
        public IHttpActionResult Put(string token, [FromBody] StateModel state)
        {
            //Validate input
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (String.IsNullOrEmpty(token))
                return BadRequest();
            try
            {
                //Find exist record
                var keboolaUser = db.KeboolaUser.FirstOrDefault(a => a.Token.Value == token);

                //Ignor expired tokens or non-exist
                if (keboolaUser == null || DateTime.Now > keboolaUser.Token.Expiration)
                    return StatusCode(HttpStatusCode.NotFound);
                keboolaUser.Active = state.Active;
                db.SaveChanges();
                return StatusCode(HttpStatusCode.OK);
            }
            catch (DbEntityValidationException)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
        }
    }
}