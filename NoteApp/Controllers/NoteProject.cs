using Newtonsoft.Json;
using NoteApp.Models;

namespace NoteApp.Controllers
{
    public class NoteProject
    {
        public ICollection<Note> Notes { get; set; }
        [JsonIgnore]
        public Note? SelectedNote { get; set; }
        [JsonIgnore]
        public IEnumerable<Note> FilteredNotes { get; private set; }
        /// <summary>
        /// Создание пустого конструктора
        /// </summary>
        public NoteProject()
        {
            Notes = new List<Note>();
            FilteredNotes = Enumerable.Empty<Note>();
        }
        /// <summary>
        /// Созадние новой пустой заметки
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void CreateNewNote()
        {
            try
            {
                Notes.Add(new Note());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void AddNewNote(Note note)
        {
            try
            {
                Notes.Add(note);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Изменить заметку
        /// </summary>
        /// <param name="note"></param>
        /// <param name="name"></param>
        /// <param name="noteCategory"></param>
        /// <param name="text"></param>
        /// <exception cref="Exception"></exception>
        public void ChangeNote(Note note,
                               string name,
                               NoteCategory noteCategory,
                               string text)
        {
            try
            {
                note.ChangeNote(name,
                                noteCategory,
                                text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Удалить заметку
        /// </summary>
        /// <param name="note"></param>
        /// <exception cref="Exception"></exception>
        public void RemoveNote(Note note)
        {
            try
            {
                Notes.Remove(note);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Вернуть список всех категорий.
        /// </summary>
        public static IEnumerable<NoteCategory> ReturnListOfAllCategories
        {
            get
            {
                return new List<NoteCategory>()
                {
                    NoteCategory.All,
                    NoteCategory.Work,
                    NoteCategory.Home,
                    NoteCategory.HealthAndSport,
                    NoteCategory.People,
                    NoteCategory.Documents,
                    NoteCategory.Finance,
                    NoteCategory.Different,
                };
            }
        }

        /// <summary>
        /// Вернуть список категорий, которые можно задать заметки.
        /// </summary>
        public static IEnumerable<NoteCategory> ReturnListOfNoteCategories
        {
            get
            {
                return new List<NoteCategory>()
                {
                    NoteCategory.Work,
                    NoteCategory.Home,
                    NoteCategory.HealthAndSport,
                    NoteCategory.People,
                    NoteCategory.Documents,
                    NoteCategory.Finance,
                    NoteCategory.Different,
                };
            }
        }

        /// <summary>
        /// Вернуть список заметок. Если категория не является "все", то вернется список
        /// с заметками со соответсвующей категорией.
        /// </summary>
        /// <param name="noteCategory"></param>
        /// <returns></returns>
        public void SetNotesFilter(NoteCategory noteCategory)
        {
            if (noteCategory is NoteCategory.All)
            {
                FilteredNotes = Notes; // если категория "все" - вернуть все заметки
            }
            else
            {
                FilteredNotes = Notes.Where(n => n.NoteCategory.Equals(noteCategory)); // в противном случае - вернуть отфильтрованный список
            }
        }
    }
}
