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
        private readonly Guid documentId;

        public DeleteDocumentWindow(IDocumentsRepository documentsRepository , Guid documentId)
        {
            InitializeComponent();
            this.documentsRepository = documentsRepository;
            this.documentId = documentId;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void btnSubmitDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await documentsRepository.DeleteDocument(documentId);
                NotificationEventsManager.OnShowMessage("حواله مورد نظر با موفیت حذف شد", MessageTypeEnum.Success);
                CartableEventsManager.OnUpdateDocumentsDatagrid();
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                NotificationEventsManager.OnShowMessage("در عملیات حذف خطایی رخ داده", MessageTypeEnum.Error);
            }
        }
    }
}
