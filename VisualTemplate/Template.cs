using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualTemplate
{
   public  class Template
    {
        private ArrayList _elements;
        
        public List<CsvVar> CsvVars { get; set; }

        [JsonIgnore]
        public ArrayList Elements
        {
            get
            {
                _elements.Clear();
                _elements.AddRange(Cycles);
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
        public string CurPath { get; set; }
        public string Name { get; set; }
        public List<Cycle> Cycles { get; set; }

        public Template()
        {
            _elements = new ArrayList();
            Cycles = new List<Cycle>();
            CsvVars = new List<CsvVar>();
        }

        public Template(string name)
        {
            _elements = new ArrayList();
            Cycles = new List<Cycle>();
            CsvVars = new List<CsvVar>();
            Name = name;
        }

        public void Add(Cycle c)
        {
            Cycles.Add(c);
            c.tParent = this;
        }

        public void Add(CsvVar cv)
        {
            CsvVars.Add(cv);
        }

        public CsvVar getCsvVar(string name)
        {
            foreach(CsvVar sv in CsvVars)
            {
                if (name == sv.Name) return sv;
            }
            return null;
        }
    }
}
