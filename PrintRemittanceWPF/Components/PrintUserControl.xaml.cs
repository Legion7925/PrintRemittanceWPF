using PersianDate.Standard;
using PrintRemittance.Core.Models;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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
        txtRemittanceNumber.Text = $"{documentModel.P4 + " " + documentModel.P3 + documentModel.P2 + documentModel.P1}";
        txtDriverName.Text= documentModel.DriverName;
        lblDate.Text = documentModel.CreatedDate.ToFa();
        lblPrintNumber.Text = documentModel.PrintNumber;
        txtProduct.Text = documentModel.Product;

        FillImage();
        
    }

    void FillImage()
    {
        BitmapImage myBitmapImage = new BitmapImage();
        myBitmapImage.BeginInit();
        myBitmapImage.UriSource = new Uri(@"D:\RepoMohammad\Test\PrintRemittanceWPF\PrintRemittanceWPF\Resources\Images\logo.png");
        myBitmapImage.EndInit();
        logo.Source = myBitmapImage;
    }
}
