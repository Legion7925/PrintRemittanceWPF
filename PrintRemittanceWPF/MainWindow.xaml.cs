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
    }
}