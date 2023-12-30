using System;
using System.Windows;

namespace PrintRemittanceWPF.Helper;

public class CartableEventsManager
{
    public static event EventHandler<RoutedEventArgs>? updateDocuments;

    public static void OnUpdateDocumentsDatagrid()
    {
        updateDocuments?.Invoke(default!, default!);
    }

}
