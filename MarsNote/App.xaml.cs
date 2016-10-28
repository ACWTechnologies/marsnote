using System.Windows;

namespace MarsNote
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void button_folderListBoxItem_delete_Click(object sender, RoutedEventArgs e)
        {
            (Current.MainWindow as MainWindow)?.DeleteFolderFromListBoxItem(sender);
        }

        private void button_folderListBoxItem_rename_Click(object sender, RoutedEventArgs e)
        {
            (Current.MainWindow as MainWindow)?.RenameFolderFromListBoxItem(sender);
        }
    }
}
