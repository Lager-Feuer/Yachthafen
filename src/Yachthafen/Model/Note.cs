using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yachthafen.Model
{
    class Note : ViewModelBase
    {
        private string _title;
        private string _content;

        public string Title { get { return _title; } set { _title = value; NotifyPropertyChanged(); } }
        public string Content { get { return _content; } set { _content = value; NotifyPropertyChanged(); } }
    }
}
