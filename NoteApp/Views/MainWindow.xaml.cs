using NoteApp.Controllers;
using NoteApp.Models;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;

namespace NoteApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly NoteProjectManager _noteProjectManager;
        private readonly NoteProject _currentNoteProject;
        /// <summary>
        /// Конструктор основного окна
        /// </summary>
        public MainWindow()
        {
            try
            {
                _noteProjectManager = new NoteProjectManager();
                _noteProjectManager.LoadProject();
                _currentNoteProject = _noteProjectManager.NoteProject;
                DataContext = _currentNoteProject;
                InitializeComponent();
                SetNotesListBoxItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }
        /// <summary>
        /// Метод обновления списка заметок
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void UpdateListBox()
        {
            try
            {
                if (NotesListBox != null)
                {
                    NotesListBox.ItemsSource = _currentNoteProject.FilteredNotes;
                    NotesListBox.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Событие при изменения выбора фильтра по категориям
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            { 
                var selectedNote = NotesListBox.SelectedItem as Note ?? new Note();
                SelectNote(selectedNote);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Метод при изменении выбора заметки
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void SetNotesListBoxItems()
        {
            try
            {
                var selectedNoteCategory = (NoteCategory)NoteCategoryComboBox.SelectedItem;
                _currentNoteProject.SetNotesFilter(selectedNoteCategory);
                UpdateListBox();
                if (_currentNoteProject.SelectedNote != null)
                {
                    NotesListBox.SelectedIndex = -1;
                    SelectNote(null);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Метод для отправки записки (или пустоты) в border для привязок
        /// </summary>
        /// <param name="note"></param>
        private void SelectNote(Note? note)
        {
            _currentNoteProject.SelectedNote = note;
            NoteBorder.DataContext = _currentNoteProject.SelectedNote;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoteCategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SetNotesListBoxItems();
                UpdateListBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Событие при закрытии окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                _noteProjectManager.SaveProject();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }
        /// <summary>
        /// Событие при нажатии кнопки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CreateNewNote();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChangeNote();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RemoveNote();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Метод создания заметки
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void CreateNewNote()
        {
            try
            {
                var note = new Note();
                _currentNoteProject.AddNewNote(note);
                var workNoteWindow = new WorkNoteWindow(_currentNoteProject, note);
                workNoteWindow.OnCreateOrChangeNote += UpdateListBox;
                workNoteWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Метод изменения заметки
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void ChangeNote()
        {
            try
            {
                if (NotesListBox.SelectedIndex != -1)
                {

                    var note = NotesListBox.SelectedItem as Note ?? new Note();
                    var workNoteWindow = new WorkNoteWindow(_currentNoteProject, note);
                    workNoteWindow.OnCreateOrChangeNote += UpdateListBox;
                    workNoteWindow.ShowDialog();
                }
                else
                {
                    throw new Exception("Не выбран элемент для изменения!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Метод удаления заметки
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void RemoveNote()
        {
            try
            {
                if (NotesListBox.SelectedIndex != -1)
                {
                    var result = MessageBox.Show("Вы уверены, что хотите удалить заметку?", "Удаление", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                    if (result.Equals(MessageBoxResult.OK))
                    {
                        var note = NotesListBox.SelectedItem as Note ?? new Note();
                        _currentNoteProject.RemoveNote(note);
                        NotesListBox.SelectedIndex = -1;
                        SelectNote(null);
                        UpdateListBox();
                    }
                }
                else
                {
                    throw new Exception("Не выбран элемент для удаления!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Событие при нажатии на меню Close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }
        /// <summary>
        /// Событие при нажатии на меню Add note
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNoteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CreateNewNote();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Событие при нажатии на меню Edit note
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditNoteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChangeNote();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Событие при нажатии на меню Remove note
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveNoteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RemoveNote();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Событие при нажатии на меню about
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                var aboutWindow = new AboutWindow();
                aboutWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}