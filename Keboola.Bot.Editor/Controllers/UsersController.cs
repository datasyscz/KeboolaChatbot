using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using Keboola.Bot.Editor.Models;

namespace Keboola.Bot.Editor.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Users
        public async Task<ActionResult> Index()
        {
            return View(await db.Customer.ToListAsync());
        }

        //public async Task<ActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    int parse = int.Parse(id);

        //    var customer = await db.Customer.FirstOrDefaultAsync(a => a.Id == parse);
        //    if (customer == null)
        //    {
        //        return HttpNotFound();
        //    }


        //     db.Customer.Remove(customer);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}