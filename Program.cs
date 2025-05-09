using NEWCODES.Infraestructura;
using System.Diagnostics;
using System.Security.Principal;

namespace NEWCODES
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Verificar si estamos en modo administrador
            if (!EsAdministrador())
            {
                try
                {
                    // Reiniciar la aplicación con privilegios elevados
                    var psi = new ProcessStartInfo
                    {
                        FileName = Application.ExecutablePath,
                        UseShellExecute = true,
                        Verb = "runas" // Esto lanza el UAC
                    };
                    Process.Start(psi);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Se requieren permisos de administrador.\n" + ex.Message);
                }

                return; // Salir del proceso actual
            }

            // Aquí continúa la app con privilegios elevados
            using (var context = new EevntoContext())
            {
                context.Database.EnsureCreated();
            }

            ApplicationConfiguration.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        static bool EsAdministrador()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }
    }
}
