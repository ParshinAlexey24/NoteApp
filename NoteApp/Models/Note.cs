namespace NoteApp.Models
{
    public class Note : ICloneable
    {
        private const string _standartNoteName = "Без названия";
        private const int _limitOfName = 50;
        public string Name { get; set; }
        public NoteCategory NoteCategory { get; set; }
        public string Text { get; set; }
        public DateTime DateOfCreate { get; set; }
        public DateTime LastDateOfChange { get; set; }
        /// <summary>
        /// Создание новой пустой заметки
        /// </summary>
        public Note()
        {
            Name = _standartNoteName;
            NoteCategory = NoteCategory.Different;
            Text = string.Empty;
            DateOfCreate = DateTime.Now;
            LastDateOfChange = DateTime.Now;
        }
        /// <summary>
        /// Закрытый конструктор, для клонирования заметки
        /// </summary>
        /// <param name="name"></param>
        /// <param name="noteCategory"></param>
        /// <param name="text"></param>
        /// <param name="dateOfCreate"></param>
        /// <param name="lastDateOfChange"></param>
        private Note(string name,
                     NoteCategory noteCategory,
                     string text,
                     DateTime dateOfCreate,
                     DateTime lastDateOfChange)
        {
            Name = name;
            NoteCategory = noteCategory;
            Text = text;
            DateOfCreate = dateOfCreate;
            LastDateOfChange = lastDateOfChange;
        }
        /// <summary>
        /// Метод изменения заметки
        /// </summary>
        /// <param name="name"></param>
        /// <param name="noteCategory"></param>
        /// <param name="text"></param>
        /// <exception cref="ArgumentException"></exception>
        public void ChangeNote(string name,
                               NoteCategory noteCategory,
                               string text)
        {
            if (name.Length <= _limitOfName)
            {
                Name = name;
                NoteCategory = noteCategory;
                Text = text;
                LastDateOfChange = DateTime.Now;
            }
            else
            {
                throw new ArgumentException("Название заметки не может быть длинее 50 символов!");
            }
        }
        /// <summary>
        /// Клонирование существующей заметки
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new Note(Name,
                            NoteCategory,
                            Text,
                            DateOfCreate,
                            LastDateOfChange);
        }
    }
}
