using System;
using System.Windows;
using System.Windows.Controls;

namespace MarsNote
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void button_folderListBoxItem_delete_Click(object sender, RoutedEventArgs e)
        {
            (Current.MainWindow as MainWindow).DeleteFolderFromListBoxItem(sender, e);
        }
    }
}
