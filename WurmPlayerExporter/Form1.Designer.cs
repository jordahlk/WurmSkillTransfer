namespace WurmPlayerExporter
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.Export = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnSelectItemDB = new System.Windows.Forms.Button();
            this.cbxExportInventory = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSelectDB = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lblCharacterName = new System.Windows.Forms.Label();
            this.btnSelectCharacter = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSelectDBImport = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.cmbPriest = new System.Windows.Forms.ComboBox();
            this.lblDumpPlayerName = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnSelectCharacterDump = new System.Windows.Forms.Button();
            this.btnImportDump = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnDumpSelectDB = new System.Windows.Forms.Button();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog3 = new System.Windows.Forms.OpenFileDialog();
            this.btnImportItemDb = new System.Windows.Forms.Button();
            this.Export.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Title = "Select database";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // Export
            // 
            this.Export.Controls.Add(this.tabPage1);
            this.Export.Controls.Add(this.tabPage2);
            this.Export.Controls.Add(this.tabPage3);
            this.Export.Location = new System.Drawing.Point(12, 12);
            this.Export.Name = "Export";
            this.Export.SelectedIndex = 0;
            this.Export.Size = new System.Drawing.Size(415, 241);
            this.Export.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnSelectItemDB);
            this.tabPage1.Controls.Add(this.cbxExportInventory);
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Controls.Add(this.btnExport);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.comboBox1);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.btnSelectDB);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(407, 215);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Export";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnSelectItemDB
            // 
            this.btnSelectItemDB.Location = new System.Drawing.Point(214, 94);
            this.btnSelectItemDB.Name = "btnSelectItemDB";
            this.btnSelectItemDB.Size = new System.Drawing.Size(88, 23);
            this.btnSelectItemDB.TabIndex = 21;
            this.btnSelectItemDB.Text = "Select Item DB";
            this.btnSelectItemDB.UseVisualStyleBackColor = true;
            this.btnSelectItemDB.Visible = false;
            this.btnSelectItemDB.Click += new System.EventHandler(this.btnSelectItemDB_Click);
            // 
            // cbxExportInventory
            // 
            this.cbxExportInventory.AutoSize = true;
            this.cbxExportInventory.Location = new System.Drawing.Point(42, 98);
            this.cbxExportInventory.Name = "cbxExportInventory";
            this.cbxExportInventory.Size = new System.Drawing.Size(154, 17);
            this.cbxExportInventory.TabIndex = 20;
            this.cbxExportInventory.Text = "Attempt to Export Inventory";
            this.cbxExportInventory.UseVisualStyleBackColor = true;
            this.cbxExportInventory.CheckedChanged += new System.EventHandler(this.cbxExportInventory_CheckedChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(16, 91);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(20, 24);
            this.label11.TabIndex = 19;
            this.label11.Text = "3";
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(42, 130);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 18;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(16, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 24);
            this.label3.TabIndex = 17;
            this.label3.Text = "4";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 24);
            this.label1.TabIndex = 15;
            this.label1.Text = "1";
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(42, 49);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(290, 28);
            this.comboBox1.TabIndex = 14;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 24);
            this.label2.TabIndex = 16;
            this.label2.Text = "2";
            // 
            // btnSelectDB
            // 
            this.btnSelectDB.Location = new System.Drawing.Point(42, 17);
            this.btnSelectDB.Name = "btnSelectDB";
            this.btnSelectDB.Size = new System.Drawing.Size(95, 23);
            this.btnSelectDB.TabIndex = 13;
            this.btnSelectDB.Text = "Select Player DB";
            this.btnSelectDB.UseVisualStyleBackColor = true;
            this.btnSelectDB.Click += new System.EventHandler(this.btnSelectDB_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnImportItemDb);
            this.tabPage2.Controls.Add(this.lblCharacterName);
            this.tabPage2.Controls.Add(this.btnSelectCharacter);
            this.tabPage2.Controls.Add(this.btnImport);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.btnSelectDBImport);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(407, 215);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Import";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lblCharacterName
            // 
            this.lblCharacterName.AutoSize = true;
            this.lblCharacterName.Location = new System.Drawing.Point(175, 61);
            this.lblCharacterName.Name = "lblCharacterName";
            this.lblCharacterName.Size = new System.Drawing.Size(0, 13);
            this.lblCharacterName.TabIndex = 20;
            // 
            // btnSelectCharacter
            // 
            this.btnSelectCharacter.Location = new System.Drawing.Point(42, 53);
            this.btnSelectCharacter.Name = "btnSelectCharacter";
            this.btnSelectCharacter.Size = new System.Drawing.Size(127, 23);
            this.btnSelectCharacter.TabIndex = 19;
            this.btnSelectCharacter.Text = "Select Character File";
            this.btnSelectCharacter.UseVisualStyleBackColor = true;
            this.btnSelectCharacter.Click += new System.EventHandler(this.btnSelectCharacter_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(42, 90);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 18;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(16, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 24);
            this.label4.TabIndex = 17;
            this.label4.Text = "3";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(16, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 24);
            this.label5.TabIndex = 15;
            this.label5.Text = "1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(16, 53);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 24);
            this.label6.TabIndex = 16;
            this.label6.Text = "2";
            // 
            // btnSelectDBImport
            // 
            this.btnSelectDBImport.Location = new System.Drawing.Point(42, 17);
            this.btnSelectDBImport.Name = "btnSelectDBImport";
            this.btnSelectDBImport.Size = new System.Drawing.Size(101, 23);
            this.btnSelectDBImport.TabIndex = 13;
            this.btnSelectDBImport.Text = "Select Player DB";
            this.btnSelectDBImport.UseVisualStyleBackColor = true;
            this.btnSelectDBImport.Click += new System.EventHandler(this.btnSelectDBImport_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.cmbPriest);
            this.tabPage3.Controls.Add(this.lblDumpPlayerName);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.btnSelectCharacterDump);
            this.tabPage3.Controls.Add(this.btnImportDump);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Controls.Add(this.label10);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.btnDumpSelectDB);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(407, 215);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Import Dump";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // cmbPriest
            // 
            this.cmbPriest.FormattingEnabled = true;
            this.cmbPriest.Items.AddRange(new object[] {
            "Not a priest",
            "Fo",
            "Magranon",
            "Vynora",
            "Libila"});
            this.cmbPriest.Location = new System.Drawing.Point(41, 97);
            this.cmbPriest.Name = "cmbPriest";
            this.cmbPriest.Size = new System.Drawing.Size(121, 21);
            this.cmbPriest.TabIndex = 27;
            this.cmbPriest.Text = "Select Priesthood";
            // 
            // lblDumpPlayerName
            // 
            this.lblDumpPlayerName.AutoSize = true;
            this.lblDumpPlayerName.Location = new System.Drawing.Point(174, 76);
            this.lblDumpPlayerName.Name = "lblDumpPlayerName";
            this.lblDumpPlayerName.Size = new System.Drawing.Size(0, 13);
            this.lblDumpPlayerName.TabIndex = 26;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(174, 112);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 13);
            this.label8.TabIndex = 25;
            // 
            // btnSelectCharacterDump
            // 
            this.btnSelectCharacterDump.Location = new System.Drawing.Point(41, 60);
            this.btnSelectCharacterDump.Name = "btnSelectCharacterDump";
            this.btnSelectCharacterDump.Size = new System.Drawing.Size(127, 23);
            this.btnSelectCharacterDump.TabIndex = 24;
            this.btnSelectCharacterDump.Text = "Select Character File";
            this.btnSelectCharacterDump.UseVisualStyleBackColor = true;
            this.btnSelectCharacterDump.Click += new System.EventHandler(this.btnSelectCharacterDump_Click);
            // 
            // btnImportDump
            // 
            this.btnImportDump.Location = new System.Drawing.Point(41, 124);
            this.btnImportDump.Name = "btnImportDump";
            this.btnImportDump.Size = new System.Drawing.Size(75, 23);
            this.btnImportDump.TabIndex = 23;
            this.btnImportDump.Text = "Import";
            this.btnImportDump.UseVisualStyleBackColor = true;
            this.btnImportDump.Click += new System.EventHandler(this.btnImportDump_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(15, 124);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(20, 24);
            this.label9.TabIndex = 22;
            this.label9.Text = "3";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(15, 60);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(20, 24);
            this.label10.TabIndex = 21;
            this.label10.Text = "2";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(15, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(20, 24);
            this.label7.TabIndex = 17;
            this.label7.Text = "1";
            // 
            // btnDumpSelectDB
            // 
            this.btnDumpSelectDB.Location = new System.Drawing.Point(41, 18);
            this.btnDumpSelectDB.Name = "btnDumpSelectDB";
            this.btnDumpSelectDB.Size = new System.Drawing.Size(127, 23);
            this.btnDumpSelectDB.TabIndex = 16;
            this.btnDumpSelectDB.Text = "Select Player DB";
            this.btnDumpSelectDB.UseVisualStyleBackColor = true;
            this.btnDumpSelectDB.Click += new System.EventHandler(this.btnDumpSelectDB_Click);
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // openFileDialog3
            // 
            this.openFileDialog3.FileName = "openFileDialog3";
            // 
            // btnImportItemDb
            // 
            this.btnImportItemDb.Location = new System.Drawing.Point(165, 17);
            this.btnImportItemDb.Name = "btnImportItemDb";
            this.btnImportItemDb.Size = new System.Drawing.Size(101, 23);
            this.btnImportItemDb.TabIndex = 21;
            this.btnImportItemDb.Text = "Select Item DB";
            this.btnImportItemDb.UseVisualStyleBackColor = true;
            this.btnImportItemDb.Click += new System.EventHandler(this.btnImportItemDb_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 265);
            this.Controls.Add(this.Export);
            this.Name = "Form1";
            this.Text = "Wurm Player Export/Import";
            this.Export.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TabControl Export;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSelectDB;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSelectDBImport;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.Button btnSelectCharacter;
        private System.Windows.Forms.OpenFileDialog openFileDialog3;
        private System.Windows.Forms.Label lblCharacterName;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnSelectCharacterDump;
        private System.Windows.Forms.Button btnImportDump;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnDumpSelectDB;
        private System.Windows.Forms.Label lblDumpPlayerName;
        private System.Windows.Forms.ComboBox cmbPriest;
        private System.Windows.Forms.CheckBox cbxExportInventory;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnSelectItemDB;
        private System.Windows.Forms.Button btnImportItemDb;
    }
}

