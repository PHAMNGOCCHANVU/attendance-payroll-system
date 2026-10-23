using System;
using System.Data;
using cuoiky.Repositories;

namespace cuoiky.Controllers
{
    public class TruongPhongNhanSuController
    {
        private TruongPhongNhanSuRepository _repo = new TruongPhongNhanSuRepository();
        private NhanSuRepository _nhanSuRepo = new NhanSuRepository();

        public DataTable LayDanhSach(string role, int maNV)
        {
            if (role != "Trưởng phòng nhân sự") return null;
            return _repo.LayDanhSachNV(maNV);
        }

        public DataTable XemLichSu(string role, string keyword)
        {
            if (role != "Trưởng phòng nhân sự") return null;
            return _repo.XemLichSu(keyword);
        }

        public string SuaNV(string role, dynamic nv, int maNVThucHien)
        {
            if (role != "Trưởng phòng nhân sự") return "Lỗi: Sai quyền hạn!";

            // VALIDATE NGHIÊM NGẶT THEO YÊU CẦU
            if (string.IsNullOrEmpty(nv.HoTen)) return "Họ tên không được để trống!";
            if (nv.MaPB == 0) return "Phòng ban không được để trống!";
            if (nv.MaVT == 0) return "Vai trò không được để trống!";
            if (string.IsNullOrEmpty(nv.TenDN)) return "Tên đăng nhập không được để trống!";
            if (!string.IsNullOrEmpty(nv.MatKhau) && nv.MatKhau.Length < 6) return "Mật khẩu mới phải từ 6 ký tự!";

            try
            {
                _nhanSuRepo.CapNhatNhanVien(nv, maNVThucHien);
                return "OK";
            }
            catch (Exception ex) { return ex.Message; }
        }
    }
}