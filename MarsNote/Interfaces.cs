namespace MarsNote
{
    /// <summary>
    /// Indicates that an object can be pinned.
    /// </summary>
    public interface IPinnable
    {
        bool Pinned { get; set; }
    }
}
