using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace cuoiky.Repositories
{
    public class TruongPhongRepository
    {
        private readonly string constr = ConfigurationManager.ConnectionStrings["connectStr"].ConnectionString;

        public DataTable LayDanhSachNhanVienPhongBan(int maNV)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SP_TruongPhong_XemDanhSachNhanVien", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaNVDangNhap", maNV);
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
    }
}