using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace VisualTemplate
{
   public class Master
    {
        
        public string Name { get; set; }
        public List<string> Files { get; set; }

        [NonSerialized]
        public Dictionary<int,TempTabPage> tempTabPagesDic;

        [NonSerialized]
        public string curPath;

        public Master()
        {
            tempTabPagesDic = new Dictionary<int, TempTabPage>();
            Files = new List<string>();
        }

    }
}
