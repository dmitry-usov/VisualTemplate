using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualTemplate
{
    static class Service
    {
        public static bool Replace(Element el, string str1, string str2)
        {

           if(!(el.Name is null)) el.Name = el.Name.Replace(str1, str2);

            switch (el.GetType().ToString())
            {
                case "VisualTemplate.Cycle":
                    Cycle c = el as Cycle;
                    foreach (Variant v in c.Variatns)
                    {
                        v.Name = v.Name.Replace(str1, str2);
                        v.Value = v.Value.Replace(str1, str2);
                    }
                    break;
                case "VisualTemplate.Signal":
                    Signal s = el as Signal;
                    foreach (Property p in s.Properties)
                    {
                        p.Id = p.Id.Replace(str1, str2);
                        p.Type = p.Type.Replace(str1, str2);
                        p.Value = p.Value.Replace(str1, str2);
                    }
                    break;
            }
            if (el.HasElements)
            {
                foreach(Element e in el.Elements)
                {
                    Replace(e, str1, str2);
                }
            }

            return true;
        }

        public static int getImgCodeByType(string type)
        {
            switch (type)
            {
                case "Int1":
                    return 1;
                case "UInt1":
                    return 2;
                case "Int2":
                    return 3;
                case "UInt2":
                    return 4;
                case "Int4":
                    return 5;
                case "UInt4":
                    return 6;
                case "Int8":
                    return 7;
                case "UInt8":
                    return 8;
                case "Float":
                    return 9;
                case "Double":
                    return 10;
                case "Bool":
                    return 11;
                case "String":
                    return 12;
                case "Folder":
                    return 0;
                default:
                    return 13;
            }
        }

        public static int getImgCodeById(string Id)
        {
            switch (Id)
            {
                case "1":
                    return 1;
                case "3":
                    return 2;
                case "9":
                    return 3;
                case "8":
                    return 4;
                case "7":
                    return 5;
                case "6":
                    return 6;
                case "13":
                    return 7;
                case "12":
                    return 8;
                case "14":
                    return 9;
                case "15":
                    return 10;
                case "5":
                    return 11;
                case "17":
                    return 12;
                case "Folder":
                    return 0;
                default:
                    return 13;
            }
        }



    }
}
