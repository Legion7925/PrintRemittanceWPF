using PrintRemittance.Core.Entities;
using PrintRemittance.Core.Interfaces.Repositories;
using PrintRemittanceWPF.Helper;
using System.Windows;

namespace PrintRemittanceWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IDocumentsRepository documentsRepository;

        private IEnumerable<Document> _listReport = new List<Document>();

        public MainWindow(IDocumentsRepository documentsRepository)
        {
            InitializeComponent();
            this.documentsRepository = documentsRepository;
            dpFilterStart.SelectedDate = Mohsen.PersianDate.Today.AddDays(-3);
            dpFilterEnd.SelectedDate = Mohsen.PersianDate.Today;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await GetReport();
        }

        private Task GetReport()
        {
            try
            {
                var report = documentsRepository.GetDocuments(new PrintRemittance.Core.Models.GetDocumentsQueryParameter
                {
                    StartDate = dpFilterStart.SelectedDate.ToDateTime(),
                    EndDate = dpFilterEnd.SelectedDate.ToDateTime(),
                    Destination = txtDestination.Text
                });
                dgReport.ItemsSource = (System.Collections.IEnumerable)report;
            }
            catch (Exception)
            {
                ShowSnackbarMessage("در واکشی اطلاعات خطایی رخ داده است", MessageTypeEnum.Error);
            }

            return Task.CompletedTask;
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
            txtDestination.Text = string.Empty;
            GetReport();
        }

        private void btnReportRemitance_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _listReport = (IEnumerable<Document>)documentsRepository.GetDocuments(new PrintRemittance.Core.Models.GetDocumentsQueryParameter
                {
                    StartDate = dpFilterStart.SelectedDate.ToDateTime(),
                    EndDate = dpFilterEnd.SelectedDate.ToDateTime(),
                    Destination = txtDestination.Text
                });
                if (_listReport.Any())
                {
                    ShowSnackbarMessage("داده ای برای نمایش وجود ندارد", MessageTypeEnum.Information);
                    dgReport.ItemsSource = null;
                    return;
                }
                else
                {
                    dgReport.ItemsSource = _listReport;
                }

            }
            catch (Exception)
            {
                ShowSnackbarMessage("در واکشی اطلاعات خطایی رخ داده است", MessageTypeEnum.Error);
            }
        }

        private void btnEditRemitance_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnDeleteRemitance_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ShowSnackbarMessage(object? sender, MessageTypeEnum messageType)
        {
            if (sender == null)
                return;

            switch (messageType)
            {
                case MessageTypeEnum.Success:
                    snackBarSuccess.MessageQueue?.Enqueue((string)sender, null, null, null, false, true, TimeSpan.FromSeconds(3));
                    break;
                case MessageTypeEnum.Error:
                    snackBarError.MessageQueue?.Enqueue((string)sender, null, null, null, false, true, TimeSpan.FromSeconds(3));
                    break;
                case MessageTypeEnum.Warning:
                    snackBarWarning.MessageQueue?.Enqueue((string)sender, null, null, null, false, true, TimeSpan.FromSeconds(3));
                    break;
                case MessageTypeEnum.Information:
                    snackBarInfo.MessageQueue?.Enqueue((string)sender, null, null, null, false, true, TimeSpan.FromSeconds(3));
                    break;
                default:
                    snackBarInfo.MessageQueue?.Enqueue((string)sender, null, null, null, false, true, TimeSpan.FromSeconds(3));
                    break;
            }
        }
    }
}