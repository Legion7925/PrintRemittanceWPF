using PrintRemittance.Core.Models;
using PrintRemittanceWPF.Components;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace PrintRemittanceWPF.Helper;

public static class PrintManager
{
    /// <summary>
    /// پرینت یوزر کنترل طراحی شده و ارسال اطلاعات به یوزر کنترل
    /// </summary>
    public static void PrintVisual(PrintDocumentModel document)
    {

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
                printTicket.PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA7);

                printDialog.PrintTicket = printTicket;


                // Update the print ticket for the print queue
                printQueue.UserPrintTicket = printTicket;
                // Set the default printer name
                printDialog.PrintQueue = new PrintQueue(printServer, defaultPrinter);

                var printUC = new PrintUserControl(document);
                printDialog.PrintVisual(printUC, "Print Document");

            }
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            NotificationEventsManager.OnShowMessage("در انجام عملیات پرینت خطایی رخ داده است", MessageTypeEnum.Error);
        }

    }

    public static void PrintVisualV2(PrintDocumentModel document)
    {
        try
        {
            // Create a PrintDialog to select a printer
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == true)
            {
                var userControl = new PrintUserControl(document);
                // Get the selected printer and its printable area size
                PrintQueue printQueue = printDialog.PrintQueue;
                PrintCapabilities capabilities = printQueue.GetPrintCapabilities();

                // Get the print ticket for the print queue
                // Set the paper size to A6
               // Update the print ticket for the print queue
                PrintTicket printTicket = printQueue.DefaultPrintTicket;
                printTicket.PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA6);
                printDialog.PrintTicket = printTicket;
                printQueue.UserPrintTicket = printTicket;


                // Calculate the printable area size
                double printableWidth = printTicket.PageMediaSize.Width.Value;
                double printableHeight = printTicket.PageMediaSize.Height.Value;

                // Create a FixedDocument for printing
                FixedDocument fixedDocument = new FixedDocument();
                fixedDocument.DocumentPaginator.PageSize = new Size(printableWidth, printableHeight);

                // Create a FixedPage for each page
                FixedPage fixedPage = new FixedPage();
                fixedPage.Width = printableWidth;
                fixedPage.Height = printableHeight;

                // Set margins for the content
                double margin = 5;
                double contentWidth = printableWidth - (2 * margin);
                double contentHeight = printableHeight - (2 * margin);

                // Set UserControl dimensions to fit within the content area
                userControl.Width = contentWidth;
                userControl.Height = contentHeight;

                // Calculate the position to center the UserControl
                double left = (printableWidth - contentWidth) / 2;
                double top = (printableHeight - contentHeight) / 2;
                Canvas.SetLeft(userControl, left);
                Canvas.SetTop(userControl, top);

                // Add the UserControl to the FixedPage
                fixedPage.Children.Add(userControl);

                // Add the FixedPage to the FixedDocument
                fixedDocument.Pages.Add(new PageContent { Child = fixedPage });

                // Send the FixedDocument to the printer
                printDialog.PrintDocument(fixedDocument.DocumentPaginator, "Printing UserControl");
            }
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            NotificationEventsManager.OnShowMessage("در انجام عملیات پرینت خطایی رخ داده است", MessageTypeEnum.Error);
        }
    }
}
