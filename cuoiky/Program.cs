using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using cuoiky.Controllers;
using cuoiky.Repositories;
using cuoiky.Services;

namespace cuoiky
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            IAuthRepository authRepository = new AuthRepository();
            AuthService authService = new AuthService(authRepository);
            AuthController authController = new AuthController(authService);

            Application.Run(new FormLogin(authController));
        }
    }
}
