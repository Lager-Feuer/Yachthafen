using System.Collections.ObjectModel;

namespace Yachthafen.Model
{
    class Notebook : ViewModelBase
    {
        public Notebook()
        {
            this.Notizen = new ObservableCollection<Note>();
        }

        private string _title;

        public ObservableCollection<Note> Notizen { get; set; }
        public string Title { get { return _title; } set { _title = value; NotifyPropertyChanged(); } }
    }
}
