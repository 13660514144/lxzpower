using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;
using DBUtil;
using Utils;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NC.Common;
using DbModel;
using Model生成器.Utils;
using System.IO;
using Reptile;
using AllModel;
namespace Model
{
    public partial class Form1 : Form
    {
        public static List<Dictionary<string, string>> tableList = null;
        IDal dal;
        public static Form1 FrmMe;
        public static string DataCurrent=string.Empty;
        public static bool AutoChg = false;
        public static DataConnection CONN = new DataConnection();
        public static List<Dictionary<string, string>> columnList;
        public static string FieldsIdent = string.Empty;
        public static string Conn = ConfigurationManager.ConnectionStrings["MSSQLConnection"].ToString();
        public static string UserToken = string.Empty;
        public static HttpModel.UserInfo UserModel = new HttpModel.UserInfo();
        #region Form1
        public Form1()
        {
            InitializeComponent();
            FrmMe = this;
        }
        #endregion

        #region Form1_Load
        private void Form1_Load(object sender, EventArgs e)
        {

            SetGridHead();
            SetParentHead();
            dal = DalFactory.CreateDal(ConfigurationManager.AppSettings["DBType"],Conn);
            tableList = dal.GetAllTables();
            BuildView(tableList);
            SetCombox(dal);


            CONN.Host = "Data Source=8.129.46.155,1488;";
            CONN.Data = "Initial Catalog=";
            CONN.Account = "User ID=sa;";
            CONN.Pwd = "Password=0BI8DKAFO7V0@;";
            CONN.ModelType = "Integrated Security=false;";

            
        }
        private void SetGridHead()
        {
            DataGridViewTextBoxColumn dc = new DataGridViewTextBoxColumn();
            dc.HeaderText = "Tb";
            dc.DataPropertyName = "Tb";
            dataGridView1.Columns.Add(dc);
            dc = new DataGridViewTextBoxColumn();
            dc.HeaderText = "Remarks";
            dc.DataPropertyName = "Remarks";
            dataGridView1.Columns.Add(dc);

            DataGridViewTextBoxColumn dc1 = new DataGridViewTextBoxColumn();
            dc1.HeaderText = "Field";
            dc1.DataPropertyName = "Field";
            dataGridView2.Columns.Add(dc1);
            dc1 = new DataGridViewTextBoxColumn();
            dc1.HeaderText = "FieldCn";
            dc1.DataPropertyName = "FieldCn";
            dataGridView2.Columns.Add(dc1);
            /*
            dc1 = new DataGridViewTextBoxColumn();
            dc1.HeaderText = "EditOrBrow";
            dc1.DataPropertyName = "EditOrBrow";
            dataGridView2.Columns.Add(dc1);
            */
            DataGridViewComboBoxColumn dc2 = new DataGridViewComboBoxColumn();
            dc2.HeaderText = "EditOrBrow";
            dc2.DataPropertyName = "EditOrBrow";
            dc2.Name = "EditOrBrow";
            dataGridView2.Columns.Add(dc2);
        }
        private void SetParentHead()
        {
            DataGridViewTextBoxColumn dc1 = new DataGridViewTextBoxColumn();
            dc1.HeaderText = "Field";
            dc1.Name = "Field";
            dc1.DataPropertyName = "Field";
            dataGridView3.Columns.Add(dc1);
            dc1 = new DataGridViewTextBoxColumn();
            dc1.HeaderText = "FieldCn";
            dc1.Name = "FieldCn";
            dc1.DataPropertyName = "FieldCn";
            dataGridView3.Columns.Add(dc1);

            /*编码转换输出 通常IfConst=1时，必需加上转换输出且显示*/
            DataGridViewComboBoxColumn dc2 = new DataGridViewComboBoxColumn();
            dc2.HeaderText = "ChangeCol";
            dc2.Name = "ChangeCol";
            dataGridView3.Columns.Add(dc2);
            /*验证方法*/
            dc2 = new DataGridViewComboBoxColumn();
            dc2.HeaderText = "IdentCmb";
            dc2.Name = "IdentCmb";
            dataGridView3.Columns.Add(dc2);
            /*是否输出前端*/
            dc2 = new DataGridViewComboBoxColumn();
            dc2.HeaderText = "IfOutList";
            dc2.Name = "IfOutList";
            dataGridView3.Columns.Add(dc2);
            /*在前端输出中是否隐藏*/
            dc2 = new DataGridViewComboBoxColumn();
            dc2.HeaderText = "IfOutHidden";
            dc2.Name = "IfOutHidden";
            dataGridView3.Columns.Add(dc2);
            /*是否做做库列表查询字段*/
            dc2 = new DataGridViewComboBoxColumn();
            dc2.HeaderText = "IfSearch";
            dc2.Name = "IfSearch";
            dataGridView3.Columns.Add(dc2);
            /*是否是系统常量值 通常系统常量值在输出前端必需带转换输出*/
            dc2 = new DataGridViewComboBoxColumn();
            dc2.HeaderText = "IfConst";
            dc2.Name = "IfConst";
            dataGridView3.Columns.Add(dc2);
            /*常量在数据库中的自定义转换函数*/
            dc1 = new DataGridViewTextBoxColumn();
            dc1.HeaderText = "ConstValue";
            dc1.Name = "ConstValue";
            dataGridView3.Columns.Add(dc1);
            /*凡做Combo项的表单及LIST 数据源 可以是数据库自定义函数*/
            dc1 = new DataGridViewTextBoxColumn();
            dc1.HeaderText = "ComboRestlt";
            dc1.Name = "ComboRestlt";
            dataGridView3.Columns.Add(dc1);
            /**/
            dc1 = new DataGridViewTextBoxColumn();
            dc1.HeaderText = "DataType";
            dc1.Name = "DataType";
            dataGridView3.Columns.Add(dc1);

            dc1 = new DataGridViewTextBoxColumn();
            dc1.HeaderText = "MaxLeng";
            dc1.Name = "MaxLeng";
            dataGridView3.Columns.Add(dc1);
        }
        private void SetCombox(IDal dal)
        {
            DataTable BaseData = dal.GetDataBase();
            CbList.SelectedIndexChanged -= CbList_SelectedIndexChanged;
            CbList.DataSource = BaseData;
            this.CbList.DisplayMember = "name";
            this.CbList.ValueMember = "name";
            CbList.SelectedIndexChanged += CbList_SelectedIndexChanged;
        }
        #endregion
        private void SetParentTb(List<Dictionary<string, string>> columnList)
        {
         
            foreach (Dictionary<string, string> column in columnList) //遍历字段
            {
                this.Invoke((EventHandler)(delegate
                {
                    int index = dataGridView3.Rows.Add();
                    dataGridView3.Rows[index].Cells[0].Value = column["columns_name"];
                    dataGridView3.Rows[index].Cells[1].Value = column["comments"].ToString().Split(' ')[0];
                    
                    DataGridViewComboBoxCell comboBoxCell = (DataGridViewComboBoxCell)dataGridView3.Rows[index].Cells[2];
                    comboBoxCell.Items.Clear();
                    comboBoxCell.Items.Add("0");
                    comboBoxCell.Items.Add("1");
                    
                    comboBoxCell = (DataGridViewComboBoxCell)dataGridView3.Rows[index].Cells[3];
                    comboBoxCell.Items.Clear();
                    comboBoxCell.Items.Add("_isNumber");
                    comboBoxCell.Items.Add("_isNull");
                    comboBoxCell.Items.Add("_isInteger");
                    comboBoxCell.Items.Add("_isDecimal");
                    comboBoxCell.Items.Add("_isStrLong");
                    comboBoxCell.Items.Add("_IsValidDate"); 

                    comboBoxCell = (DataGridViewComboBoxCell)dataGridView3.Rows[index].Cells[4];
                    comboBoxCell.Items.Clear();
                    comboBoxCell.Items.Add("0");
                    comboBoxCell.Items.Add("1");

                    comboBoxCell = (DataGridViewComboBoxCell)dataGridView3.Rows[index].Cells[5];
                    comboBoxCell.Items.Clear();
                    comboBoxCell.Items.Add("0");
                    comboBoxCell.Items.Add("1");

                    comboBoxCell = (DataGridViewComboBoxCell)dataGridView3.Rows[index].Cells[6];
                    comboBoxCell.Items.Clear();
                    comboBoxCell.Items.Add("0");
                    comboBoxCell.Items.Add("1");

                    comboBoxCell = (DataGridViewComboBoxCell)dataGridView3.Rows[index].Cells[7];
                    comboBoxCell.Items.Clear();
                    comboBoxCell.Items.Add("0");
                    comboBoxCell.Items.Add("1");

                    dataGridView3.Rows[index].Cells[10].Value = column["data_type"]; 
                    dataGridView3.Rows[index].Cells[11].Value = column["maxleng"];
                }
                ));
            }
        }
        private void BuildView(List<Dictionary<string, string>> tblist)
        {
            dataGridView1.Rows.Clear();
            foreach (var item in tblist)
            {

                int index = dataGridView1.Rows.Add();
                var ss = item.ToArray();
                string[] n = ss[0].ToString().Split(',');
                string[] v = ss[1].ToString().Split(',');
                this.dataGridView1.Rows[index].Cells[0].Value = n[1].Substring(0, n[1].Length - 1);
                this.dataGridView1.Rows[index].Cells[1].Value = v[1].Substring(0, v[1].Length - 1);
            }
        }
        //生成
        private void btnCreate_Click(object sender, EventArgs e)
        {
            StringBuilder Sb = new StringBuilder();
            StringBuilder Edit = new StringBuilder();
            StringBuilder Brow = new StringBuilder();
            string tb = string.Empty;
            string table_comments = string.Empty;
            for (int x=0;x<dataGridView1.RowCount;x++)
            {
                if (dataGridView1.Rows[x].Selected && dataGridView1.Rows[x].Cells[0].Value!=null)
                {
                    tb = dataGridView1.Rows[x].Cells[0].Value.ToString().Trim();
                    table_comments = dataGridView1.Rows[x].Cells[1].Value.ToString().Trim();
                    MessageBox.Show(tb);
                    break;
                }
            }
            if (!string.IsNullOrEmpty(tb))
            {

                new Thread(new ThreadStart(delegate ()
                {
                    try
                    {

                        string strNamespace = ConfigurationManager.AppSettings["Namespace"];
                        string strClassTemplate = string.Empty;
                        string strClassExtTemplate = string.Empty;
                        string strFieldTemplate = string.Empty;
                        Regex regField = new Regex(@"[ \t]*#field start([\s\S]*)#field end", RegexOptions.IgnoreCase);

                        #region 操作控件
                        InvokeDelegate invokeDelegate = delegate ()
                        {
                            btnCreate.Enabled = false;
                            progressBar1.Visible = true;
                            progressBar1.Minimum = 0;
                            progressBar1.Maximum = tableList.Count;
                            progressBar1.Value = 0;
                        };
                        InvokeUtil.Invoke(this, invokeDelegate);
                        #endregion

                        #region 读取模板
                        strClassTemplate = FileHelper.ReadFile(Application.StartupPath + "\\Template\\class.txt");
                        strClassExtTemplate = FileHelper.ReadFile(Application.StartupPath + "\\Template\\class_ext.txt");
                        Match matchField = regField.Match(strClassTemplate);
                        if (matchField.Success)
                        {
                            strFieldTemplate = matchField.Groups[1].Value.TrimEnd(' ');
                        }
                        #endregion

                        int i = 0;
                        //foreach (Dictionary<string, string> table in tableList) //遍历表
                        //{
                        //string tableName = table["table_name"].ToUpper();
                        string tableName = tb.Trim();
                        StringBuilder sbFields = new StringBuilder();
                        columnList = null;
                        columnList = dal.GetAllColumns(tableName);
                        
                        
                        #region 原始Model
                        string strClass = strClassTemplate.Replace("#table_comments", table_comments);
                        strClass = strClass.Replace("#table_name", tableName);


                        Edit.Append(SetHtmlHead(table_comments));//表单头
                        Brow.Append(SetHtmlHead(table_comments));//表单头

                        Sb.Append("{\r\n");
                        Sb.Append($"    \"DB\":\"{DataCurrent}\",\r\n");
                        Sb.Append($"    \"TB\":\"{tb}\",\r\n");
                        Sb.Append($"    \"TBcn\":\"{table_comments}\",\r\n");
                        Sb.Append($"    \"Fields\":[\r\n");

                        int conter = 1;
                        foreach (Dictionary<string, string> column in columnList) //遍历字段
                        {

                            string data_type = dal.ConvertDataType(column);

                            string strField = strFieldTemplate.Replace("#field_comments", column["comments"]);

                            if (column["constraint_type"] != "P")
                            {
                                strField = strField.Replace("        [IsId]\r\n", string.Empty);
                            }

                            strField = strField.Replace("#data_type", data_type);
                            strField = strField.Replace("#field_name", column["columns_name"]);

                            sbFields.Append(strField);
                            /*开始JSON*/
                            if (conter == 0)
                            {
                                Sb.Append("       {\"ColName\":\"" + column["columns_name"] + "\",\r\n");
                            }
                            else
                            {
                                Sb.Append("       ,{\"ColName\":\"" + column["columns_name"] + "\",\r\n");
                            }
                            Sb.Append($"       \"ColCn\":\"{column["comments"].ToString().Split(' ')[0]}\",\r\n");
                            Sb.Append($"       \"ColType\":\"{data_type}\",\r\n");
                            Sb.Append($"       \"IfNull\":\"{column["notnull"]}\",\r\n");                            
                            Sb.Append("       \"Maxleng\":\""+ column["maxleng"] + "\"}\r\n");

                            Edit.Append(SetHtmlEditBody(column["columns_name"], column["comments"],data_type));//表单体
                            Brow.Append(SetHtmlBrowBody(column["columns_name"], column["comments"], data_type));//表单体
                            if (conter % 3 == 0)
                            {
                                Edit.Append("       <div style=\"clear: both\"></div>\r\n");
                            }
                            conter++;
                        }
                        //if (dataGridView2.RowCount > 0)
                        //{
                        //    Edit.Append("   <table id=\"SubList\"></table>\r\n");
                        //    Brow.Append("   <table id=\"SubList\"></table>\r\n");
                        //}
                        Edit.Append(SetHtmlBooter(DataCurrent,tb,1));//表单尾
                        Brow.Append(SetHtmlBooter(DataCurrent, tb,0));//表单尾

                        /**/
                        FileHelper.WriteForm(Application.StartupPath + "\\Models", Edit.ToString(),true);
                        FileHelper.WriteForm(Application.StartupPath + "\\Models", Brow.ToString(), false);
                        /**/
                        Sb.Append($"       ],\r\n");
                        Sb.Append($"   \"SubTb\":\"{this.SubTxt.Text.ToString().Trim()}\",\r\n");
                        Sb.Append($"   \"LinkField\":\"{this.LinkFields.Text.ToString().Trim()}\",\r\n");
                        Sb.Append($"   \"ParentField\":\"{this.ParentFields.Text.ToString().Trim()}\"\r\n");
                        Sb.Append("}");

                        FileHelper.WriteFile(Application.StartupPath + "\\Models", Sb.ToString(), DataCurrent, tb);

                        strClass = regField.Replace(strClass, sbFields.ToString());

                        FileHelper.WriteFile(Application.StartupPath + "\\Models", strClass, tableName);
                        #endregion

                        #region 扩展Model
                        string strClassExt = strClassExtTemplate.Replace("#table_comments", table_comments);
                        strClassExt = strClassExt.Replace("#table_name", tableName);

                        FileHelper.WriteFile(Application.StartupPath + "\\ExtModels", strClassExt.ToString(), tableName);
                        #endregion

                        #region 操作控件
                        invokeDelegate = delegate ()
                        {
                            progressBar1.Value = ++i;
                        };
                        InvokeUtil.Invoke(this, invokeDelegate);
                        #endregion
                        //}

                        #region 操作控件
                        invokeDelegate = delegate ()
                        {
                            btnCreate.Enabled = true;
                            progressBar1.Visible = false;
                            progressBar1.Value = 0;
                        };
                        InvokeUtil.Invoke(this, invokeDelegate);
                        #endregion

                        MessageBox.Show("完成");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
                    }
                })).Start();
            }
            else
            {
                MessageBox.Show("Pleate Check Row");
            }
        }
        /// <summary>
        /// 表单头
        /// </summary>
        /// <param name="tbname"></param>
        /// <returns></returns>
        private string SetHtmlHead(string tbname)
        {
            string Rstr = string.Empty;
            Rstr += $"<!DOCTYPE html>\r\n";
            Rstr += $"<html>\r\n";
            Rstr += $"<head>\r\n";

            Rstr += "<meta charset=\"UTF-8\">\r\n";
            Rstr += "<meta http-equiv=\"Expires\" content=\"0\">\r\n";
            Rstr += "<meta http-equiv=\"Pragma\" content=\"no-cache\">\r\n";
            Rstr += "<meta http-equiv=\"Cache-control\" content=\"no-cache\">\r\n";
            Rstr += "<meta http-equiv=\"Cache\" content=\"no-cache\">\r\n";

            Rstr += $"  <title>{tbname}</title>\r\n";
            Rstr += $"  <link rel=\"stylesheet\" id=\"templatecss\" type=\"text/css\" href=\"/css/Form.css\" >\r\n";
            Rstr += $"  <link rel=\"stylesheet\" id=\"Tb\" type=\"text/css\" href=\"/css/Formtable.css\" >\r\n";
            Rstr += $"  <link rel=\"stylesheet\"  type=\"text/css\" href=\"/css/daterangepicker.css\" >\r\n";
            Rstr += $"  <link rel=\"stylesheet\" id=\"btn5\" type=\"text/css\" href=\"/BtnCSS/set_5.css\" >\r\n";
            Rstr += "   <style  type=\"text/css\">\r\n";
            Rstr += "   </style>\r\n";

            Rstr += $"  <script>\r\n";
            Rstr += $"      var _PageStart = 0;\r\n";
            Rstr += $"      document.write('<script src=\"/js/clientpack.js?v='+Math.round(Math.random()*10000)+'\"><\\/script>');\r\n";
            Rstr += $"      document.write('<script src=\"/js/ReqServer.js?v='+Math.round(Math.random()*10000)+'\"><\\/script>');\r\n";
            Rstr += $"      document.write('<script src=\"/js/client.js?v=' + Math.round(Math.random()*10000) + '\"><\\/script>');\r\n";
            Rstr += $"      document.write('<script src=\"/js/PatePaging.js?v='+Math.round(Math.random()*10000)+'\"><\\/script>');\r\n";
            Rstr += $"      document.write('<script src=\"/js/SearchBuild.js?v='+Math.round(Math.random()*10000)+'\"><\\/script>');\r\n";
            Rstr += $"      document.write('<script src=\"/js/BtnImint.js?v=' + Math.round(Math.random() * 10000) + '\"><\\/script>');\r\n";
            Rstr += $"      document.write('<script src=\"/js/Tbcreat.js?v=' + Math.round(Math.random() * 10000) + '\"><\\/script>');\r\n";
            Rstr += $"  </script>\r\n";

            Rstr += $"  <script type=\"text/javascript\" src=\"/Jqjvs/jquery.min.js\"></script>\r\n";
            Rstr += $"  <script type=\"text/javascript\" src=\"/Jqjvs/moment.min.js\"></script>\r\n";
            Rstr += $"  <script type=\"text/javascript\" src=\"/Jqjvs/daterangepicker.min.js\"></script>\r\n";

            Rstr += "</head>\r\n";
            Rstr += "<body>\r\n";
            Rstr += "<div id=\"comment\">\r\n";
            Rstr += "<div id=\"poptitle\">\r\n";
            Rstr += "    <div style=\"width:26px;height:26px;line-height:26px;font-size:26px;border:1px solid #3c3939;float:right;text-align:center;cursor:pointer;\" onclick=\"_TitleClsWin()\">X</div>\r\n";
            Rstr += "</div>\r\n";
            Rstr += $"  <form  id=\"form1\" name=\"form1\" method=\"post\" class=\"bootstrap-frm\">\r\n";
            Rstr += $"       <h1>{tbname}\r\n";
            Rstr += $"          <span></span>\r\n";
            Rstr += $"       </h1>\r\n";
            return Rstr;
        }
       
