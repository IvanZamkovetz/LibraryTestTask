using Library.Model;
using Library.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Library.Web.Controllers
{
    public class BooksController: Controller
    {
        public LibraryContext context { get; set; }

        public BooksController()
        {
            context = new LibraryContext();
        }

        public ActionResult Index()
        {
            List<Book> books = context.Books.ToList();

            return View(books);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book detailedBook = context.Books.Find(id);

            return View(detailedBook);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create([Bind(Include = "Name, Pages, PublishingDate, PublishingHouse")]Book book)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    context.Books.Add(book);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(book);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book bookToUpdate = context.Books.Find(id);

            return View(bookToUpdate);
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int id)
        {
            Book bookToUpdate = context.Books.Find(id);
            if (TryUpdateModel(bookToUpdate, "",
               new string[] { "Name", "Pages", "PublishingDate", "PublishingHouse" }))
            {
                try
                {
                    context.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(bookToUpdate);
        }
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Book bookToDelete = context.Books.Find(id);
            if (bookToDelete == null)
            {
                return HttpNotFound();
            }
            return View(bookToDelete);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Book bookToDelete = context.Books.Find(id);
                context.Books.Remove(bookToDelete);
                context.SaveChanges();
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }
    }
}