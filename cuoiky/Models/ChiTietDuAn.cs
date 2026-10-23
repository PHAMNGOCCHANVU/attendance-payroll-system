using System;

namespace cuoiky.Models
{
    public class ChiTietDuAn
    {
        public int MaChiTiet { get; set; }
        public int? MaDuAn { get; set; }
        public string NoiDung { get; set; }
        public string TrangThai { get; set; }
        public DateTime? NgayCapNhat { get; set; }
    }
}