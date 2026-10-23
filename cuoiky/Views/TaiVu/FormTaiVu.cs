using System;
using System.Data;
using System.Windows.Forms;
using cuoiky.Controllers;

namespace cuoiky
{
    public partial class FormTaiVu : Form
    {
        private int maNV;
        private string role = "Nhân viên phòng tài vụ";
        private TaiVuController _tvController = new TaiVuController();

        public FormTaiVu(int maNV)
        {
            InitializeComponent();
            this.maNV = maNV;
            this.Load += FormTaiVu_Load; // Đăng ký sự kiện Load
        }

        public FormTaiVu()
        {
            InitializeComponent();
        }

        private void FormTaiVu_Load(object sender, EventArgs e)
        {
            LoadDanhSach();
        }

        private void LoadDanhSach()
        {
            DataTable dt = _tvController.LayDanhSach(role, maNV);

            // Tùy chọn: Thay thế các ô NULL từ SQL bằng chữ "*** Bảo mật ***" trực tiếp trên DataGridView cho đẹp
            foreach (DataRow row in dt.Rows)
            {
                if (row["HoTen"] == DBNull.Value) row["HoTen"] = "*** Bảo mật ***";
                if (row["SoDienThoai"] == DBNull.Value) row["SoDienThoai"] = "*** Bảo mật ***";
                if (row["NgaySinh"] == DBNull.Value) row["NgaySinh"] = "*** Bảo mật ***";
                if (row["Phai"] == DBNull.Value) row["Phai"] = "*** Bảo mật ***";
                if (row["TenPhongBan"] == DBNull.Value) row["TenPhongBan"] = "*** Bảo mật ***";
                if (row["TenVaiTro"] == DBNull.Value) row["TenVaiTro"] = "*** Bảo mật ***";
            }

            dgvNhanVien.DataSource = dt;
            dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            if (dgvNhanVien.Columns.Contains("HinhAnh")) dgvNhanVien.Columns["HinhAnh"].Visible = false;
        }

        private void FormTaiVu_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormLogin frmLogin = new FormLogin();
            frmLogin.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}