using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Keboola.Bot.Editor.Models;
using Keboola.Shared;

namespace Keboola.Bot.Editor.Controllers
{
    [Authorize]
    public class IntentAnswersController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: IntentAnswers
        public async Task<ActionResult> Index()
        {
            return View(await db.IntentAnswer.ToListAsync());
        }

        // GET: IntentAnswers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var intentAnswer = await db.IntentAnswer.FindAsync(id);
            if (intentAnswer == null)
                return HttpNotFound();
            return View(intentAnswer);
        }

        // GET: IntentAnswers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: IntentAnswers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Answer,Advanced")] IntentAnswer intentAnswer)
        {
            if (ModelState.IsValid)
            {
                intentAnswer.Advanced = false;
                db.IntentAnswer.Add(intentAnswer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(intentAnswer);
        }

        // GET: IntentAnswers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var intentAnswer = await db.IntentAnswer.FindAsync(id);
            if (intentAnswer == null)
                return HttpNotFound();
            return View(intentAnswer);
        }

        // POST: IntentAnswers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Answer,Advanced")] IntentAnswer intentAnswer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(intentAnswer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(intentAnswer);
        }

        // GET: IntentAnswers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var intentAnswer = await db.IntentAnswer.FindAsync(id);
            if (intentAnswer == null)
                return HttpNotFound();
            return View(intentAnswer);
        }

        // POST: IntentAnswers/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var intentAnswer = await db.IntentAnswer.FindAsync(id);
            db.IntentAnswer.Remove(intentAnswer);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}