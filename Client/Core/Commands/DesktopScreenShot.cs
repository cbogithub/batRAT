using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Client.Core.Commands {
    public class DesktopScreenShot {
        public static byte[] TakeScreenShot() {
            byte[] buffer;
            using(Bitmap bmpScreenCapture = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                                       Screen.PrimaryScreen.Bounds.Height)) {
                using(Graphics g = Graphics.FromImage(bmpScreenCapture)) {
                    g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                     Screen.PrimaryScreen.Bounds.Y,
                                     0, 0,
                                     bmpScreenCapture.Size,
                                     CopyPixelOperation.SourceCopy);

                    using(MemoryStream ms = new MemoryStream()) {
                        bmpScreenCapture.Save(ms, ImageFormat.Png);
                        ms.Close();
                        buffer = ms.ToArray();
                    }
                }
            }

            return buffer;
        }
    }
}
