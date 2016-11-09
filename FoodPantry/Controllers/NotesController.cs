using FoodPantry.Data.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace FoodPantry.Controllers
{
    [Authorize(Roles = "FoodPantry_Users")]
    public class NotesController : Controller
    {
        /// <summary>
        /// Access to the food pantry database.
        /// </summary>
        private readonly Data.Models.FoodPantryDataModel _db = new Data.Models.FoodPantryDataModel();

        // POST: Notes/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "StudentIDNO,Notes")] Note newNote)
        {
            var note = _db.Notes.FirstOrDefault(s => s.StudentIDNO == newNote.StudentIDNO);
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
                _db.Notes.Add(note);
            }
            else
            {
                _db.Entry(note).State = EntityState.Modified;
            }
            _db.SaveChanges();
            return RedirectToAction("Index", "Home", new { studentId = note.StudentIDNO });
        }
    }
}