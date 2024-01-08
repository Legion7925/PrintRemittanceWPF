using ControlPlateText;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtFactoryName.Focus();
        }


        private async void btnPrintDocument_Click(object sender, RoutedEventArgs e)
        {
            btnPrintDocument.IsEnabled = false;
            var isValid = ValidateInputs();
            if (isValid is not true)
            {
                NotificationEventsManager.OnShowMessage("لطفا همه ی فیلد ها را پر کنید", MessageTypeEnum.Warning);
                btnPrintDocument.IsEnabled = true;
                return;
            }

            var document = new AddDocumentModel
            {
                CarName = txtCarType.Text,
                CreatedDate = dpDate.SelectedDate.ToDateTime(),
                Destination = txtDestination.Text,
                DriverName = txtDriverName.Text,
                FactoryName = txtFactoryName.Text,
                Product = txtProduct.Text,
                PlateNumber = txtPlate.PlateText,
            };
            try
            {
                var printNumber = await SavePrintedDocument(document);
                var printModel = new PrintDocumentModel
                {
                    CarName = txtCarType.Text,
                    CreatedDate = dpDate.SelectedDate.ToDateTime(),
                    Destination = txtDestination.Text,
                    DriverName = txtDriverName.Text,
                    FactoryName = txtFactoryName.Text,
                    Product = txtProduct.Text,
                    PlateNumber = txtPlate.PlateText,
                    PrintNumber = printNumber

                };//todo add auto mapper if this happened again
                PrintManager.PrintVisualV2(printModel);
                btnPrintDocument.IsEnabled = true;
                ClearInputs();
                CartableEventsManager.OnUpdateDocumentsDatagrid();
                txtFactoryName.Focus();

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

        private bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(txtCarType.Text) ||
                string.IsNullOrEmpty(txtPlate.PlateText) ||
                string.IsNullOrEmpty(txtDriverName.Text) ||
                string.IsNullOrEmpty(txtFactoryName.Text) ||
                string.IsNullOrEmpty(txtProduct.Text) ||
                string.IsNullOrEmpty(txtDestination.Text))

            {
                return false;
            }
            if (!ControlPlate.ControlPleat(txtPlate.PlateText))
            {
                MessageBox.Show("لطفا شماره پلاک را به درستی وارد کنید");
                return false;
            }
            return true;
        }

        private void ClearInputs()
        {
            txtCarType.Text =
                txtDestination.Text =
                txtDriverName.Text =
                txtFactoryName.Text =
                txtProduct.Text = txtPlate.PlateText = string.Empty;
            txtPlate.PlateText = "00ا00000";
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// ذخیره اطلاعات حواله پرینت شده داخل پایگاه داده
        /// </summary>
        private async Task<string> SavePrintedDocument(AddDocumentModel documentModel)
        {
            var printNumber = await documentsRepository.AddDocument(documentModel);
            CartableEventsManager.OnUpdateDocumentsDatagrid();
            return printNumber;
        }

        private void txtCarType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtDriverName.Focus();
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
                txtProduct.Focus();
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Move focus to the next control
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                ((UIElement)sender).MoveFocus(request);

                // Select all text in the next TextBox
                if (Keyboard.FocusedElement is TextBox textBox)
                {
                    textBox.SelectAll();
                }
            }
        }

        private void txtDriverName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtDestination.Focus();
        }

        private void txtPlate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtFactoryName.Focus();
        }
    }
}
