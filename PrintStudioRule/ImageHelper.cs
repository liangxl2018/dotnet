using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Drawing;
using FreeImageAPI;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using GDI = System.Drawing;
using ImageMagick;
namespace PrintStudioRule
{
    public class ImageHelper
    {
        #region FreeImage加载图像
        /// <summary>
        /// 资源释放
        /// </summary>
        /// <param name="hObject"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// 从Bitmap转换成ImageSource
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static ImageSource ChangeBitmapToImageSource(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            if (!DeleteObject(hBitmap))
            {
                throw new System.ComponentModel.Win32Exception(string.Format("释放句柄【{0}】资源异常.", hBitmap.ToString()));
            }
            return wpfBitmap;
        }

        /// <summary>
        /// 加载图像
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Bitmap LoadImageFormFreeImage(string fileName)
        {
            FREE_IMAGE_FORMAT format = FreeImage.GetFileType(fileName, 0);
            return FreeImage.LoadBitmap(fileName, FREE_IMAGE_LOAD_FLAGS.DEFAULT, ref format);
        }

        /// <summary>
        /// 图片缩放
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Bitmap Rescale(string fileName, float scale)
        {
            FREE_IMAGE_FORMAT format = FreeImage.GetFileType(fileName, 0);
            FIBITMAP f = FreeImage.LoadEx(fileName, ref format);
            int width = (int)(FreeImage.GetWidth(f) * scale);
            int height = (int)(FreeImage.GetHeight(f) * scale);
            FIBITMAP fi = FreeImage.Rescale(f, width, height, FREE_IMAGE_FILTER.FILTER_BILINEAR);
            return FreeImage.GetBitmap(fi);
        }

        /// <summary>
        /// 图片缩放
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="des"></param>
        /// <param name="scale"></param>
        public static void Resize(string fileName, string des, double scale)
        {
            using (MagickImage m = new MagickImage(fileName))
            {
                m.Resize((int)(m.Width * scale), (int)(m.Height * scale));
                m.ColorType = ColorType.Bilevel;
                m.Write(des);
            }
        }

        public static void ChangeFormat(string fileName, string des, int format)
        {
            using (MagickImage m = new MagickImage(fileName))
            {
                m.ColorType = ColorType.Bilevel;
                m.Write(des);
            }
        }
        #endregion

        #region 图像合成
        /// <summary>
        /// 添加签名
        /// </summary>
        /// <param name="bgImagePath"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public static BitmapSource MakePicture(string bgImagePath, string signature)
        {
            //获取背景图
            BitmapImage bgImage = new BitmapImage(new Uri(bgImagePath, UriKind.Relative));
            //创建一个RenderTargetBitmap 对象，将WPF中的Visual对象输出
            RenderTargetBitmap composeImage = new RenderTargetBitmap(bgImage.PixelWidth, bgImage.PixelHeight, bgImage.DpiX, bgImage.DpiY, PixelFormats.Default);
            FormattedText signatureTxt = new FormattedText(signature,
                                                   System.Globalization.CultureInfo.CurrentCulture,
                                                   System.Windows.FlowDirection.LeftToRight,
                                                   new Typeface(System.Windows.SystemFonts.MessageFontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal),
                                                   28,
                                                   System.Windows.Media.Brushes.Black);
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawImage(bgImage, new Rect(0, 0, bgImage.Width, bgImage.Height));
            double x2 = 0;
            double y2 = 0;
            drawingContext.DrawText(signatureTxt, new System.Windows.Point(x2, y2));
            drawingContext.Close();
            composeImage.Render(drawingVisual);
            JpegBitmapEncoder bitmapEncoder = new JpegBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(composeImage));
            return composeImage;
        }

