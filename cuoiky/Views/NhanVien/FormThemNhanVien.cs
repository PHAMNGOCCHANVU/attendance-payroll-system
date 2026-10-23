using System;
using System.Windows.Forms;
using cuoiky.Controllers;
using System.Drawing;

namespace cuoiky
{
    public partial class FormThemNhanVien : Form
    {
        private int maNVThucHien;
        private string role;
        private NhanSuController _nhanSuController = new NhanSuController();
        private GiamDocController _commonController = new GiamDocController();
        private string pathAnhThemTam = "";

        public FormThemNhanVien(int maNV, string role)
        {
            InitializeComponent();
            this.maNVThucHien = maNV;
            this.role = role;
        }


        private void FormThemNhanVien_Load(object sender, EventArgs e)
        {
            cboPhongBan_Them.DataSource = _nhanSuController.LayDanhSachPhongBan();
            cboPhongBan_Them.DisplayMember = "TenPhongBan";
            cboPhongBan_Them.ValueMember = "MaPhongBan";

            cboVaiTro_Them.DataSource = _nhanSuController.LayDanhSachVaiTro();
            cboVaiTro_Them.DisplayMember = "TenVaiTro";
            cboVaiTro_Them.ValueMember = "MaVaiTro";
        }



        private void btnLuu_Them_Click(object sender, EventArgs e)
        {

            string tenFile = "default.png";
            if (!string.IsNullOrEmpty(pathAnhThemTam))
                tenFile = _nhanSuController.CopyAnhVaoThuMuc(pathAnhThemTam);

            var nv = new
            {
                HoTen = txtHoTen_Them.Text,
                Phai = cboPhai_Them.Text,
                NgaySinh = dtpNgaySinh_Them.Value,
                SDT = txtSDT_Them.Text,
                MST = txtMaSoThue_Them.Text,
                MaPB = (int)cboPhongBan_Them.SelectedValue,
                MaVT = (int)cboVaiTro_Them.SelectedValue,
                HinhAnh = tenFile, 
                TenDN = txtTenDangNhap_Them.Text,
                MatKhau = txtMatKhau_Them.Text
            };

            string result = _nhanSuController.ThemNV(this.role, nv, maNVThucHien);
            if (result == "OK")
            {
                MessageBox.Show("Thêm thành công!");
                this.Close();
            }
            else MessageBox.Show(result);
        }

        // hiển thị message box xác nhận hủy thêm
        private void btnHuy_Them_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn hủy? Dữ liệu sẽ không được lưu.", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pathAnhThemTam = ofd.FileName;
                picHinhAnh.Image = Image.FromFile(pathAnhThemTam);
            }
        }

        
    }
}