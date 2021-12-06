using System.Runtime.InteropServices;

namespace NotifyIconWpf.Interop
{
    /// <summary>
    /// Win API struct providing coordinates for a single point.
    /// </summary>
    /// <MetaDataID>{e32d5d51-44e5-44ee-b682-63f67364c98d}</MetaDataID>
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        /// <summary>
        /// X coordinate.
        /// </summary>
        public int X;
        /// <summary>
        /// Y coordinate.
        /// </summary>
        public int Y;
    }
}