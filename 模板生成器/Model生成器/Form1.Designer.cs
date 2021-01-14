namespace Model
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCreate = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.CbList = new System.Windows.Forms.ComboBox();
            this.SubTxt = new System.Windows.Forms.TextBox();
            this.LinkFields = new System.Windows.Forms.TextBox();
            this.ParentFields = new System.Windows.Forms.TextBox();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.GetParentFields = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.BtnCreatCfg = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.SetRedis = new System.Windows.Forms.Button();
            this.ReadRedis = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.dataGridView4 = new System.Windows.Forms.DataGridView();
            this.button7 = new System.Windows.Forms.Button();
            this.UserTxt = new System.Windows.Forms.TextBox();
            this.PwdTxt = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(27, 330);
            this.btnCreate.Margin = new System.Windows.Forms.Padding(4);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(100, 34);
            this.btnCreate.TabIndex = 0;
            this.btnCreate.Text = "生成表单";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(13, 42);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(394, 14);
            this.progressBar1.TabIndex = 3;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(16, 65);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(391, 240);
            this.dataGridView1.TabIndex = 4;
            // 
            // CbList
            // 
            this.CbList.FormattingEnabled = true;
            this.CbList.Location = new System.Drawing.Point(16, 12);
            this.CbList.Name = "CbList";
            this.CbList.Size = new System.Drawing.Size(391, 23);
            this.CbList.TabIndex = 5;
            this.CbList.SelectedIndexChanged += new System.EventHandler(this.CbList_SelectedIndexChanged);
            // 
            // SubTxt
            // 
            this.SubTxt.Location = new System.Drawing.Point(454, 65);
            this.SubTxt.Name = "SubTxt";
            this.SubTxt.Size = new System.Drawing.Size(156, 25);
            this.SubTxt.TabIndex = 6;
            // 
            // LinkFields
            // 
            this.LinkFields.Location = new System.Drawing.Point(454, 126);
            this.LinkFields.Name = "LinkFields";
            this.LinkFields.Size = new System.Drawing.Size(156, 25);
            this.LinkFields.TabIndex = 7;
            // 
            // ParentFields
            // 
            this.ParentFields.Location = new System.Drawing.Point(454, 192);
            this.ParentFields.Name = "ParentFields";
            this.ParentFields.Size = new System.Drawing.Size(156, 25);
            this.ParentFields.TabIndex = 8;
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(650, 67);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowTemplate.Height = 27;
            this.dataGridView2.Size = new System.Drawing.Size(486, 238);
            this.dataGridView2.TabIndex = 9;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(650, 322);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 37);
            this.button1.TabIndex = 10;
            this.button1.Text = "取子表字段";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(868, 322);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(84, 36);
            this.button2.TabIndex = 11;
            this.button2.Text = "Clear";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dataGridView3
            // 
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Location = new System.Drawing.Point(16, 371);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.RowTemplate.Height = 27;
            this.dataGridView3.Size = new System.Drawing.Size(1426, 293);
            this.dataGridView3.TabIndex = 12;
            // 
            // GetParentFields
            // 
            this.GetParentFields.Location = new System.Drawing.Point(154, 330);
            this.GetParentFields.Name = "GetParentFields";
            this.GetParentFields.Size = new System.Drawing.Size(104, 35);
            this.GetParentFields.TabIndex = 13;
            this.GetParentFields.Text = "取主表字段";
            this.GetParentFields.UseVisualStyleBackColor = true;
            this.GetParentFields.Click += new System.EventHandler(this.GetParentFields_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(527, 327);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(83, 35);
            this.button3.TabIndex = 14;
            this.button3.Text = "更新配置";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // BtnCreatCfg
            // 
            this.BtnCreatCfg.Location = new System.Drawing.Point(276, 333);
            this.BtnCreatCfg.Name = "BtnCreatCfg";
            this.BtnCreatCfg.Size = new System.Drawing.Size(91, 32);
            this.BtnCreatCfg.TabIndex = 15;
            this.BtnCreatCfg.Text = "生成配置";
            this.BtnCreatCfg.UseVisualStyleBackColor = true;
            this.BtnCreatCfg.Click += new System.EventHandler(this.BtnCreatCfg_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(1292, 334);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 16;
            this.button4.Text = "tojson";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // SetRedis
            // 
            this.SetRedis.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SetRedis.Location = new System.Drawing.Point(1292, 114);
            this.SetRedis.Name = "SetRedis";
            this.SetRedis.Size = new System.Drawing.Size(113, 37);
            this.SetRedis.TabIndex = 17;
            this.SetRedis.Text = "SetRedis";
            this.SetRedis.UseVisualStyleBackColor = true;
            this.SetRedis.Click += new System.EventHandler(this.SetRedis_Click);
            // 
            // ReadRedis
            // 
            this.ReadRedis.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ReadRedis.Location = new System.Drawing.Point(1292, 193);
            this.ReadRedis.Name = "ReadRedis";
            this.ReadRedis.Size = new System.Drawing.Size(126, 37);
            this.ReadRedis.TabIndex = 18;
            this.ReadRedis.Text = "ReadRedis";
            this.ReadRedis.UseVisualStyleBackColor = true;
            this.ReadRedis.Click += new System.EventHandler(this.ReadRedis_Click);
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button5.Location = new System.Drawing.Point(1007, 329);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(99, 28);
            this.button5.TabIndex = 19;
            this.button5.Text = "事务";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(824, 5);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(552, 284);
            this.textBox1.TabIndex = 20;
            this.textBox1.Visible = false;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(385, 332);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(91, 35);
            this.button6.TabIndex = 22;
            this.button6.Text = "较验配置";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // dataGridView4
            // 
            this.dataGridView4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView4.Location = new System.Drawing.Point(1173, 98);
            this.dataGridView4.Name = "dataGridView4";
            this.dataGridView4.RowTemplate.Height = 27;
            this.dataGridView4.Size = new System.Drawing.Size(128, 191);
            this.dataGridView4.TabIndex = 23;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(454, 281);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(110, 40);
            this.button7.TabIndex = 24;
            this.button7.Text = "登录";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // UserTxt
            // 
            this.UserTxt.Location = new System.Drawing.Point(431, 239);
            this.UserTxt.Name = "UserTxt";
            this.UserTxt.Size = new System.Drawing.Size(100, 25);
            this.UserTxt.TabIndex = 25;
            this.UserTxt.Text = "User";
            // 
            // PwdTxt
            // 
            this.PwdTxt.Location = new System.Drawing.Point(538, 239);
            this.PwdTxt.Name = "PwdTxt";
            this.PwdTxt.Size = new System.Drawing.Size(100, 25);
            this.PwdTxt.TabIndex = 26;
            this.PwdTxt.Text = "Pwd";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1454, 693);
            this.Controls.Add(this.PwdTxt);
            this.Controls.Add(this.UserTxt);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.dataGridView4);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.ReadRedis);
            this.Controls.Add(this.SetRedis);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.BtnCreatCfg);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.GetParentFields);
            this.Controls.Add(this.dataGridView3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.ParentFields);
            this.Controls.Add(this.LinkFields);
            this.Controls.Add(this.SubTxt);
            this.Controls.Add(this.CbList);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnCreate);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Model生成器";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox CbList;
        private System.Windows.Forms.TextBox SubTxt;
        private System.Windows.Forms.TextBox LinkFields;
        private System.Windows.Forms.TextBox ParentFields;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button GetParentFields;
        private System.Windows.Forms.Button button3;
        public System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.Button BtnCreatCfg;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button SetRedis;
        private System.Windows.Forms.Button ReadRedis;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.DataGridView dataGridView4;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TextBox UserTxt;
        private System.Windows.Forms.TextBox PwdTxt;
    }
}

