using NoteApp.Controllers;
using NoteApp.Models;
using System.Windows;
using System.Windows.Controls;

namespace NoteApp.Views
{
    /// <summary>
    /// Логика взаимодействия для CreateNoteWindow.xaml
    /// </summary>
    public partial class WorkNoteWindow : Window
    {
        public delegate void UpdateNoteList();
        public event UpdateNoteList OnCreateOrChangeNote;

        private readonly NoteProject _noteProject;

        private readonly Note _note;
        /// <summary>
        /// Конструктор окна работы с заметкой
        /// </summary>
        /// <param name="noteProject"></param>
        /// <param name="note"></param>
        public WorkNoteWindow(NoteProject noteProject, Note note)
        {
            _noteProject = noteProject;
            _note = note;
            DataContext = _note;
            InitializeComponent();
            NoteCategoryComboBox.ItemsSource = NoteProject.ReturnListOfNoteCategories;
            NoteCategoryComboBox.SelectedItem = _note.NoteCategory;
        }
        /// <summary>
        /// Событие при нажатии на кнопку OK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsValidInfo())
                {
                    string name = NameTextBox.Text;
                    var noteCategory = (NoteCategory)NoteCategoryComboBox.SelectedItem;
                    string text = TextNoteTextBox.Text ?? string.Empty;
                    _noteProject.ChangeNote(_note,
                                            name,
                                            noteCategory,
                                            text);
                    MessageBox.Show("Операция успешно завершена", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                    OnCreateOrChangeNote?.Invoke();
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Событие при нажатии на кнопку отмены
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Вы уверены, что хотите выйти? Все изменения не сохранятся!", "Выход", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (result.Equals(MessageBoxResult.OK))
                {
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }
        /// <summary>
        /// Метод на проверку объективность и целостности данных
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private bool IsValidInfo()
        {
            if ((NameTextBox.Text ?? string.Empty).Equals(string.Empty))
            {
                throw new Exception("Наименование заметки не может быть пустым!");
            }
            if (NoteCategoryComboBox.SelectedIndex.Equals(-1))
            {
                throw new Exception("Заметка должна иметь категорию!");
            }
            return true;
        }
    }
}
