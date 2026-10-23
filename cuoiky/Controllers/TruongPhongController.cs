using System;
using System.Data;
using cuoiky.Repositories;

namespace cuoiky.Controllers
{
    public class TruongPhongController
    {
        private TruongPhongRepository _repo;

        public TruongPhongController()
        {
            _repo = new TruongPhongRepository();
        }

        public DataTable LayDanhSachNhanVien(int maNV, string currentRole)
        {
            // Validate cứng Role, chặn việc lạm quyền từ các form khác
            if (currentRole != "Trưởng phòng")
            {
                return null;
            }
            if (maNV <= 0) return null;

            return _repo.LayDanhSachNhanVienPhongBan(maNV);
        }

        public string LayHinhAnh(int maNV)
        {
            if (maNV <= 0) return null;
            return _repo.LayHinhAnh(maNV);
        }
    }
}