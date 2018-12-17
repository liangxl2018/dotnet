using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZXing.Common;
using ZXing;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using ZXing.QrCode;

namespace PrintStudioRule
{
    public class BarCodeHelper
    {
        /// <summary>
        /// 条码生成
        /// </summary>
        /// <param name="content"></param>
        /// <param name="format"></param>
        /// <param name="encodingOptions"></param>
        /// <returns></returns>
        public static Bitmap CreateBarCode(string content, BarcodeFormat format, EncodingOptions encodingOptions)
        {
            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = format;
            writer.Options = encodingOptions;
            return writer.Write(content);
        }

        /// <summary>
        /// 条码解析
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static Result DecodeBarCode(Image img)
        {
            BarcodeReader reader = new BarcodeReader();
            return reader.Decode((Bitmap)img);
        }
    }
}
