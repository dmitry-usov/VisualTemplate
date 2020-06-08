using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualTemplate
{
    class Variant : ICloneable
    {
        private string _value;
        public string Name { get; set; }
        public string Id { get; set; }
        public string Link { get; set; }
        public int Step { get; set; }

        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                _originalValue = value;
            }
        }

        private string _originalValue;
        [JsonIgnore]
        public string NameS { get { return "$" + Name + "$"; } }

        //public Variant()
        //{
        //}

        //public Variant(string name, string value)
        //{
        //    _originalValue = value;
        //    Name = name;
        //    Value = value;
        //}

        public Variant(string name, string value, string id)
        {
            _originalValue = value;
            Name = name;
            Value = value;
            Id = id;
            Step = 1;
        }

        public void ResetValue()
        {
            Value = _originalValue;
        }
        public void Move()
        {
            int val;
            if (int.TryParse(_value, out val))
            {
                setTempValue((val + Step).ToString());
                // v.setTempValue((val + 1).ToString());
            }
            
        }

        public void setTempValue(string value)
        {
            _value = value;
        }

        public object Clone()
        {
            Variant v = new Variant(Name, Value,"");
            v.Step = Step;
            return v;
        }
    }
}
