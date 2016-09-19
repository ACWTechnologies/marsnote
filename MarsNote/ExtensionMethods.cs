using System.Collections.ObjectModel;

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
        public static bool ContainsFolder(this Collection<Profile> profiles, Folder folder)
        {
            foreach (Profile profile in profiles)
            {
                if (profile.Folders.Contains(folder))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if an Collection of <see cref="Profile"/>s contains a <see cref="Note"/>.
        /// </summary>
        /// <param name="profiles">The Collection of <see cref="Profile"/>s to check.</param>
        /// <param name="note">The <see cref="Note"/> to look for.</param>
        /// <returns>Whether or not the note exists.</returns>
        public static bool ContainsNote(this Collection<Profile> profiles, Note note)
        {
            foreach (Profile profile in profiles)
            {
                foreach (Folder folder in profile.Folders)
                {
                    if (folder.Notes.Contains(note))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
