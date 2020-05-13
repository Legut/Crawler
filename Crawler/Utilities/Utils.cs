using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crawler.Utilities
{
    public static class Utils
    {
        // TODO: sortowanie liczb w widoku jest alfabetyczne zamiast wielkościowego
        public static readonly string[] SizeSuffixes = { "B", "KB", "MB", "GB", "TB" };


        public static Task WhenMouseUp(this Control control)
        {
            var tcs = new TaskCompletionSource<object>();
            MouseEventHandler onMouseUp = null;
            onMouseUp = (sender, e) =>
            {
                control.MouseUp -= onMouseUp;
                tcs.TrySetResult(null);
            };
            control.MouseUp += onMouseUp;
            return tcs.Task;
        }
    }
}
