using System;
using System.Data;
using cuoiky.Repositories;

namespace cuoiky.Controllers
{
    public class NhanVienController
    {
        private NhanVienRepository _repo;

        public NhanVienController()
        {
            _repo = new NhanVienRepository();
        }

        // Validate đầu vào và kiểm tra Role cứng trước khi gọi DB
        public DataTable LayDanhSachNhanVien(int maNV, string currentRole)
        {
            if (currentRole != "Nhân viên")
            {
                return null; // Từ chối truy cập nếu không phải Nhân viên
            }
            if (maNV <= 0) return null;

            return _repo.LayDanhSachNhanVienCungPhong(maNV);
        }

        public string LayHinhAnh(int maNV)
        {
            return _repo.LayHinhAnh(maNV);
        }
    }
}