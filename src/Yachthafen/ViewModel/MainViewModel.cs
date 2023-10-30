using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Yachthafen.Model;

namespace Yachthafen.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        // Commands
        public ICommand NewNotebookCommand { get; set; }
        public ICommand NewNoteCommand { get; set; }
        public ICommand DeleteSelectedNoteCommand { get; set; }
        public ICommand DeleteSelectedNotebookCommand { get; set; }

        // Views and Collections for Views
        public ObservableCollection<Notebook> NotebookCollection = new ObservableCollection<Notebook>();
        public ObservableCollection<object> DynamicNotesCollection = new ObservableCollection<object>();
        public ICollectionView NotebookView { get; set; }
        public ICollectionView NotesListView { get; set; }
        public ICollectionView DynamicContentView { get; set; }

        public MainViewModel()
        {
            getSavedNotebooks();
            // Commands
            NewNotebookCommand = new DelegateCommand(x => AddNotebook());
            NewNoteCommand = new DelegateCommand(x => AddNote());
            DeleteSelectedNoteCommand = new DelegateCommand(x => DeleteNote());
            DeleteSelectedNotebookCommand = new DelegateCommand(x => DeleteNotebook());

            // Views
            NotebookView = CollectionViewSource.GetDefaultView(NotebookCollection);
            DynamicContentView = CollectionViewSource.GetDefaultView(DynamicNotesCollection);
        }

        // Private Methoden
        private void confirmView(string message)
        {
            View.ConfirmationView confirmationView = new View.ConfirmationView();
            confirmationView.messageLabel.Text = message;
            confirmationView.ShowDialog();
            confirmationView.Close();
        }
        private void AddNotebook()
        {
            Notebook newNotebook = new Notebook() { Title = "New-Notebook" };
            NotebookCollection.Add(newNotebook);
        }

        private void DeleteNotebook()
        {
            ListView notebookView = View.MainView.MainViewInstance.NotebookView;
            ListView notesView = View.MainView.MainViewInstance.NotesListView;
            Notebook selectedNotebook = (Notebook)notebookView.SelectedItem;

            if (selectedNotebook != null)
                NotebookCollection.Remove(selectedNotebook);
            else
                confirmView("Wählen Sie vorher ein Notizbuch aus, welches gelöscht werden soll.");

            if (NotebookCollection.Count == 0)
            {
                notesView.ItemsSource = null;
                notesView.Items.Clear();
            }
        }

        private void AddNote()
        {
            ListView notebookView = View.MainView.MainViewInstance.NotebookView;
            Notebook selectedNotebook = (Notebook)notebookView.SelectedItem;
            Note note = new Note() { Title = "Neue Notiz", Content = "Neuer Inhalt" };
            if (notebookView.SelectedItem != null)
                selectedNotebook.Notizen.Add(note);
            else
                confirmView("Wählen Sie zunächst ein Notizbuch aus, zu dem Sie eine Notiz hinzufügen möchten.");
        }

        private void DeleteNote()
        {
            ListView notebookView = View.MainView.MainViewInstance.NotebookView;
            ListView notesView = View.MainView.MainViewInstance.NotesListView;
            Notebook selectedNotebook = (Notebook)notebookView.SelectedItem;
            if ((Note)notesView.SelectedItem != null)
            {
                Note note = (Note)notesView.SelectedItem;
                selectedNotebook.Notizen.Remove(note);
            }
            else
                confirmView("Wählen Sie vorher eine Notiz aus, die gelöscht werden soll.");
        }

        private void getSavedNotebooks()
        {
            try
            {
                using (StreamReader JsonStreamReader = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\note-it\notebooks\notebooks.json"))
                {
                    string json = JsonStreamReader.ReadToEnd();
                    NotebookCollection = JsonConvert.DeserializeObject<ObservableCollection<Notebook>>(json);
                }
            }
            catch { }
        }

        //Public Methoden für SelectionChanged-Events und Public-Methode zum Speichern der Notebooks

        public void saveNotebooks()
        {
            string notebooks = JsonConvert.SerializeObject(NotebookCollection);
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\note-it\notebooks\notebooks.json", notebooks);
        }

        public void ListViewSelectionChanged(Note noteItem)
        {
            DynamicNotesCollection.Clear();
            if (noteItem != null)
                DynamicNotesCollection.Add(noteItem);
        }

        public void NotebookViewSelectionChanged(Notebook notebookItem)
        {
            ListView notesView = View.MainView.MainViewInstance.NotesListView;
            if (notebookItem != null)
                notesView.ItemsSource = notebookItem.Notizen;
        }

        public void TextBoxTextChanged(String searchText)
        {
            try
            {
                Note noteItem = DynamicNotesCollection[0] as Note;
                if (noteItem.Content.Contains(searchText))
                    MessageBox.Show("Notiz beinhaltet den Text");
            }
            catch { }
        }
    }
}
