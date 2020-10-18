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
using System.Xml.Serialization;

namespace VisualTemplate
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        static private string _curDir = Directory.GetCurrentDirectory();
        static public Dictionary<int,TempTabPage> TemplatesPages;
        static public Master MasterFile;


        static string templateFilePath = _curDir;
        static string templateFileName = "data.json";
        static string templateFullFileName = templateFilePath + @"\" + templateFileName;

        public static Dictionary<string, string> TypeOfProperty;
        public static Dictionary<int, string> Types;
        public static Dictionary<string, string> CDTTypes;

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

        public static MainForm pMainForm;

        [STAThread]
        static void Main()
        {
            
            TemplatesPages = new Dictionary<int, TempTabPage>();
           MasterFile = new Master() ;
            //MasterFile.Files.Add("diag");
            //НЕОПТИМИЗИРОВАННОЕ ГОВНО ДАЛЕЕ


            //t = new Template();
            //t.Name = "new";

            TypeOfProperty = new Dictionary<string, string>();
            CDTTypes = new Dictionary<string, string>();
            Types = new Dictionary<int, string>();
            VariantsDic = new Dictionary<string, Variant>();
            setDic();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(pMainForm = new MainForm());
        }

        public static void newMaster(TreeView trV)
        {
            MasterFile = new Master() { Name = "new"};

        }

        internal static void openMaster(TreeView trV)
        {
            trV.Nodes.Clear();
            trV.Nodes.Add(MasterFile.Name);
            if (MasterFile.Files is null) return;
            foreach (TempTabPage tmp in MasterFile.tempTabPagesDic.Values)
            {
                trV.Nodes[0].Nodes.Add(tmp.Id.ToString(), tmp.Template.Name);
            }
        }

        //Работа с деревом сигналов
        #region work whith tree 

        public static void addToTree(object o, TreeView tr, int tempId, TreeNode parentTreeNode = null)
        {
            if (o.GetType().ToString() == "VisualTemplate.Template")
            {
                Template temp = o as Template;

                parentTreeNode = tr.Nodes.Add(TemplatesPages[tempId].Elements.Count.ToString(), temp.Name);

                TemplatesPages[tempId].Elements.Add(temp);

                if (temp.HasElements)
                {
                    foreach (object chObj in temp.Elements)
                    {
                        addToTree(chObj, tr, tempId, parentTreeNode);
                    }
                }
            }

            switch (o.GetType().ToString())
            {
                case "VisualTemplate.Cycle":
                    Cycle tempC = o as Cycle;
                    tempC.Id = TemplatesPages[tempId].Elements.Count.ToString();
                    parentTreeNode = parentTreeNode.Nodes.Add(tempC.Id, tempC.ToString(), 15);

                    TemplatesPages[tempId].Elements.Add(tempC);

                    RestoreLinks(tempC);

                    if (tempC.HasElements)
                    {
                        foreach (object chObj in tempC.Elements)
                        {
                            addToTree(chObj, tr,tempId, parentTreeNode);
                        }
                    }
                    break;
                case "VisualTemplate.Signal":
                    Signal tempS = o as Signal;
                    tempS.Id = TemplatesPages[tempId].Elements.Count.ToString();
                    parentTreeNode = parentTreeNode.Nodes.Add(tempS.Id, tempS.Name, tempS.ImgCode);
                    TemplatesPages[tempId].Elements.Add(tempS);

                    if (tempS.HasElements)
                    {
                        foreach (object chObj in tempS.Elements)
                        {
                            addToTree(chObj, tr, tempId, parentTreeNode);
                        }
                    }
                    break;
            }
        }

        public static void deleteSelected(TempTabPage ttp)
        {
            Element e = getElementById(ttp.TreeView.SelectedNode.Name, ttp.Id);
            ttp.TreeView.SelectedNode.Remove();
            e.Remove();
            e = null;
        }

        public static object getObjectFromTree(string key, int tempId)
        {
            int id;
            if (int.TryParse(key, out id))
            {
                return getObject(id, tempId);
            }
            else
            {
                return null;
            }
        }

        private static object getObject(int id, int tempId)
        {
            return TemplatesPages[tempId].Elements[id];
        }

        public static Element getElementById(int id, int tempId)
        {
            return TemplatesPages[tempId].Elements[id] as Element;
        }

        public static Element getElementById(string key, int ttpId)
        {
            int id;
            if (int.TryParse(key, out id))
            {
                return TemplatesPages[ttpId].Elements[id] as Element;
            }
            else
            {
                return null;
            }
        }

        public static void ReplaceInTree(TreeNode trN, string find, string rep)
        {
            //Element el = getElementById(trN.Name);
            //Service.Replace(el,find,rep);
        }


        #endregion

        //Работа со вкладками шаблонов
        #region work with Template Tab Page

        public static void addCsvVar(TempTabPage ttp)
        {
            CsvVar csv = new CsvVar(@"C:\file.csv");
            csv.Name = "Csv1";
            csv.Separator = ';';
            ttp.Template.Add(csv);
        }

        public static void getCsvVars(TempTabPage ttp)
        {
            ttp.dgProps.Rows.Clear();
            foreach (CsvVar cv in ttp.Template.CsvVars)
            {
                ttp.dgProps.Rows.Add(cv.Name, cv.Path, cv.Separator.ToString(), cv.encodingStr);
            }
        }

        //Создание новго темплейта
        public static TempTabPage CreateNewTemplate()
        {
            TempTabPage tmtp = new TempTabPage("new");
            TemplatesPages.Add(TemplatesPages.Count, tmtp);
            return tmtp;
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

        public static void saveTemplate(TempTabPage ttp)
        {
            string json = JsonConvert.SerializeObject(ttp.Template);
            var fs = new FileStream(ttp.Template.CurPath, FileMode.Create);
            var sw = new StreamWriter(fs);
            sw.Write(json);
            sw.Close();
        }
        #endregion

        //Работа с шаблоном
        #region work whith template

        //восстановление ссылок на родительские элементы после чтения из json
        public static void RestoreParentsInTemplate(Template t)
        {
            foreach (Cycle c in t.Elements)
            {
                c.tParent = t;
                RestoreParents(c);
                foreach (Variant v in c.Variatns)
                {
                    RestoreVar(v);
                }
            }
        }

        public static void loadTemplate(OpenFileDialog op, TempTabPage opTmp)
        {
            Cursor.Current = Cursors.WaitCursor;
            opTmp.TreeView.Nodes.Clear();
            var fs = new FileStream(op.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(fs);
            string jsonStr = sr.ReadToEnd();
            opTmp.Template = JsonConvert.DeserializeObject<Template>(jsonStr);
            opTmp.Template.Name = op.SafeFileName.Replace(".json","");
            opTmp.Template.CurPath = op.FileName;
            RestoreParentsInTemplate(opTmp.Template);

            addToTree(opTmp.Template, opTmp.TreeView, opTmp.Id);
            opTmp.TreeView.Nodes[0].Expand();
            sr.Close();
            defaultPathJson = Path.GetDirectoryName(op.FileName);
            Cursor.Current = Cursors.Default;
        } 


        public static bool loadTemplate(string FileName, TempTabPage opTmp)
        {
            FileInfo fi = new FileInfo(FileName);
            if (!fi.Exists) return false;
            Cursor.Current = Cursors.WaitCursor;
            opTmp.TreeView.Nodes.Clear();
            // templateElements.Clear();
            var fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(fs);
            string jsonStr = sr.ReadToEnd();
            opTmp.Template = JsonConvert.DeserializeObject<Template>(jsonStr);
            opTmp.Template.CurPath = FileName;
            RestoreParentsInTemplate(opTmp.Template);

            addToTree(opTmp.Template, opTmp.TreeView, opTmp.Id);
            //opTmp.TreeView.Nodes[0].Expand();
            // trVi.ExpandAll();
            sr.Close();
            defaultPathJson = Path.GetDirectoryName(FileName);
            Cursor.Current = Cursors.Default;
            return true;
        }

        #endregion

        //Работа с Циклами
        #region work with Сycle

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

        public static void getSettings(Cycle c, TempTabPage t)
        {
            DataGridView dgV = t.dgSettings;
            dgV.Rows.Clear();


            DataGridViewRow csvrow = new DataGridViewRow();
            DataGridViewTextBoxCell namecell = new DataGridViewTextBoxCell();
            DataGridViewComboBoxCell csv_cell = new DataGridViewComboBoxCell();
            DataGridViewButtonCell btn_cell = new DataGridViewButtonCell();
            btn_cell.Value = "updt";
            csv_cell.Items.Add("");
            foreach (CsvVar cv in t.Template.CsvVars)
            {
                csv_cell.Items.Add(cv.Name);
            }
            csv_cell.Value = c.csvVarFile;

            csvrow.Cells.Add(namecell);
            namecell.Value = "csv";
            csvrow.Cells.Add(csv_cell);
            csvrow.Cells.Add(btn_cell);
            dgV.Rows.Add("Name", c.Name);
            dgV.Rows.Add("Start", c.Start);
            dgV.Rows.Add("End", c.End);
            dgV.Rows.Add(csvrow);
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

        public static void addVariant(Cycle c)
        {
            int maxID = 0;
            foreach (Variant v in VariantsDic.Values)
            {
                int vId;
                int.TryParse(v.Id, out vId);
                if (vId > maxID) maxID = vId;

            }
            Variant vn = new Variant("name", "value", (maxID + 1).ToString());
            c.Add(vn);
            VariantsDic.Add(vn.Id, vn);
        }

        public static void setCycle(TreeNode trN, string st, string end, string scvFile)
        {
            //Cycle c = getElementById(trN.Name) as Cycle;
            //c.Start = int.Parse(st);
            //c.End = int.Parse(end);
            //c.csvVarFile = scvFile;
        }

        public static void setCycle(TempTabPage ttp)
        {
            Cycle c = getElementById(ttp.TreeView.SelectedNode.Name, ttp.Id) as Cycle;
            c.Name = ttp.dgSettings.Rows[0].Cells[1].Value is null ? "" : ttp.dgSettings.Rows[0].Cells[1].Value.ToString();
            ////dataGridSettings.Rows[0].Cells[1].Value.ToString(), dataGridSettings.Rows[1].Cells[1].Value.ToString(), dataGridSettings.Rows[2].Cells[1].Value.ToString()
            c.Start = int.Parse(ttp.dgSettings.Rows[1].Cells[1].Value.ToString());
            c.End = int.Parse(ttp.dgSettings.Rows[2].Cells[1].Value.ToString());
            ////if (dgv.Rows[2].Cells[1].Value is null)
            c.csvVarFile = ttp.dgSettings.Rows[3].Cells[1].Value is null ? "" : ttp.dgSettings.Rows[3].Cells[1].Value.ToString();//dgv.Rows[2].Cells[1].Value.ToString();
            c.Description = ttp.dgSettings.Rows[4].Cells[1].Value is null ? "" : ttp.dgSettings.Rows[4].Cells[1].Value.ToString();//dgv.Rows[2].Cells[1].Value.ToString();

        }

        public static void setVariants(Cycle c, DataGridView dgV)
        {
            int vNum = 0;
            int st = 1;
            foreach (DataGridViewRow row in dgV.Rows)
            {
                c.Variatns[vNum].Name = row.Cells[0].Value.ToString();
                c.Variatns[vNum].Value = row.Cells[1].Value.ToString();
                c.Variatns[vNum].Step = int.TryParse(row.Cells[2].Value.ToString(), out st) ? st : 1;
                vNum++;
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

        #endregion

        //Работа с сигналами
        #region work with signal

        public static void setSignal(TempTabPage ttp)
        {
            Signal s = getElementById(ttp.TreeView.SelectedNode.Name, ttp.Id) as Signal;
            s.Name = ttp.dgSettings.Rows[0].Cells[1].Value.ToString();
        }

        public static void setProperties(Signal s, DataGridView dgV, TreeNode trN = null)
        {
            int pNum = 0;
            foreach (DataGridViewRow row in dgV.Rows)
            {
                s.Properties[pNum].Id = row.Cells[0].Value.ToString();
                s.Properties[pNum].Type = row.Cells[1].Value.ToString();
                s.Properties[pNum].Value = row.Cells[2].Value == null ? "" : row.Cells[2].Value.ToString();
                if (!(trN is null) && s.Properties[pNum].Id == "1")
                {
                    trN.ImageIndex = Service.getImgCodeById(s.Properties[pNum].Value);
                    trN.SelectedImageIndex = Service.getImgCodeById(s.Properties[pNum].Value);
                }
                pNum++;
            }
        }

        public static void getProperties(Signal s, DataGridView dgV, TreeNode trN = null)
        {
            dgV.Rows.Clear();
            if (!(trN is null))
            {
                trN.ImageIndex = 0;
                trN.SelectedImageIndex = 0;
            }
            int rInd = 0;
            foreach (Property p in s.Properties)
            {
                dgV.Rows.Add(p.Id, p.Type, p.Value);
                dgV.Rows[rInd].Cells[2].Value = p.Value;
                rInd++;
                if (!(trN is null) && p.Id == "1")
                {
                    trN.ImageIndex = Service.getImgCodeById(p.Value);
                    trN.SelectedImageIndex = Service.getImgCodeById(p.Value);
                }
            }
        }

        public static void getVariantsToAdd(Signal s, TempTabPage tmp)
        {

            tmp.dgVariants.Rows.Clear();
            int r = 0;
            Cycle c = getParentCycle(s);
            bool one = false;


            do
            {
                if (c.HasVariants)
                {
                    foreach (Variant v in c.Variatns)
                    {
                        tmp.dgVariants.Rows.Add();
                        tmp.dgVariants.Rows[r].Cells[0].Value = v.Name;
                        tmp.dgVariants.Rows[r].Cells[1].Value = v.Value;
                        r++;
                    }
                }
                c = getParentCycle(c);
                if (c == null) return;

                if (c.Parent == null & one == false)
                {
                    one = true;
                }
                else
                {
                    one = false;
                }

            } while (c.Parent != null | one);


            //while (c.Parent != null | one)
            //{

            //    if (c.HasVariants)
            //    {
            //        foreach(Variant v in c.Variatns)
            //        {
            //            tmp.dgVariants.Rows.Add();
            //            tmp.dgVariants.Rows[r].Cells[0].Value = v.Name;
            //            tmp.dgVariants.Rows[r].Cells[1].Value = v.Value;
            //            r++;
            //        }
            //    }
            //    one = false;
            //    c = getParentCycle(c);
            //    if (c == null) return;
            //}



        }

        #endregion

        //Работа с переменными
        #region work with Varinats

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

        #endregion

        //Работа с Элементами(Циклы Сигналы)
        #region work with Elements


        public static Cycle getParentCycle(Signal s)
        {
            if (s.Parent.GetType().ToString() == "VisualTemplate.Cycle")
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
        #endregion

        //Вывод csv файла для альфы
        #region Out CSV

        static void readCsv(Cycle c, CsvVar csv, Template t)
        {
            try
            {
                using (StreamReader sr = new StreamReader(getCsvPath(csv, t), csv.Encoding))
                {
                    string line;
                    string headers = sr.ReadLine();
                    string[] headersArray = headers.Split(csv.Separator);
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] lineArray = line.Split(csv.Separator);

                        for (int k = 0; k < headersArray.Length; k++)
                        {
                            for (int i = 0; i < c.Variatns.Count; i++)
                            {
                                if (c.Variatns[i].Value == "%" + headersArray[k] + "%")
                                {
                                    c.Variatns[i].setTempValue(lineArray[k]);
                                }
                            }
                        }
                        goOnCycle(c, t);
                        culcValuec(c);
                        RemeberCsvStr(c.CsvString);
                        c.CsvString = "";
                        c.ResetValues();
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        public static void getCsv(Template t)
        {
            foreach (Cycle c in t.Elements)
            {
                runCycle(c, t);
            }
        }

        private static void runCycle(Cycle c, Template t)
        {
            c.CsvString = null;
            if (c.csvVarFile is null || c.csvVarFile == "")
            {
                for (c.Counter = c.Start; c.isGoOn; c.Increase())
                {
                    goOnCycle(c, t);
                    //   //c.CsvString = mathOnDollar(c.CsvString, ref k); //c.CsvString.Replace("$$", k.ToString());
                    culcValuec(c);
                }
                // Console.WriteLine(c.CsvString);
                RemeberCsvStr(c.CsvString);
                c.ResetValues();
            }
            else
            {
                readCsv(c, t.getCsvVar(c.csvVarFile), t);
                // Console.WriteLine(c.CsvString);
                //   RemeberCsvStr(c.CsvString);
                c.ResetValues();
            }
        }

        private static void goOnCycle(Cycle c, Template t, Signal sParent = null, string propStr = "")
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

                            // if (p.Type == "String" && p.Value.Substring(0, 1) != "\"") p.Value = "\"" + p.Value + "\"";

                            // Проверяем на ковычки:

                            string csvStr = "\"" + sCh.FullPath + "\"" + "," + p.Id + "," + p.Type + "," + getNormQuotes(p);
                            //    Console.WriteLine(csvStr);

                            csvStr = replaceVariants(c, csvStr);
                            csvStr = replaceParentVariant(csvStr, sCh);
                            csvStr = mathOnDollar(csvStr, c);

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
                        goOnCycle(c, t, sCh, propStr);
                    }
                    //если сигнал имеет дочерние циклы то запускаем цикл
                    if (sCh.HasCycles)
                    {
                        foreach (Cycle cCh in sCh.Cycles)
                        {
                            runCycle(cCh, t);
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
                    runCycle(cCh, t);
                }
            }
        }

        private static void RemeberCsvStr(string str)
        {
            globalCsvString += str;
        }
        #endregion

        //Вывод для signal excel
        #region work signal excel


        public static void getForExcel(Template t)
        {
            foreach (Cycle c in t.Elements)
            {
                runCycleForExcel(c, t);
            }
        }

        private static void runCycleForExcel(Cycle c, Template t)
        {
            c.CsvString = "";
            for (c.Counter = c.Start; c.isGoOn; c.Increase())
            {
                goOnCycleForExcel(c, t);
                culcValuec(c);
            }

           // goOnCycleForExcel(c, t);
           // culcValuec(c);
            RemeberCsvStr(c.CsvString);
            c.ResetValues();
        }

        private static void goOnCycleForExcel(Cycle c, Template t, Signal sParent = null, string propStr = "")
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

                //перебираем сигналы
                foreach (Signal sCh in listOfSignals)
                {
                    replaceVariantsInNames(c, sCh);
                    if (sCh.Name.IndexOf("$") > 0) sCh.setTempName(mathOnDollar(sCh.Name, c));
                    //если у сигнала есть свойства тогда ...
                    bool hasType = false;
                    if (sCh.HasProperties)
                    {
                        //... перебираем и добавляем их в ...
                        foreach (Property p in sCh.Properties)
                        {
                            // Проверяем на ковычки:
                            string csvStr;



                            if (p.Id == "1")
                            {
                                csvStr = ";" + "\"" + sCh.FullPath + "\"" + ";Type;;" + CDTTypes[p.Value];//+ p.Id + "," + p.Type + "," + getNormQuotes(p);
                                hasType = true;
                            }
                            else if(p.Id == "2")
                            {
                                csvStr = ";" + "\"" + sCh.FullPath + "\"" + ";Value;;" + getNormQuotes(p);//+ p.Id + "," + p.Type + "," + getNormQuotes(p);
                                hasType = true;
                            }
                            else if(p.Id == "3")
                            {
                                csvStr = ";" + "\"" + sCh.FullPath + "\"" + ";Quality;;" + getNormQuotes(p);//+ p.Id + "," + p.Type + "," + getNormQuotes(p);
                                hasType = true;
                            }
                            else if(p.Id == "999004")
                            {
                                string[] str = p.Value.Split("\n".ToCharArray());
                                csvStr = "";
                                for (int k=0; k <= str.Length - 1; k++)
                                {
                                    if(str[k] != "")
                                    {
                                        if (k != 0) csvStr += "\n";
                                         csvStr += ";" + "\"" + sCh.FullPath + "\"" + ";" + p.Id + ";" + p.Type + ";" + str[k] ;
                                        //if (k != str.Length - 1) csvStr += "\n";
                                    }
                                }
                            }
                            else
                            {
                                 csvStr = ";" +  "\"" + sCh.FullPath + "\"" + ";" + p.Id + ";" + p.Type + ";" + getNormQuotes(p);
                            }

                            csvStr = replaceVariants(c, csvStr);
                            csvStr = replaceParentVariant(csvStr, sCh);
                            csvStr = mathOnDollar(csvStr, c);
                            c.CsvString += csvStr + "\n";
                        }
                        if (!hasType)
                        {
                            //c.CsvString += ";" + "\"" + sCh.FullPath + "\"" + ";Type;;Folder" + "\n";
                            string fPath = sCh.FullPath;
                            fPath = replaceVariants(c, fPath);
                            fPath = mathOnDollar(fPath, c);

                            c.CsvString += ";" + "\"" + fPath + "\"" + ";Type;;Folder" + "\n";
                            //c.CsvString += ";" + "\"" + mathOnDollar(sCh.FullPath,c) + "\"" + ";Type;;Folder" + "\n";
                        }
                    }
                    else
                    {
                        string fPath = sCh.FullPath;
                        fPath = replaceVariants(c, fPath);
                        fPath = mathOnDollar(fPath, c);
                        //c.CsvString += ";" + "\"" + sCh.FullPath + "\"" + ";Type;;Folder" + "\n";
                        c.CsvString += ";" + "\"" + mathOnDollar(sCh.FullPath, c) + "\"" + ";Type;;Folder" + "\n";
                        c.CsvString += ";" + "\"" + fPath + "\"" + ";Type;;Folder" + "\n";
                        //c.CsvString += ";" + "\"" + mathOnDollar(sCh.FullPath, c) + "\"" + ";Type;;Folder" + "\n";
                    }



                    //если сигнал имеет дочерние сигналы тогда вызываем себя ещё раз
                    if (sCh.HasSignals)
                    {
                        goOnCycleForExcel(c, t, sCh, propStr);
                    }
                    //если сигнал имеет дочерние циклы то запускаем цикл
                    if (sCh.HasCycles)
                    {
                        foreach (Cycle cCh in sCh.Cycles)
                        {
                            runCycleForExcel(cCh, t);
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
                    runCycleForExcel(cCh, t);
                }
            }
        }


        #endregion

        //Работа с набором вводных данных
        #region work with input data

        public static string getCsvPath(CsvVar csv, Template t)
        {
            if (csv is null) return null;
            if (csv.Path.IndexOf(@":\") > 0)
            {
                return csv.Path;
            }
            else
            {
                return Path.GetDirectoryName(t.CurPath) + @"\" + csv.Path;
            }
        }

        public static void setCsvVarPath(TempTabPage ttp)
        {
            int cvNum = 0;
            foreach (DataGridViewRow row in ttp.dgProps.Rows)
            {
                ttp.Template.CsvVars[cvNum].Name = row.Cells[0].Value.ToString();
                ttp.Template.CsvVars[cvNum].Path = row.Cells[1].Value.ToString();
                ttp.Template.CsvVars[cvNum].Separator = row.Cells[2].Value.ToString().ToCharArray()[0];
                switch (row.Cells[3].Value.ToString())
                {
                    case "Default":
                        ttp.Template.CsvVars[cvNum].Encoding = Encoding.Default;
                        break;
                    case "ASCII":
                        ttp.Template.CsvVars[cvNum].Encoding = Encoding.ASCII;
                        break;
                    case "UTF8":
                        ttp.Template.CsvVars[cvNum].Encoding = Encoding.UTF8;
                        break;
                }
                cvNum++;
            }
        }

        #endregion

        //Работа с подстановкой
        #region work with replace
        private static void replaceVariantsInNames(Cycle c, Signal s)
        {
            foreach (Variant v in c.Variatns)
            {
                if (s.Name == v.NameS)
                {

                    s.setTempName(v.Value);
                }
            }

            foreach (Variant v in c.Variatns)
            {
                if (s.Name.IndexOf(v.NameS) >= 0)
                {

                    s.setTempName(replnmathVariant(s.Name, v)); //s.Name.Replace(v.NameS,v.Value));
                }
            }

        }

        private static string replaceVariants(Cycle c, string csvStr)
        {
            foreach (Variant v in c.Variatns)
            {
                csvStr = replnmathVariant(csvStr, v);// csvStr.Replace(v.NameS, v.Value);
            }

            return csvStr;
        }

        private static void ResetSignalsNames(List<Signal> listOfSignals)
        {
            foreach (Signal s in listOfSignals)
            {
                s.ResetName();
            }
        }

        static string mathOnDollar(string fullStr, Cycle c, string rStr = "$")
        {
            int indOfdollar;
            if (fullStr is null) return null;
            if (rStr != "$")
            {
                indOfdollar = fullStr.IndexOf(rStr);
            }
            else
            {
                if ((indOfdollar = fullStr.IndexOf("$$")) < 0)
                {
                    if ((indOfdollar = fullStr.IndexOf("$+")) < 0)
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
            if (fullStr.Contains("$+") | fullStr.Contains("$-") | (rStr == "$" ? fullStr.Contains("$$") : fullStr.Contains(rStr)))
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
            if (fullStrCharArray.Length > IndexOfVarNameEnd) operation = fullStrCharArray[IndexOfVarNameEnd].ToString();

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
            if (int.TryParse(v.Value, out modPerem))
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
                if (v.Value.IndexOf("\"\"") > 0)
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
                csvStr = replnmathVariant(csvStr, v);
            }
            return csvStr;
        }

        private static void replaceInVarinatValue(Cycle c, Cycle pC)
        {
            foreach (Variant v in c.Variatns)
            {
                if (v.Value.IndexOf("$") >= 0)
                {
                    foreach (Variant vp in pC.Variatns)
                    {
                        if (v.Value == vp.NameS)
                        {
                            v.setTempValue(vp.Value);
                        }
                    }
                }
            }
        }

        private static string getNormQuotes(Property p)
        {
            if (p.Value == "") return p.Value;
            string outStr = p.Value;
            if (p.Type == "String" && outStr.Substring(0, 1) == "\"")
            {
                outStr = outStr.Remove(0, 1);
                if (outStr.IndexOf("\"", outStr.Length - 1) > 0)
                {
                    outStr = outStr.Remove(outStr.Length - 1);
                }
            }

            if (p.Type == "String" && outStr != "" && outStr.IndexOf("\"", 1) > 1 && outStr.IndexOf("\"", 1) != outStr.Length - 1 && outStr.IndexOf("\"\"") < 0)
            {
                outStr = outStr.Replace("\"", "\"\"");
            }

            if (p.Type == "String" && outStr.Substring(0, 1) != "\"") { outStr = "\"" + outStr + "\""; }

            return outStr;
        }

        private static string replaceParentVariant(string CsvStr, Signal sCh)
        {
            string outStr = CsvStr;
            Cycle pCycle = getParentCycle(getParentCycle(sCh));
            int deep = 1;

            while (!(pCycle is null))
            {
                int p;
                int indexOf = outStr.IndexOf("$@");
                outStr = replaceVariants(pCycle, outStr);
                while (indexOf > 0 & int.TryParse(outStr[indexOf + 2].ToString(), out p))
                {
                    int od = 0;
                    int d = 0;
                    string rmbStr = "$@";
                    for (int h = indexOf; h < outStr.Length; h++)
                    {
                        if (int.TryParse(outStr[h + 2].ToString(), out od))
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
                        indexOf = outStr.IndexOf("$@");
                        //outStr = outStr.Replace(rmbStr, pCycle.Counter.ToString());
                    }

                }
                pCycle = getParentCycle(pCycle);
                deep++;
            }
            return outStr;
        }

        #endregion



        public static void addCycle(TempTabPage ttp)
        {
            Cycle c = new Cycle(1, 1);

            if (ttp.TreeView.SelectedNode.Text == ttp.Template.Name)
            {
                ttp.Template.Add(c);
            }
            else
            {
                Element curElement = getElementById(ttp.TreeView.SelectedNode.Name, ttp.Id);
                curElement.Add(c);
            }

            c.Id = ttp.Elements.Count.ToString();
            ttp.TreeView.SelectedNode.Nodes.Add(c.Id, c.ToString(), 15);
            ttp.Elements.Add(c);
            ttp.TreeView.SelectedNode = ttp.TreeView.SelectedNode.LastNode;
        }

        public static void addSignal(TempTabPage ttp)
        {

            Signal s = new Signal("Signal");
            if (ttp.TreeView.SelectedNode is null) return;
            Element curElement = getElementById(ttp.TreeView.SelectedNode.Name, ttp.Id);
            if (curElement is null) return;
            curElement.Add(s);


            s.Id = ttp.Elements.Count.ToString();
           ttp.TreeView.SelectedNode.Nodes.Add(s.Id, s.ToString(), 0);
            ttp.Elements.Add(s);

            s.Add(new Property("1", "UInt4", "8"));
            ttp.TreeView.SelectedNode = ttp.TreeView.SelectedNode.LastNode;
        }

        private static void removeVariant(Cycle c)
        {
            foreach (Variant v in c.Variatns)
            {
                if (v.Value.IndexOf("%") >= 0)
                {
                    c.Variatns.Remove(v);
                    removeVariant(c);
                    return;
                }
            }
        }

        public static void updateVarsFromCsv(TempTabPage curTempTabPage, Cycle c)
        {
            removeVariant(c);
            string[] headersArray = null;
            try
            {
                CsvVar cv = curTempTabPage.Template.getCsvVar(c.csvVarFile);
                using (StreamReader sr = new StreamReader(getCsvPath(cv, curTempTabPage.Template), cv.Encoding))
                {
                    string headers = sr.ReadLine();
                    headersArray = headers.Split(cv.Separator);
                }
            }
            catch (NullReferenceException)
            {
                return;
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            for (int k = 0;  k < headersArray.Length; k++)
            {
                c.Add(new Variant(headersArray[k], "%" + headersArray[k] + "%", ""));
            }
            

        }

        public static void addFolder(TempTabPage ttp)
        {

            Signal s = new Signal("Folder");
            if (ttp.TreeView.SelectedNode is null) return;
            Element curElement = getElementById(ttp.TreeView.SelectedNode.Name, ttp.Id);
            if (curElement is null) return;
            curElement.Add(s);


            s.Id = ttp.Elements.Count.ToString();
            ttp.TreeView.SelectedNode.Nodes.Add(s.Id, s.ToString(), 0);
            ttp.Elements.Add(s);
            ttp.TreeView.SelectedNode = ttp.TreeView.SelectedNode.LastNode;
        }

        internal static void saveMaster(string fileName)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(Master));
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, MasterFile);
            }
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
            TypeOfProperty.Add("6500", "String");
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

            CDTTypes.Add("1", "Int1");
            CDTTypes.Add("3", "UInt1");
            CDTTypes.Add("9", "Int2");
            CDTTypes.Add("8", "UInt2");
            CDTTypes.Add("7", "Int4");
            CDTTypes.Add("6", "UInt4");
            CDTTypes.Add("13", "Int8");
            CDTTypes.Add("12", "UInt8");
            CDTTypes.Add("14", "Float");
            CDTTypes.Add("15", "Double");
            CDTTypes.Add("5", "Bool");
            CDTTypes.Add("17", "String");
        }


    }
}
