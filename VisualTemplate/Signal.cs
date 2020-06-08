using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualTemplate
{

    class Signal : Element, ICloneable
    {

        [JsonIgnore]
        public string Id { get; set; }

        private int _imgCode;

        public int ImgCode
        {
            get
            {
                if (HasProperties)
                {
                    foreach(Property p in Properties)
                    {
                        if(p.Id == "1")
                        {
                            return Service.getImgCodeById(p.Value);
                        }
                    }
                    return 0;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                _imgCode = value;
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public List<Property> Properties { get; set; }

        [JsonIgnore]
        public bool HasProperties
        {
            get
            {
                if (Properties.Count > 0)
                {
                    return true;
                }
                {
                    return false;
                }
            }
        }

        public Signal(string name)
        {
            _elements = new ArrayList();
            _originalName = name;
            Name = name;
            Properties = new List<Property>();
            Signals = new List<Signal>();
            Cycles = new List<Cycle>();
        }

        public void Add(Property p)
        {
            Properties.Add(p);
        }

        private void setPath(string parentPath)
        {
            _fullpath = parentPath + "." + Name;
            
        }
        public void setTempName(string tName)
        {
            _name = tName;
        }

        public void ResetName()
        {
            Name = _originalName;
        }

        public object Clone()
        {
            Signal s = new Signal(Name);
            foreach(Signal chS in Signals)
            {
                s.Add((Signal)chS.Clone());
            }
            foreach (Property chP in Properties)
            {
                s.Add((Property)chP.Clone());
            }
            foreach (Cycle chC in Cycles)
            {
                s.Add((Cycle)chC.Clone());
            }
            return s;
        }
    }
}
