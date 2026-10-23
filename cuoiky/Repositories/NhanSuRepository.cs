using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace cuoiky.Repositories
{
    public class NhanSuRepository
    {
        private readonly string constr = ConfigurationManager.ConnectionStrings["connectStr"].ConnectionString;

        public DataTable LayDanhSachNV(int maNV)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SP_NhanSu_LayDanhSachNV", conn))
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

        public bool ThemNhanVien(dynamic nv, int maNVThucHien)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SP_NhanSu_ThemNV", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@HoTen", nv.HoTen);
                    cmd.Parameters.AddWithValue("@Phai", nv.Phai);
                    cmd.Parameters.AddWithValue("@NgaySinh", nv.NgaySinh);
                    cmd.Parameters.AddWithValue("@SDT", nv.SDT);
                    cmd.Parameters.AddWithValue("@MST", nv.MST);
                    cmd.Parameters.AddWithValue("@MaPB", nv.MaPB);
                    cmd.Parameters.AddWithValue("@MaVT", nv.MaVT);
                    cmd.Parameters.AddWithValue("@HinhAnh", nv.HinhAnh);
                    cmd.Parameters.AddWithValue("@TenDN", nv.TenDN);
                    cmd.Parameters.AddWithValue("@MatKhau", nv.MatKhau);
                    cmd.Parameters.AddWithValue("@MaNVThucHien", maNVThucHien);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool CapNhatNhanVien(dynamic nv, int maNVThucHien)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SP_NhanSu_CapNhatNV", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaNV", nv.MaNV);
                    cmd.Parameters.AddWithValue("@HoTen", nv.HoTen);
                    cmd.Parameters.AddWithValue("@Phai", nv.Phai);
                    cmd.Parameters.AddWithValue("@NgaySinh", nv.NgaySinh);
                    cmd.Parameters.AddWithValue("@SDT", nv.SDT);
                    cmd.Parameters.AddWithValue("@MST", nv.MST);
                    cmd.Parameters.AddWithValue("@MaPB", nv.MaPB);
                    cmd.Parameters.AddWithValue("@MaVT", nv.MaVT);
                    cmd.Parameters.AddWithValue("@HinhAnh", nv.HinhAnh);
                    cmd.Parameters.AddWithValue("@TenDN", nv.TenDN);
                    cmd.Parameters.AddWithValue("@MatKhau", nv.MatKhau ?? "");
                    cmd.Parameters.AddWithValue("@MaNVThucHien", maNVThucHien);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public DataTable LayDanhSachPhongBan()
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SP_LayDanhSachPhongBan", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable LayDanhSachVaiTro()
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SP_LayDanhSachVaiTro", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }
    }
}