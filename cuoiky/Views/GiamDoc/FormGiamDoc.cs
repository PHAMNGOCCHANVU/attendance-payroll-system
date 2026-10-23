using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using cuoiky.Controllers;

namespace cuoiky
{
    public partial class FormGiamDoc : Form
    {
        private int maNV;
        private string role = "Giám đốc"; 
        private GiamDocController _controller;

        public FormGiamDoc(int maNV = 0)
        {
            InitializeComponent();
            this.maNV = maNV;
            _controller = new GiamDocController(); 
        }

        private void FormGiamDoc_Load(object sender, EventArgs e)
        {
            // 1. Bật tính năng tự động thay đổi kích thước
            this.AutoSize = true;

            // 2. Tùy chọn chế độ tự động (GrowAndShrink: tự động nới rộng và thu hẹp ôm sát component)
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            this.Text = "Giao diện Giám đốc - Mã NV: " + maNV;
            LoadDanhSachNhanVien();
            LoadPhongBan();
            LoadComboBoxVaiTro();
            LoadAnhNhanVien(this.maNV);
            LoadLichSu("");
        }

        private void LoadDanhSachNhanVien()
        {
            DataTable dt = _controller.LayDanhSachNhanVien(role);
            if (dt != null)
            {
                dgvNhanVien.DataSource = dt;
                dgvNhanVien.ReadOnly = true; // Chặn sửa trực tiếp trên lưới để bắt buộc dùng TextBox có Validate
                if (dgvNhanVien.Columns.Contains("MaPhongBan"))
                {
                    dgvNhanVien.Columns["MaPhongBan"].Visible = false;
                }
            }
        }

        private void LoadPhongBan()
        {
            cboPhongBan.DataSource = _controller.LayPhongBan();
            cboPhongBan.DisplayMember = "TenPhongBan";
            cboPhongBan.ValueMember = "MaPhongBan";
        }

        private void LoadComboBoxVaiTro()
        {
            cboVaiTro.DataSource = _controller.LayVaiTro();
            cboVaiTro.DisplayMember = "TenVaiTro";
            cboVaiTro.ValueMember = "MaVaiTro";
        }

        private void LoadLichSu(string keyword)
        {
            dgvLichSu.DataSource = _controller.LayLichSu(keyword);
            dgvLichSu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (dgvLichSu.Columns.Contains("ChiTiet"))
            {
                dgvLichSu.Columns["ChiTiet"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dgvLichSu.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgvLichSu.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            }
        }

        private void LoadAnhNhanVien(int ma)
        {
            if (pictureBox2.Image != null)
            {
                pictureBox2.Image.Dispose(); 
                pictureBox2.Image = null;    
            }
            string fileName = _controller.LayHinhAnh(ma);

         
            if (!string.IsNullOrEmpty(fileName))
            {
                string path = Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\image", fileName));

                if (File.Exists(path))
                {
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        pictureBox2.Image = Image.FromStream(fs);
                    }
                    pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                else
                {
                    // pictureBox2.Image = Properties.Resources.DefaultAvatar; 
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

                
                cboVaiTro.Text = row.Cells["TenVaiTro"].Value.ToString();
                string hoTen = row.Cells["HoTen"].Value.ToString();
                string tenVaiTro = row.Cells["TenVaiTro"].Value.ToString();
                lbl_vaitro.Text = hoTen + " - " + tenVaiTro;
                if (row.Cells["MaPhongBan"].Value != DBNull.Value)
                    cboPhongBan.SelectedValue = row.Cells["MaPhongBan"].Value;

                // Load Lương giải mã
                txtLuong.Text = row.Cells["LuongCoBan"].Value.ToString();
                txtPhuCap.Text = row.Cells["PhuCap"].Value.ToString();

                
                txtThuong.Text = row.Cells["Thuong"].Value.ToString();
                txtKhauTru.Text = row.Cells["KhauTru"].Value.ToString();

                // Khóa thông tin, chỉ mở Lương
                txtHoTen.ReadOnly = txtPhai.ReadOnly = txtSDT.ReadOnly = txtMaSoThue.ReadOnly = true;
                cboVaiTro.Enabled = cboPhongBan.Enabled = dtpNgaySinh.Enabled = false;

                txtLuong.ReadOnly = txtPhuCap.ReadOnly = false;
                txtThuong.ReadOnly = txtKhauTru.ReadOnly = false;
                txtKhauTru.ReadOnly = false;
            }
        }

        // Sự kiện nút Cập nhật
        private void button2_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên để cập nhật!");
                return;
            }

            int targetMaNV = Convert.ToInt32(dgvNhanVien.SelectedRows[0].Cells["MaNV"].Value);

            /* LƯU Ý: NẾU BẠN CHƯA THÊM 2 Ô NÀY TRONG DESIGNER THÌ TRUYỀN VÀO LÀ "0" */
            string thuongStr = txtThuong.Text;
            string khauTruStr = txtKhauTru.Text;

            // Gọi Controller xử lý
            string result = _controller.CapNhatLuong(role, this.maNV, targetMaNV, txtLuong.Text, txtPhuCap.Text, thuongStr, khauTruStr);

            if (result == "OK")
            {
                MessageBox.Show("Cập nhật và mã hóa lương thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDanhSachNhanVien();
                LoadLichSu(""); // Tự động làm mới lịch sử
            }
            else
            {
                MessageBox.Show(result, "Lỗi cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtTimKiemLichSu_TextChanged(object sender, EventArgs e)
        {
            LoadLichSu(txtTimKiemLichSu.Text.Trim());
        }

        // Các event rỗng giữ nguyên để không lỗi designer
        private void button1_Click(object sender, EventArgs e) { this.Close(); }
        private void btnLamMoi_Click(object sender, EventArgs e) { /* Code xóa rỗng ô text như cũ */ }
        private void tabPage1_Click(object sender, EventArgs e) { }
        private void tabPage2_Click(object sender, EventArgs e) { }
        private void dgvLichSu_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dgvNhanVien_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void label12_Click_1(object sender, EventArgs e) { }
        private void txtPhai_TextChanged(object sender, EventArgs e) { }
    }
}