using Yachthafen.ViewModel;
using System;
using Yachthafen.Model;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Yachthafen.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ConfirmationView
    {
        public bool isActionConfirmed { get; set; }
        public ConfirmationView()
        {
            InitializeComponent();
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            isActionConfirmed = true;
            Close();
        }
        private void Cancel(object sender, RoutedEventArgs e)
        {
            isActionConfirmed = false;
            Close();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (FileMenu != null)
                FileMenu.Visibility = Visibility.Collapsed;

            if (CheckUpdateButton != null)
                CheckUpdateButton.Visibility = Visibility.Collapsed;

            if (MaximizeButton != null)
                MaximizeButton.Visibility = Visibility.Collapsed;

            if (RestoreButton != null)
                RestoreButton.Visibility = Visibility.Collapsed;

            if (HelpMenu != null)
                HelpMenu.Visibility = Visibility.Collapsed;

            if (MinimizeButton != null)
                MinimizeButton.Visibility = Visibility.Collapsed;
            //MinimizeButton.SetValue(Grid.ColumnProperty, 7);
        }
    }
}