        /// <summary>
        /// 表单尾
        /// </summary>
        /// <returns></returns>
        private string SetHtmlBooter(string db,string tb,int Bflg)
        {
            string Rstr = string.Empty;
            Rstr += $"  </form>\r\n";
            Rstr += $"</div>\r\n";
            Rstr += $"<div id=\"footer\">\r\n";
            Rstr += $"  <button  id=\"SaveCash\">本地存储</button>\r\n";
            Rstr += $"  <button  id=\"DelCash\">删除本地存储</button>\r\n";
            Rstr += $"  <button  id=\"ToServer\">提交</button>\r\n";
            Rstr += $"  <button  id=\"ToPageExcel\">导出Excel</button>\r\n";
            Rstr += $"  <button  id=\"ToPageWord\">导出Word</button>\r\n";

            Rstr += $"</div>\r\n";
            Rstr += $"<table id=\"SubList\"></table>\r\n";
            Rstr += $"<script type=\"text/javascript\">\r\n";
            Rstr += $"  var Win=parent;\r\n";
            Rstr += $"  var LocalItem='{db}{tb}json';\r\n";
            Rstr += "   var FieldsIdent=[\r\n";
            Rstr += $"       {FieldsIdent}\r\n";
            Rstr += "   ];\r\n";
/*            
            if (Bflg == 1)
            {
                Rstr += "        function ToServer_Click() {\r\n";
                Rstr += "            try {\r\n";
                Rstr += "                var BtnId = Win._CurrentBtn.id;\r\n";
                Rstr += "                _SetPageReqParas(1, BtnId);\r\n";
                Rstr += "                PagingResult = GetPagePara();\r\n";
                Rstr += "                PagingResult.PagingMode = 'None';\r\n";
                Rstr += "                switch (_EditOrBrow) {\r\n";
                Rstr += "                    case '0':\r\n";
                Rstr += "                        PostStr = _Form_Serialize._EditSerialize(document.getElementsByName('form1')[0]);\r\n";
                Rstr += "                        break;\r\n";
                Rstr += "                    case '1':\r\n";
                Rstr += "                        PostStr = _Form_Serialize.JsonSerialize(document.getElementsByName('form1')[0]);\r\n";
                Rstr += "                        break;\r\n";
                Rstr += "                }\r\n";
                Rstr += "                var flg = _Str_Angement._GuestIdent();\r\n";
                Rstr += "                if (flg == false) {\r\n";
                Rstr += "                    return;\r\n";
                Rstr += "                }\r\n";
                            
                
                Rstr += "                var Jflg;\r\n";
                Rstr += "                switch (_EditOrBrow) {\r\n";
                Rstr += "                    case '0':\r\n";
                Rstr += "                        Jflg = _Str_Angement._IsJsonNull(PostStr.OldValue);\r\n";
                Rstr += "                        break;\r\n";
                Rstr += "                    case '1':\r\n";
                Rstr += "                        Jflg = _Str_Angement._IsJsonNull(PostStr);\r\n";
                Rstr += "                        break;\r\n";
                Rstr += "                }\r\n";
                Rstr += "                if (!Jflg) {\r\n";
                Rstr += "                    _CashRequestData = PostStr;//缓存本地表单数据，做为编辑提交后刷新列表使用;\r\n";
                Rstr += "                    var sub = _Tb_Serialize.PackSubList('SubList', Win._SubListConFig);\r\n";
                Rstr += "                    var CallBackMothed = 'CallBackToServer';\r\n";
                Rstr += "                    var url = '/ident.ashx';\r\n";
                Rstr += "                    var s = {\r\n";
                Rstr += "                        ParentData: PostStr,\r\n";
                Rstr += "                        PagePara: PagingResult\r\n";
                Rstr += "                    };\r\n";
                Rstr += "                    var data = _PackClient(s, sub, [], []);\r\n";
                Rstr += "                    console.log(JSON.stringify(data));\r\n";
                Rstr += "                    _AsyncRequest._AsyncPost(data, CallBackMothed, url);\r\n";
                Rstr += "                }\r\n";
                Rstr += "                else {\r\n";
                Rstr += "                    _ErrMsg('没有可以提交的数据');\r\n";
                Rstr += "                }\r\n";
                Rstr += "            }\r\n";
                Rstr += "            catch (err) {\r\n";
                Rstr += "                _ErrMsg(err.message);\r\n";
                Rstr += "            }\r\n";
                Rstr += "        }\r\n";


                Rstr += "  function CallBackToServer(data){\r\n";
                Rstr += "      _ErrMsg(data.Msg);\r\n";
                Rstr += "      if(data.scuess){\r\n";
                Rstr += "          _ClientPack.DelLocalSave(LocalItem);\r\n";
                Rstr += "          _FrmRfresh(_EditOrBrow,_CashRequestData);\r\n";
                Rstr += "          _TitleClsWin();\r\n";
                Rstr += "      }\r\n";
                Rstr += "  }\r\n";

            }
            else
            {
                Rstr += "  function ToPageExcel_Click(){\r\n";
                Rstr += "      _CreatEccelOut.ExportToExcel('comment');\r\n";
                Rstr += "      _ErrMsg('完成导出');\r\n";
                Rstr += "  }\r\n";
                Rstr += "  function ToPageWord_Click(){\r\n";
                Rstr += "      _CreatEccelOut.HtmlExportToWord('comment');\r\n";
                Rstr += "      _ErrMsg('完成导出');\r\n";
                Rstr += "  }\r\n";
            }
            
    
            Rstr += "  function PageLoad(){\r\n";
            Rstr += "      _FormSetValue._FromRequestList();\r\n";
            Rstr += "  }\r\n";

            Rstr += "        function CallBackPageLoad(data) {\r\n";
            Rstr += "            _ErrMsg(data.Msg);\r\n";
            Rstr += "            if (data.scuess) {\r\n";
            Rstr += "                var ParentData = data.Result.Data;\r\n";
            Rstr += "                var SubData, FrmSublist;\r\n";
            Rstr += "                if (data.Result.hasOwnProperty('SubData')) {\r\n";
            Rstr += "                    SubData = data.Result.SubData;\r\n";
            Rstr += "                    FrmSublist = data.Result.ListConFig;\r\n";
            Rstr += "                }\r\n";

            Rstr += "                _Form_Serialize.SetFormData(ParentData[0], _EditOrBrow);\r\n";

            Rstr += "                if (data.Result.hasOwnProperty('SubData')) {\r\n";
            Rstr += "                    if (SubData.length > 0) {\r\n";
            Rstr += "                        switch (_EditOrBrow) {\r\n";
            Rstr += "                            case '2':\r\n";
            Rstr += "                                _Tb_Serialize.SetTbBrow('SubList', Win._SubListConFig, SubData);\r\n";
            Rstr += "                                break;\r\n";
            Rstr += "                            default:\r\n";
            Rstr += "                                _Tb_Serialize.SetTbMsg('SubList', Win._SubListConFig, SubData);\r\n";
            Rstr += "                                break;\r\n";
            Rstr += "                        }\r\n";
            Rstr += "                    }\r\n";
            Rstr += "                }\r\n";
            Rstr += "            }\r\n";
            Rstr += "        }\r\n";
            */
            string FrmSubFields = string.Empty;
            string Flg = string.Empty;
            if (dataGridView2.RowCount > 0)
            {
                FrmSubFields = "ID";
                for (int x = 1; x < dataGridView2.RowCount; x++)
                {
                    /*Fields,EditOrBrow,FieldCn*/
                    if (dataGridView2.Rows[x].Selected)
                    {                        
                        FrmSubFields += $",{dataGridView2.Rows[x].Cells[0].Value.ToString().Trim()}";
                        DataGridViewRow r = dataGridView2.Rows[x];
                        var s = r.Cells["EditOrBrow"].EditedFormattedValue;
                        string ChgValue = (string.IsNullOrEmpty(s.ToString())) ? "0" : s.ToString();
                        Flg = (Bflg == 0) ? "0" : ChgValue;
                        FrmSubFields += $":{Flg}";
                        FrmSubFields += $":{dataGridView2.Rows[x].Cells[1].Value.ToString().Trim()}";
                    }
                }
                //Rstr += $"  var FrmSublist='{FrmSubFields}';\r\n";
            }
          
            /*
            Rstr += "  Frm_Imint._BtnImint();\r\n";
            Rstr += "  _Form_Serialize.GetHttpParas();\r\n";
            Rstr += "  if(_EditOrBrow=='0' || _EditOrBrow == '2'){\r\n";
            Rstr += "      PageLoad();\r\n";
            Rstr += "  }\r\n";
            Rstr += "        if (_EditOrBrow == '1') {\r\n";
            Rstr += "            _Form_Serialize.SetFormAdd();//初始表单下拉全部下拉，修改窗口不需要，修改窗口在初始数据加载中实现\r\n";
            Rstr += "        }\r\n";
            */
            Rstr += $"</script>\r\n";
            Rstr += $"</body>\r\n";
            Rstr += $"</html>\r\n";
            return Rstr;
        }
        private string SetHtmlEditBody(string Col,string ColCn,string ColType)
        {
            string Rstr = string.Empty;
            switch (Col)
            {
                case "CreateDate":
                case "LastModiDate":
                case "Author":
                case "Manager":
                case "DelFlg":
                case "UnitCode":
                    Rstr += $"      <label style=\"display:none;\">\r\n";
                    Rstr += $"          <span>{ColCn}</span>\r\n";
                    Rstr += "           <input id=\"" + Col + "\" type=\"text\" name=\"" + Col + "\" placeholder=\"请输入 " + ColCn + "\" />\r\n";
                    Rstr += $"      </label>\r\n";
                    break;
                default:
                    Rstr += $"      <label>\r\n";
                    Rstr += $"          <span>{ColCn}</span>\r\n";
                    Rstr += "           <input id=\"" + Col + "\" type=\"text\" name=\"" + Col + "\" placeholder=\"请输入 " + ColCn + "\" />\r\n";
                    Rstr += $"      </label>\r\n";
                    break;
            }

            return Rstr;
        }
        private string SetHtmlBrowBody(string Col, string ColCn, string ColType)
        {
            string Rstr = string.Empty;
            switch (Col)
            {
                case "CreateDate":
                case "LastModiDate":
                case "Author":
                case "Manager":
                case "DelFlg":
                case "UnitCode":
                    Rstr += $"      <label style=\"display:none;\">\r\n";
                    Rstr += $"          <span>{ColCn}</span>\r\n";
                    Rstr += $"           <b id=\"{Col}\" ></b>\r\n";
                    Rstr += $"      </label>\r\n";
                    break;
                default:
                    Rstr += $"      <label>\r\n";
                    Rstr += $"          <span>{ColCn}</span>\r\n";
                    Rstr += $"           <b id=\"{Col}\" ></b>\r\n";
                    Rstr += $"      </label>\r\n";
                    break;
            }

            return Rstr;
        }
        private void CbList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataCurrent = CbList.SelectedValue.ToString();
            MessageBox.Show(DataCurrent);

