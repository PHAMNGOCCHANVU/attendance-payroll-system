namespace cuoiky
{
    partial class FormTruongPhongTaiVu
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
            System.Windows.Forms.TabPage tabPage1;
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.btnDangXuat = new System.Windows.Forms.Button();
            this.dtvNhanVienTaiVu = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.dtvNhanVienPhongKhac = new System.Windows.Forms.DataGridView();
            tabPage1 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtvNhanVienTaiVu)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtvNhanVienPhongKhac)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 450);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(this.btnDangXuat);
            tabPage1.Controls.Add(this.dtvNhanVienTaiVu);
            tabPage1.Location = new System.Drawing.Point(4, 25);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(3);
            tabPage1.Size = new System.Drawing.Size(792, 421);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Xem nhân viên phòng tài vụ";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnDangXuat
            // 
            this.btnDangXuat.Location = new System.Drawing.Point(312, 302);
            this.btnDangXuat.Name = "btnDangXuat";
            this.btnDangXuat.Size = new System.Drawing.Size(114, 33);
            this.btnDangXuat.TabIndex = 1;
            this.btnDangXuat.Text = "Đăng xuất";
            this.btnDangXuat.UseVisualStyleBackColor = true;
            // 
            // dtvNhanVienTaiVu
            // 
            this.dtvNhanVienTaiVu.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dtvNhanVienTaiVu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtvNhanVienTaiVu.Location = new System.Drawing.Point(6, 42);
            this.dtvNhanVienTaiVu.Name = "dtvNhanVienTaiVu";
            this.dtvNhanVienTaiVu.RowHeadersWidth = 51;
            this.dtvNhanVienTaiVu.RowTemplate.Height = 24;
            this.dtvNhanVienTaiVu.Size = new System.Drawing.Size(776, 227);
            this.dtvNhanVienTaiVu.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.dtvNhanVienPhongKhac);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(792, 355);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Xem nhân viên phòng khác";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(317, 305);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(114, 33);
            this.button1.TabIndex = 2;
            this.button1.Text = "Đăng xuất";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // dtvNhanVienPhongKhac
            // 
            this.dtvNhanVienPhongKhac.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dtvNhanVienPhongKhac.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtvNhanVienPhongKhac.Location = new System.Drawing.Point(6, 49);
            this.dtvNhanVienPhongKhac.Name = "dtvNhanVienPhongKhac";
            this.dtvNhanVienPhongKhac.ReadOnly = true;
            this.dtvNhanVienPhongKhac.RowHeadersWidth = 51;
            this.dtvNhanVienPhongKhac.RowTemplate.Height = 24;
            this.dtvNhanVienPhongKhac.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtvNhanVienPhongKhac.Size = new System.Drawing.Size(776, 227);
            this.dtvNhanVienPhongKhac.TabIndex = 1;
            // 
            // FormTruongPhongTaiVu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormTruongPhongTaiVu";
            this.Text = "FormTruongPhongTaiVu";
            this.tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtvNhanVienTaiVu)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtvNhanVienPhongKhac)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Button btnDangXuat;
        private System.Windows.Forms.DataGridView dtvNhanVienTaiVu;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dtvNhanVienPhongKhac;
    }
}