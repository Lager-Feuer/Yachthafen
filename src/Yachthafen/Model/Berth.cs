using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yachthafen.Model
{
    class Berth : ViewModelBase
    {
        private int _berthID;
        private int _depth;
        private int _width;
        private int _length;
        private int _status;
        private byte[] _berthImage;

        public int BerthID { get { return _berthID; } set { _berthID = value; NotifyPropertyChanged(); } }
        public int Depth { get { return _depth; } set { _depth = value; NotifyPropertyChanged(); } }
        public int Width { get { return _width; } set { _width = value; NotifyPropertyChanged(); } }
        public int Length { get { return _length; } set { _length = value; NotifyPropertyChanged(); } }
        public int Status { get { return _status; } set { _status = value; NotifyPropertyChanged(); } }
        public byte[] BerthImage { get { return _berthImage; } set { _berthImage = value; NotifyPropertyChanged(); } }
    }
}
