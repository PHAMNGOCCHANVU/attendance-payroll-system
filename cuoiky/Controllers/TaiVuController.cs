using System;
using System.Data;
using cuoiky.Repositories;

namespace cuoiky.Controllers
{
    public class TaiVuController
    {
        private TaiVuRepository _repo = new TaiVuRepository();

        public DataTable LayDanhSach(string role, int maNV)
        {
           
            if (role != "Nhân viên phòng tài vụ")
            {
                return null;
            }
            if (maNV <= 0)
            {
                return null;
            }

            return _repo.LayDanhSachNhanVien(maNV);
        }
    }
}