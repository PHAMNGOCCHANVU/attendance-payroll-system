using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cuoiky.Models
{
    public class NhanVien
    {
        public int MaNV { get; set; }
        public string HoTen { get; set; }
        public string Phai { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string SoDienThoai { get; set; }
        public string MaSoThue { get; set; }
        public int? MaPhongBan { get; set; }
        public int? MaVaiTro { get; set; }
        public string HinhAnh { get; set; }

    }
}