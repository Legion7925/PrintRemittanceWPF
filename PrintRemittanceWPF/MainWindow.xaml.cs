using PrintRemittance.Core.Entities;
using PrintRemittance.Core.Exception;
using PrintRemittance.Core.Interfaces.Repositories;
using PrintRemittance.Core.Models;
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

        private IEnumerable<DocumentsResultModel> _listReport = new List<DocumentsResultModel>();

        private DocumentsResultModel selectedDocument = new();

        public MainWindow(IDocumentsRepository documentsRepository)
        {
            InitializeComponent();
            this.documentsRepository = documentsRepository;
            dpFilterStart.SelectedDate = Mohsen.PersianDate.Today.AddDays(-3);
            dpFilterEnd.SelectedDate = Mohsen.PersianDate.Today;

            CartableEventsManager.updateDocuments += GetReport;
            NotificationEventsManager.showMessage += ShowSnackbarMessage;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetReport(null!, null!);
        }

        private async void GetReport(object? sender, RoutedEventArgs e)
        {
            try
            {
                _listReport = await documentsRepository.GetDocuments(new GetDocumentsQueryParameter
                {
                    StartDate = dpFilterStart.SelectedDate.ToDateTime(),
                    EndDate = dpFilterEnd.SelectedDate.ToDateTime(),
                    Destination = txtDestination.Text
                });
                if (_listReport.Any() is not true)
                {
                    ShowSnackbarMessage("داده ای برای نمایش وجود ندارد", MessageTypeEnum.Information);
                    dgReport.ItemsSource = null;
                    return;
                }
                dgReport.ItemsSource = _listReport;

            }
            catch (Exception ex)
            {
                ShowSnackbarMessage("در واکشی اطلاعات خطایی رخ داده است", MessageTypeEnum.Error);
                Logger.LogException(ex);
            }
        }

        private void btnReportRemitance_Click(object sender, RoutedEventArgs e)
        {
            GetReport(null, null!);
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
            GetReport(null!, null!);
        }

        private void btnDeleteRemitance_Click(object sender, RoutedEventArgs e)
        {
            new DeleteDocumentWindow(documentsRepository, selectedDocument.Id).ShowDialog(); 
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

        private void txtDestination_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter )
            {
                GetReport(null, null!);
            }
        }

        private void btnPrintDocument_Click(object sender, RoutedEventArgs e)
        {
            PrintManager.PrintVisual(new PrintDocumentModel
            {
                CarName = selectedDocument.CarName,
                CreatedDate = selectedDocument.CreatedDate,
                Destination = selectedDocument.Destination,
                DriverName = selectedDocument.DriverName,
                FactoryName = selectedDocument.DriverName,
                Product = selectedDocument.Product,
                PlateNumber = selectedDocument.PlateNumber
            });
        }

        private void dgReport_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            if(dgReport.SelectedItems.Count > 0)
                selectedDocument = dgReport.SelectedItem as DocumentsResultModel ?? new DocumentsResultModel();
        }
    }
}