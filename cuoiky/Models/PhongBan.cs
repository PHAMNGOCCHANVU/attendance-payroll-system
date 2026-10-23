namespace cuoiky.Models
{
    public class PhongBan
    {
        public int MaPhongBan { get; set; }
        public string TenPhongBan { get; set; }
        public int? TruongPhong { get; set; } // int? vì có thể NULL
    }
}