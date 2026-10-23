using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace cuoiky.Repositories
{
    public class TaiVuRepository
    {
        private readonly string constr = ConfigurationManager.ConnectionStrings["connectStr"].ConnectionString;

        public DataTable LayDanhSachNhanVien(int maNV)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SP_TaiVu_XemDanhSachNhanVien", conn))
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
    }
}