            Conn = $"Data Source=8.129.46.155,1488;Initial Catalog={DataCurrent};User ID=sa;Password=0BI8DKAFO7V0@;Integrated Security=false;";
            dal = DalFactory.CreateDal(ConfigurationManager.AppSettings["DBType"], Conn);
            tableList = dal.GetAllTables();
            BuildView(tableList);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tb=string.Empty, table_comments=string.Empty;
            for (int x = 0; x < dataGridView1.RowCount; x++)
            {
                if (dataGridView1.Rows[x].Selected && dataGridView1.Rows[x].Cells[0].Value != null)
                {
                    tb = dataGridView1.Rows[x].Cells[0].Value.ToString().Trim();
                    table_comments = dataGridView1.Rows[x].Cells[1].Value.ToString().Trim();
                    MessageBox.Show(tb);
                    break;
                }
            }
            string tableName = tb.Trim();            
            List<Dictionary<string, string>> columnList = dal.GetAllColumns(tableName);
            foreach (Dictionary<string, string> column in columnList) //遍历字段
            {
                int index = dataGridView2.Rows.Add();
                this.dataGridView2.Rows[index].Cells[0].Value = column["columns_name"];
                this.dataGridView2.Rows[index].Cells[1].Value = column["comments"].ToString().Split(' ')[0];
                DataGridViewComboBoxCell comboBoxCell = (DataGridViewComboBoxCell)dataGridView2.Rows[index].Cells[2];
                comboBoxCell.Items.Clear();
                comboBoxCell.Items.Add("0");
                comboBoxCell.Items.Add("1");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            //Ast("123","456");
        }
        private void Ast(string aa, string bb="021")
        {
            MessageBox.Show(aa);
            MessageBox.Show(bb);
        }
        private void GetParentFields_Click(object sender, EventArgs e)
        {
            string tb = string.Empty;
            string table_comments = string.Empty;
            for (int x = 0; x < dataGridView1.RowCount; x++)
            {
                if (dataGridView1.Rows[x].Selected && dataGridView1.Rows[x].Cells[0].Value != null)
                {
                    tb = dataGridView1.Rows[x].Cells[0].Value.ToString().Trim();
                    table_comments = dataGridView1.Rows[x].Cells[1].Value.ToString().Trim();                    
                    break;
                }
            }
            if (!string.IsNullOrEmpty(tb))
            {
                string tableName = tb.Trim();
                columnList = null;
                columnList = dal.GetAllColumns(tableName);
                dataGridView3.Rows.Clear();
                SetParentTb(columnList);//主表配置
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(UserToken))
            {
                MessageBox.Show("没有认证，请登录");
            }
            else
            {
                string Url = $"{ConfigurationManager.AppSettings["RequestHost"]}{ConfigurationManager.AppSettings["Order"]}";
                string Str = RequestMothed.GetOrder(Url, JsonConvert.SerializeObject(""));
                HttpClientFactory Req = new HttpClientFactory
                {
                    RequestMothed = "JSON",
                    Url = Url,
                    paras = Str
                };
                string Rmsg = Req.Post();
            }
            //for (int x = 0; x < dataGridView3.RowCount; x++)
            /*
            foreach(DataGridViewRow row in dataGridView3.Rows)
            {
                if (row.Selected)
                {
                    var s = row.Cells["IfConst"].EditedFormattedValue;
                    var ss = row.Cells["Field"].Value;
                    MessageBox.Show(ss.ToString());
                    //string str = dataGridView3.Rows[x].Cells[0,1].Value.ToString().Trim();
                    if (string.IsNullOrEmpty(s.ToString()))
                    {
                        MessageBox.Show("string Empty");
                    }
                    else
                    {
                        MessageBox.Show(s.ToString());
                    }
                    //break;
                }
            }
            */
        }

