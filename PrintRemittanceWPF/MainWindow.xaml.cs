using Microsoft.Win32;
using System.IO;
using System.Printing;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrintRemittanceWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }


        private void PrintDocument()
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                // Create a FixedDocument
                FixedDocument fixedDocument = new FixedDocument();

                // Set the page size and orientation
                fixedDocument.DocumentPaginator.PageSize = new Size(420, 594); // A6 size in pixels (landscape)

                // Create a FixedPage
                FixedPage fixedPage = new FixedPage();
                fixedPage.Width = fixedDocument.DocumentPaginator.PageSize.Width;
                fixedPage.Height = fixedDocument.DocumentPaginator.PageSize.Height;

                double horizontalOffset = 20; // Horizontal offset for elements
                double verticalOffset = 20; // Vertical offset for elements

                // Logo
                Image logoImage = new Image();
                // Set the logo image source
                logoImage.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logo.png")));
                // Set the logo image position
                logoImage.SetValue(Canvas.TopProperty, verticalOffset);
                logoImage.SetValue(Canvas.LeftProperty, (fixedPage.Width - logoImage.Width) / 2);
                fixedPage.Children.Add(logoImage);

                // Header
                TextBlock headerTextBlock = new TextBlock();
                // Set the header text
                headerTextBlock.Text = "Header";
                // Set the header font size
                headerTextBlock.FontSize = 20;
                // Set the header position
                headerTextBlock.SetValue(Canvas.TopProperty, verticalOffset);
                headerTextBlock.SetValue(Canvas.LeftProperty, fixedPage.Width - headerTextBlock.ActualWidth - horizontalOffset);
                fixedPage.Children.Add(headerTextBlock);

                // Document Number
                TextBlock docNumberTextBlock = new TextBlock();
                // Set the document number text
                docNumberTextBlock.Text = "Doc Number";
                // Set the document number position
                docNumberTextBlock.SetValue(Canvas.TopProperty, verticalOffset);
                docNumberTextBlock.SetValue(Canvas.LeftProperty, horizontalOffset);
                fixedPage.Children.Add(docNumberTextBlock);

                // Number and Name
                TextBlock numberAndNameTextBlock = new TextBlock();
                // Set the number and name text
                numberAndNameTextBlock.Text = "Number and Name";
                // Set the number and name position
                numberAndNameTextBlock.SetValue(Canvas.TopProperty, verticalOffset + 40);
                numberAndNameTextBlock.SetValue(Canvas.LeftProperty, fixedPage.Width - numberAndNameTextBlock.ActualWidth - horizontalOffset);
                fixedPage.Children.Add(numberAndNameTextBlock);

                // Date and Phone Number
                TextBlock dateAndPhoneTextBlock = new TextBlock();
                // Set the date and phone number text
                dateAndPhoneTextBlock.Text = "Date and Phone Number";
                // Set the date and phone number position
                dateAndPhoneTextBlock.SetValue(Canvas.TopProperty, verticalOffset + 40);
                dateAndPhoneTextBlock.SetValue(Canvas.LeftProperty, horizontalOffset);
                fixedPage.Children.Add(dateAndPhoneTextBlock);

                // User-Entered Data
                double dataTopOffset = verticalOffset + 80; // Specify the top offset for user-entered data
                int maxDataPerLine = 2; // Specify the maximum number of data per line
                int dataCount = 4; // Specify the total number of data elements

                for (int i = 0; i < dataCount; i++)
                {
                    double dataLeft;
                    double dataTop;

                    if (i % maxDataPerLine == 0)
                    {
                        dataLeft = horizontalOffset;
                        dataTop = dataTopOffset + ((i / maxDataPerLine) * 30);
                    }
                    else
                    {
                        dataLeft = (fixedPage.Width / 2) + horizontalOffset;
                        dataTop = dataTopOffset + ((i / maxDataPerLine) * 30);
                    }

                    // Data Label
                    TextBlock dataLabel = new TextBlock();
                    // Set the data label text
                    dataLabel.Text = "Data Label " + (i + 1);
                    // Set the data label position
                    dataLabel.SetValue(Canvas.TopProperty, dataTop);
                    dataLabel.SetValue(Canvas.LeftProperty, dataLeft);
                    fixedPage.Children.Add(dataLabel);

                    // Data Value
                    TextBlock dataValue = new TextBlock();
                    // Set the data value text
                    dataValue.Text = "Data Value " + (i + 1);
                    // Set the data value position
                    dataValue.SetValue(Canvas.TopProperty, dataTop);
                    dataValue.SetValue(Canvas.LeftProperty, dataLeft + 100);
                    fixedPage.Children.Add(dataValue);
                }

                // Add the FixedPage to the FixedDocument
                PageContent pageContent = new PageContent();
                ((IAddChild)pageContent).AddChild(fixedPage);
                fixedDocument.Pages.Add(pageContent);

                // Print the FixedDocument
                printDialog.PrintDocument(fixedDocument.DocumentPaginator, "Print Document");
            }

        }

        private void PrintVisual()
        {
            // Get the default print server
            LocalPrintServer printServer = new LocalPrintServer();

            // Get the default printer name
            string defaultPrinter = printServer.DefaultPrintQueue.Name;
            // Create a PrintDialog instance
            PrintDialog printDialog = new PrintDialog();

            // Set the default printer name
            printDialog.PrintQueue = new PrintQueue(printServer, defaultPrinter);

            if (printDialog.ShowDialog() == true)
            {

                ScaleTransform scale = new ScaleTransform();
                scale.ScaleX = 0.75; // Adjust this value as needed to fit your content
                scale.ScaleY = 0.75;

                this.LayoutTransform = scale;
                printDialog.PrintVisual(this, "Print Document");
                this.LayoutTransform = null; // Reset the scale after printing
            }
        }

        private void FindDefaultPrinter()
        {
            // Get the default print server
            LocalPrintServer printServer = new LocalPrintServer();

            // Get the default printer name
            string defaultPrinter = printServer.DefaultPrintQueue.Name;
            // Create a PrintDialog instance
            PrintDialog printDialog = new PrintDialog();

            // Set the default printer name
            printDialog.PrintQueue = new PrintQueue(printServer, defaultPrinter);

            // Show the PrintDialog to select print settings
            if (printDialog.ShowDialog() == true)
            {
                // Create a FlowDocument to hold the content to be printed
                FlowDocument document = new FlowDocument();

                // Add content to the document (e.g., document.Blocks.Add(...))

                // Print the document
                printDialog.PrintDocument(((IDocumentPaginatorSource)document).DocumentPaginator, "Document Title");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PrintVisual();
        }
    }
}