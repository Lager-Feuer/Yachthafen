using Yachthafen.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using Yachthafen.Model;

namespace Yachthafen.View
{
    public partial class MainView : SWWindow
    {
        public static MainView MainViewInstance { get; private set; }
        public MainView()
        {
            //DataContext = new MainViewModel();
            MainViewInstance = this;
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.CloseButton != null)
            {
                this.CloseButton.Click -= CloseButton_Click;
                this.CloseButton.Click += CloseButtonNew_Click;
            }
        }

        protected void CloseButtonNew_Click(object sender, RoutedEventArgs e)
        {
            ConfirmationView confirmationView = new ConfirmationView();
            confirmationView.Height = 200;
            confirmationView.messageLabel.Text = "Möchten Sie das Programm wirklich beenden?\nAlle Änderungen werden automatisch gespeichert.";
            confirmationView.cancelBtn.Visibility = Visibility.Visible;
            confirmationView.ShowDialog();
            confirmationView.Close();
            if (confirmationView.isActionConfirmed)
                Application.Current.Shutdown();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            MainViewInstance = null;
            ((MainViewModel)DataContext).SaveData(false);
        }

        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "BerthImage" || e.PropertyName == "BerthID" || e.PropertyName == "Length" || e.PropertyName == "Width" || e.PropertyName == "Depth")
            {
                e.Column = null;
            }
        }
    }
}
