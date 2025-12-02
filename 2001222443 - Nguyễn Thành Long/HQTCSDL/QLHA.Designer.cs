namespace HQTCSDL
{
    partial class QLHA
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textURL = new System.Windows.Forms.TextBox();
            this.cbMaSP = new System.Windows.Forms.ComboBox();
            this.checkIsMain = new System.Windows.Forms.CheckBox();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.dgvHinhAnh = new System.Windows.Forms.DataGridView();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRefesh = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHinhAnh)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mã SP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "URL:";
            // 
            // textURL
            // 
            this.textURL.Location = new System.Drawing.Point(78, 76);
            this.textURL.Multiline = true;
            this.textURL.Name = "textURL";
            this.textURL.Size = new System.Drawing.Size(243, 30);
            this.textURL.TabIndex = 3;
            // 
            // cbMaSP
            // 
            this.cbMaSP.FormattingEnabled = true;
            this.cbMaSP.Location = new System.Drawing.Point(78, 31);
            this.cbMaSP.Name = "cbMaSP";
            this.cbMaSP.Size = new System.Drawing.Size(120, 24);
            this.cbMaSP.TabIndex = 5;
            // 
            // checkIsMain
            // 
            this.checkIsMain.AutoSize = true;
            this.checkIsMain.Location = new System.Drawing.Point(14, 125);
            this.checkIsMain.Name = "checkIsMain";
            this.checkIsMain.Size = new System.Drawing.Size(68, 20);
            this.checkIsMain.TabIndex = 6;
            this.checkIsMain.Text = "isMain";
            this.checkIsMain.UseVisualStyleBackColor = true;
            // 
            // btnThem
            // 
            this.btnThem.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnThem.Location = new System.Drawing.Point(601, 12);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(75, 40);
            this.btnThem.TabIndex = 7;
            this.btnThem.Text = "Thêm";
            this.btnThem.UseVisualStyleBackColor = false;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnEdit.Location = new System.Drawing.Point(601, 58);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 40);
            this.btnEdit.TabIndex = 8;
            this.btnEdit.Text = "Sửa";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // dgvHinhAnh
            // 
            this.dgvHinhAnh.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHinhAnh.Location = new System.Drawing.Point(26, 207);
            this.dgvHinhAnh.Name = "dgvHinhAnh";
            this.dgvHinhAnh.RowHeadersWidth = 51;
            this.dgvHinhAnh.RowTemplate.Height = 24;
            this.dgvHinhAnh.Size = new System.Drawing.Size(650, 211);
            this.dgvHinhAnh.TabIndex = 9;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnDelete.Location = new System.Drawing.Point(601, 104);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 40);
            this.btnDelete.TabIndex = 10;
            this.btnDelete.Text = "Xóa";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRefesh
            // 
            this.btnRefesh.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnRefesh.Location = new System.Drawing.Point(601, 150);
            this.btnRefesh.Name = "btnRefesh";
            this.btnRefesh.Size = new System.Drawing.Size(75, 40);
            this.btnRefesh.TabIndex = 11;
            this.btnRefesh.Text = "Làm mới";
            this.btnRefesh.UseVisualStyleBackColor = false;
            this.btnRefesh.Click += new System.EventHandler(this.btnRefesh_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkIsMain);
            this.groupBox1.Controls.Add(this.cbMaSP);
            this.groupBox1.Controls.Add(this.textURL);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(26, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(351, 174);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông tin hình ảnh";
            // 
            // QLHA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 430);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnRefesh);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.dgvHinhAnh);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnThem);
            this.Name = "QLHA";
            this.Text = "QLHA";
            ((System.ComponentModel.ISupportInitialize)(this.dgvHinhAnh)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textURL;
        private System.Windows.Forms.ComboBox cbMaSP;
        private System.Windows.Forms.CheckBox checkIsMain;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.DataGridView dgvHinhAnh;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRefesh;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}