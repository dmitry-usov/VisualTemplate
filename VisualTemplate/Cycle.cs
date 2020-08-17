using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualTemplate 
{
    public class Cycle : Element, ICloneable
    {
        public List<Variant> Variatns { get; set; }

        private int _start;
        private int _end;
        private int _counter;

        //public new string Name { get; set; }
        [JsonIgnore]
        public string Id { get; set; }

        public string csvVarFile { get; set; }

        [JsonIgnore]
        public int Counter
        { get
            {
                return _counter;
            }
            set
            {
                _counter = value;
            }
        }

        [JsonIgnore]
        public bool HasVariants
        {
            get
            {
                if (Variatns.Count > 0)
                {
                    return true;
                }
                {
                    return false;
                }
            }
        }

        [JsonIgnore]
        public bool isGoOn {
            get
            {
                if(_counter <= _end)
                {
                    return true;
                }
                else
                {
                    _counter = _start;
                    return false;
                }
            }
        }


        public override string ToString()
        {
            if (!(csvVarFile is null) && csvVarFile != "")
            {
                return "[csv] " + Name;
            }
            return "[" + Start.ToString() + " to " + End.ToString() + "] " + Name;
        }

        public int Start {
            get
            {
                return _start;
            }

            set
            {
                _start = value;
            }
        }

        public int End
        {
            get
            {
                return _end;
            }

            set
            {
                _end = value;
            }
        }
        [JsonIgnore]
        public string CsvString { get; set; }
        [JsonIgnore]
        public bool HasCsvString
        {
            get
            {
                if (CsvString is null)
                {
                    return false;
                }
                return true;
            }
        }



        public Cycle(int start, int end)
        {
            _elements = new ArrayList();
            _start = start;
            _end = end;
            Cycles = new List<Cycle>();
            Variatns = new List<Variant>();
            Signals = new List<Signal>();
            _counter = Counter;
        }
        public void Increase()
        {
            _counter++;
        }

        public void Add(Variant v)
        {
            Variatns.Add(v);
        }

        public void ResetValues()
        {
            foreach (Variant v in Variatns)
            {
                v.ResetValue();
            }
        }
        public void ResetSignalsNames()
        {
            foreach (Signal s in Signals)
            {
                s.ResetName();
            }
        }

        public object Clone()
        {
            Cycle c = new Cycle(Start, End);
            c.Name = Name;
            c.Description = Description;
            
            foreach (Signal chS in Signals)
            {
                c.Add((Signal)chS.Clone());
            }
            foreach (Cycle chC in Cycles)
            {
                c.Add((Cycle)chC.Clone());
            }
            foreach (Variant chV in Variatns)
            {
                c.Add((Variant)chV.Clone());
            }
            c.csvVarFile = csvVarFile;
            //foreach (Property chP in Properties)
            //{
            //    s.Add((Property)chP.Clone());
            //}
            return c;
        }

    }
}
