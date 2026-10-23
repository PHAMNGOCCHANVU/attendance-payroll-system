using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace cuoiky.Repositories
{
    public class GiamDocRepository
    {
        private readonly string constr = ConfigurationManager.ConnectionStrings["connectStr"].ConnectionString;

        public DataTable XemDanhSachNhanVien_KemLuong()
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GiamDoc_XemDanhSachNhanVien", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public bool CapNhatLuong(int maNV, decimal luong, decimal phuCap, decimal thuong, decimal khauTru)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SP_GiamDoc_CapNhatLuong", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    cmd.Parameters.AddWithValue("@LuongCoBan", luong);
                    cmd.Parameters.AddWithValue("@PhuCap", phuCap);
                    cmd.Parameters.AddWithValue("@Thuong", thuong);
                    cmd.Parameters.AddWithValue("@KhauTru", khauTru);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

       
        public DataTable LayDuLieu(string tenSP, string paramName = null, object paramValue = null)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(tenSP, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (paramName != null) cmd.Parameters.AddWithValue(paramName, paramValue);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public string LayHinhAnh(int maNV)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SP_LayHinhAnhNhanVien", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    object result = cmd.ExecuteScalar();
                    return result != DBNull.Value ? result.ToString() : null;
                }
            }
        }

        public void GhiLichSu(int maNV, string user, string hanhDong, string chiTiet)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SP_ThemLichSuHoatDong", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    cmd.Parameters.AddWithValue("@TenDangNhap", user);
                    cmd.Parameters.AddWithValue("@HanhDong", hanhDong);
                    cmd.Parameters.AddWithValue("@ChiTiet", chiTiet);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}