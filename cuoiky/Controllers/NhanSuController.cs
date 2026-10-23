using System;
using System.Data;
using System.IO;
using cuoiky.Repositories;

namespace cuoiky.Controllers
{
    public class NhanSuController
    {
        private NhanSuRepository _repo = new NhanSuRepository();

        public DataTable LayDanhSach(string role, int maNV)
        {
            if (role != "Nhân viên phòng nhân sự") return null;
            return _repo.LayDanhSachNV(maNV);
        }

        public string ThemNV(string role, dynamic nv, int maNVThucHien)
        {
            if (role != "Nhân viên phòng nhân sự" && role != "Trưởng phòng nhân sự")
            {
                return "Bạn không có quyền!";
            }

            // Validate dữ liệu trống
            if (string.IsNullOrEmpty(nv.HoTen)) return "Họ tên không được để trống!";
            if (nv.MaPB == 0) return "Phòng ban không được để trống!";
            if (nv.MaVT == 0) return "Vai trò không được để trống!";
            if (string.IsNullOrEmpty(nv.TenDN)) return "Tên đăng nhập không được để trống!";
            if (nv.MatKhau.Length < 6) return "Mật khẩu phải từ 6 ký tự!";

            try
            {
                _repo.ThemNhanVien(nv, maNVThucHien);
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string SuaNV(string role, dynamic nv, int maNVThucHien)
        {
            if (role != "Nhân viên phòng nhân sự") return "Lỗi: Sai quyền hạn!";

            if (string.IsNullOrEmpty(nv.HoTen)) return "Họ tên không được để trống!";
            if (nv.MaPB == 0) return "Phòng ban không được để trống!";
            if (nv.MaVT == 0) return "Vai trò không được để trống!";
            if (string.IsNullOrEmpty(nv.TenDN)) return "Tên đăng nhập không được để trống!";
            if (!string.IsNullOrEmpty(nv.MatKhau) && nv.MatKhau.Length < 6) return "Mật khẩu mới phải từ 6 ký tự!";

            try
            {
                _repo.CapNhatNhanVien(nv, maNVThucHien);
                return "OK";
            }
            catch (Exception ex) { return ex.Message; }
        }

        public string CopyAnhVaoThuMuc(string pathGoc)
        {
            try
            {
                string tenFile = Path.GetFileName(pathGoc);
                // Đường dẫn tương ứng cuoiky\cuoiky\image\
                string folderDich = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\image");

                if (!Directory.Exists(folderDich)) Directory.CreateDirectory(folderDich);

                string pathDich = Path.Combine(folderDich, tenFile);
                File.Copy(pathGoc, pathDich, true);
                return tenFile; // Trả về tên file để lưu vào DB
            }
            catch { return "default.png"; }
        }

        public DataTable LayDanhSachPhongBan() { return _repo.LayDanhSachPhongBan(); }
        public DataTable LayDanhSachVaiTro() { return _repo.LayDanhSachVaiTro(); }
    }
}