        /// <summary>
        /// 图片合成加签名
        /// </summary>
        /// <param name="bgImagePath"></param>
        /// <param name="headerImagePath"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public static BitmapSource MakePicture(string bgImagePath, string headerImagePath, string signature)
        {
            //获取背景图
            BitmapSource bgImage = new BitmapImage(new Uri(bgImagePath, UriKind.Relative));
            //获取头像
            BitmapSource headerImage = new BitmapImage(new Uri(headerImagePath, UriKind.Relative));
            //创建一个RenderTargetBitmap 对象，将WPF中的Visual对象输出
            RenderTargetBitmap composeImage = new RenderTargetBitmap(bgImage.PixelWidth, bgImage.PixelHeight, bgImage.DpiX, bgImage.DpiY, PixelFormats.Default);
            FormattedText signatureTxt = new FormattedText(signature,
                                                   System.Globalization.CultureInfo.CurrentCulture,
                                                   System.Windows.FlowDirection.LeftToRight,
                                                   new Typeface(System.Windows.SystemFonts.MessageFontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal),
                                                   30,
                                                   System.Windows.Media.Brushes.Black);
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawImage(bgImage, new Rect(0, 0, bgImage.Width, bgImage.Height));
            //计算头像的位置
            double x = (bgImage.Width / 2 - headerImage.Width) / 2;
            double y = (bgImage.Height - headerImage.Height) / 2 - 100;
            drawingContext.DrawImage(headerImage, new Rect(x, y, headerImage.Width, headerImage.Height));
            //计算签名的位置
            double x2 = (bgImage.Width / 2 - signatureTxt.Width) / 2;
            double y2 = y + headerImage.Height + 20;
            drawingContext.DrawText(signatureTxt, new System.Windows.Point(x2, y2));
            drawingContext.Close();
            composeImage.Render(drawingVisual);
            //定义一个JPG编码器
            JpegBitmapEncoder bitmapEncoder = new JpegBitmapEncoder();
            //加入第一帧
            bitmapEncoder.Frames.Add(BitmapFrame.Create(composeImage));
            //保存至文件（不会修改源文件，将修改后的图片保存至程序目录下）
            string savePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\合成.jpg";
            bitmapEncoder.Save(File.OpenWrite(System.IO.Path.GetFileName(savePath)));
            return composeImage;
        }

        /// <summary>
        /// 图片合成加签名
        /// </summary>
        /// <param name="bgImagePath"></param>
        /// <param name="headerImagePath"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public static BitmapSource MakePictureGDI(string bgImagePath, string headerImagePath, string signature)
        {
            GDI.Image bgImage = GDI.Bitmap.FromFile(bgImagePath);
            GDI.Image headerImage = GDI.Bitmap.FromFile(headerImagePath);
            //新建一个画板，画板的大小和底图一致
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(bgImage.Width, bgImage.Height);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);
            //先在画板上面画底图
            g.DrawImage(bgImage, new GDI.Rectangle(0, 0, bitmap.Width, bitmap.Height));
            //再在画板上画头像
            int x = (bgImage.Width / 2 - headerImage.Width) / 2;
            int y = (bgImage.Height - headerImage.Height) / 2 - 100;
            g.DrawImage(headerImage, new GDI.Rectangle(x, y, headerImage.Width, headerImage.Height),
                                     new GDI.Rectangle(0, 0, headerImage.Width, headerImage.Height),
                                     GDI.GraphicsUnit.Pixel);
            //在画板上写文字
            using (GDI.Font f = new GDI.Font("Arial", 20, GDI.FontStyle.Bold))
            {
                using (GDI.Brush b = new GDI.SolidBrush(GDI.Color.White))
                {
                    float fontWidth = g.MeasureString(signature, f).Width;
                    float x2 = (bgImage.Width / 2 - fontWidth) / 2;
                    float y2 = y + headerImage.Height + 20;
                    g.DrawString(signature, f, b, x2, y2);
                }
            }
            try
            {
                string savePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\GDI+合成.jpg";
                bitmap.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ToBitmapSource(bitmap);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                bgImage.Dispose();
                headerImage.Dispose();
                g.Dispose();
            }
        }

        #region GDI+ Image 转化成 BitmapSource
        public static BitmapSource ToBitmapSource(GDI.Bitmap bitmap)
        {
            IntPtr ip = bitmap.GetHbitmap();
            BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip, IntPtr.Zero, System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            DeleteObject(ip);//释放对象
            return bitmapSource;
        }
        #endregion
        #endregion
    }
}
