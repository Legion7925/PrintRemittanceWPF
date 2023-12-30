using PersianDate.Standard;
using PrintRemittance.Core.Models;
using System.Windows.Controls;

namespace PrintRemittanceWPF;

/// <summary>
/// Interaction logic for PrintUserControl.xaml
/// </summary>
public partial class PrintUserControl : UserControl
{
    public PrintUserControl(AddDocumentModel documentModel)
    {
        InitializeComponent();
        txtCarType.Text = documentModel.CarName;
        txtDestination.Text = documentModel.Destination;
        txtDriverName.Text = documentModel.DriverName;
        txtFactoryName.Text = documentModel.FactoryName;
        txtRemittanceNumber.Text =  documentModel.RemittanceNumber;
        txtDriverName.Text= documentModel.DriverName;
        lblDate.Text = documentModel.CreatedDate.ToFa();
        lblPrintNumber.Text = documentModel.PrintNumber.ToString();
        txtProduct.Text = documentModel.Product;
    }
}
