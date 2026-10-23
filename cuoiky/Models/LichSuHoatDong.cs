using System;

namespace cuoiky.Models
{
    public class LichSuHoatDong
    {
        public int ID { get; set; }
        public int? MaNV { get; set; }
        public string TenDangNhap { get; set; }
        public string HanhDong { get; set; }
        public string ChiTiet { get; set; }
        public DateTime? ThoiGian { get; set; }
    }
}