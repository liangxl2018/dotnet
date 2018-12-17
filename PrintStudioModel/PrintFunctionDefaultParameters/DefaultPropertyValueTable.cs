using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintStudioModel
{
    public class DefaultPropertyValueTable
    {
        static DefaultPropertyValueTable()
        {
            PrintFunctionDefaultParameters.Add("PrintTextWorld",new PrintTextWorldDefaultParameter());
            PrintFunctionDefaultParameters.Add("PrintBarcode", new PrintBarcodeDefaultParameter());
        }

        public static Dictionary<string, Dictionary<string, string>> PrintFunctionDefaultParameters = new Dictionary<string, Dictionary<string, string>>();
    }
}
