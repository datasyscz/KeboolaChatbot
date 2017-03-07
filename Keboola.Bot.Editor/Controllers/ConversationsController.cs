using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Keboola.Bot.Editor.Models;
using PagedList;

namespace Keboola.Bot.Editor.Controllers
{
    [Authorize]
    public class ConversationsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Conversations
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;
            ViewBag.CurrentFilter = searchString;

            var conversationsSortedQ =
                from conversation in db.Conversation
                where !conversation.BaseUri.Contains("localhost")
                orderby (from message in conversation.Messages
                    orderby message.Date descending
                    select message.Date).FirstOrDefault()
                descending
                select conversation;

            if (!string.IsNullOrEmpty(searchString))
                conversationsSortedQ = (IOrderedQueryable<ConversationExt>) conversationsSortedQ.Where(a =>
                    a.User.Name.Contains(searchString) ||
                    a.User.UserChannel.Name.Contains(searchString));

            var query = conversationsSortedQ.
                Include(a => a.Messages)
                .Include(a => a.Messages);

            var pageSize = 10;
            var pageNumber = page ?? 1;
            var queryView = query.ToPagedList(pageNumber, pageSize);
            return View(queryView);
        }

        // GET: Conversations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var conversation = db.Conversation.Find(id);
            if (conversation == null)
                return HttpNotFound();
            return View(conversation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}