using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintStudioModel
{
    public class PrintBarcodeDefaultParameter : Dictionary<string, string>
    {
        public PrintBarcodeDefaultParameter()
        {
            Add("X", "10");
            Add("Y", "20");
        }
    }
}
