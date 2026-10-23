using System;
using System.Windows.Forms;
using cuoiky.Controllers; // Gọi namespace Controllers

namespace cuoiky
{
    public partial class FormLogin : Form
    {
        private AuthController _authController;

        public FormLogin()
        {
            InitializeComponent();
            _authController = new AuthController();
        }

        public FormLogin(AuthController authController)
        {
            InitializeComponent();
            _authController = authController;
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void txtUser_TextChanged(object sender, EventArgs e) { }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string user = txtUser.Text.Trim();
            string pass = txtPass.Text.Trim();

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!");
                return;
            }

            // Các biến để hứng dữ liệu trả về từ Controller
            int maNV;
            string role;
            string department; 

            // Gọi Controller để xử lý đăng nhập
            bool isSuccess = _authController.Login(user, pass, out maNV, out role, out department);

            if (isSuccess)
            {
                this.Hide(); // Ẩn form đăng nhập
                Form nextForm = null;

                // Tự động chuyển Form tương ứng dựa vào vai trò
                if (role == "Giám đốc")
                {
                    nextForm = new FormGiamDoc(maNV);
                }
                else if (role == "Nhân viên")
                {
                    if (department == "Phòng Nhân Sự")
                        nextForm = new FormNhanSu(maNV);
                    else if (department == "Phòng Tài Vụ")
                        nextForm = new FormTaiVu(maNV);
                    else
                        nextForm = new FormNhanVien(maNV); 
                }
                else if (role == "Trưởng phòng" || role == "Trưởng phòng nhân sự") 
                {
                    if (department == "Phòng Nhân Sự")
                        nextForm = new FormTruongPhongNhanSu(maNV);
                    else if (department == "Phòng Tài Vụ")
                        nextForm = new FormTruongPhongTaiVu(maNV);
                    else
                        nextForm = new FormTruongPhong(maNV); 
                }
                else
                {
                    MessageBox.Show($"Chưa cấu hình Form cho Vai trò: '{role}' - Phòng: '{department}'");
                    this.Show();
                    return;
                }

                if (nextForm != null)
                {
                    nextForm.ShowDialog();
                    this.Show(); // Hiện lại form login khi form kia đóng
                }
            }
            else
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}