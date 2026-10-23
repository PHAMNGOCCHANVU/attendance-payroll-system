using System;

namespace cuoiky.Models
{
    public class HoSoNhanVien
    {
        public int MaHoSo { get; set; }
        public int? MaNV { get; set; }
        public DateTime? NgayTaoHoSo { get; set; }
        public string BangCap { get; set; }
        public string KinhNghiem { get; set; }
        public string GhiChu { get; set; }
    }
}