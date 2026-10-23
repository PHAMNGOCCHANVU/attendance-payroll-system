using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using cuoiky.Controllers;

namespace cuoiky
{
    public partial class FormTruongPhongNhanSu : Form
    {
        public int maNV;
        private string role = "Trưởng phòng nhân sự";
        private TruongPhongNhanSuController _tpController = new TruongPhongNhanSuController();
        private NhanSuController _nsController = new NhanSuController(); 
        private string pathAnhTam = "";

        public FormTruongPhongNhanSu(int maNV)
        {
            InitializeComponent();
            this.maNV = maNV;
        }
       

        private void FormTruongPhongNhanSu_Load(object sender, EventArgs e)
        {
            dgvNhanVien.ReadOnly = true;
            dgvNhanVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            LoadDanhSach();
            LoadComboBoxes();
            LoadLichSu("");
        }

        private void LoadDanhSach()
        {
            dgvNhanVien.DataSource = _tpController.LayDanhSach(role, this.maNV);
            dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            // Ẩn cột Hình Ảnh trên dgvNhanVien
            if (dgvNhanVien.Columns.Contains("HinhAnh")) dgvNhanVien.Columns["HinhAnh"].Visible = false;
            if (dgvNhanVien.Columns.Contains("MaPhongBan")) dgvNhanVien.Columns["MaPhongBan"].Visible = false;
            if (dgvNhanVien.Columns.Contains("MaVaiTro")) dgvNhanVien.Columns["MaVaiTro"].Visible = false;
        }

        private void LoadLichSu(string keyword)
        {
            // Giả sử tên dgv bên Tab 2 của bạn là dgvLichSu
            if (dgvLichSu != null)
            {
                dgvLichSu.DataSource = _tpController.XemLichSu(role, keyword);
            }
        }

        private void LoadComboBoxes()
        {
            cboPhongBan.DataSource = _nsController.LayDanhSachPhongBan();
            cboPhongBan.DisplayMember = "TenPhongBan";
            cboPhongBan.ValueMember = "MaPhongBan";

            cboVaiTro.DataSource = _nsController.LayDanhSachVaiTro();
            cboVaiTro.DisplayMember = "TenVaiTro";
            cboVaiTro.ValueMember = "MaVaiTro";
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show("Các thông tin chỉnh sửa sẽ không được lưu. Bạn có muốn làm mới?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (res == DialogResult.OK)
            {
                // Bỏ chọn Grid và Xóa TextBoxes
                dgvNhanVien.ClearSelection();
                txtMaNV.Clear();
                txtHoTen.Clear();
                txtSDT.Clear();
                txtMaSoThue.Clear();
                txtTenDangNhap.Clear();
                txtMatKhau.Clear();
                txtLuong.Clear(); 
                txtPhuCap.Clear(); 
                txtThuong.Clear();
                txtKhauTru.Clear();
                txtLuongNhan.Clear();
                picHinhAnh.Image = null;
                pathAnhTam = "";
                LoadDanhSach();
            }
        }



        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaNV.Text))
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần sửa!"); return;
            }

            string tenFileAnh = Path.GetFileName(picHinhAnh.ImageLocation ?? "default.png");
            if (!string.IsNullOrEmpty(pathAnhTam)) tenFileAnh = _nsController.CopyAnhVaoThuMuc(pathAnhTam);

            var nv = new
            {
                MaNV = int.Parse(txtMaNV.Text),
                HoTen = txtHoTen.Text,
                Phai = cboPhai.Text,
                NgaySinh = dtpNgaySinh.Value,
                SDT = txtSDT.Text,
                MST = txtMaSoThue.Text,
                MaPB = (int)cboPhongBan.SelectedValue,
                MaVT = (int)cboVaiTro.SelectedValue,
                HinhAnh = tenFileAnh,
                TenDN = txtTenDangNhap.Text,
                MatKhau = txtMatKhau.Text
            };

            string result = _tpController.SuaNV(role, nv, this.maNV);
            if (result == "OK")
            {
                MessageBox.Show("Cập nhật thành công!");
                pathAnhTam = ""; LoadDanhSach(); LoadLichSu(""); // Load lại cả 2 Tab
            }
            else MessageBox.Show(result);
        }

        private void txtTimKiemLichSu_TextChanged(object sender, EventArgs e)
        {
            LoadLichSu(txtTimKiemLichSu.Text.Trim());
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];
                txtMaNV.Text = row.Cells["MaNV"].Value.ToString();
                txtHoTen.Text = row.Cells["HoTen"].Value.ToString();
                cboPhai.Text = row.Cells["Phai"].Value.ToString();
                dtpNgaySinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);
                txtSDT.Text = row.Cells["SoDienThoai"].Value.ToString();
                txtMaSoThue.Text = row.Cells["MaSoThue"].Value.ToString();
                cboPhongBan.SelectedValue = row.Cells["MaPhongBan"].Value;
                cboVaiTro.SelectedValue = row.Cells["MaVaiTro"].Value;
                txtTenDangNhap.Text = row.Cells["TenDangNhap"].Value.ToString();
                txtMatKhau.Text = ""; // Bắt buộc nhập lại pass nếu sửa

                string fileName = row.Cells["HinhAnh"].Value.ToString();
                if (!string.IsNullOrEmpty(fileName))
                {
                    string path = Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\image", fileName));
                    if (File.Exists(path)) picHinhAnh.ImageLocation = path;
                    else picHinhAnh.Image = null;
                }
                else picHinhAnh.Image = null;

                if (row.Cells["LuongCoBan"].Value == DBNull.Value)
                {
                    txtLuong.Text = "Bảo mật";
                    txtPhuCap.Text = "Bảo mật";
                    txtThuong.Text = "Bảo mật";
                    txtKhauTru.Text = "Bảo mật";
                    txtLuongNhan.Text = "Bảo mật";
                }
                else
                {
                    txtLuong.Text = row.Cells["LuongCoBan"].Value.ToString();
                    txtPhuCap.Text = row.Cells["PhuCap"].Value.ToString();
                    txtThuong.Text = row.Cells["Thuong"].Value.ToString();
                    txtKhauTru.Text = row.Cells["KhauTru"].Value.ToString();
                    txtLuongNhan.Text = row.Cells["LuongThucLinh"].Value.ToString();
                }
            }
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pathAnhTam = ofd.FileName; // Lưu đường dẫn vào biến tạm
                picHinhAnh.Image = Image.FromFile(pathAnhTam); // Hiển thị tạm lên giao diện
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            FormThemNhanVien f = new FormThemNhanVien(this.maNV, this.role);
            f.ShowDialog(); 

            LoadDanhSach();
            LoadLichSu("");
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}