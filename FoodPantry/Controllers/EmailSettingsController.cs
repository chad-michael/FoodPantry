using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using FoodPantry.Models;
using PagedList;

namespace FoodPantry.Controllers
{
    /// <summary>
    /// Manage email settings for the webapp.
    /// </summary>
    [Authorize(Roles = "FoodPantry_Admins")]
    public class EmailSettingsController : Controller
    {
        /// <summary>
        /// Access to the food pantry database.
        /// </summary>
        private Models.FoodPantry db = new Models.FoodPantry();

        /// <summary>
        /// Displays all the email settings.
        /// </summary>
        /// <param name="page">to display</param>
        /// <returns>the index view.</returns>
        // GET: EmailSettings
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(db.EmailSettings.OrderBy(s => s.EmailID).ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// Details will display information about the email settings. We will keep in place but 
        /// just rediret to edit action.
        /// </summary>
        /// <param name="id">of settings to look at.</param>
        /// <returns>The details view.</returns>
        // GET: EmailSettings/Details/5
        public ActionResult Details(int? id)
        {
            return RedirectToAction("Edit", new { @id = id });
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmailSetting emailSetting = db.EmailSettings.Find(id);
            if (emailSetting == null)
            {
                return HttpNotFound();
            }
            return View(emailSetting);
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
                db.EmailSettings.Add(emailSetting);
                db.SaveChanges();
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
            EmailSetting emailSetting = db.EmailSettings.Find(id);
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
            db.Entry(emailSetting).State = EntityState.Modified;
            db.SaveChanges();
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
            EmailSetting emailSetting = db.EmailSettings.Find(id);
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
            EmailSetting emailSetting = db.EmailSettings.Find(id);
            db.EmailSettings.Remove(emailSetting);
            db.SaveChanges();
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
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
