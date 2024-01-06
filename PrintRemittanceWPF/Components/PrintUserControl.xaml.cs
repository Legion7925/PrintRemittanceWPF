using PersianDate.Standard;
using PrintRemittance.Core.Models;
using System.Windows.Controls;

namespace PrintRemittanceWPF;

/// <summary>
/// Interaction logic for PrintUserControl.xaml
/// </summary>
public partial class PrintUserControl : UserControl
{
    public PrintUserControl(PrintDocumentModel documentModel)
    {
        InitializeComponent();
        txtCarType.Text = documentModel.CarName;
        txtDestination.Text = documentModel.Destination;
        txtDriverName.Text = documentModel.DriverName;
        txtFactoryName.Text = documentModel.FactoryName;
        txtRemittanceNumber.Text =  documentModel.PlateNumber;
        txtDriverName.Text= documentModel.DriverName;
        lblDate.Text = documentModel.CreatedDate.ToFa();
        lblPrintNumber.Text = documentModel.PrintNumber;
        txtProduct.Text = documentModel.Product;
    }
}