        private void BtnCreatCfg_Click(object sender, EventArgs e)
        {
            FieldsIdent = string.Empty;
            string DB = DataCurrent;
            string tb = string.Empty, tbcn = string.Empty;
            JObject CfgObj = new JObject();
            JArray ConstValue = new JArray()
            {
                new JObject(new JProperty("InLine","data.dbo.tb"),
                            new JProperty("owner","field"),
                            new JProperty("onfield","field"),
                            new JProperty("Fileds",new JArray(
                                new JObject(                                    
                                    new JProperty("valuefield","valuefield"))
                                    )
                                ))                
            };
            JArray ComboRestlt = new JArray()
            {
                new JObject(new JProperty("OutLine","data.dbo.tb"),
                            new JProperty("owner","field"),
                            new JProperty("onfield","field"),
                            new JProperty("Fileds",new JArray(
                                new JObject(                                    
                                    new JProperty("valuefield","valuefield"))
                                    )
                                ))
            };
            JArray Fields = new JArray();
            JObject Tmp = new JObject();
            StringBuilder Sb = new StringBuilder();
            for (int x = 0; x < dataGridView1.RowCount; x++)
            {
                
                if (dataGridView1.Rows[x].Selected)
                {
                    tb = dataGridView1.Rows[x].Cells[0].Value.ToString().Trim();
                    tbcn = dataGridView1.Rows[x].Cells[1].Value.ToString().Trim();
                    break;
                }
            }
        
            CfgObj.Add(new JProperty("Db", $"{DB}"));         
            CfgObj.Add(new JProperty("Tb", $"{tb}"));
            CfgObj.Add(new JProperty("TbCn", $"{tbcn}"));

            CfgObj.Add(new JProperty("InLink", ConstValue));
            CfgObj.Add(new JProperty("OutLink", ComboRestlt));
   
            string ChgValue = string.Empty;

            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                if (row.Cells["Field"].Value!=null)
                {
                    Tmp = new JObject();

                    Tmp.Add(new JProperty("Field", $"{row.Cells["Field"].Value}"));
                    Tmp.Add(new JProperty("FieldCn", $"{row.Cells["FieldCn"].Value}"));

                    var s = row.Cells["ChangeCol"].EditedFormattedValue;
                    ChgValue = (string.IsNullOrEmpty(s.ToString())) ? "0" : s.ToString();
                    Tmp.Add(new JProperty("ChangeCol", $"{ChgValue}"));

                    s = row.Cells["IdentCmb"].EditedFormattedValue;
                    ChgValue = (string.IsNullOrEmpty(s.ToString())) ? "0" : s.ToString();
                    Tmp.Add(new JProperty("IdentCmb", $"{ChgValue}"));

                    if (ChgValue != "0")
                    {
                        FieldsIdent += (string.IsNullOrEmpty(FieldsIdent)) ? "{\r\n" +
                            $"      Field:'{row.Cells["Field"].Value}',\r\n" +
                            $"      FieldCn:'{row.Cells["FieldCn"].Value}',\r\n" +
                            $"      ChgMothed:'{ChgValue}',\r\n" +
                            $"      MaxLeng:'{row.Cells["MaxLeng"].EditedFormattedValue}'\r\n" + "}\r\n"
                            : ",{\r\n" +
                            $"      Field:'{row.Cells["Field"].Value}',\r\n" +
                            $"      FieldCn:'{row.Cells["FieldCn"].Value}',\r\n" +
                            $"      ChgMothed:'{ChgValue}',\r\n" +
                            $"      MaxLeng:'{row.Cells["MaxLeng"].EditedFormattedValue}'\r\n" + "}\r\n";
                    }
                    s = row.Cells["IfOutList"].EditedFormattedValue;
                    ChgValue = (string.IsNullOrEmpty(s.ToString())) ? "0" : s.ToString();
                    Tmp.Add(new JProperty("IfOutList", $"{ChgValue}"));


                    s = row.Cells["IfOutHidden"].EditedFormattedValue;
                    ChgValue = (string.IsNullOrEmpty(s.ToString())) ? "0" : s.ToString();
                    Tmp.Add(new JProperty("IfOutHidden", $"{ChgValue}"));


                    s = row.Cells["IfSearch"].EditedFormattedValue;
                    ChgValue = (string.IsNullOrEmpty(s.ToString())) ? "0" : s.ToString();
                    Tmp.Add(new JProperty("IfSearch", $"{ChgValue}"));


                    s = row.Cells["IfConst"].EditedFormattedValue;
                    ChgValue = (string.IsNullOrEmpty(s.ToString())) ? "0" : s.ToString();
                    Tmp.Add(new JProperty("IfConst", $"{ChgValue}"));


                    s = row.Cells["DataType"].EditedFormattedValue;
                    ChgValue = (string.IsNullOrEmpty(s.ToString())) ? "0" : s.ToString();
                    Tmp.Add(new JProperty("DataType", $"{ChgValue}"));

                    s = row.Cells["MaxLeng"].EditedFormattedValue;
                    ChgValue = (string.IsNullOrEmpty(s.ToString())) ? "0" : s.ToString();
                    Tmp.Add(new JProperty("MaxLeng", $"{ChgValue}"));

                    s = row.Cells["ConstValue"].EditedFormattedValue;
                    ChgValue = (string.IsNullOrEmpty(s.ToString())) ? "" : s.ToString();
                    Tmp.Add(new JProperty("ConstValue", $"{ChgValue}"));

                    s = row.Cells["ComboRestlt"].EditedFormattedValue;
                    ChgValue = (string.IsNullOrEmpty(s.ToString())) ? "" : s.ToString();
                    Tmp.Add(new JProperty("ComboRestlt", $"{ChgValue}"));
                    Fields.Add(new JObject(Tmp));
                }
            }
            CfgObj.Add(new JProperty("Fields", Fields));
            //FileHelper.WriteCfg(Application.StartupPath + "\\Models",Sb.ToString(),DB,tb);
            FileHelper.WriteCfg(Application.StartupPath + "\\Models", ConvertJsonString(JsonConvert.SerializeObject(CfgObj)), DB, tb);
            MessageBox.Show("Over");
        }

