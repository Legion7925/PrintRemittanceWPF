using PrintRemittance.Core.Interfaces.Repositories;
using System.Windows;

namespace PrintRemittanceWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IDocumentsRepository documentsRepository;

        public MainWindow(IDocumentsRepository documentsRepository)
        {
            InitializeComponent();
            this.documentsRepository = documentsRepository;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }


        private void btnAddDocument_Click(object sender, RoutedEventArgs e)
        {
            new AddDocumentWindow(documentsRepository).ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void gridHears_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnReportRemoveFilter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnReportRemitance_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEditRemitance_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnDeleteRemitance_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}