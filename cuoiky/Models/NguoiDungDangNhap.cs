using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cuoiky.Models
{
    public class NguoiDungDangNhap
    {
        public string TenDangNhap { get; set; }
        public byte[] MatKhauHash { get; set; }

        public int? MaNV { get; set; }
        public int? MaVaiTro { get; set; }
    }
}