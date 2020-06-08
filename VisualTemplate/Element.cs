using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualTemplate
{
    class Element
    {
        protected ArrayList _elements;
        protected Element _parent;
        protected string _fullpath;
        protected string _originalName;
        protected string _name;

        

        [JsonIgnore]
        public Template tParent { get; set; }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                _originalName = value;
            }
        }

        public string Description { get; set; }


        [JsonIgnore]
        public ArrayList Elements
        {
            get
            {
                _elements.Clear();
                _elements.AddRange(Cycles);
                _elements.AddRange(Signals);
                return _elements;
            }
        }

        [JsonIgnore]
        public bool HasElements
        {
            get
            {
                if (Elements.Count > 0)
                {
                    return true;
                }
                {
                    return false;
                }
            }
        }


        [JsonIgnore]
        public bool HasSignals
        {
            get
            {
                if (Signals.Count > 0)
                {
                    return true;
                }
                {
                    return false;
                }
            }
        }

        [JsonIgnore]
        public bool HasCycles
        {
            get
            {
                if (Cycles.Count > 0)
                {
                    return true;
                }
                {
                    return false;
                }
            }
        }

        [JsonIgnore]
        public Element Parent
        {
            get
            {
                return _parent;
            }
        }

        public List<Cycle> Cycles { get; set; }
        public List<Signal> Signals { get; set; }

        [JsonIgnore]
        public string FullPath
        {
            get
            {
                _fullpath = Name;
                getPath(this);
                return _fullpath;
            }
        }

        public void Add(Signal s)
        {
            Signals.Add(s);
            s._parent = this;
        }

        public void Add(Cycle c)
        {
            Cycles.Add(c);
            c._parent = this;
        }

        private void getPath(Element element)
        {
            Element parent = element.Parent;
            if (parent is null) return;
            if(parent.GetType().ToString() == "VisualTemplate.Cycle")
            {
                _fullpath = _fullpath.Insert(0, "");
            }
            else
            {
                _fullpath = _fullpath.Insert(0, parent.Name + ".");
            }
            getPath(parent);
        }

        public void restoreParent(Element e)
        {
            _parent = e;
        }

        public void Remove()
        {
            switch (GetType().ToString())
            {
                case "VisualTemplate.Cycle":
                    if (Parent is null)
                    {
                        tParent.Cycles.Remove(this as Cycle);
                        return;
                    }
                    Parent.Cycles.Remove(this as Cycle);
                    break;
                case "VisualTemplate.Signal":
                    Parent.Signals.Remove(this as Signal);
                    break;

            }

            if(GetType().ToString() == "VisualTemplate.Cycle")
            {

            }
        }


    }
}
