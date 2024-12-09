using Newtonsoft.Json;
using System.IO;

namespace NoteApp.Controllers
{
    public class NoteProjectManager
    {
        private const string _projectName = "Note.notes";

        private readonly string _currentUser;
        private readonly string _projectPath;
        private readonly string _fullFilePath;

        public NoteProject NoteProject { get; set; }
        /// <summary>
        /// Конструктор для создания менеджера проекта
        /// </summary>
        public NoteProjectManager()
        {
            _currentUser = Environment.UserName;
            _projectPath = $"C:\\Users\\{_currentUser}\\Documents";
            _fullFilePath = $"{_projectPath}\\{_projectName}";
            NoteProject = new NoteProject();
        }
        /// <summary>
        /// Сохранение проекта в файл
        /// </summary>
        public void SaveProject()
        {
            try
            {
                // создаем сериалайзер, для сохранения объекта
                var serializer = new JsonSerializer();
                // открываем поток для записи файла
                using (var streamWritter = new StreamWriter(_fullFilePath))
                using (var jsonWritter = new JsonTextWriter(streamWritter))
                {
                    serializer.Serialize(jsonWritter, NoteProject); // запись проекта в json
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Загрузка проекта из файла
        /// </summary>
        public void LoadProject()
        {
            try
            {
                // создаем ltсериалайзер, для загрузки объекта
                var deserializer = new JsonSerializer();
                // открываем поток для чтения файла
                if (File.Exists(_fullFilePath)) // проверяем существование файла
                {
                    using (var streamReader = new StreamReader(_fullFilePath))
                    using (var jsonReader = new JsonTextReader(streamReader))
                    {
                        NoteProject = deserializer.Deserialize<NoteProject>(jsonReader) ?? new NoteProject(); // чтение json в проект 
                                                                                                              // (?? реакция, если чтение выдаст null ссылку)
                    }
                }
                else // если его нет - создаем новый пустой файл
                {
                    SaveProject();
                    LoadProject();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
