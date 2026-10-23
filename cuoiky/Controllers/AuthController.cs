using System;
using cuoiky.Repositories;
using cuoiky.Services;

namespace cuoiky.Controllers
{
    public class AuthController
    {
        private readonly AuthService _authService;

        public AuthController()
        {
            _authService = new AuthService(new AuthRepository());
        }

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // Hàm Login thực hiện Validation và gọi sang Repository
        public bool Login(string username, string password, out int maNV, out string tenVaiTro, out string tenPhongBan)
        {
            return _authService.Login(username, password, out maNV, out tenVaiTro, out tenPhongBan);
        }
    }
}