        private void BuildListSql(string db,string tb)
        {
            string FileJson = Application.StartupPath + $"\\Models\\{db}.{tb}.ConFig.json";
            string FileStr = FileHelper.ReadFile(FileJson);
            JObject Obj = (JObject)JsonConvert.DeserializeObject(FileStr);
            JArray Fildes = (JArray)Obj["Fields"];
            StringBuilder Sb = new StringBuilder();

        }
        private string ConvertJsonString(string str)
        {
            //格式化json字符串
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else
            {
                return str;
            }
        }
        public bool JtokenFlg(JToken Exists)
        {
            bool Flg;
            if (Exists == null)
            {
                Flg = false;
            }
            else
            {
                Flg = (Exists.Type == JTokenType.None || Exists.Type == JTokenType.Null)
                    ? false : true;
            }
            return Flg;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            /*
            var s = new
            {
                s1="ss",
                s2="no"
            };
            JObject ss = (JObject)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(s));
            bool x = JtokenFlg(ss);
            */
            string x = "[{\"Code\":\"100000000\",\"Name\":\"修改\",\"ServiceCode\":\"S100105\",\"ServiceName\":\"爬虫修改服务\",\"ServiceType\":\"Edit\",\"IsCreateWin\":\"000001\",\"WinMothed\":\"_CreatFrame\",\"ParasResult\":\"Edit.html?_EditOrBrow=0\",\"BtnOwerId\":\"OwnerEdit\",\"IfGetId\":\"000001\"},{\"Code\":\"100000001\",\"Name\":\"增加\",\"ServiceCode\":\"S100104\",\"ServiceName\":\"爬虫增加服务\",\"ServiceType\":\"Add\",\"IsCreateWin\":\"000001\",\"WinMothed\":\"_CreatFrame\",\"ParasResult\":\"Edit.html?_EditOrBrow=1\",\"BtnOwerId\":\"OwnerAdd\",\"IfGetId\":\"000000\"},{\"Code\":\"100000002\",\"Name\":\"查看\",\"ServiceCode\":\"S100103\",\"ServiceName\":\"爬虫查看服务\",\"ServiceType\":\"Get\",\"IsCreateWin\":\"000001\",\"WinMothed\":\"_CreatFrame\",\"ParasResult\":\"Brow.html?_EditOrBrow=2\",\"BtnOwerId\":\"OwnerBrow\",\"IfGetId\":\"000001\"},{\"Code\":\"100000003\",\"Name\":\"查询\",\"ServiceCode\":\"S100101\",\"ServiceName\":\"爬虫查询\",\"ServiceType\":\"Get\",\"IsCreateWin\":\"000001\",\"WinMothed\":\"_CreatFrame\",\"ParasResult\":\"CurrencySearch.html\",\"BtnOwerId\":\"OwnerSearch\",\"IfGetId\":\"000000\"}]";
            JArray xx = JArray.Parse(x);
            string i = "0";
            /*
            DataTable dt = dal.ListData("Tbl_ParityUnit");
            string s = JsonConvert.SerializeObject(dt);
            string path  = Application.StartupPath + "\\Models";
            FileHelper.WriteFile(path, s);
            */
            //textBox1.Text= FileHelper.FileToBase64("D:\\ME-WORK\\DISK-F\\安装套件\\学习\\tmp\\333-7.jpg");
        }

