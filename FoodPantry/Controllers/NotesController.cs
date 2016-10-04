using FoodPantry.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodPantry.Controllers
{
    [Authorize(Roles = "FoodPantry_Users")]
    public class NotesController : Controller
    {
        /// <summary>
        /// Access to the food pantry database.
        /// </summary>
        private Models.FoodPantry db = new Models.FoodPantry();

        // POST: Notes/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "StudentIDNO,Notes")] Note newNote)
        {
            var note = db.Notes.Where(s => s.StudentIDNO == newNote.StudentIDNO).FirstOrDefault();
            if (note != null)
            {
                note.Notes = newNote.Notes;
            }
            else
            {
                note = newNote;
            }
            note.Updated = DateTime.Now;
            if (note.NoteID == 0)
            {
                db.Notes.Add(note);
            }
            else
            {
                db.Entry(note).State = EntityState.Modified;
            }
            db.SaveChanges();
            return RedirectToAction("Index", "Home", new { studentId = note.StudentIDNO });
        }
    }
}
