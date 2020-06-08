using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace VisualTemplate
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        static private string _curDir = Directory.GetCurrentDirectory();



        static string templateFilePath = _curDir;
        static string templateFileName = "data.json";
        static string templateFullFileName = templateFilePath + @"\" + templateFileName;

        public static Dictionary<string, string> TypeOfProperty;
        public static Dictionary<int, string> Types;

        public static Dictionary<string, Variant> VariantsDic;

        public static Signal bufSignal;
        public static Cycle bufCycle;
        public static Element bufElem;
        public static Property bufProp;
        public static Variant bufVar;
        public static string defaultPathJson;
        public static string defaultPathOutCsv;

        public static string globalCsvString ="";

        public static Template t;
        public static ArrayList templateElements;
        

        [STAThread]
        static void Main()
        {
            templateElements = new ArrayList();
            t = new Template();
            t.Name = "new";
            
            TypeOfProperty = new Dictionary<string, string>();
            Types = new Dictionary<int, string>();
            VariantsDic = new Dictionary<string, Variant>();
            setDic();

            //var fs = new FileStream(templateFullFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //var sw = new StreamReader(fs);
            //string jsonStr = sw.ReadToEnd();
            //sw.Close();
            //t = JsonConvert.DeserializeObject<Template>(jsonStr);
            //RestoreParentsInTemplate(t);


            //getCsv(tm);






            //t = new Template();
            //t.Name = "chrp";

            //Cycle c1 = new Cycle(1, 2);
            //Cycle c2 = new Cycle(3, 4);
            //Cycle c3 = new Cycle(5, 6);

            //Variant v = new Variant("addr", "123");

            //t.Add(c1);
            //t.Add(new Cycle(7, 8));

            //Signal sFix = new Signal("FIX_1");
            //c1.Add(sFix);
            //Signal sMns = new Signal("$Lolka$");
            //sFix.Add(sMns);
            //Signal sLink = new Signal("F_CV");
            //sMns.Add(sLink);
            //sLink.Add(new Cycle(0, 15));
            //sLink.Cycles[0].Add(new Signal("byte"));

            //Property p = new Property("$num$", "UInt4", "8");
            //Property p1 = new Property("$num$", "UInt4", "$name$");

            //sLink.Add(p1);
            //Variant vd = new Variant("name", "Zalupka");
            //Variant vn = new Variant("num", "1");
            //Variant vn2 = new Variant("num", "1");
            //Variant vn3 = new Variant("Lolka", "10");
            //c1.Add(vd);
            //c1.Add(vn);
            //c1.Add(vn3);
            //sLink.Cycles[0].Add(vn);
            //sLink.Cycles[0].Signals[0].Add(p);

            //string json = JsonConvert.SerializeObject(t);
            //Console.WriteLine(json);

            //Template tm = JsonConvert.DeserializeObject<Template>(json);






            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void RemeberCsvStr(string str)
        {
            //if (str.LastIndexOf('\n') == str.Length)
            //{

            //}
            globalCsvString += str;// + "\n";
            
            //if (globalCsvString == "")
            //{
            //    globalCsvString += str + "\n";
            //}
            //else if(globalCsvString.Last() != '\n')
            //{
            //    globalCsvString +=  str;
            // //   globalCsvString += "\n" + str;
            //}
            //else
            //{
            //    globalCsvString += str;
            //}
        }

        public static void loadTemplate(OpenFileDialog op, TreeView trVi)
        {
            op.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            if (op.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            trVi.Nodes.Clear();
            templateElements.Clear();
            var fs = new FileStream(op.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(fs);
            string jsonStr = sr.ReadToEnd();
            t = JsonConvert.DeserializeObject<Template>(jsonStr);
            RestoreParentsInTemplate(t);

            addToTree(t, trVi);
            trVi.Nodes[0].Expand();
            // trVi.ExpandAll();
            sr.Close();
            defaultPathJson = Path.GetDirectoryName(op.FileName);
            Cursor.Current = Cursors.Default;
        }

        public static void deleteSelected(TreeNode trN)
        {
            
            Element e = getElementById(trN.Name);
            trN.Remove();
            templateElements[int.Parse(trN.Name)] = null;
            e.Remove();
        }

        public static void saveTemplate(SaveFileDialog sf)
        {
            if (!(defaultPathJson is null)) sf.InitialDirectory = defaultPathJson;
            sf.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            if (sf.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            string json = JsonConvert.SerializeObject(t);
            var fs = new FileStream(sf.FileName, FileMode.Create);
            var sw = new StreamWriter(fs);
            sw.Write(json);
            sw.Close();
            defaultPathJson = Path.GetDirectoryName(sf.FileName);
        }


        public static void setCycle(TreeNode trN, string st, string end, string scvFile)
        {
            Cycle c = getElementById(trN.Name) as Cycle;
            c.Start = int.Parse(st);
            c.End = int.Parse(end);
            c.csvVarFile = scvFile;
        }

        public static void setCycle(TreeNode trN, DataGridView dgv)
        {
            Cycle c = getElementById(trN.Name) as Cycle;
            c.Name = dgv.Rows[0].Cells[1].Value is null ? "":  dgv.Rows[0].Cells[1].Value.ToString();
            //dataGridSettings.Rows[0].Cells[1].Value.ToString(), dataGridSettings.Rows[1].Cells[1].Value.ToString(), dataGridSettings.Rows[2].Cells[1].Value.ToString()
            c.Start = int.Parse(dgv.Rows[1].Cells[1].Value.ToString());
            c.End = int.Parse(dgv.Rows[2].Cells[1].Value.ToString());
            //if (dgv.Rows[2].Cells[1].Value is null)
            c.csvVarFile = dgv.Rows[3].Cells[1].Value is null ? "" : dgv.Rows[3].Cells[1].Value.ToString();//dgv.Rows[2].Cells[1].Value.ToString();
            c.Description = dgv.Rows[4].Cells[1].Value is null ? "" : dgv.Rows[4].Cells[1].Value.ToString();//dgv.Rows[2].Cells[1].Value.ToString();
        }

        public static void setSignal(TreeNode trN, string name)
        {
            Signal s = getElementById(trN.Name) as Signal;
            s.Name = name;
        }

        //восстановление ссылок на родительские элементы после чтения из json
        public static void RestoreParentsInTemplate(Template t)
        {
            foreach (Cycle c in t.Elements)
            {
                c.tParent = t;
                RestoreParents(c);
                foreach(Variant v in c.Variatns)
                {
                    RestoreVar(v);
                }
            }
        }

        public static void addVariant(Cycle c)
        {
            int maxID=0;
            foreach(Variant v in VariantsDic.Values)
            {
                int vId;
                int.TryParse(v.Id, out vId);
                if (vId > maxID) maxID = vId;
                
            }
            Variant vn = new Variant("name", "value", (maxID+1).ToString());
            c.Add(vn);
            VariantsDic.Add(vn.Id, vn);
        }

        public static void RestoreVar(Variant v)
        {
           
            if (v.Id is null)
            {
                v.Id = (VariantsDic.Count + 1).ToString();
            }
            try
            {
                VariantsDic.Add(v.Id, v);
            }
            catch 
            {

            }
        }

        public static void newVarsInDic(Cycle c)
        {

            foreach (Variant v in c.Variatns)
            {
                int maxID = 0;
                foreach (Variant vd in VariantsDic.Values)
                {
                    int vId;
                    int.TryParse(vd.Id, out vId);
                    if (vId > maxID) maxID = vId;

                }

                v.Id = (maxID + 1).ToString();
                try
                {
                    VariantsDic.Add(v.Id, v);
                }
                catch
                {

                }
            }
        }

        // рекурсивная фунция восстановления
        private static void RestoreParents(Element eParent)
        {
            foreach (Element eChild in eParent.Elements)
            {
                eChild.restoreParent(eParent);
                if (eChild.GetType().ToString() == "VisualTemplate.Cycle")
                {
                    Cycle c = eChild as Cycle;
                    foreach (Variant v in c.Variatns)
                    {
                        RestoreVar(v);
                    }
                }
                if (eChild.HasElements)
                {
                    RestoreParents(eChild);
                }

            }
        }

        public static void setIdtoVar(Variant va)
        {
            int maxID = 0;
            foreach (Variant v in VariantsDic.Values)
            {
                int vId;
                int.TryParse(v.Id, out vId);
                if (vId > maxID) maxID = vId;

            }
            va.Id = (maxID + 1).ToString();
            VariantsDic.Add(va.Id, va);

        }

        public static void getSettings(Cycle c, TreeNode teN, DataGridView dgV)
        {
            dgV.Rows.Clear();
            dgV.Rows.Add("Name", c.Name);
            dgV.Rows.Add("Start",c.Start);
            dgV.Rows.Add("End", c.End);
            dgV.Rows.Add("CsvFile", c.csvVarFile);
            dgV.Rows.Add("Description", c.Description);
        }
        public static void getSettings(Signal s, TreeNode teN, DataGridView dgV)
        {
            dgV.Rows.Clear();
            dgV.Rows.Add("Name", s.Name);
        }
        public static void getSettings(Template t, TreeNode teN, DataGridView dgV)
        {
            dgV.Rows.Clear();
            dgV.Rows.Add("Name", t.Name);
        }



        public static void getCsv(Template t)
        {
            foreach(Cycle c in t.Elements)
            {
                runCycle(c);
            }
        }

        private static void runCycle (Cycle c)
        {
            c.CsvString = null;
            if (c.csvVarFile is null || c.csvVarFile == "")
            {
                for (c.Counter = c.Start; c.isGoOn; c.Increase())
                {
                    goOnCycle(c);
                 //   //c.CsvString = mathOnDollar(c.CsvString, ref k); //c.CsvString.Replace("$$", k.ToString());
                    culcValuec(c);
                }
               // Console.WriteLine(c.CsvString);
                RemeberCsvStr(c.CsvString);
                c.ResetValues();
            }
            else
            {
                readCsv(c, t.getCsvVar(c.csvVarFile));
               // Console.WriteLine(c.CsvString);
             //   RemeberCsvStr(c.CsvString);
                c.ResetValues();
            }
        }

        public static Cycle getParentCycle (Signal s)
        {
            if(s.Parent.GetType().ToString() == "VisualTemplate.Cycle")
            {
                return (Cycle)s.Parent;
            }
            else
            {
                return getParentCycle((Signal)s.Parent);
            }
        }

        public static Cycle getParentCycle(Element el)
        {
            if (el.Parent is null) return null;
            if (el.Parent.GetType().ToString() == "VisualTemplate.Cycle")
            {
                return (Cycle)el.Parent;
            }
            else
            {
                return getParentCycle(el.Parent);
            }
        }

        static void readCsv(Cycle c, CsvVar csv)
        {
            using (StreamReader sr = new StreamReader(csv.Path, csv.Encoding))
            {
                string line;
                string headers = sr.ReadLine();
                string[] headersArray = headers.Split(csv.Separator);
               // List<string> backupCycleParams = new List<string>(cycleParams);
                while ((line = sr.ReadLine()) != null)
                {
                    string[] lineArray = line.Split(csv.Separator);

                    for (int k = 0; k < headersArray.Length; k++)
                    {
                        for (int i = 0; i < c.Variatns.Count; i++)
                        {
                            if(c.Variatns[i].Value == "%"+headersArray[k]+"%")
                            {
                                c.Variatns[i].setTempValue(lineArray[k]);
                            }
                            
                        }
                    }
                    goOnCycle(c);
                //   // c.CsvString = mathOnDollar(c.CsvString, ref q);
                    culcValuec(c);

                   // Console.WriteLine(c.CsvString);
                    RemeberCsvStr(c.CsvString);
                    c.CsvString = "";
                    c.ResetValues();
                    //makeCsv2(getStatr(cycleParams[0]), getEnd(cycleParams[0]), 0);

                    //cycleParams.Clear();
                    //cycleParams.AddRange(backupCycleParams);

                }
            }
        }

        private static void culcValuec(Cycle c)
        {
            foreach (Variant v in c.Variatns)
            {

                int val;
                if (int.TryParse(v.Value, out val))
                {
                    v.Move();
                  // v.setTempValue((val + 1).ToString());
                }
               // c.CsvString = c.CsvString.Replace(v.NameS, v.Value);
            }
        }

        private static string replaceVariants(Cycle c, string csvStr)
        {
            foreach(Variant v in c.Variatns)
            {
                csvStr = replnmathVariant(csvStr, v);// csvStr.Replace(v.NameS, v.Value);
            }
            
            return csvStr;
        }

        private static void replaceVariantsInNames(Cycle c, Signal s)
        {
            foreach(Variant v in c.Variatns)
            {
                if(s.Name == v.NameS)
                {
                    
                    s.setTempName(v.Value);
                }
            }

            foreach (Variant v in c.Variatns)
            {
                if (s.Name.IndexOf(v.NameS) >= 0 )
                {

                    s.setTempName(replnmathVariant(s.Name, v)); //s.Name.Replace(v.NameS,v.Value));
                }
            }

        }

        private static void ResetSignalsNames(List<Signal> listOfSignals)
        {
            foreach(Signal s in listOfSignals)
            {
                s.ResetName();
            }
        }

        static string mathOnDollar(string fullStr, Cycle c, string rStr = "$")
        {
            int indOfdollar;
            if (fullStr is null) return null;
            if(rStr != "$")
            {
                indOfdollar = fullStr.IndexOf(rStr);
            }
            else
            {
                if((indOfdollar= fullStr.IndexOf("$$"))<0)
                {
                    if((indOfdollar = fullStr.IndexOf("$+")) < 0)
                    {
                        if ((indOfdollar = fullStr.IndexOf("$-")) < 0)
                        {
                            return fullStr;
                        }
                    }
                }
            }
          
          //  indOfdollar = fullStr.IndexOf("$");
            
            if (indOfdollar == -1) return fullStr;
            char[] fullStrCharArray = fullStr.ToCharArray();
            int modK = c.Counter;

            string remStr = rStr;
            int rSLen = rStr.Length;
            string remStrInt = "";
            int number;
            string operation = "";

            switch (fullStrCharArray[indOfdollar + rSLen])
            {
                //case '$':
                //    remStr += "$";
                //    operation = "";
                //    break;
                case '+':
                    remStr += "+";
                    operation = "+";
                    for (int i = indOfdollar + rSLen; i < fullStrCharArray.Length; i++)
                    {

                        if (int.TryParse(fullStrCharArray[i + 1].ToString(), out number))
                        {
                            remStr += fullStrCharArray[i + 1].ToString();
                            remStrInt += fullStrCharArray[i + 1].ToString();

                        }
                        else if (fullStrCharArray[i + 1] == '=')
                        {
                            remStr += "=";
                            operation += "=";
                        }
                        else
                        {
                            break;
                        }
                    }
                    break;
                case '-':
                    remStr += "-";
                    operation = "-";
                    for (int i = indOfdollar + rSLen; i < fullStrCharArray.Length; i++)
                    {

                        if (int.TryParse(fullStrCharArray[i + 1].ToString(), out number))
                        {
                            remStr += fullStrCharArray[i + 1].ToString();
                            remStrInt += fullStrCharArray[i + 1].ToString();

                        }
                        else
                        {
                            break;
                        }
                    }
                    break;

            }

            int.TryParse(remStrInt, out number);
            remStr += "$";
            switch (operation)
            {
                case "+":
                    modK += number;
                    break;
                case "-":
                    modK -= number;
                    break;
                case "+=":
                    c.Counter += number;
                    modK = c.Counter;
                    break;
            }

            fullStr = fullStr.Replace(remStr, modK.ToString());
            if ( fullStr.Contains("$+") | fullStr.Contains("$-") | (rStr=="$"? fullStr.Contains("$$") : fullStr.Contains(rStr)))
            {
                fullStr = mathOnDollar(fullStr, c, rStr);
            }
            return fullStr;
        }

        static string replnmathVariant(string csvStr, Variant v)
        {
            
            char[] fullStrCharArray = csvStr.ToCharArray(); //преобразовываем в массив символов
            int IndexOfVarName = csvStr.IndexOf(v.NameS); //ищем номер вхождения имени искомой переменной
            string remStr = v.NameS; //запоминаем строку для последующей замены
            int IndexOfVarNameEnd = IndexOfVarName + v.NameS.Length;
            int operand;
            string operandStr = "";
            string operation = "";
            if (fullStrCharArray.Length > IndexOfVarNameEnd)  operation = fullStrCharArray[IndexOfVarNameEnd].ToString();

            if (operation == "+" | operation == "-")
            {
                remStr += operation;
                for (int k = IndexOfVarNameEnd + 1; k < fullStrCharArray.Length; k++)
                {
                    if (int.TryParse(fullStrCharArray[k].ToString(), out operand))
                    {
                        remStr += fullStrCharArray[k].ToString();
                        operandStr += fullStrCharArray[k].ToString();
                    }
                    else if (fullStrCharArray[k] == '=')
                    {
                        remStr += "=";
                        operation += "=";
                    }
                    else
                    {
                        break;
                    }
                }
                remStr += "$";
            }

            //запоминаем значение переменной для модификации
            int modPerem;
            if(int.TryParse(v.Value, out modPerem))
            {
                int.TryParse(operandStr, out operand);
                switch (operation)
                {
                    case "+":
                        modPerem += operand;
                        break;
                    case "-":
                        modPerem -= operand;
                        break;
                    case "+=":
                        modPerem += operand;
                        v.setTempValue(modPerem.ToString());
                        break;
                    case "-=":
                        modPerem -= operand;
                        v.setTempValue(modPerem.ToString());
                        break;
                }
                csvStr = csvStr.Replace(remStr, modPerem.ToString());
            }
            else
            {
                if (v.Value.IndexOf("\"\"")>0)
                {
                    csvStr = csvStr.Replace("\"" + remStr + "\"", v.Value);
                }
                else
                {
                    csvStr = csvStr.Replace(remStr, v.Value);
                }
                
            }
            
            if (csvStr.Contains(v.NameS))
            {
                csvStr = replnmathVariant(csvStr,v);
            }
            return csvStr;
        }

        private static void replaceInVarinatValue(Cycle c, Cycle pC)
        {
            foreach(Variant v in c.Variatns)
            {
                if (v.Value.IndexOf("$") >= 0)
                {
                    foreach(Variant vp in pC.Variatns)
                    {
                        if(v.Value == vp.NameS)
                        {
                            v.setTempValue(vp.Value);
                        }
                    }
                }
            }
        }

        private static void goOnCycle(Cycle c, Signal sParent = null, string propStr = "")
        {
            List<Signal> listOfSignals;
            //если цикл имеет сигналы
            if (c.HasSignals)
            {
                //если нет родительского сигнала, тогда перебираем элементы в цикле, иначе перебираем элементы в сигнале
                if (sParent is null)
                {
                    listOfSignals = c.Signals;
                }
                else
                {
                    listOfSignals = sParent.Signals;
                }

                //Cycle prtCycle = getParentCycle(c);
                //if (!(prtCycle is null)) replaceInVarinatValue(c, prtCycle);

                //перебираем сигналы
                foreach (Signal sCh in listOfSignals)
                {
                    replaceVariantsInNames(c, sCh);
                    if (sCh.Name.IndexOf("$") > 0) sCh.setTempName(mathOnDollar(sCh.Name, c));
                    //если у сигнала есть свойства тогда ...
                    if (sCh.HasProperties)
                    {
                        //... перебираем и добавляем их в ...
                        foreach (Property p in sCh.Properties)
                        {
                           // if (c.HasCsvString) c.CsvString += "\n";

                            if (p.Type == "String" && p.Value.Substring(0, 1) != "\"") p.Value = "\"" + p.Value + "\"";
                            string csvStr = "\""+ sCh.FullPath +"\"" + "," + p.Id + "," + p.Type + "," + p.Value;
                        //    Console.WriteLine(csvStr);
                            
                            csvStr = replaceVariants(c, csvStr);
                            csvStr = replaceParentVariant(csvStr, sCh);
                            csvStr =  mathOnDollar(csvStr,c);
                            
                            //если у нас есть хоть одна переменная из родительского цикла
                            // то перебираем все переменные из всех родительских циклов.
                            //if (csvStr.IndexOf("$.") > 0)
                            //{
                            
                        //    }
                            //
                            //здесь можно добавить функцию вывода
                            //Console.WriteLine(csvStr);
                            //

                            c.CsvString += csvStr + "\n";
                      }
                    }

                    //если сигнал имеет дочерние сигналы тогда вызываем себя ещё раз
                    if (sCh.HasSignals)
                    {
                        goOnCycle(c, sCh, propStr);
                    }
                    //если сигнал имеет дочерние циклы то запускаем цикл
                    if (sCh.HasCycles)
                    {
                        foreach (Cycle cCh in sCh.Cycles)
                        {
                            runCycle(cCh);
                        }
                    }
                    ResetSignalsNames(listOfSignals);
                }
            }
            // если цикл имеет дочерние циклы
            if (c.HasCycles)
            {
                foreach (Cycle cCh in c.Cycles)
                {
                    runCycle(cCh);
                }
            }
        }


        private static string replaceParentVariant(string CsvStr, Signal sCh)
        {
            string outStr = CsvStr;
            Cycle pCycle = getParentCycle(getParentCycle(sCh));
            int deep = 1;
            
            while (!(pCycle is null))
            {
                int p;
                int indexOf = outStr.IndexOf("$.");
                outStr = replaceVariants(pCycle, outStr);
                while (indexOf>0 & int.TryParse(outStr[indexOf+2].ToString(), out p) )
                {
                    int od = 0;
                    int d = 0;
                    string rmbStr = "$.";
                    for(int h = indexOf; h < outStr.Length; h++)
                    {
                        if ( int.TryParse( outStr[h+2].ToString(),out od))
                        {
                            rmbStr += outStr[h + 2].ToString();
                            d = od;
                        }
                        else
                        {
                            break;
                        }
                    }
                   // rmbStr += "$";
                    if (deep == d)
                    {
                        outStr = mathOnDollar(CsvStr, pCycle, rmbStr);
                        indexOf = outStr.IndexOf("$.");
                        //outStr = outStr.Replace(rmbStr, pCycle.Counter.ToString());
                    }
                    
                }
                pCycle = getParentCycle(pCycle);
                deep++;
            }
            return outStr;
        }



        public static object getObjectFromTree(string key)
        {
            int id;
           if( int.TryParse(key, out id))
            {
                return getObject(id);
            }
           else
            {
                return null;
            }
        }

        private static object getObject(int id)
        {
            return templateElements[id];
        }

        public static Element getElementById(int id)
        {
            return templateElements[id] as Element;
        }
        public static Element getElementById(string key)
        {
            int id;
            if (int.TryParse(key, out id))
            {
                return templateElements[id] as Element;
            }
            else
            {
                return null;
            }
        }

        public static void ReplaceInTree(TreeNode trN, string find, string rep)
        {
            Element el = getElementById(trN.Name);
            Service.Replace(el,find,rep);
        }

        public static void setVariants(Cycle c, DataGridView dgV)
        {
            int vNum = 0;
            int st = 1;
            foreach(DataGridViewRow row in dgV.Rows)
            {
                c.Variatns[vNum].Name = row.Cells[0].Value.ToString();
                c.Variatns[vNum].Value = row.Cells[1].Value.ToString();
                c.Variatns[vNum].Step =  int.TryParse(row.Cells[2].Value.ToString(), out st) ? st : 1;
                vNum++;
            }
        }

        public static void setCsvVarPath(DataGridView dgV)
        {
            int cvNum = 0;
            foreach (DataGridViewRow row in dgV.Rows)
            {
                t.CsvVars[cvNum].Name = row.Cells[0].Value.ToString();
                t.CsvVars[cvNum].Path = row.Cells[1].Value.ToString();
                t.CsvVars[cvNum].Separator = row.Cells[2].Value.ToString().ToCharArray()[0];
                switch (row.Cells[3].Value.ToString())
                {
                    case "Default":
                        t.CsvVars[cvNum].Encoding = Encoding.Default;
                        break;
                    case "ASCII":
                        t.CsvVars[cvNum].Encoding = Encoding.ASCII;
                        break;
                    case "UTF8":
                        t.CsvVars[cvNum].Encoding = Encoding.UTF8;
                        break;
                }
                cvNum++;
            }
        }

        public static void setProperties(Signal s, DataGridView dgV, TreeNode trN = null)
        {
            int pNum = 0;
            foreach (DataGridViewRow row in dgV.Rows)
            {
                s.Properties[pNum].Id = row.Cells[0].Value.ToString();
                s.Properties[pNum].Type = row.Cells[1].Value.ToString();
                s.Properties[pNum].Value = row.Cells[2].Value.ToString();
                if (!(trN is null) && s.Properties[pNum].Id == "1")
                {
                    trN.ImageIndex = Service.getImgCodeById(s.Properties[pNum].Value);
                    trN.SelectedImageIndex = Service.getImgCodeById(s.Properties[pNum].Value);
                }
                pNum++;
                

            }
        }

        public static void getVariants(Cycle c, DataGridView dgV)
        {
            dgV.Rows.Clear();
            foreach (Variant v in c.Variatns)
            {
                dgV.Rows.Add(v.Name, v.Value, v.Step);
            }
        }
        public static void addCsvVar()
        {
            CsvVar csv = new CsvVar(@"C:\file.csv");
            csv.Name = "Csv1";
            csv.Separator = ';';
            t.Add(csv);
        }

        public static void getProperties(Signal s, DataGridView dgV, TreeNode trN = null)
        {
            dgV.Rows.Clear();
            if(!(trN is null) )
            {
                trN.ImageIndex = 0;
                trN.SelectedImageIndex = 0;
            }
            foreach (Property p in s.Properties)
            {
                dgV.Rows.Add(p.Id, p.Type,p.Value);
                if(!(trN is null) && p.Id == "1")
                {
                        trN.ImageIndex = Service.getImgCodeById(p.Value);
                        trN.SelectedImageIndex = Service.getImgCodeById(p.Value);
                }
            }
        }

        public static void getCsvVars (DataGridView dgV)
        {
            dgV.Rows.Clear();
            foreach (CsvVar cv in t.CsvVars)
            {
                dgV.Rows.Add(cv.Name,cv.Path, cv.Separator.ToString(), cv.encodingStr);
            }
        }

        public static void addCycle(TreeNode trN)
        {

            Cycle c = new Cycle(1, 1);

            if (trN.Text == t.Name)
            {
                t.Add(c);
            }
            else
            {
                Element curElement = getElementById(trN.Name);
                curElement.Add(c);
            }

            c.Id = templateElements.Count.ToString();
            trN.Nodes.Add(c.Id, c.ToString(),15);
            templateElements.Add(c);
        }
        public static void addSignal(TreeNode trN)
        {

            Signal s = new Signal("signal");
            if (trN is null) return;
            Element curElement = getElementById(trN.Name);
            if (curElement is null) return;
            curElement.Add(s);


            s.Id = templateElements.Count.ToString();
            trN.Nodes.Add(s.Id, s.ToString(), 0);
            templateElements.Add(s);
        }

        public static void RestoreLinks(Cycle c)
        {
            List<Variant> toReplace = new List<Variant>();
            foreach (Variant v in c.Variatns)
            {
                if (!(v.Link is null))
                {
                    toReplace.Add(v);
                }
            }
            foreach(Variant v in toReplace)
            {
                string link = v.Link;
                c.Variatns.Remove(v);
                c.Add(VariantsDic[link]);
            }

        }

       // public static void addToTree(Element o, TreeNode tr, TreeNode parentTreeNode = null)

        public static void addToTree(object o , TreeView tr, TreeNode parentTreeNode = null)
        {

            if(o.GetType().ToString() == "VisualTemplate.Template")
            {
                Template temp = o as Template;

                parentTreeNode = tr.Nodes.Add(templateElements.Count.ToString(), temp.Name);

                templateElements.Add(temp);

                if (temp.HasElements)
                {
                    foreach (object chObj in temp.Elements)
                    {
                        addToTree(chObj, tr, parentTreeNode);
                    }
                }
            }

            switch (o.GetType().ToString())
            {
                case "VisualTemplate.Cycle":
                    Cycle tempC = o as Cycle;
                    tempC.Id = templateElements.Count.ToString();
                    parentTreeNode = parentTreeNode.Nodes.Add(tempC.Id, tempC.ToString(),15);

                    templateElements.Add(tempC);

                    RestoreLinks(tempC);

                    if (tempC.HasElements)
                    {
                        foreach (object chObj in tempC.Elements)
                        {
                            addToTree(chObj, tr, parentTreeNode);
                        }
                    }
                    break;
                case "VisualTemplate.Signal":
                    Signal tempS = o as Signal;
                    tempS.Id = templateElements.Count.ToString();
                    parentTreeNode = parentTreeNode.Nodes.Add(tempS.Id, tempS.Name, tempS.ImgCode);
                    templateElements.Add(tempS);

                    if (tempS.HasElements)
                    {
                        foreach (object chObj in tempS.Elements)
                        {
                            addToTree(chObj, tr, parentTreeNode);
                        }
                    }
                    break;
            }
        }


        private static void setDic()
        {
            TypeOfProperty.Add("1", "UInt4");
            TypeOfProperty.Add("2", "VAR");
            TypeOfProperty.Add("3", "UInt4");
            TypeOfProperty.Add("4", "STL:time");
            TypeOfProperty.Add("5", "UInt4");
            TypeOfProperty.Add("6", "Float");
            TypeOfProperty.Add("100", "String");
            TypeOfProperty.Add("101", "String");
            TypeOfProperty.Add("6500", "String");
            TypeOfProperty.Add("5000", "String");
            TypeOfProperty.Add("5001", "String");
            TypeOfProperty.Add("5002", "VAR");
            TypeOfProperty.Add("5100", "Double");
            TypeOfProperty.Add("5101", "Double");
            TypeOfProperty.Add("5102", "Double");
            TypeOfProperty.Add("5103", "Double");
            TypeOfProperty.Add("5104", "Double");
            TypeOfProperty.Add("5105", "Double");
            TypeOfProperty.Add("5106", "Bool");
            TypeOfProperty.Add("5107", "Bool");
            TypeOfProperty.Add("5108", "Bool");
            TypeOfProperty.Add("6001", "String");
            TypeOfProperty.Add("6002", "UInt4");
            TypeOfProperty.Add("6003", "Bool");
            TypeOfProperty.Add("6004", "Bool");
            TypeOfProperty.Add("6005", "Bool");
            TypeOfProperty.Add("6100", "String");
            TypeOfProperty.Add("7000", "Bool");
            TypeOfProperty.Add("8000", "Bool");
            TypeOfProperty.Add("9001", "Bool");
            TypeOfProperty.Add("9002", "String");
            TypeOfProperty.Add("10000", "Bool");
            TypeOfProperty.Add("777005", "String");
            TypeOfProperty.Add("777006", "String");
            TypeOfProperty.Add("777010", "String");
            TypeOfProperty.Add("777011", "String");
            TypeOfProperty.Add("777012", "String");
            TypeOfProperty.Add("777013", "String");
            TypeOfProperty.Add("777015", "UInt4");
            TypeOfProperty.Add("777016", "String");
            TypeOfProperty.Add("777017", "String");
            TypeOfProperty.Add("777018", "String");
            TypeOfProperty.Add("6000", "UInt4");
            TypeOfProperty.Add("999000", "String");
            TypeOfProperty.Add("999001", "UInt4");
            TypeOfProperty.Add("999002", "String");
            TypeOfProperty.Add("999003", "Bool");
            TypeOfProperty.Add("999004", "String");
            TypeOfProperty.Add("999005", "Bool");

            Types.Add(1, "Int1");
            Types.Add(2, "UInt1");
            Types.Add(3, "Int2");
            Types.Add(4, "UInt2");
            Types.Add(5, "Int4");
            Types.Add(6, "UInt4");
            Types.Add(7, "Int8");
            Types.Add(8, "UInt8");
            Types.Add(9, "Float");
            Types.Add(10, "Double");
            Types.Add(11, "Bool");
            Types.Add(12, "String");
            Types.Add(13, "Folder");
            Types.Add(14, "VAR");
        }


    }
}
