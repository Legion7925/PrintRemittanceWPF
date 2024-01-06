using PrintRemittance.Core.Interfaces.Repositories;
using PrintRemittanceWPF.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PrintRemittanceWPF
{
    /// <summary>
    /// Interaction logic for DeleteDocumentWindow.xaml
    /// </summary>
    public partial class DeleteDocumentWindow : Window
    {
        private readonly IDocumentsRepository documentsRepository;
        public DeleteDocumentWindow(IDocumentsRepository documentsRepository)
        {
            InitializeComponent();
            this.documentsRepository = documentsRepository;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnSubmitDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //todo
                NotificationEventsManager.OnShowMessage("حواله مورد نظر با موفیت حذف شد", MessageTypeEnum.Success);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                NotificationEventsManager.OnShowMessage("در عملیات حذف خطایی رخ داده", MessageTypeEnum.Error);
            }
        }
    }
}
