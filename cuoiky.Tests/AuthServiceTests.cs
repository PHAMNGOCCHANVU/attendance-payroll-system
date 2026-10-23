using cuoiky.Repositories;
using cuoiky.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace cuoiky.Tests
{
    [TestClass]
    public class AuthServiceTests
    {
        [TestMethod]
        public void Login_WithEmptyCredentials_ReturnsFalseWithoutCallingRepository()
        {
            var repository = new FakeAuthRepository();
            var service = new AuthService(repository);

            var result = service.Login("", "", out var maNV, out var role, out var department);

            Assert.IsFalse(result);
            Assert.AreEqual(0, maNV);
            Assert.AreEqual(string.Empty, role);
            Assert.AreEqual(string.Empty, department);
            Assert.IsFalse(repository.WasCalled);
        }

        [TestMethod]
        public void Login_WithValidCredentials_ReturnsRepositoryResult()
        {
            var repository = new FakeAuthRepository
            {
                Result = true,
                MaNV = 12,
                Role = "Nhân viên",
                Department = "Phòng Nhân Sự"
            };
            var service = new AuthService(repository);

            var result = service.Login("user", "password", out var maNV, out var role, out var department);

            Assert.IsTrue(result);
            Assert.IsTrue(repository.WasCalled);
            Assert.AreEqual(12, maNV);
            Assert.AreEqual("Nhân viên", role);
            Assert.AreEqual("Phòng Nhân Sự", department);
        }

        [TestMethod]
        public void Login_WithInvalidCredentials_ReturnsFalseAndRepositoryResult()
        {
            var repository = new FakeAuthRepository
            {
                Result = false
            };
            var service = new AuthService(repository);

            var result = service.Login("user", "wrong-password", out _, out _, out _);

            Assert.IsFalse(result);
            Assert.IsTrue(repository.WasCalled);
        }

        private sealed class FakeAuthRepository : IAuthRepository
        {
            public bool Result { get; set; }
            public int MaNV { get; set; }
            public string Role { get; set; } = string.Empty;
            public string Department { get; set; } = string.Empty;
            public bool WasCalled { get; private set; }

            public bool KiemTraDangNhap(string username, string password, out int maNV, out string tenVaiTro, out string tenPhongBan)
            {
                WasCalled = true;
                maNV = MaNV;
                tenVaiTro = Role;
                tenPhongBan = Department;
                return Result;
            }
        }
    }
}
