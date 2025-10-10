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
            if (!EsAdministrador())
            {
                try
                {
                    var exePath = Process.GetCurrentProcess().MainModule.FileName;
                    ProcessStartInfo psi = new ProcessStartInfo(exePath)
                    {
                        UseShellExecute = true,
                        Verb = "runas" // Ejecutar como administrador
                    };

                    Process.Start(psi);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Se requieren permisos de administrador para ejecutar esta aplicación.\n\n" + ex.Message,
                        "Permisos requeridos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Salir del proceso actual (no administrador)
                return;
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
