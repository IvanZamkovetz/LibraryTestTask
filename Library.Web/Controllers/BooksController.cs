using Library.Model;
using Library.Web.Models;
using PagedList;
using PagedList.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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

        public async Task<ActionResult> Index(IndexBooksViewModel model)
        {
            model.Page = model.Page < 1 ? 1 : model.Page;

            IQueryable<Book> books = from bk in context.Books
                                     select bk;

            if (!String.IsNullOrEmpty(model.Filter))
            {
                books = books.Where(bk => bk.Name.Contains(model.Filter));
            }
            switch (model.Sort)
            {
                case "name_desc":
                    books = books.OrderByDescending(b => b.Name);
                    break;
                case "date":
                    books = books.OrderBy(b => b.PublishingDate);
                    break;
                case "date_desc":
                    books = books.OrderByDescending(b => b.PublishingDate);
                    break;
                default:
                    books = books.OrderBy(b => b.Name);
                    break;
            }
            int pageSize = 3;
            model.Books = await books.ToPagedListAsync(model.Page, pageSize);

            return View(model);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, byte[] rowVersion)
        {
            string[] fieldsToBind = new string[] { "Name", "Pages", "PublishingDate", "PublishingHouse", "RowVersion" };

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var bookToUpdate = await context.Books.FindAsync(id);
            if (bookToUpdate == null)
            {
                Book deletedBook = new Book();
                TryUpdateModel(deletedBook, fieldsToBind);
                ModelState.AddModelError(string.Empty,
                    "Unable to save changes. The Book was deleted by another user.");
                return View(deletedBook);
            }

            if (TryUpdateModel(bookToUpdate, fieldsToBind))
            {
                try
                {
                    context.Entry(bookToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    await context.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Book)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The Book was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Book)databaseEntry.ToObject();

                        if (databaseValues.Name != clientValues.Name)
                            ModelState.AddModelError("Name", "Current value: "
                                + databaseValues.Name);
                        if (databaseValues.Pages != clientValues.Pages)
                            ModelState.AddModelError("Pages", "Current value: "
                                + String.Format("{0:c}", databaseValues.Pages));
                        if (databaseValues.PublishingDate != clientValues.PublishingDate)
                            ModelState.AddModelError("PublishingDate", "Current value: "
                                + String.Format("{0:d}", databaseValues.PublishingDate));
                        if (databaseValues.PublishingHouse != clientValues.PublishingHouse)
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                            + "was modified by another user after you got the original value. The "
                            + "edit operation was canceled and the current values in the database "
                            + "have been displayed. If you still want to edit this record, click "
                            + "the Save button again. Otherwise click the Back to List hyperlink.");

                        bookToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (RetryLimitExceededException /* dex */)
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.context.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}