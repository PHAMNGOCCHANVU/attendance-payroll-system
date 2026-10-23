using System;

namespace cuoiky.Models
{
    public class ChamCong
    {
        public int MaChamCong { get; set; }
        public int? MaNV { get; set; }
        public DateTime? NgayLam { get; set; }
        public TimeSpan? GioVao { get; set; }
        public TimeSpan? GioRa { get; set; }
        public string GhiChu { get; set; }
    }
}