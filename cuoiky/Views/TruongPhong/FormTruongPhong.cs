using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using cuoiky.Controllers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace cuoiky
{
    public partial class FormTruongPhong : Form
    {
        private int maNV;
        private string role = "Trưởng phòng";
        private TruongPhongController _controller;

        public FormTruongPhong(int maNV)
        {
            InitializeComponent();
            this.maNV = maNV;
            _controller = new TruongPhongController();
        }

        public FormTruongPhong()
        {
            InitializeComponent();
        }

        private void FormTruongPhong_Load(object sender, EventArgs e)
        {
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Text = "Giao diện Trưởng Phòng - Mã NV: " + maNV;

            LoadDanhSachNhanVien();

            // Khóa toàn bộ TextBox
            txtHoTen.ReadOnly = txtPhai.ReadOnly = txtSDT.ReadOnly = txtMaSoThue.ReadOnly = true;
            txtPhongBan.ReadOnly = txtVaiTro.ReadOnly = txtLuong.ReadOnly = txtPhuCap.ReadOnly = true;
            dtpNgaySinh.Enabled = false;

            // Kết nối sự kiện
            dgvNhanVien.CellClick += new DataGridViewCellEventHandler(dgvNhanVien_CellClick);
            btnDangXuat.Click += new EventHandler(btnDangXuat_Click);
        }

        private void LoadDanhSachNhanVien()
        {
            DataTable dt = _controller.LayDanhSachNhanVien(this.maNV, this.role);
            if (dt != null)
            {
                dgvNhanVien.DataSource = dt;
                dgvNhanVien.ReadOnly = true;
            }
        }

        private void LoadAnhNhanVien(int ma)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
                pictureBox1.Image = null;
            }

            string fileName = _controller.LayHinhAnh(ma);
            if (!string.IsNullOrEmpty(fileName))
            {
                string path = Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\image", fileName));
                if (File.Exists(path))
                {
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        pictureBox1.Image = Image.FromStream(fs);
                    }
                }
            }
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];

                int selectedMaNV = Convert.ToInt32(row.Cells["MaNV"].Value);
                LoadAnhNhanVien(selectedMaNV);

                txtHoTen.Text = row.Cells["HoTen"].Value.ToString();
                txtPhai.Text = row.Cells["Phai"].Value.ToString();
                dtpNgaySinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);
                txtSDT.Text = row.Cells["SoDienThoai"].Value.ToString();
                txtMaSoThue.Text = row.Cells["MaSoThue"].Value.ToString();
                txtPhongBan.Text = row.Cells["TenPhongBan"].Value.ToString();
                txtVaiTro.Text = row.Cells["TenVaiTro"].Value.ToString();

                txtLuong.Text = row.Cells["LuongCoBan"].Value.ToString();
                txtPhuCap.Text = row.Cells["PhuCap"].Value.ToString();

                
                txtThuong.Text = row.Cells["Thuong"].Value.ToString();
                txtKhauTru.Text = row.Cells["KhauTru"].Value.ToString();
                txtLuongNhan.Text = row.Cells["LuongThucLinh"].Value.ToString();
            }
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtThuong_TextChanged(object sender, EventArgs e)
        {

        }
    }
}