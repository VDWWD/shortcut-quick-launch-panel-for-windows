using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace QuickLauncher.Classes
{
    internal static class SnapHelper
    {
        //the distance in pixel of when the window snaps
        private const int SnapDistance = 20;

        private const int WM_MOVING = 0x0216;
        private const int WM_ENTERSIZEMOVE = 0x0231;
        private const int WM_EXITSIZEMOVE = 0x0232;
        private static readonly Dictionary<IntPtr, SnapState> _states = new Dictionary<IntPtr, SnapState>();


        /// <summary>
        /// Add the snapping function to a Window.
        /// </summary>
        /// <param name="window">The Window that needs snapping.</param>
        public static void EnableEdgeSnapping(Window window)
        {
            if (window.IsLoaded)
            {
                AttachHook(window);
            }
            else
            {
                window.SourceInitialized += (s, e) => AttachHook(window);
            }
        }


        /// <summary>
        /// Add the hook to the Window to capture the dragging.
        /// </summary>
        /// <param name="window">The Window that needs snapping.</param>
        private static void AttachHook(Window window)
        {
            var source = (HwndSource)PresentationSource.FromVisual(window);

            if (source == null)
            {
                return;
            }

            //create the snapstate class
            var state = new SnapState();
            _states[source.Handle] = state;

            //add the hook to the source
            source.AddHook((IntPtr handle, int message_id, IntPtr wparam, IntPtr lparam, ref bool handled) =>
            {
                switch (message_id)
                {
                    case WM_ENTERSIZEMOVE:
                        GetCursorPos(out POINT cursor);
                        NativeGetWindowRect(handle, out RECT windowrect);

                        state.CursorOffsetX = cursor.X - windowrect.Left;
                        state.CursorOffsetY = cursor.Y - windowrect.Top;
                        state.SnappedLeft = state.SnappedRight = false;
                        state.SnappedTop = state.SnappedBottom = false;

                        break;

                    case WM_MOVING:
                        var bounds = Marshal.PtrToStructure<RECT>(lparam);
                        SnapToEdges(handle, ref bounds, state);
                        Marshal.StructureToPtr(bounds, lparam, true);
                        handled = true;

                        break;

                    case WM_EXITSIZEMOVE:
                        state.SnappedLeft = state.SnappedRight = false;
                        state.SnappedTop = state.SnappedBottom = false;

                        break;
                }

                return IntPtr.Zero;
            });

            window.Closed += (s, e) => _states.Remove(source.Handle);
        }


        /// <summary>
        /// The method that handles the actual snapping to the edges of the screen.
        /// </summary>
        /// <param name="handle">The handler for the Window.</param>
        /// <param name="bounds">The window current bounds on screen.</param>
        /// <param name="state">The snap state class.</param>
        private static void SnapToEdges(IntPtr handle, ref RECT bounds, SnapState state)
        {
            int width = bounds.Right - bounds.Left;
            int height = bounds.Bottom - bounds.Top;

            //calculate the raw desired position from the actual cursor position, ignoring whatever snapping was applied last time
            GetCursorPos(out POINT cursor);

            int raw_left = cursor.X - state.CursorOffsetX;
            int raw_top = cursor.Y - state.CursorOffsetY;
            int raw_right = raw_left + width;
            int raw_bottom = raw_top + height;

            //get the working area
            var workarea = new Rectangle(bounds.Left, bounds.Top, bounds.Right - bounds.Left, bounds.Bottom - bounds.Top);
            var screen = Screen.FromRectangle(workarea);

            //hysteresis, must move 2x the snap distance to break free
            int unsnap = SnapDistance * 2;

            //horizontal
            if (!state.SnappedLeft && !state.SnappedRight)
            {
                if (Math.Abs(raw_left - screen.WorkingArea.Left) < SnapDistance)
                {
                    bounds.Left = screen.WorkingArea.Left;
                    bounds.Right = bounds.Left + width;
                    state.SnappedLeft = true;
                }
                else if (Math.Abs(raw_right - screen.WorkingArea.Right) < SnapDistance)
                {
                    bounds.Right = screen.WorkingArea.Right;
                    bounds.Left = bounds.Right - width;
                    state.SnappedRight = true;
                }
                else
                {
                    bounds.Left = raw_left;
                    bounds.Right = raw_right;
                }
            }
            else if (state.SnappedLeft)
            {
                if (Math.Abs(raw_left - screen.WorkingArea.Left) > unsnap)
                {
                    bounds.Left = raw_left;
                    bounds.Right = raw_right;
                    state.SnappedLeft = false;
                }
                else
                {
                    bounds.Left = screen.WorkingArea.Left;
                    bounds.Right = bounds.Left + width;
                }
            }
            else if (state.SnappedRight)
            {
                if (Math.Abs(raw_right - screen.WorkingArea.Right) > unsnap)
                {
                    bounds.Right = raw_right;
                    bounds.Left = raw_left;
                    state.SnappedRight = false;
                }
                else
                {
                    bounds.Right = screen.WorkingArea.Right;
                    bounds.Left = bounds.Right - width;
                }
            }

            //vertical
            if (!state.SnappedTop && !state.SnappedBottom)
            {
                if (Math.Abs(raw_top - screen.WorkingArea.Top) < SnapDistance)
                {
                    bounds.Top = screen.WorkingArea.Top;
                    bounds.Bottom = bounds.Top + height;
                    state.SnappedTop = true;
                }
                else if (Math.Abs(raw_bottom - screen.WorkingArea.Bottom) < SnapDistance)
                {
                    bounds.Bottom = screen.WorkingArea.Bottom;
                    bounds.Top = bounds.Bottom - height;
                    state.SnappedBottom = true;
                }
                else
                {
                    bounds.Top = raw_top; bounds.Bottom = raw_bottom;
                }
            }
            else if (state.SnappedTop)
            {
                if (Math.Abs(raw_top - screen.WorkingArea.Top) > unsnap)
                {
                    bounds.Top = raw_top;
                    bounds.Bottom = raw_bottom;
                    state.SnappedTop = false;
                }
                else
                {
                    bounds.Top = screen.WorkingArea.Top;
                    bounds.Bottom = bounds.Top + height;
                }
            }
            else if (state.SnappedBottom)
            {
                if (Math.Abs(raw_bottom - screen.WorkingArea.Bottom) > unsnap)
                {
                    bounds.Bottom = raw_bottom;
                    bounds.Top = raw_top;
                    state.SnappedBottom = false;
                }
                else
                {
                    bounds.Bottom = screen.WorkingArea.Bottom;
                    bounds.Top = bounds.Bottom - height;
                }
            }
        }


        private static void NativeGetWindowRect(IntPtr handle, out RECT rect) => GetWindowRect(handle, out rect);


        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr handle, out RECT lpRect);


        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);


        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left, Top, Right, Bottom;
        }


        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X, Y;
        }


        private class SnapState
        {
            public bool SnappedLeft;
            public bool SnappedRight;
            public bool SnappedTop;
            public bool SnappedBottom;

            //cursor.x - window.left at drag start
            public int CursorOffsetX;

            //cursor.y - window.top at drag start
            public int CursorOffsetY;
        }
    }
}
