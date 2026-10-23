using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cuoiky.Models
{
    public class Luong
    {
        public int MaLuong { get; set; }
        public int? MaNV { get; set; }
        public int? Thang { get; set; }
        public int? Nam { get; set; }
        public byte[] LuongCoBan_Encrypted { get; set; }
        public byte[] PhuCap_Encrypted { get; set; }
        public byte[] Thuong_Encrypted { get; set; }
        public byte[] KhauTru_Encrypted { get; set; }
        public byte[] LuongThucLinh_Encrypted { get; set; }
    }
}