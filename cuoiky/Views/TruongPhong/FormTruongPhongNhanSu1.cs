using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace cuoiky
{
    public partial class FormTruongPhongNhanSu1 : Form
    {
        public int maNV;
        string constr = ConfigurationManager.ConnectionStrings["connectStr"].ConnectionString;


        public FormTruongPhongNhanSu1(int maNV = 0)
        {
            InitializeComponent();
            this.maNV = maNV;
        }
        // ✅ Constructor 2: Dành cho Designer, giúp mở giao diện được

        private void FormTruongPhongNhanSu_Load(object sender, EventArgs e)
        {
            LoadDanhSachNhanVien();
            this.Text = "Trưởng phòng nhân sự - Mã NV: " + maNV;
            LoadPhongBan();
            LoadComboBoxVaiTro();
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
        private void LoadComboBoxVaiTro()
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                string sql = "SELECT * FROM VaiTro";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cboVaiTro.DataSource = dt;
                cboVaiTro.DisplayMember = "TenVaiTro";
                cboVaiTro.ValueMember = "MaVaiTro";
            }
        }
        private void LoadDanhSachNhanVien()
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                string sql = @"SELECT MaNV, HoTen, Phai, NgaySinh, SoDienThoai, 
                       MaSoThue, MaPhongBan, MaVaiTro, LuongCoBan, PhuCap 
                       FROM NhanVien";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvNhanVien.DataSource = dt;
                // Ẩn cột Lương và Phụ cấp trên Grid để người khác không nhìn thấy
                dgvNhanVien.Columns["LuongCoBan"].Visible = false;
                dgvNhanVien.Columns["PhuCap"].Visible = false;
            }
        }
        private void LoadPhongBan()
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                string sql = "SELECT MaPhongBan, TenPhongBan FROM PhongBan";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cboPhongBan.DataSource = dt;
                cboPhongBan.DisplayMember = "TenPhongBan";
                cboPhongBan.ValueMember = "MaPhongBan";
            }
        }
        public FormTruongPhongNhanSu1()
        {
            InitializeComponent();
        }
        private void btnDangxuat_Click(object sender, EventArgs e)
        {
            this.Close();
            FormLogin login = new FormLogin();
            login.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];
                txtHoTen.Text = row.Cells["HoTen"].Value.ToString();
                txtPhai.Text = row.Cells["Phai"].Value.ToString();
                dtpNgaySinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);
                txtSDT.Text = row.Cells["SoDienThoai"].Value.ToString();
                txtMaSoThue.Text = row.Cells["MaSoThue"].Value.ToString();
                cboPhongBan.SelectedValue = row.Cells["MaPhongBan"].Value;
                // Kiểm tra xem cột có tồn tại trong DataGridView không trước khi lấy giá trị
                if (row.Cells["MaVaiTro"].Value != null)
                {
                    cboVaiTro.SelectedValue = row.Cells["MaVaiTro"].Value;
                }
                // SO SÁNH: MaNV trên lưới có trùng với MaNV của người đang đăng nhập không?
                // 1. Lấy MaNV từ dòng đang click trên lưới (Ép kiểu sang int)
                int maNVDuocChon = Convert.ToInt32(row.Cells["MaNV"].Value);
                if (maNVDuocChon == this.maNV)
                {
                    // Kiểm tra xem cột có tồn tại trong DataGridView không trước khi truy cập
                    if (dgvNhanVien.Columns.Contains("LuongCoBan"))
                        txtLuong.Text = row.Cells["LuongCoBan"].Value.ToString();
                    else
                        txtLuong.Text = "N/A"; // Nếu không tìm thấy cột trong lưới

                    if (dgvNhanVien.Columns.Contains("PhuCap"))
                        txtPhuCap.Text = row.Cells["PhuCap"].Value.ToString();
                    else
                        txtPhuCap.Text = "N/A";
                }
                else
                {
                    // Nếu không trùng (nhân viên khác): Ẩn đi
                    txtLuong.Text = "null";
                    txtPhuCap.Text = "null";
                }

                // Luôn khóa 2 ô này để đúng yêu cầu "không thể xem/chỉnh sửa lương người khác"
                txtLuong.ReadOnly = true;
                txtPhuCap.ReadOnly = true;
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.SelectedRows.Count == 0) return;
            int ma = Convert.ToInt32(dgvNhanVien.SelectedRows[0].Cells["MaNV"].Value);
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                string sql = @"UPDATE NhanVien SET 
                        HoTen=@ten, Phai=@phai, NgaySinh=@ns, 
                        SoDienThoai=@sdt, MaSoThue=@mst, MaPhongBan=@pb, MaVaiTro = @mavaitro
                        WHERE MaNV=@ma";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ma", ma);
                cmd.Parameters.AddWithValue("@ten", txtHoTen.Text);
                cmd.Parameters.AddWithValue("@phai", txtPhai.Text);
                cmd.Parameters.AddWithValue("@ns", dtpNgaySinh.Value);
                cmd.Parameters.AddWithValue("@sdt", txtSDT.Text);
                cmd.Parameters.AddWithValue("@mst", txtMaSoThue.Text);
                cmd.Parameters.AddWithValue("@mavaitro", cboVaiTro.SelectedValue);
                cmd.Parameters.AddWithValue("@pb", cboPhongBan.SelectedValue);
                cmd.ExecuteNonQuery();
                // Sau khi cmd.ExecuteNonQuery() thành công
                GhiLichSu(this.maNV, "Trưởng phòng", "Sửa thông tin", "Đã cập nhật hồ sơ cho nhân viên: " + txtHoTen.Text);
                LoadDanhSachNhanVien();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                string sql = @"INSERT INTO NhanVien
                (HoTen, Phai, NgaySinh, SoDienThoai, MaSoThue, MaPhongBan, MaVaiTro)
                VALUES (@ten, @phai, @ngaysinh, @sdt, @mst, @pb, @mavaitro)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ten", txtHoTen.Text);
                cmd.Parameters.AddWithValue("@phai", txtPhai.Text);
                cmd.Parameters.AddWithValue("@ngaysinh", dtpNgaySinh.Value);
                cmd.Parameters.AddWithValue("@sdt", txtSDT.Text);
                cmd.Parameters.AddWithValue("@mst", txtMaSoThue.Text);
                cmd.Parameters.AddWithValue("@pb", cboPhongBan.SelectedValue);
                cmd.Parameters.AddWithValue("@mavaitro", cboVaiTro.SelectedValue);
                cmd.ExecuteNonQuery();
                GhiLichSu(this.maNV, "Trưởng phòng", "Thêm nhân viên", "Đã thêm nhân viên mới: " + txtHoTen.Text);
                LoadDanhSachNhanVien();
                MessageBox.Show("Thêm nhân viên thành công!");
            }

        }
        // Đặt hàm này nằm riêng biệt, ngang hàng với các hàm btnThem_Click, btnSua_Click
        void GhiLichSu(int maNV, string nguoi, string hanhDong, string chiTiet)
        {
            using (SqlConnection connLog = new SqlConnection(constr)) // Đổi tên thành connLog để tránh trùng
            {
                connLog.Open();
                string sqlLog = "INSERT INTO LichSuHoatDong (MaNV, TenDangNhap, HanhDong, ChiTiet) VALUES (@ma, @user, @hd, @ct)";
                SqlCommand cmdLog = new SqlCommand(sqlLog, connLog);
                cmdLog.Parameters.AddWithValue("@ma", maNV);
                cmdLog.Parameters.AddWithValue("@user", nguoi);
                cmdLog.Parameters.AddWithValue("@hd", hanhDong);
                cmdLog.Parameters.AddWithValue("@ct", chiTiet);
                cmdLog.ExecuteNonQuery();
            }
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.SelectedRows.Count == 0) return;

            // Lấy mã nhân viên cần xóa
            int ma = Convert.ToInt32(dgvNhanVien.SelectedRows[0].Cells["MaNV"].Value);

            // Hỏi xác nhận trước khi xóa (nên có để tránh bấm nhầm)
            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này và toàn bộ lịch sử dự án liên quan?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dr == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    // Sử dụng Transaction để đảm bảo nếu xóa bảng con lỗi thì bảng chính không bị xóa
                    SqlTransaction trans = conn.BeginTransaction();
                    try
                    {
                        // Bước 1: Xóa dữ liệu ở bảng trung gian NhanVien_DuAn trước
                        string sql1 = "DELETE FROM NhanVien_DuAn WHERE MaNV = @ma";
                        SqlCommand cmd1 = new SqlCommand(sql1, conn, trans);
                        cmd1.Parameters.AddWithValue("@ma", ma);
                        cmd1.ExecuteNonQuery();

                        // Bước 2: Bây giờ mới xóa ở bảng NhanVien
                        string sql2 = "DELETE FROM NhanVien WHERE MaNV = @ma";
                        SqlCommand cmd2 = new SqlCommand(sql2, conn, trans);
                        cmd2.Parameters.AddWithValue("@ma", ma);
                        cmd2.ExecuteNonQuery();

                        // Hoàn tất giao dịch
                        trans.Commit();

                        MessageBox.Show("Xóa nhân viên thành công!");
                        // Giả sử 'ma' là biến lưu mã nhân viên bạn vừa xóa
                        GhiLichSu(this.maNV, "Trưởng phòng", "Xóa nhân viên", "Đã xóa nhân viên có mã số: " + ma);
                        LoadDanhSachNhanVien();
                    }
                    catch (Exception ex)
                    {
                        // Nếu có lỗi, hoàn tác lại toàn bộ
                        trans.Rollback();
                        MessageBox.Show("Lỗi khi xóa: " + ex.Message);
                    }
                }
            }
        }

        private void grpThongTin_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void cboVaiTro_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void ClearFields()
        {
            txtHoTen.Clear();
            txtPhai.Clear();
            txtSDT.Clear();
            txtMaSoThue.Clear();
            dtpNgaySinh.Value = DateTime.Now; // Trả về ngày hiện tại

            // Đặt lại các ComboBox về mục đầu tiên (nếu có dữ liệu)
            if (cboPhongBan.Items.Count > 0) cboPhongBan.SelectedIndex = 0;
            if (cboVaiTro.Items.Count > 0) cboVaiTro.SelectedIndex = 0;

            // Trạng thái mặc định cho Lương và Phụ cấp theo yêu cầu của bạn
            txtLuong.Text = "null";
            txtPhuCap.Text = "null";

            // Đưa con trỏ chuột về ô Họ tên để nhập mới cho nhanh
            txtHoTen.Focus();
        }
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
