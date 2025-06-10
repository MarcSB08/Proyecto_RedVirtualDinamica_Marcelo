using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_RedVirtualDinamica_Marcelo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Red red = new Red();

            string opcion;
            do
            {
                opcion = Interfaz.Menu();
                switch (opcion)
                {
                    case "1":
                        red.ConfigurarRed();
                        break;
                    case "2":
                        red.CrearMensaje();
                        break;
                    case "3":
                        red.Enviar();
                        break;
                    case "4":
                        red.VisualizarTrazaPaquete();
                        break;
                    case "5":
                        red.MostrarStatusRed();
                        break;
                    case "6":
                        red.MostrarStatusSubred();
                        break;
                    case "7":
                        red.MostrarStatusEquipo();
                        break;
                    case "8":
                        red.EliminarPaquete();
                        break;
                    case "9":
                        red.VisualizarMensajesRecibidos();
                        break;
                    case "10":
                        red.ConsultarInformacionPaquete();
                        break;
                    case "11":
                        red.VaciarColaDispositivo();
                        break;
                    case "12":
                        red.EliminarSubredEspecifica();
                        break;
                    case "13":
                        red.EliminarTodaLaRed();
                        break;
                    case "0":
                        Interfaz.Adios();
                        Interfaz.ImprimirLogoUSM();
                        break;
                    default:
                        Interfaz.XY(17, 23); Interfaz.Error("Opción no válida. Intente nuevamente.");
                        Console.ReadKey();
                        break;
                }
            } while (opcion != "0");
        }
    }
}
