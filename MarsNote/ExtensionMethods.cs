using System;
using System.Collections.Generic;
using System.Linq;

namespace MarsNote
{
    public static class ExtensionMethods
    {
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
