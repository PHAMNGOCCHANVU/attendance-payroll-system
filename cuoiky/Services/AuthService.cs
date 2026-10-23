using cuoiky.Repositories;

namespace cuoiky.Services
{
    public class AuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public bool Login(string username, string password, out int maNV, out string tenVaiTro, out string tenPhongBan)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                maNV = 0;
                tenVaiTro = "";
                tenPhongBan = "";
                return false;
            }

            return _authRepository.KiemTraDangNhap(username, password, out maNV, out tenVaiTro, out tenPhongBan);
        }
    }
}
