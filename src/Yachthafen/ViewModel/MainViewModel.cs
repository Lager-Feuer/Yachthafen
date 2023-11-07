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
using Windows.Storage;
using Windows.Storage.Streams;
using System.Windows.Media.Animation;
using System.Net.Http;
using System.ComponentModel.Design;
using Newtonsoft.Json.Linq;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Windows.Media;

namespace Yachthafen.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        // Commands
        public ICommand ChangeImageCommand { get; set; }
        public ICommand BerthSelectionChangedCommand { get; set; }
        public ICommand CellChangedCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        // Views and Collections for Views
        private ObservableCollection<Berth> _berths;
        private Berth _selectedBerth;
        private byte[] _imageBytes;

        public ObservableCollection<Berth> Berths { get { return _berths; } set { _berths = value; NotifyPropertyChanged(); } }
        public Berth SelectedBerth { get { return _selectedBerth; } set { _selectedBerth = value; NotifyPropertyChanged(); } }
        public byte[] ImageBytes { get { return _imageBytes; } set { _imageBytes = value; NotifyPropertyChanged(); } }

        // Constructor
        public MainViewModel()
        {
            Berths = new ObservableCollection<Berth>();
            ChangeImageCommand = new DelegateCommand(x => OpenImageDialog());
            BerthSelectionChangedCommand = new DelegateCommand(x => BerthSelectionChanged());
            CellChangedCommand = new DelegateCommand(x => CellChanged());
            SaveCommand = new DelegateCommand(x => SaveData(true));

            LoadData();
            if(Berths.Count > 0) ImageBytes = Berths[0].BerthImage;
        }

        // General methods
        private void LoadData()
        {
            try
            {
                using (StreamReader JsonStreamReader = new StreamReader(@"res/json/data.json"))
                {
                    string json = JsonStreamReader.ReadToEnd();
                    Berths = JsonConvert.DeserializeObject<ObservableCollection<Berth>>(json);
                }
            }
            catch { }
        }
        public void SaveData(bool isCommand)
        {
            string berthString = JsonConvert.SerializeObject(Berths);
            File.WriteAllText(@"res/json/data.json", berthString);

            if (isCommand) confirmView("Daten wurden gespeichert.");
        }

        private void confirmView(string message)
        {
            View.ConfirmationView confirmationView = new View.ConfirmationView();
            confirmationView.messageLabel.Text = message;
            confirmationView.ShowDialog();
            confirmationView.Close();
        }

        // Converter methods
        private byte[] BitmapToBytes(object value, bool isTransformed)
        {
            byte[] imageBytes;

            using (MemoryStream ms = new MemoryStream())
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();

                if (!isTransformed) encoder.Frames.Add(BitmapFrame.Create((BitmapImage)value));
                else encoder.Frames.Add(BitmapFrame.Create((TransformedBitmap)value));
                encoder.Save(ms);
                imageBytes = ms.ToArray();
            }
            return imageBytes;
        }

        private BitmapImage BytesToBitmap(byte[] value)
        {
            if (value is byte[] byteArray)
            {
                BitmapImage imageSource = new BitmapImage();
                imageSource.BeginInit();
                imageSource.StreamSource = new MemoryStream(byteArray);
                imageSource.EndInit();
                return imageSource;
            }
            return null;
        }


        // Command methods for View
        private void OpenImageDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.gif;*.bmp|All Files|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                if (SelectedBerth == null)
                {
                    confirmView("Select an entry first");
                    return;
                }
                Uri uri = new Uri(openFileDialog.FileName);
                BitmapImage selectedImg = new BitmapImage(uri);
                var targetMap = new TransformedBitmap(selectedImg, new ScaleTransform(0.5, 0.5));

                Berths[Berths.IndexOf(SelectedBerth)].BerthImage = BitmapToBytes(targetMap, true);
                ImageBytes = Berths[Berths.IndexOf(SelectedBerth)].BerthImage;
            }
        }

        private void BerthSelectionChanged()
        {
            if (SelectedBerth == null) return;
            ImageBytes = Berths[Berths.IndexOf(SelectedBerth)].BerthImage;
        }

        private void CellChanged()
        {
            for (int i = 0; i < Berths.Count; i++)
            {
                Berths[i].BerthID = i + 1;
            }
        }
    }
}
