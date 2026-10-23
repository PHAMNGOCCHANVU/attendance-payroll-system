namespace cuoiky.Repositories
{
    public interface IAuthRepository
    {
        bool KiemTraDangNhap(string username, string password, out int maNV, out string tenVaiTro, out string tenPhongBan);
    }
}
