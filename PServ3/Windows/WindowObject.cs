using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace pserv3.Windows
{
    public class WindowObject : IServiceObject
    {
        private readonly object[] Objects = new object[(int)WindowItemTypes.MAX];
        public string ToolTipText;
        public Color ForegroundColor = Color.Black;
        public readonly int Handle;

        public WindowObject(int hwnd)
        {
            Handle = hwnd;
            Objects[(int)WindowItemTypes.Handle] = string.Format("{0:x8}", hwnd);
            Refresh();
        }

        public string Title
        {
            get
            {
                return GetText((int)WindowItemTypes.Title);
            }
        }

        public string Class
        {
            get
            {
                return GetText((int)WindowItemTypes.Class);
            }
        }
                
        private const int WM_GETTEXT = 0x000D; 

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessageTimeout(int hwnd, int msg, int wParam, StringBuilder sb, int fuFlags, int uTimeout, out UIntPtr result);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern uint GetWindowThreadProcessId(int hwnd, out UIntPtr lpdwProcessId);
  
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetClassName(int hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern uint GetWindowLong(int hWnd, int nID);

        private const int GWL_STYLE  = -16;
        private const int GWL_EXSTYLE = -20;
        private const int GWL_ID = -12;

        private const int SMTO_ABORTIFHUNG = 0x0002;

        public void Refresh()
        {
            StringBuilder sb = new StringBuilder();

            // get caption
            sb.EnsureCapacity(10240);

            bool WindowTimedOut = false;
           
            UIntPtr lRes = new UIntPtr(1860);
            int lResult = SendMessageTimeout(Handle, WM_GETTEXT, 10240, sb, SMTO_ABORTIFHUNG, 1000, out lRes);
            if (lResult == 0)
            {
                Trace.TraceError("SendMessageTimeout() failed with {0}", Marshal.GetLastWin32Error());
                WindowTimedOut = true;
                Objects[(int)WindowItemTypes.Title] = "?";
            }
            else
            {
                //Trace.TraceInformation("lResult: {0}, lRes: {1}", lResult, lRes.ToUInt32());
                Objects[(int)WindowItemTypes.Title] = sb.ToString();
            }

            // get class name
            sb = new StringBuilder();
            sb.EnsureCapacity(10240);
            GetClassName(Handle, sb, 10240);
            Objects[(int)WindowItemTypes.Class] = sb.ToString();

            uint style = GetWindowLong(Handle, GWL_STYLE);
            Objects[(int)WindowItemTypes.Style] = DecodeWindowStyle(style);
            Objects[(int)WindowItemTypes.ExStyle] = GetWindowLong(Handle, GWL_EXSTYLE);
            Objects[(int)WindowItemTypes.ID] = GetWindowLong(Handle, GWL_ID);

            RECT r = new RECT();
            GetWindowRect(Handle, ref r);

            Objects[(int)WindowItemTypes.Size] = string.Format("({0}, {1})", r.Width, r.Height);
            Objects[(int)WindowItemTypes.Position] = string.Format("({0}, {1})", r.Top, r.Left);

            UIntPtr ProcessID = new UIntPtr(0);                        
            uint ThreadID = GetWindowThreadProcessId(Handle, out ProcessID);
            Objects[(int)WindowItemTypes.TID] = ThreadID;
            Objects[(int)WindowItemTypes.PID] = ProcessID.ToUInt32();

            ForegroundColor = Color.Black;
            if ((r.Width == r.Height) && (r.Width == 0))
            {
                ForegroundColor = Color.Gray;
            }
            if ((style & WS_VISIBLE) == 0)
            {
                ForegroundColor = Color.Gray;
            }
            if (WindowTimedOut)
            {
                ForegroundColor = Color.Red;
            }
        }
        #region IServiceObject Members

        public object GetObject(int nID)
        {
            return Objects[nID];
        }

        public string GetText(int nID)
        {
            object o = Objects[nID];
            if (o == null)
                return "";
            return o.ToString();
        }

        public string GetToolTipText()
        {
            return ToolTipText;
        }

        public Color GetForegroundColor()
        {
            return ForegroundColor;
        }

        #endregion

        private const uint WS_EX_DLGMODALFRAME = 0x00000001;
        private const uint WS_EX_NOPARENTNOTIFY = 0x00000004;
        private const uint WS_EX_TOPMOST = 0x00000008;
        private const uint WS_EX_ACCEPTFILES = 0x00000010;
        private const uint WS_EX_TRANSPARENT = 0x00000020;
        private const uint WS_EX_MDICHILD = 0x00000040;
        private const uint WS_EX_TOOLWINDOW = 0x00000080;
        private const uint WS_EX_WINDOWEDGE = 0x00000100;
        private const uint WS_EX_CLIENTEDGE = 0x00000200;
        private const uint WS_EX_CONTEXTHELP = 0x00000400;
        private const uint WS_EX_RIGHT = 0x00001000;
        private const uint WS_EX_LEFT = 0x00000000;
        private const uint WS_EX_RTLREADING = 0x00002000;
        private const uint WS_EX_LTRREADING = 0x00000000;
        private const uint WS_EX_LEFTSCROLLBAR = 0x00004000;
        private const uint WS_EX_RIGHTSCROLLBAR = 0x00000000;
        private const uint WS_EX_CONTROLPARENT = 0x00010000;
        private const uint WS_EX_STATICEDGE = 0x00020000;
        private const uint WS_EX_APPWINDOW = 0x00040000;
        private const uint WS_EX_LAYERED = 0x00080000;
        private const uint WS_EX_NOINHERITLAYOUT = 0x00100000; // Disable inheritence of mirroring by children
        private const uint WS_EX_LAYOUTRTL = 0x00400000; // Right to left mirroring
        private const uint WS_EX_COMPOSITED        = 0x02000000;
        private const uint WS_EX_NOACTIVATE        = 0x08000000;

        private const uint WS_POPUP             = 0x80000000;
        private const uint WS_CHILD             = 0x40000000;
        private const uint WS_MINIMIZE          = 0x20000000;
        private const uint WS_VISIBLE           = 0x10000000;
        private const uint WS_DISABLED          = 0x08000000;
        private const uint WS_CLIPSIBLINGS      = 0x04000000;
        private const uint WS_CLIPCHILDREN      = 0x02000000;
        private const uint WS_MAXIMIZE          = 0x01000000;
        private const uint WS_BORDER            = 0x00800000;
        private const uint WS_DLGFRAME          = 0x00400000;
        private const uint WS_VSCROLL           = 0x00200000;
        private const uint WS_HSCROLL           = 0x00100000;
        private const uint WS_SYSMENU           = 0x00080000;
        private const uint WS_THICKFRAME        = 0x00040000;
        private const uint WS_MINIMIZEBOX       = 0x00020000;
        private const uint WS_MAXIMIZEBOX       = 0x00010000;

        static Dictionary<uint, string> KnownWindowStyles = new Dictionary<uint, string>()
        {
            {WS_POPUP             , "POPUP"},
            {WS_CHILD             , "CHILD"},
            {WS_MINIMIZE          , "MINIMIZE"},
            {WS_VISIBLE           , "VISIBLE"},
            {WS_DISABLED          , "DISABLED"},
            {WS_CLIPSIBLINGS      , "CLIPSIBLINGS"},
            {WS_CLIPCHILDREN      , "CLIPCHILDREN"},
            {WS_MAXIMIZE          , "MAXIMIZE"},
            {WS_BORDER            , "BORDER"},
            {WS_DLGFRAME          , "DLGFRAME"},
            {WS_VSCROLL           , "VSCROLL"},
            {WS_HSCROLL           , "HSCROLL"},
            {WS_SYSMENU           , "SYSMENU"},
            {WS_THICKFRAME        , "THICKFRAME"},
            {WS_MINIMIZEBOX       , "MINIMIZEBOX"},
            {WS_MAXIMIZEBOX       , "MAXIMIZEBOX"},
        };

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left_, int top_, int right_, int bottom_)
            {
                Left = left_;
                Top = top_;
                Right = right_;
                Bottom = bottom_;
            }

            public int Height { get { return Bottom - Top; } }
            public int Width { get { return Right - Left; } }
            public Size Size { get { return new Size(Width, Height); } }

            public Point Location { get { return new Point(Left, Top); } }

            // Handy method for converting to a System.Drawing.Rectangle
            public Rectangle ToRectangle()
            { return Rectangle.FromLTRB(Left, Top, Right, Bottom); }

            public static RECT FromRectangle(Rectangle rectangle)
            {
                return new RECT(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
            }

            public override int GetHashCode()
            {
                return Left ^ ((Top << 13) | (Top >> 0x13))
                  ^ ((Width << 0x1a) | (Width >> 6))
                  ^ ((Height << 7) | (Height >> 0x19));
            }

            #region Operator overloads

            public static implicit operator Rectangle(RECT rect)
            {
                return rect.ToRectangle();
            }

            public static implicit operator RECT(Rectangle rect)
            {
                return FromRectangle(rect);
            }

            #endregion
        }

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(int hWnd, ref RECT lpRect);

        private static string DecodeWindowStyle(uint style)
        {

            StringBuilder result = new StringBuilder();
            bool first = true;
            foreach (uint bitmask in KnownWindowStyles.Keys)
            {
                if ((style & bitmask) == bitmask)
                {
                    if (first)
                        first = false;
                    else
                        result.Append("|");
                    style &= ~bitmask;
                    result.Append(KnownWindowStyles[bitmask]);
                }
            }
            if (style > 0)
            {
                if (first)
                    first = false;
                else
                    result.Append("|");
                result.Append(style.ToString());
            }

            return result.ToString();
        }
    }
}
