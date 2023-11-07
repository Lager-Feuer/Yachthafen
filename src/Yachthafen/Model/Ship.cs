using System.Collections.ObjectModel;

namespace Yachthafen.Model
{
    class Ship : ViewModelBase
    {
        public Ship()
        {
            this.Notizen = new ObservableCollection<Berth>();
        }

        private string _title;

        public ObservableCollection<Berth> Notizen { get; set; }
        public string Title { get { return _title; } set { _title = value; NotifyPropertyChanged(); } }
    }
}
