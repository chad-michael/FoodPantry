using FoodPantry.Data.Models;
using PagedList;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace FoodPantry.Controllers
{
    /// <summary>
    /// Manage email settings for the webapp.
    /// </summary>
    [Authorize(Roles = "FoodPantry_Admins")]
    public class EmailSettingsController : Controller
    {
        private readonly FoodPantryDataModel _db = new FoodPantryDataModel();

        // GET: EmailSettings
        public ActionResult Index(int? page)
        {
            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            return View(_db.EmailSettings.OrderBy(s => s.EmailID).ToPagedList(pageNumber, pageSize));
        }

        // GET: EmailSettings/Details/5
        public ActionResult Details(int? id)
        {
            return RedirectToAction("Edit", new { @id = id });
            if (id != null)
            {
                EmailSetting emailSetting = _db.EmailSettings.Find(id);
                if (emailSetting == null)
                {
                    return HttpNotFound();
                }
                return View(emailSetting);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Create a new email setting. Not need right now.
        /// </summary>
        /// <returns>The create view.</returns>
        // GET: EmailSettings/Create
        public ActionResult Create()
        {
            return RedirectToAction("Index");
            return View();
        }

        /// <summary>
        /// Create a new email setting. Not need right now.
        /// </summary>
        /// <param name="emailSetting">to save to the database.</param>
        /// <returns>The create view or redirect if successful</returns>
        // POST: EmailSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmailID,EmailContents,SubjectContents,DateAdded,DateModified,DateDeleted,ActiveEmail,Deleted")] EmailSetting emailSetting)
        {
            return RedirectToAction("Index");
            emailSetting.DateAdded = DateTime.Now;
            emailSetting.DateModified = DateTime.Now;
            if (ModelState.IsValid)
            {
                _db.EmailSettings.Add(emailSetting);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(emailSetting);
        }

        /// <summary>
        /// Edit a given email setting.
        /// </summary>
        /// <param name="id">of the email setting.</param>
        /// <returns>the edit view.</returns>
        // GET: EmailSettings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmailSetting emailSetting = _db.EmailSettings.Find(id);
            if (emailSetting == null)
            {
                return HttpNotFound();
            }
            return View(emailSetting);
        }

        /// <summary>
        /// Displays the view to edit a email setting.
        /// </summary>
        /// <param name="id">of the email setting to edit.</param>
        /// <returns>editing view of for the email setting.</returns>
        // POST: EmailSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmailID,EmailContents,SubjectContents,DateAdded,DateModified,DateDeleted,ActiveEmail,Deleted")] EmailSetting emailSetting)
        {
            emailSetting.DateModified = DateTime.Now;
            emailSetting.DateAdded = DateTime.Now;

            if (!ModelState.IsValid) return View(emailSetting);
            _db.Entry(emailSetting).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Display information about the email settings to delete. This is not enable but we can enable later.
        /// </summary>
        /// <param name="id">of the email setting to delete.</param>
        /// <returns>the view</returns>
        // GET: EmailSettings/Delete/5
        public ActionResult Delete(int? id)
        {
            return RedirectToAction("Index");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmailSetting emailSetting = _db.EmailSettings.Find(id);
            if (emailSetting == null)
            {
                return HttpNotFound();
            }
            return View(emailSetting);
        }

        /// <summary>
        /// Confirm the deletion of the email setting. This is not enable but we can enable later.
        /// </summary>
        /// <param name="id">of email setting to delete.</param>
        /// <returns>the redirect</returns>
        // POST: EmailSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            return RedirectToAction("Index");
            EmailSetting emailSetting = _db.EmailSettings.Find(id);
            _db.EmailSettings.Remove(emailSetting);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Close database connections.
        /// </summary>
        /// <param name="disposing">is disposing controller.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}