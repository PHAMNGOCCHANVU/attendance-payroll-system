using System;
using System.IO;
using System.Windows.Forms;
using cuoiky.Controllers;
using System.Drawing;

namespace cuoiky
{
    public partial class FormNhanSu : Form
    {
        private int maNV;
        private string role = "Nhân viên phòng nhân sự";
        private NhanSuController _nhanSuController = new NhanSuController();
        private GiamDocController _commonController = new GiamDocController();
        private string pathAnhTam = "";

        public FormNhanSu(int maNV)
        {
            InitializeComponent();
            this.maNV = maNV;
        }

        private void FormNhanSu_Load(object sender, EventArgs e)
        {
            dgvNhanVien.ReadOnly = true;
            dgvNhanVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNhanVien.AllowUserToAddRows = false;

            LoadComboBoxes();
            LoadDanhSach();
        }

        private void LoadDanhSach()
        {
            dgvNhanVien.DataSource = _nhanSuController.LayDanhSach(role, this.maNV);
            dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            if (dgvNhanVien.Columns.Contains("HinhAnh")) dgvNhanVien.Columns["HinhAnh"].Visible = false;
            if (dgvNhanVien.Columns.Contains("MaPhongBan")) dgvNhanVien.Columns["MaPhongBan"].Visible = false;
            if (dgvNhanVien.Columns.Contains("MaVaiTro")) dgvNhanVien.Columns["MaVaiTro"].Visible = false;
            if (dgvNhanVien.Columns.Contains("LuongCoBan")) dgvNhanVien.Columns["LuongCoBan"].Visible = false;
            if (dgvNhanVien.Columns.Contains("PhuCap")) dgvNhanVien.Columns["PhuCap"].Visible = false;
            if (dgvNhanVien.Columns.Contains("Thuong")) dgvNhanVien.Columns["Thuong"].Visible = false;
            if (dgvNhanVien.Columns.Contains("KhauTru")) dgvNhanVien.Columns["KhauTru"].Visible = false;
            if (dgvNhanVien.Columns.Contains("LuongThucLinh")) dgvNhanVien.Columns["LuongThucLinh"].Visible = false;
        }
        

        private void LoadComboBoxes()
        {
            cboPhongBan.DataSource = _nhanSuController.LayDanhSachPhongBan();
            cboPhongBan.DisplayMember = "TenPhongBan";
            cboPhongBan.ValueMember = "MaPhongBan";

            cboVaiTro.DataSource = _nhanSuController.LayDanhSachVaiTro();
            cboVaiTro.DisplayMember = "TenVaiTro";
            cboVaiTro.ValueMember = "MaVaiTro";
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

                // Gán đúng ID cho ComboBox để hiển thị Tên tương ứng
                cboPhongBan.SelectedValue = row.Cells["MaPhongBan"].Value;
                cboVaiTro.SelectedValue = row.Cells["MaVaiTro"].Value;

                textBox1.Text = row.Cells["TenDangNhap"].Value.ToString();

               
                textBox2.Text = "";

                // Load ảnh
                string fileName = row.Cells["HinhAnh"].Value.ToString();
                if (!string.IsNullOrEmpty(fileName))
                {
                    string path = Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\image", fileName));
                    if (File.Exists(path)) picHinhAnh.ImageLocation = path;
                    else picHinhAnh.Image = null;
                }
                else picHinhAnh.Image = null;
            }
        }


      

        private void btnThem_Click(object sender, EventArgs e)
        {
            FormThemNhanVien f = new FormThemNhanVien(this.maNV, this.role);
            f.ShowDialog();
            LoadDanhSach(); // Load lại sau khi thêm
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaNV.Text))
            {
                MessageBox.Show("Vui lòng chọn một nhân viên từ danh sách để cập nhật!");
                return;
            }

            string tenFileAnh = Path.GetFileName(picHinhAnh.ImageLocation ?? "default.png");
            if (!string.IsNullOrEmpty(pathAnhTam))
                tenFileAnh = _nhanSuController.CopyAnhVaoThuMuc(pathAnhTam);

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
                TenDN = textBox1.Text, // txtTenDangNhap
                MatKhau = textBox2.Text // txtMatKhau
            };

            string result = _nhanSuController.SuaNV(role, nv, this.maNV);
            if (result == "OK")
            {
                MessageBox.Show("Sửa thành công!");
                pathAnhTam = "";
                LoadDanhSach();
            }
            else MessageBox.Show(result);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pathAnhTam = ofd.FileName;
                picHinhAnh.Image = Image.FromFile(pathAnhTam); // Hiển thị tạm lên PictureBox
            }
        }

        private void btnLamMoi_Click_1(object sender, EventArgs e)
        {
            var res = MessageBox.Show("Các thay đổi chưa lưu sẽ bị mất. Bạn có chắc muốn làm mới?", "Xác nhận", MessageBoxButtons.OKCancel);
            if (res == DialogResult.OK)
            {
                // Xóa sạch các ô nhập liệu
                txtMaNV.Clear();
                txtHoTen.Clear();
                txtSDT.Clear();
                txtMaSoThue.Clear();
                textBox1.Clear();
                textBox2.Clear();
                picHinhAnh.Image = null;
                pathAnhTam = "";
                LoadDanhSach();
            }
        }
    }
}