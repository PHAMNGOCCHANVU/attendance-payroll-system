using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace cuoiky
{
    public partial class FormNhanVien1 : Form
    {

        private int maNV;
        string constr = ConfigurationManager.ConnectionStrings["connectStr"].ConnectionString;
        public FormNhanVien1(int maNV)
        {
            InitializeComponent();
            this.maNV = maNV;
        }
        public FormNhanVien1()
        {
            InitializeComponent();
        }

        private void FormNhanVien_Load(object sender, EventArgs e)
        {
            LoadThongTinCaNhan();
            LoadDuAnCuaNhanVien();
            LoadAnhNhanVien(this.maNV);
        }
        private void LoadAnhNhanVien(int maNV)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT HinhAnh FROM NhanVien WHERE MaNV = @manv";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@manv", maNV);

                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        string fileName = result.ToString().Trim();

                        // 1. SỬA TẠI ĐÂY: Dùng Path.Combine để tự động xử lý dấu gạch chéo chuẩn xác
                        string path = Path.Combine(Application.StartupPath, "Images", fileName);
                        if (File.Exists(path))
                        {
                            // Giải phóng ảnh cũ để tránh rác bộ nhớ
                            if (pictureBox2.Image != null) pictureBox2.Image.Dispose();

                            // 2. SỬA TẠI ĐÂY: Dùng FileStream thay vì Image.FromFile
                            // Cách này giúp file ảnh không bị "khóa", bạn có thể sửa/xóa file ảnh lúc app đang chạy
                            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                            {
                                pictureBox2.Image = Image.FromStream(fs);
                            }

                            // 3. THAY ĐỔI: Dùng Zoom thay vì StretchImage để ảnh không bị méo mặt
                            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                        }
                        else
                        {
                            // Thông báo để bạn kiểm tra folder bin\Debug\Images
                            MessageBox.Show("Không tìm thấy ảnh tại: " + path);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi load ảnh: " + ex.Message);
                }
            }
        }
        private void LoadThongTinCaNhan()
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                string sql = @"SELECT NV.HoTen, NV.Phai, NV.NgaySinh, NV.SoDienThoai, 
                               NV.LuongCoBan, NV.PhuCap, NV.MaSoThue, PB.TenPhongBan
                               FROM NhanVien NV
                               JOIN PhongBan PB ON NV.MaPhongBan = PB.MaPhongBan
                               WHERE NV.MaNV = @maNV";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@maNV", maNV);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txtHoTen.Text = dr["HoTen"].ToString();
                    txtPhai.Text = dr["Phai"].ToString();
                    dtpNgaySinh.Value = Convert.ToDateTime(dr["NgaySinh"]);
                    txtSDT.Text = dr["SoDienThoai"].ToString();
                    txtLuong.Text = Convert.ToDecimal(dr["LuongCoBan"]).ToString("N0");
                    txtPhuCap.Text = Convert.ToDecimal(dr["PhuCap"]).ToString("N0");
                    txtMST.Text = dr["MaSoThue"].ToString();
                    txtPhong.Text = dr["TenPhongBan"].ToString();
                }
            }
            // Khoá chỉnh sửa
            foreach (Control c in this.Controls)
            {
                if (c is TextBox txt) txt.ReadOnly = true;
                if (c is DateTimePicker dtp) dtp.Enabled = false;
            }
        }
        private void LoadDuAnCuaNhanVien()
        {
            // 1. Xóa sạch các label cũ trước khi load mới
            pnlDuAn.Controls.Clear();
            // Đảm bảo FlowLayoutPanel xếp từ trên xuống dưới
            pnlDuAn.FlowDirection = FlowDirection.TopDown;
            // Ngăn việc các control tự động nhảy sang cột bên cạnh
            pnlDuAn.WrapContents = false;
            // Bật thanh cuộn dọc
            pnlDuAn.AutoScroll = true;

            // 2. Kiểm tra chuỗi kết nối (đảm bảo dùng đúng database QuanLyNhanSu)
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    // SQL lấy tên dự án dựa trên MaNV
                    string sql = @"SELECT TenDuAn FROM DuAn 
                           INNER JOIN NhanVien_DuAn ON DuAn.MaDuAn = NhanVien_DuAn.MaDuAn 
                           WHERE NhanVien_DuAn.MaNV = @maNV";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@maNV", maNV); // Đảm bảo biến maNV của bạn đang là 6

                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Label lbl = new Label();
                        // Gán nội dung
                        lbl.Text = "🔹 " + dr["TenDuAn"].ToString();

                        // Thiết kế kích thước để hiển thị đẹp trong FlowLayoutPanel
                        lbl.AutoSize = true; // Để nó tự dãn theo độ dài tên dự án
                        lbl.MaximumSize = new Size(pnlDuAn.Width - 30, 0); // Tránh tràn chiều ngang
                        lbl.Padding = new Padding(5);
                        lbl.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                        lbl.ForeColor = Color.FromArgb(0, 102, 204);

                        // Thêm vào Panel
                        pnlDuAn.Controls.Add(lbl);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi load dự án: " + ex.Message);
                }
            }
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            this.Close();
            FormLogin login = new FormLogin();
            login.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dtpNgaySinh_ValueChanged(object sender, EventArgs e)
        {

        }

        private void pnlDuAn_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtPhai_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSDT_TextChanged(object sender, EventArgs e)
        {

        }

        private void pnlDuAn_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }
}
