using Library.Model;
using Library.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Library.Web.Controllers
{
    public class PublicationsController : Controller
    {
        public LibraryContext context { get; set; }

        public PublicationsController()
        {
            context = new LibraryContext();
        }

        // GET: Publications
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAll()
        {
            List<GetAllPublicationsViewModel> publications = new List<GetAllPublicationsViewModel>();
            publications.AddRange(context.Books.Select(bk => new GetAllPublicationsViewModel() { Name = bk.Name, Type = "book" }));
            publications.AddRange(context.Magazines.Select(m => new GetAllPublicationsViewModel() { Name = m.Name, Type = "magazine" }));

            return Json(publications, JsonRequestBehavior.AllowGet);
        }
    }
}