
using System.Web.Mvc;
using RAPSimple.DAL;

namespace RAPSimple.Controllers
{
    public class FileController : Controller
    {
        private RAPDbContext db = new RAPDbContext();
        // GET: File
        public ActionResult Index(int id)
        {
            var fileToRetrieve = db.Files.Find(id);
            return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
        }
    }
}