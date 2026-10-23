using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace cuoiky.Repositories
{
    public class TruongPhongNhanSuRepository
    {
        private readonly string constr = ConfigurationManager.ConnectionStrings["connectStr"].ConnectionString;

        public DataTable LayDanhSachNV(int maNV)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SP_TruongPhongNhanSu_LayDanhSachNV", conn))
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

        public DataTable XemLichSu(string keyword)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SP_TruongPhongNhanSu_XemLichSu", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Keyword", keyword);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }
    }
}