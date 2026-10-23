using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace cuoiky.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        // Lấy chuỗi kết nối từ file App.config
        private readonly string constr = ConfigurationManager.ConnectionStrings["connectStr"].ConnectionString;

        // Hàm gọi Procedure kiểm tra đăng nhập. Sử dụng từ khóa 'out' để trả về nhiều kết quả.
        public bool KiemTraDangNhap(string username, string password, out int maNV, out string tenVaiTro, out string tenPhongBan)
        {
            maNV = 0;
            tenVaiTro = "";
            tenPhongBan = "";

            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                // Khởi tạo command gọi Stored Procedure
                using (SqlCommand cmd = new SqlCommand("SP_KiemTraDangNhap", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure; 
                    cmd.Parameters.AddWithValue("@TenDangNhap", username);
                    cmd.Parameters.AddWithValue("@MatKhau", password); // C# chỉ truyền pass trần, SQL tự băm

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        // Nếu Read() thành công nghĩa là đăng nhập đúng
                        if (dr.Read())
                        {
                            maNV = Convert.ToInt32(dr["MaNV"]);
                            tenVaiTro = dr["TenVaiTro"].ToString();
                            tenPhongBan = dr["TenPhongBan"].ToString();
                            return true;
                        }
                    }
                }
            }
            return false; // Trả về false nếu sai tài khoản/mật khẩu
        }
    }
}