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

        private int _pageSize = 5;
        private int _pageIndex = 1;
        private double _totalPages = 1;
        private double _totalCount = 0;

        public MainWindow(IDocumentsRepository documentsRepository)
        {
            InitializeComponent();
            this.documentsRepository = documentsRepository;
            dpFilterStart.SelectedDate = Mohsen.PersianDate.Today.AddDays(-3);
            dpFilterEnd.SelectedDate = Mohsen.PersianDate.Today;

            CartableEventsManager.updateDocuments += btnReportRemitance_Click!;
            NotificationEventsManager.showMessage += ShowSnackbarMessage;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillPaginationComboBox();
            btnReportRemitance_Click(null!, null!);
        }


        private async void FillDatagrid(object? sender, EventArgs? e)
        {
            _pageSize = Convert.ToInt32(cmbPaginationSize.SelectedValue);
            _totalPages = Math.Ceiling(_totalCount / _pageSize);
            if (_totalPages == 0)
                _totalPages = 1;

             GetPagination();

            if (_totalCount == 0)
            {
                btnLastPage.IsEnabled = false;
                btnFirstPage.IsEnabled = false;
                btnNextPage.IsEnabled = false;
                btnPreviousPage.IsEnabled = false;
            }
            else
            {
                btnLastPage.IsEnabled = true;
                btnFirstPage.IsEnabled = true;
                btnNextPage.IsEnabled = true;
                btnPreviousPage.IsEnabled = true;
            }

            if (_pageIndex == 1)
            {
                btnPreviousPage.IsEnabled = false;
                btnFirstPage.IsEnabled = false;
            }

            if(_pageIndex == _totalPages)
            {
                btnLastPage.IsEnabled = false;
                btnNextPage.IsEnabled = false;
            }
            else
            {
                btnLastPage.IsEnabled = true;
                btnNextPage.IsEnabled = true;
            }
            lblTotalCount.Text = _totalCount.ToString();
            lblPageNumber.Text = $"{((_pageIndex - 1) * _pageSize) + 1} - {((_pageIndex - 1) * _pageSize) + _listReport.Count()}";
            dgReport.ItemsSource = _listReport;
        }

        private async void GetPagination()
        {
            try
            {
                _listReport = await documentsRepository.GetDocumentsAsync(new GetDocumentsQueryParameter
                {
                    StartDate = dpFilterStart.SelectedDate.ToDateTime(),
                    EndDate = dpFilterEnd.SelectedDate.ToDateTime(),
                    Destination = txtDestination.Text,
                    Page = _pageIndex,
                    Size = _pageSize
                });
                dgReport.ItemsSource = _listReport;

            }
            catch (Exception ex)
            {
                ShowSnackbarMessage("در واکشی اطلاعات خطایی رخ داده است", MessageTypeEnum.Error);
                Logger.LogException(ex);
            }
        }


        private async void btnReportRemitance_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _totalCount = await documentsRepository.GetDocumentsCountAsync();
                if (_totalCount == 0)
                {
                    ShowSnackbarMessage("داده ای برای نمایش وجود ندارد", MessageTypeEnum.Information);
                    dgReport.ItemsSource = null;
                    return;
                }
                btnFirstPage_Click(null!, null!);
                FillDatagrid(null!, null!);
                gridPagination.IsEnabled = true;
            }
            catch (Exception ex)
            {
                ShowSnackbarMessage("در واکشی تعداد گزارش خطایی رخ داده است", MessageTypeEnum.Error);
                Logger.LogException(ex);
            }
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
            btnReportRemitance_Click(null!, null!);
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
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                btnReportRemitance_Click(null!, null!);
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
            if (dgReport.SelectedItems.Count > 0)
                selectedDocument = dgReport.SelectedItem as DocumentsResultModel ?? new DocumentsResultModel();
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            _pageIndex++;
            FillDatagrid(null, null);
        }

        private void btnLastPage_Click(object sender, RoutedEventArgs e)
        {
            _pageIndex = Convert.ToInt32(_totalPages);
            FillDatagrid(null, null);
        }

        private void btnPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            _pageIndex--;
            FillDatagrid(null, null);
        }

        private void btnFirstPage_Click(object sender, RoutedEventArgs e)
        {
            _pageIndex = 1;
            FillDatagrid(null, null);
        }

        private void cmbPaginationSize_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (!_listReport.Any())
                return;
            _pageIndex = 1;
            FillDatagrid(null, null);
        }

        private void FillPaginationComboBox()
        {
            Dictionary<int, string> paginationSizeValuePairs = new Dictionary<int, string>
            {
                { 0, "10" },
                { 1, "20" },
                { 2, "30" }
            };
            cmbPaginationSize.ItemsSource = paginationSizeValuePairs;
        }
    }
}