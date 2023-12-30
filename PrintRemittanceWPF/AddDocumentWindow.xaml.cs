using PrintRemittance.Core.Exception;
using PrintRemittance.Core.Interfaces.Repositories;
using PrintRemittance.Core.Models;
using PrintRemittanceWPF.Helper;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PrintRemittanceWPF
{
    /// <summary>
    /// Interaction logic for AddDocumentWindow.xaml
    /// </summary>
    public partial class AddDocumentWindow : Window
    {
        private readonly IDocumentsRepository documentsRepository;
        public AddDocumentWindow(IDocumentsRepository documentsRepository)
        {
            InitializeComponent();
            this.documentsRepository = documentsRepository;
        }

        private void btnPrintDocument_Click(object sender, RoutedEventArgs e)
        {
            var document = new AddDocumentModel
            {
                CarName = txtCarType.Text,
                CreatedDate = dpDate.SelectedDate.ToDateTime(),
                Destination = txtDestination.Text,
                DriverName = txtDriverName.Text,
                FactoryName = txtFactoryName.Text,
                PrintNumber = 1,//todo it should be auto increament
                Product = txtProduct.Text,
                RemittanceNumber = txtRemittanceNumber.Text,
            };
            SavePrintedDocument(document);
            PrintVisual(document);
            btnPrintDocument.IsEnabled = true; 
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// پرینت یوزر کنترل طراحی شده و ارسال اطلاعات به یوزر کنترل
        /// </summary>
        private void PrintVisual(AddDocumentModel document)
        {
            btnPrintDocument.IsEnabled = false;

            try
            {
                // Get the default print server
                LocalPrintServer printServer = new LocalPrintServer();

                // Get the default printer name
                string defaultPrinter = printServer.DefaultPrintQueue.Name;
                // Create a PrintDialog instance
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    // Get the default print queue
                    PrintQueue printQueue = LocalPrintServer.GetDefaultPrintQueue();

                    // Get the print ticket for the print queue
                    PrintTicket printTicket = printQueue.DefaultPrintTicket;


                    // Set the paper size to A6
                    printTicket.PageMediaSize = new PageMediaSize(554.48, 390.55);

                    printDialog.PrintTicket = printTicket;


                    // Update the print ticket for the print queue
                    printQueue.UserPrintTicket = printTicket;
                    // Set the default printer name
                    printDialog.PrintQueue = new PrintQueue(printServer, defaultPrinter);


                    ScaleTransform scale = new ScaleTransform();
                    scale.ScaleX = 0.75; // Adjust this value as needed to fit your content
                    scale.ScaleY = 0.75;

                    this.LayoutTransform = scale;
                    var printUC = new PrintUserControl(document);
                    printDialog.PrintVisual(printUC, "Print Document");
                    this.LayoutTransform = null; // Reset the scale after printing

                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                NotificationEventsManager.OnShowMessage("در انجام عملیات پرینت خطایی رخ داده است", MessageTypeEnum.Error);
                btnPrintDocument.IsEnabled = true;
            }

        }
        /// <summary>
        /// ذخیره اطلاعات حواله پرینت شده داخل پایگاه داده
        /// </summary>
        private void SavePrintedDocument(AddDocumentModel documentModel)
        {
            try
            {
                documentsRepository.AddDocument(documentModel);
                CartableEventsManager.OnUpdateDocumentsDatagrid();
            }
            catch (AppException ne)
            {
                NotificationEventsManager.OnShowMessage(ne.Message, MessageTypeEnum.Warning);
                btnPrintDocument.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                NotificationEventsManager.OnShowMessage("در انجام عملیات ثبت خطایی رخ داده است", MessageTypeEnum.Error);
                btnPrintDocument.IsEnabled = true;
            }
        }

        private void txtCarType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtRemittanceNumber.Focus();
        }

        private void txtFactoryName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtCarType.Focus();
        }

        private void txtRemittanceNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtDriverName.Focus();
        }

        private void txtDestination_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnPrintDocument.Focus();
        }

        private void txtProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtDestination.Focus();
        }


        private void txtDriverName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtProduct.Focus();
        }

    }
}
