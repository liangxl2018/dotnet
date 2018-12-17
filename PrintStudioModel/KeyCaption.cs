using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintStudioModel
{
    public class KeyCaption
    {
        public int ID { get; set; }

        public string Caption { get; set; }

        public KeyCaption()
        { }

        public KeyCaption(int id, string caption)
        {
            ID = id;
            Caption = caption;
        }
    }
}
