using System;
using System.Data;
using cuoiky.Repositories;

namespace cuoiky.Controllers
{
    public class GiamDocController
    {
        private GiamDocRepository _repo;

        public GiamDocController()
        {
            _repo = new GiamDocRepository();
        }

        // 1. Kiểm tra Role và Validate dữ liệu cập nhật lương
        public string CapNhatLuong(string currentRole, int maNVChinhSua, int targetMaNV, string luongStr, string phuCapStr, string thuongStr, string khauTruStr)
        {
            // BẢO MẬT: Kiểm tra cứng Role ở Controller, chặn đứng nếu không phải Giám đốc
            if (currentRole != "Giám đốc")
            {
                return "LỖI BẢO MẬT: Bạn không có quyền truy cập chức năng này!";
            }

            // Kiểm tra dữ liệu đầu vào (phải là số, không được âm)
            if (!decimal.TryParse(luongStr, out decimal luong) || luong < 0) return "Lương cơ bản không hợp lệ!";
            if (!decimal.TryParse(phuCapStr, out decimal phuCap) || phuCap < 0) return "Phụ cấp không hợp lệ!";

            // Xử lý Thưởng và Khấu trừ (nếu để trống thì gán mặc định là 0)
            decimal thuong = string.IsNullOrWhiteSpace(thuongStr) ? 0 : (decimal.TryParse(thuongStr, out decimal t) ? t : -1);
            decimal khauTru = string.IsNullOrWhiteSpace(khauTruStr) ? 0 : (decimal.TryParse(khauTruStr, out decimal k) ? k : -1);

            if (thuong < 0) return "Số tiền thưởng không hợp lệ!";
            if (khauTru < 0) return "Số tiền khấu trừ không hợp lệ!";

            // 3. Gọi Repository thực thi mã hóa xuống CSDL
            bool success = _repo.CapNhatLuong(targetMaNV, luong, phuCap, thuong, khauTru);

            if (success)
            {
                // Ghi lại lịch sử (Ai làm? Sửa cho ai? Chi tiết?)
                string chiTiet = $"Cập nhật lương cho NV {targetMaNV}: L={luong}, PC={phuCap}, T={thuong}, KT={khauTru}";
                _repo.GhiLichSu(maNVChinhSua, "Giám đốc", "Cập nhật lương", chiTiet);
                return "OK";
            }
            return "Có lỗi xảy ra khi cập nhật CSDL.";
        }

        // Các hàm chuyển tiếp lấy dữ liệu
        public DataTable LayDanhSachNhanVien(string currentRole)
        {
            if (currentRole != "Giám đốc") return null;
            return _repo.XemDanhSachNhanVien_KemLuong();
        }

        public DataTable LayVaiTro() => _repo.LayDuLieu("SP_LayDanhSachVaiTro");
        public DataTable LayPhongBan() => _repo.LayDuLieu("SP_LayDanhSachPhongBan");
        public DataTable LayLichSu(string keyword) => _repo.LayDuLieu("SP_LayLichSuHoatDong", "@Keyword", keyword);
        public string LayHinhAnh(int maNV) => _repo.LayHinhAnh(maNV);
    }
}