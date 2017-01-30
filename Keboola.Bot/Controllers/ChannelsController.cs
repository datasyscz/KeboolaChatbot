using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Keboola.Bot;
using Keboola.Shared;

namespace Keboola.Bot.Controllers
{
    public class ChannelsController : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/Channels
        public IQueryable<Channel> GetChannel()
        {
            return db.Channel;
        }

        // GET: api/Channels/5
        [ResponseType(typeof(Channel))]
        public async Task<IHttpActionResult> GetChannel(int id)
        {
            Channel channel = await db.Channel.FindAsync(id);
            if (channel == null)
            {
                return NotFound();
            }

            return Ok(channel);
        }

        // PUT: api/Channels/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutChannel(int id, Channel channel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != channel.Id)
            {
                return BadRequest();
            }

            db.Entry(channel).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChannelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Channels
        [ResponseType(typeof(Channel))]
        public async Task<IHttpActionResult> PostChannel(Channel channel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Channel.Add(channel);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = channel.Id }, channel);
        }

        // DELETE: api/Channels/5
        [ResponseType(typeof(Channel))]
        public async Task<IHttpActionResult> DeleteChannel(int id)
        {
            Channel channel = await db.Channel.FindAsync(id);
            if (channel == null)
            {
                return NotFound();
            }

            db.Channel.Remove(channel);
            await db.SaveChangesAsync();

            return Ok(channel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ChannelExists(int id)
        {
            return db.Channel.Count(e => e.Id == id) > 0;
        }
    }
}