        private void SetRedis_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> myDic = new Dictionary<string, string>
            {
                { "aaa", "111" },

                { "bbb", "222" },

                { "ccc", "333" },

                { "ddd", "444" }
            };
            string ss = JsonConvert.SerializeObject(myDic);
            bool S=RedisHelper.Default.StringSet("Domain.User.key", ss, TimeSpan.FromSeconds(1000));
            if (S)
            {
                MessageBox.Show("成功！！！");
            }
            else
            {
                MessageBox.Show("失败！！！");
            }
        }

        private void ReadRedis_Click(object sender, EventArgs e)
        {
            string s= RedisHelper.Default.StringGet("Domain.User.key");
            if (!string.IsNullOrEmpty(s))
            {
                MessageBox.Show(s);
            }
            else
            {
                MessageBox.Show("Not Key");
            }

        }

        private void test(string a,string b,string c="1")
        {
            MessageBox.Show($"a={a}   b={b}   c={c}");
        }
        private void button5_Click(object sender, EventArgs e)
        {
            //string s = "{'Db':'Collection','Tb':'Tbl_DrugReptile','TbCn':'药品爬虫','InLink':[],'OutLink':[{'OutLine':'Collection.dbo.Tbl_ParityUnit','owner':'ParityId','onfield':'ParityId','Fileds':[{'valuefield':'Name'}]}],'Fields':[{'Field':'ID','FieldCn':'主键ID','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'bigint','MaxLeng':'8'},{'Field':'Code','FieldCn':'平台品种编码','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'100'},{'Field':'Name','FieldCn':'平台品种名称','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'600'},{'Field':'Des','FieldCn':'规格','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'200'},{'Field':'Ent','FieldCn':'厂家','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'200'},{'Field':'Place','FieldCn':'产地','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'200'},{'Field':'Unit','FieldCn':'单位','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'100'},{'Field':'Pre','FieldCn':'批发价','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'100'},{'Field':'Ret','FieldCn':'零售价','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'100'},{'Field':'Img','FieldCn':'图片URL','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'300'},{'Field':'Rmark','FieldCn':'备注','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'100'},{'Field':'BarCode','FieldCn':'条码','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'200'},{'Field':'ParityId','FieldCn':'平台编码','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'100'},{'Field':'CreateDate','FieldCn':'创建时间','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'datetime','MaxLeng':'8'},{'Field':'LastModiDate','FieldCn':'修改时间','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'datetime','MaxLeng':'8'},{'Field':'Author','FieldCn':'创建用户','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'100'},{'Field':'Manager','FieldCn':'维护用户','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'100'},{'Field':'DelFlg','FieldCn':'删除标记','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'int','MaxLeng':'4'},{'Field':'UnitCode','FieldCn':'数据分组标记','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'100'}]}";
            //JObject obj = JObject.Parse(s);
            //DataHandle O = new DataHandle();
            //O.ThisConn = Conn;

            //string ss = O.GuestListFields("","", obj,"");
            //bool ysd = O.GuestINSPara(obj,"");
            //bool dl = O.GuestUPPara(obj, "");
            //DataTable dt = O.GuestListFields(obj,"");
            //dataGridView4.DataSource = dt;
            //var YesChgCol =
            //    from p in obj["Fields"].Where(p => p["Field"].ToString().Equals("CreateDate")                   
            //    )
            //    select p;
            //JArray kk = (JArray)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(YesChgCol));
            StringBuilder sb = new StringBuilder();
            test("aa","bb");
            for (int x = 0; x < 100; x++)
            {
                sb.Append("\r\n" + GetCustomCode());
                
            }
            test("aa", "bb","cc");
            textBox1.Text = sb.ToString();
            MessageBox.Show("Over");
            //textBox1.Text = ss;
            /*
            dal = DalFactory.CreateDal(ConfigurationManager.AppSettings["DBType"]);
            int flg = dal.UpOrIns();
            MessageBox.Show(flg.ToString());
            
            if (flg)
            {
                MessageBox.Show("over");
            }
            else
            {
                MessageBox.Show("NO OVER");
            }
            */
            /*
            string S1 = DateTime.Now.ToString("HH-mm-ss.ffff");
            textBox1.Text = S1;
            Thread.Sleep(1000);
            for (int x = 0; x < 10000; x++)
            {
                GetCustomCode();
            }
            string S2 = DateTime.Now.ToString("HH-mm-ss.ffff");
            textBox1.Text += "\n\r" + S2;
            */
            /*
            var a = new
            {
                A = "C-180",
                B = new[] { "tag1", "tag2", "tag3" },
                C = new string[] { },
                D = new[] { "SE" },
               
            };
            JArray j = new JArray()
            {
                new JObject(new JProperty("LIST", "33")),
                new JObject(new JProperty("LIST", "55")),
            };

            JObject obj = new JObject(
                    new JProperty("aa", "111"),
                    new JProperty("bb", "222"),
                    new JProperty("cc", "333"),
                    new JProperty("dd", "444"),
                    new JProperty("ee", new JArray())
                    );
          
            string json = @"{
  'channel': {
    'title': 'James Newton-King',
    'link': 'http://james.newtonking.com',
    'description': 'James Newton-King\'s blog.',
    'item': [
      {
        'title': 'Json.NET 1.3 + New license + Now on CodePlex',
        'description': 'Announcing the release of Json.NET 1.3, the MIT license and the source on CodePlex',
        'link': 'http://james.newtonking.com/projects/json-net.aspx',
        'categories': [
          'Json.NET',
          'CodePlex'
        ]
      },
      {
        'title': 'LINQ to JSON beta',
        'description': 'Announcing LINQ to JSON',
        'link': 'http://james.newtonking.com/projects/json-net.aspx',
        'categories': [
          'Json.NET',
          'LINQ'
        ]
      }
    ]
  }
}";
            string k = @"{'Db':'Collection','Tb':'Tbl_DrugReptile','TbCn':'药品爬虫','InLink':[{'InLine':'data.dbo.tb'},{'Fileds':[{'onfield':'field','valuefield':'valuefield'}]}],'OutLink':[],'Fields':[{'Field':'ID','FieldCn':'主键ID','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'bigint','MaxLeng':'8'},{'Field':'Code','FieldCn':'平台品种编码','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'100'},{'Field':'Name','FieldCn':'平台品种名称','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'600'},{'Field':'Des','FieldCn':'规格','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'200'},{'Field':'Ent','FieldCn':'厂家','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'200'},{'Field':'Place','FieldCn':'产地','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'200'},{'Field':'Unit','FieldCn':'单位','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'100'},{'Field':'Pre','FieldCn':'批发价','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'100'},{'Field':'Ret','FieldCn':'零售价','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'100'},{'Field':'Img','FieldCn':'图片URL','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'300'},{'Field':'Rmark','FieldCn':'备注','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'100'},{'Field':'BarCode','FieldCn':'条码','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'200'},{'Field':'ParityId','FieldCn':'平台编码','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'100'},{'Field':'CreateDate','FieldCn':'创建时间','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'datetime','MaxLeng':'8'},{'Field':'LastModiDate','FieldCn':'修改时间','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'datetime','MaxLeng':'8'},{'Field':'Author','FieldCn':'创建用户','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'100'},{'Field':'Manager','FieldCn':'维护用户','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'100'},{'Field':'DelFlg','FieldCn':'删除标记','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'int','MaxLeng':'4'},{'Field':'UnitCode','FieldCn':'数据分组标记','ChangeCol':'0','IdentCmb':'0','IfOutList':'0','IfOutHidden':'0','IfSearch':'0','IfConst':'0','DataType':'nvarchar','MaxLeng':'100'}]}";
            JObject rss = JObject.Parse(k);
            var NoChgCol =
                from p in rss["Fields"].Where(p => p["ChangeCol"].ToString().Equals("0")
                    &&
                    p["IfOutList"].ToString().Equals("0")
                )
                select p;
            foreach (JObject item in NoChgCol)
            {
                MessageBox.Show(item["Field"].ToString());
            }
            */
            /*
            var postTitles =
                from p in rss["channel"]["item"].Where(p=> p["title"].ToString().Equals("LINQ to JSON beta"))
                select p;
           
            var categories =
                from c in rss["channel"]["item"].SelectMany(i => i["categories"]).Values<string>()
                group c by c
                into g
                orderby g.Count() descending
                select new { Category = g.Key, Count = g.Count() };
            JObject obj2 = new JObject(
                            new JProperty("bb",
                                new JObject(new JProperty("cc", "33"))
                                ));
            */
            //obj.Add(obj2);

            /*
            foreach (var ITEM in obj)
            {
                MessageBox.Show($"key={ITEM.Key}  value={ITEM.Value} ");
                if (string.Equals(ITEM.Key, "CC"))
                {
                    JArray LIST = (JArray)ITEM.Value;
                    for (int x = 0; x < LIST.Count; x++)
                    {
                        foreach (var key in (JObject)LIST[x])
                        {
                            MessageBox.Show($"key={key.Key}  value={key.Value} ");
                        }
                    }
                }
            }
            */
        }
        public static string GetRand()
        {
            var rnd = new Random(Guid.NewGuid().GetHashCode());
            //Random rnd = new Random();
            int rndNum = rnd.Next(1, 10000);
            string s = rndNum.ToString();
            int len = s.Length;
            if (len == 1)
            {
                s = $"000{s}";
            }
            else if (len == 2)
            {
                s = $"00{s}";
            }
            else if (len == 3)
            {
                s = $"0{s}";
            }
            return s;
        }
        /// <summary>
        /// 自编码算法器 按年月是时分秒+随机数
        /// </summary>
        /// <returns></returns>
        public static string GetCustomCode()
        {
            string s = DateTime.Now.ToString("yyMMddHHmmssfff");
            string code = $"{s}{GetRand()}";
            return code;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //pictureBox1.Image = FileHelper.Base64ToImage(textBox1.Text);
            /*string xx = "{'status':1,'totalcount':2,'list':[{'id':'2305b1e2-4e31-4fd3-8eb6-db57641914df','code':'8147056167227050270','title':'testing','type':'产品','status':'已处理','datetime':'2014-07-12T21:16:46','replycontent':'好的，只是测试'},{'id':'3a6546f6-49a7-4a17-b679-b3812b12b27e','code':'8147056167227050269','title':'我建议龙头有多种选配方式','type':'产品','status':'未处理','datetime':'2014-07-12T18:49:08.933','replycontent':''},{'id':'f735e461-ca72-4b44-8d7b-cd97ac09802f','code':'8147056167227050268','title':'这个产品不怎么好，不好用','type':'产品','status':'未处理','datetime':'2014-07-12T15:06:19.1','replycontent':''},{'id':'15926d9d-f469-4921-b01d-4b48ef8bd93d','code':'7141054273018032465','title':'jdjbcn','type':'服务','status':'未处理','datetime':'2014-05-27T01:03:46.477','replycontent':''},{'id':'1debf78f-42b3-4037-b71f-34075eed92bc','code':'4141051277003536211','title':'jdjbxn.x','type':'服务','status':'未处理','datetime':'2014-05-27T00:53:21.18','replycontent':''},{'id':'27593c52-b327-4557-8106-b9156df53909','code':'1143051276001357050','title':'ghggghh','type':'服务','status':'未处理','datetime':'2014-05-27T00:35:05.933','replycontent':''},{'id':'040198fc-b466-46c1-89d8-0514fbde9480','code':'4142053251166372433','title':'你好，你知道啦，我不喜欢白色浴缸','type':'服务','status':'未处理','datetime':'2014-05-25T16:37:43.853','replycontent':''},{'id':'16185418-d461-4e98-83c3-824eb7e344d6','code':'4145058213013197148','title':'hdjbchh','type':'服务','status':'未处理','datetime':'2014-05-21T01:19:14.903','replycontent':''},{'id':'6c043404-c1db-42e8-adeb-d4880fa7d1b5','code':'0142051185128085372','title':'ghhjdhd','type':'服务','status':'未处理','datetime':'2014-05-18T12:08:37.997','replycontent':''},{'id':'2dca1a38-a32b-4955-a99c-2ed7d6de60fa','code':'3146050186122030382','title':'hsibcn','type':'服务','status':'未处理','datetime':'2014-05-18T12:03:38.913','replycontent':''}]}";

            string a = ConvertJsonString(xx);
            textBox1.Visible = true;
            textBox1.Text = a;
            */
            string Result = string.Empty;
            string sb = string.Empty;
            
            for (int x = 0; x < dataGridView3.Rows.Count-1; x++)
            {
                
                if (dataGridView3.Rows[x].Cells[3].EditedFormattedValue.ToString().Trim().Length>0)
                {
                    Result = $"Field:'{dataGridView3.Rows[x].Cells[0].Value.ToString()}'," +
                        $"FieldCn:'{dataGridView3.Rows[x].Cells[1].Value.ToString()}'," +
                        $"ChgMothed:'{dataGridView3.Rows[x].Cells[3].EditedFormattedValue.ToString()}'," +
                        $"MaxLeng:{dataGridView3.Rows[x].Cells[11].Value.ToString()}";
                    string T = dataGridView3.Rows[x].Cells[10].Value.ToString().ToUpper();
                    if (T == "CHAR" || T == "NCHAR" || T == "VARCHAR" || T == "NVARCHAR")
                    {
                        Result += ",DataType:'N'";
                    }
                    else
                    {
                        Result += ",DataType:'I'";
                    }
                    sb += (string.IsNullOrEmpty(sb)) ? "{"+ Result + "}" : ",{" + Result + "}";
                }
            }
            MessageBox.Show(sb);
            FieldsIdent = $"[{sb}]";
            MessageBox.Show("OVER");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string Username = UserTxt.Text.ToString().Trim();
            string Password = PwdTxt.Text.ToString().Trim();
            if (Username == "" || Password == "")
            {
                MessageBox.Show("请输入帐号密码");
                return;
            }
            JObject U = new JObject {
                {"Username", Username},{ "Password",Password}
            };
            HttpModel.ClientPara para = new HttpModel.ClientPara
            {
                ParentData=U
            };
            HttpModel.Json json = new HttpModel.Json
            {
                ParaMethod = "Login",
                servercode = "S100106",
                userkey = UserModel,
                ParentData = para,
            };
            string encode = "";
            byte[] bytes = Encoding.GetEncoding("UTF-8").GetBytes(JsonConvert.SerializeObject(json));
            encode = Convert.ToBase64String(bytes);
            Dictionary<string,string> Po =new Dictionary<string, string>()
            {
                { "postdata",encode}
            };
            try
            {
                string Url = $"{ConfigurationManager.AppSettings["RequestHost"]}";
                string Result = RequestMothed.ReqPost(Url, JsonConvert.SerializeObject(Po));
                //string Result = HttpClientFactory.HttpPost(Url,P);
                JObject Obj = JObject.Parse(Result);
                if ((bool)Obj["ApiResult"]["scuess"])
                {
                    JObject UserKey = (JObject)Obj["UserKey"];
                    if (Username == UserKey["User"].ToString().Trim())
                    {
                        UserModel.User = UserKey["User"].ToString();
                        UserModel.UserCn = UserKey["UserCn"].ToString();
                        UserModel.UnitCode = UserKey["UnitCode"].ToString();
                        UserModel.UnitCn = UserKey["UnitCn"].ToString();
                        UserModel.Role = UserKey["Role"].ToString();
                        UserModel.UserType = UserKey["UserType"].ToString();
                        UserModel.Token = UserKey["Token"].ToString();
                        UserToken = UserModel.Token;
                    }
                    else
                    {
                        MessageBox.Show("非系统管理员不可操作");
                    }
                }
                else
                {
                    MessageBox.Show(Obj["ApiResult"]["Msg"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
