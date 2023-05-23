using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace caja
{
    internal static class Program
    {
        public static string pathUpdateCaja = @"C:\SIWEB\caja\updateCaja.jar";
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (validarMultiplesInstancias("caja") > 1)
            {
                Application.Run(new contenedor("El aplicativo ya se encuentra en ejecución, \nfavor de esperar."));    
            }
            else if (validarMultiplesInstancias("java") >= 1)
            {
                Application.Run(new contenedor("El aplicativo ya se encuentra en ejecución."));
            }
            else
            {
                if (File.Exists(pathUpdateCaja))
                {
                    ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe", "/c " + $"javaw.exe -jar {pathUpdateCaja}")
                    {
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    Process process = new Process
                    {
                        StartInfo = procStartInfo
                    };
                    process.Start();

                    string st;
                    string pathCaja = "";
                    while ((st = process.StandardOutput.ReadLine()) != null)
                    {
                        if (st.Equals("pathCaja"))
                        {
                            pathCaja = process.StandardOutput.ReadLine();
                            break;
                        }
                    }
                    if (pathCaja.Equals("") || pathCaja == null || pathCaja == "false")
                    {
                        Application.Run(new contenedor("No se encontró el archivo de inicialización de la aplicación, \nfavor de validar con soporte técnico."));
                        Environment.Exit(1);
                    }
                    else
                    {
                        var mre = new ManualResetEvent(false);
                        var asyncLogic = new AsyncLogic();
                        asyncLogic.Completed += (s, e) => mre.Set();
                        asyncLogic.Start("java.exe", $"-jar {pathCaja}");
                        mre.WaitOne();
                    }
                }
                else
                {
                    Application.Run(new contenedor("No se encontró el archivo de actualización de la aplicación, \nfavor de validar con soporte técnico."));
                    Environment.Exit(1);
                }
            }
        }
        private static int validarMultiplesInstancias(String processName)
        {
            Process[] processes = null;
            
            if (processName == "caja")
            {
                processes = Process.GetProcessesByName(processName);
            }
            else if (processName == "java")
            {
                processes = Process.GetProcessesByName(processName);
            }
            return processes.Length;
        }
        class AsyncLogic
        {
            public EventHandler Completed = delegate { };

            IEnumerable WorkAsync(Action nextStep, String command, String args)
            {
                using (var timer = new System.Threading.Timer(_ => nextStep()))
                {
                    timer.Change(0, 500);

                    ProcessStartInfo procesoAplicativo = new ProcessStartInfo(command, args)
                    {
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    Process ejecutarAplicativo = new Process
                    {
                        StartInfo = procesoAplicativo
                    };

                    ejecutarAplicativo.Start();
                    yield return Type.Missing;
                }
                    this.Completed(this, EventArgs.Empty);
            }
            public void Start(String command, String args)
            {
                IEnumerator enumerator = null;
                Action nextStep = () => enumerator.MoveNext();
                enumerator = WorkAsync(nextStep, command, args).GetEnumerator();
                nextStep();
            }
        }
    }
}
