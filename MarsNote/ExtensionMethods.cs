using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MarsNote
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Checks if any of the specified <see cref="Panel"/>'s children has current logical focus within the specified focus scope.
        /// </summary>
        /// <param name="panel">The panel for which all children should be searched for focus.</param>
        /// <param name="element">The focus scope.</param>
        public static bool ChildHasFocus(this Panel panel, DependencyObject element)
        {
            if (panel == null) { throw new ArgumentNullException(nameof(panel)); }
            if (element == null) { throw new ArgumentNullException(nameof(element)); }

            if (panel.Children.Count < 1) { return false; }

            IInputElement focusedElement = FocusManager.GetFocusedElement(element);
            return focusedElement != null && panel.Children.Cast<object>().Any(child => focusedElement == child);
        }

        /// <summary>
        /// Check if an Collection of <see cref="Profile"/>s contains a <see cref="Folder"/>.
        /// </summary>
        /// <param name="profiles">The Collection of <see cref="Profile"/>s to check.</param>
        /// <param name="folder">The <see cref="Folder"/> to look for.</param>
        /// <returns>Whether or not the folder exists.</returns>
        public static bool ContainsFolder(this IEnumerable<Profile> profiles, Folder folder)
        {
            return profiles.Any(profile => profile.Folders.Contains(folder));
        }

        /// <summary>
        /// Check if an Collection of <see cref="Profile"/>s contains a <see cref="Note"/>.
        /// </summary>
        /// <param name="profiles">The Collection of <see cref="Profile"/>s to check.</param>
        /// <param name="note">The <see cref="Note"/> to look for.</param>
        /// <returns>Whether or not the note exists.</returns>
        public static bool ContainsNote(this IEnumerable<Profile> profiles, Note note)
        {
            return profiles
                .SelectMany(profile => profile.Folders)
                .Any(folder => folder.Notes.Contains(note));
        }

        /// <summary>
        /// Resets the undo queue of the RichTextBox.
        /// </summary>
        /// <param name="rtb">The RichTextBox to have its undo queue reset.</param>
        public static void ResetUndoQueue(this System.Windows.Controls.RichTextBox rtb)
        {
            if (rtb == null) { throw new ArgumentNullException(nameof(rtb)); }

            rtb.IsUndoEnabled = false;
            rtb.IsUndoEnabled = true;
        }
    }
}
