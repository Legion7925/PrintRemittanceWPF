using PrintRemittance.Core.Models;
using System.Printing;
using System.Windows.Controls;
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
                printTicket.PageMediaSize = new PageMediaSize(297.36, 419.76);

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

}
