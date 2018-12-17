using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintStudioModel
{
    public class PrintItemControlModel
    {
        public string Caption { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }

    public class PrintClientConfig
    {
        public List<PrintItemControlModel> PrintItemControls { get; set; }

        private int x = 1;
        public int X { get { return x; } set { x = value; } }

        private int y = 1;
        public int Y { get { return y; } set { y = value; } }

        private int printTypeIndex = 2;
        public int PrintTypeIndex { get { return printTypeIndex; } set { printTypeIndex = value; } }

        private int printNameIndex = 0;
        public int PrintNameIndex { get { return printNameIndex; } set { printNameIndex = value; } }
    }
}
