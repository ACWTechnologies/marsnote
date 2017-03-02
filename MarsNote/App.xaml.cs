using System.Windows;
using System.Windows.Controls;

namespace MarsNote
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Gets the data context from a sender.
        /// </summary>
        /// <typeparam name="T">The type of the data context.</typeparam>
        /// <param name="sender">The <see cref="FrameworkElement"/> that created the event.</param>
        private static T GetDataContextFromSender<T>(object sender) where T : class
        {
            var control = sender as FrameworkElement;
            return control?.DataContext as T;
        }

        #region FolderListBoxItem
        
        private void folderListBoxItem_rename_Click(object sender, RoutedEventArgs e)
        {
            var folder = GetDataContextFromSender<Folder>(sender);
            if (folder != null)
            {
                (Current.MainWindow as MainWindow)?.RenameFolder(folder);
            }
        }

        private void folderListBoxItem_move_Click(object sender, RoutedEventArgs e)
        {
            var folder = GetDataContextFromSender<Folder>(sender);
            var destinationProfile = (e.OriginalSource as MenuItem)?.DataContext as Profile;

            if (folder != null && destinationProfile != null)
            {
                (Current.MainWindow as MainWindow)?.MoveFolderToAnotherProfile(folder, null, destinationProfile);
            }
        }

        private void folderListBoxItem_delete_Click(object sender, RoutedEventArgs e)
        {
            var folder = GetDataContextFromSender<Folder>(sender);
            if (folder != null)
            {
                (Current.MainWindow as MainWindow)?.DeleteFolder(folder, true);
            }
        }

        #endregion

        #region NoteListBoxItem

        private void menuItem_noteListBoxItem_duplicate_Click(object sender, RoutedEventArgs e)
        {
            var note = GetDataContextFromSender<Note>(sender);
            (Current.MainWindow as MainWindow)?.DuplicateNote(note);
        }
        
        private void noteListBoxItem_move_Click(object sender, RoutedEventArgs e)
        {
            var note = GetDataContextFromSender<Note>(sender);
            var destinationFolder = (e.OriginalSource as MenuItem)?.DataContext as Folder;

            if (note != null && destinationFolder != null)
            {
                (Current.MainWindow as MainWindow)?.MoveNoteToAnotherFolder(note, null, destinationFolder);
            }
        }

        private void noteListBoxItem_delete_Click(object sender, RoutedEventArgs e)
        {
            var note = GetDataContextFromSender<Note>(sender);
            if (note != null)
            {
                (Current.MainWindow as MainWindow)?.DeleteNote(note, null, true);
            }
        }

        #endregion
    }
}
