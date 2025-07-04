﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Proyecto_RedVirtualDinamica_Marcelo
{
    public static class Interfaz
    {
        #region Metodos
        public static string Menu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Borde();
            Console.ForegroundColor = ConsoleColor.Red;

            Titulo();
            Console.ResetColor();
            XY(15, 11); Console.WriteLine("1. Configurar red de comunicación");
            XY(15, 12); Console.WriteLine("2. Crear un mensaje");
            XY(15, 13); Console.WriteLine("3. Enviar un mensaje");
            XY(15, 14); Console.WriteLine("4. Visualizar la traza de un paquete");
            XY(15, 15); Console.WriteLine("5. Mostrar el status de la red");
            XY(15, 16); Console.WriteLine("6. Mostrar el status de una subred");
            XY(15, 17); Console.WriteLine("7. Mostrar el status de un equipo");
            XY(15, 18); Console.WriteLine("8. Eliminar un paquete de la cola");
            XY(15, 19); Console.WriteLine("9. Visualizar mensajes recibidos");
            XY(15, 20); Console.WriteLine("10. Consultar información de un paquete");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            XY(65, 11); Console.WriteLine("FUNCIONES EXTRA:"); Console.ResetColor();
            XY(65, 12); Console.WriteLine("11. Vaciar cola de un dispositivo");
            XY(65, 13); Console.WriteLine("12. Eliminar subred específica");
            XY(65, 14); Console.WriteLine("13. Eliminar toda la red");
            Console.ForegroundColor = ConsoleColor.Red;
            XY(15, 21); Console.WriteLine("0. Salir");
            Console.ForegroundColor = ConsoleColor.Cyan;
            XY(8, 23); Console.Write("-Opción: ");

            string opcion = Console.ReadLine().Trim();
            return opcion;
        }

        public static void Borde()
        {
            int width = Console.WindowWidth - 1;
            int height = Console.WindowHeight;

            int original_left = Console.CursorLeft;
            int original_top = Console.CursorTop;

            Console.SetCursorPosition(0, 0);
            Console.Write("╔");

            Console.SetCursorPosition(width - 1, 0);
            Console.Write("╗");

            Console.SetCursorPosition(0, height - 1);
            Console.Write("╚");

            Console.SetCursorPosition(width - 1, height - 1);
            Console.Write("╝");

            for (int x = 1; x < width - 1; x++)
            {
                Console.SetCursorPosition(x, 0);
                Console.Write("═");

                Console.SetCursorPosition(x, height - 1);
                Console.Write("═");
            }

            for (int y = 1; y < height - 1; y++)
            {
                Console.SetCursorPosition(0, y);
                Console.Write("║");

                Console.SetCursorPosition(width - 1, y);
                Console.Write("║");
            }

            Console.SetCursorPosition(original_left, original_top);
        }

        public static void Titulo()
        {
            XY(16, 4); Console.WriteLine("██████╗ ███████╗██████╗      ██╗   ██╗██╗██████╗ ████████╗██╗   ██╗ █████╗ ██╗     ");
            XY(16, 5); Console.WriteLine("██╔══██╗██╔════╝██╔══██╗     ██║   ██║██║██╔══██╗╚══██╔══╝██║   ██║██╔══██╗██║     ");
            XY(16, 6); Console.WriteLine("██████╔╝█████╗  ██║  ██║     ██║   ██║██║██████╔╝   ██║   ██║   ██║███████║██║     ");
            XY(16, 7); Console.WriteLine("██╔══██╗██╔══╝  ██║  ██║     ╚██╗ ██╔╝██║██╔══██╗   ██║   ██║   ██║██╔══██║██║     ");
            XY(16, 8); Console.WriteLine("██║  ██║███████╗██████╔╝      ╚████╔╝ ██║██║  ██║   ██║   ╚██████╔╝██║  ██║███████╗");
            XY(16, 9); Console.WriteLine("╚═╝  ╚═╝╚══════╝╚═════╝        ╚═══╝  ╚═╝╚═╝  ╚═╝   ╚═╝    ╚═════╝ ╚═╝  ╚═╝╚══════╝");
        }

        public static void Adios()
        {
            int x = 15, y = 10;

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Thread.Sleep(50); XY(x, y++); Console.WriteLine("    _       _ _               ____  ");
            Thread.Sleep(50); XY(x, y++); Console.WriteLine("   / \\   __| (_) ___  ___   _|  _ \\ ");
            Thread.Sleep(50); XY(x, y++); Console.WriteLine("  / _ \\ / _` | |/ _ \\ __| (_) | | |");
            Thread.Sleep(50); XY(x, y++); Console.WriteLine(" / ___ \\ (_| | | (_) \\__\\  _| |_| |");
            Thread.Sleep(50); XY(x, y++); Console.WriteLine("/_/     \\__,_|_||___/|___/ (_)____/ ");
            Thread.Sleep(50); XY(x, y++); Console.ResetColor();
        }

        public static void ImprimirLogoUSM()
        {
            int x = 60, y = 7;
            Console.ForegroundColor = ConsoleColor.Blue;
            Borde();

            Thread.Sleep(50); XY(x, y); Console.WriteLine("         .+%%%%%%+.=#%%%%#:+%%%%%%%%%%%-        ");
            Thread.Sleep(50); XY(x, y++); Console.WriteLine("      .+%%%%%%*.-#%%%%%:=%%%%%%%%%%%%#-.      ");
            Thread.Sleep(50); XY(x, y++); Console.WriteLine("     .+%%%%%%%::*%%%%%--%%%%%%%%%%%%%%#-.     ");
            Thread.Sleep(50); XY(x, y++); Console.WriteLine("    .+%%%%%%%-.*%%%%%=:%%%%#==++++++++++:.    ");
            Thread.Sleep(50); XY(x, y++); Console.WriteLine("   .+%%%%%%%+.+%%%%%+:#%%%%==%%%%%%%%%%%#-.   ");
            Thread.Sleep(50); XY(x, y++); Console.WriteLine("  .=%%%%%%%%++%%%%%*:*%%%%+-%%%%%%%%%%%%%%-.  ");
            Thread.Sleep(50); XY(x, y++); Console.WriteLine(" .:*%%%%%%%%%%%%%%#:+%%%%*-#%%%%%%%%%%%%%%#:. ");
            Thread.Sleep(50); XY(x, y++); Console.WriteLine("  .:*%%%%%%%%%%%%#-+%%%%#:*%%%%%%%%%%%%%%%=.  ");
            Thread.Sleep(50); XY(x, y++); Console.WriteLine("    .+%%%%%%%%%%#=-#%%%%-+%%*+%#=*%%%%%%%=.   ");
            Thread.Sleep(50); XY(x, y++); Console.WriteLine("     .:==========+#%%%%-=%%*-*%=:#%%%%%%+.    ");
            Thread.Sleep(50); XY(x, y++); Console.WriteLine("      :*%%%%%%%%%%%%%%=:%%#-+%*:*%%%%%%+.     ");
            Thread.Sleep(50); XY(x, y++); Console.WriteLine("       .+%%%%%%%%%%%%*:%%%=-%%--%%%%%%*.      ");
            Thread.Sleep(50); XY(x, y++); Console.WriteLine("        .=%%%%%%%%%%#:#%%+:#%+:*%%%%%*.       ");

            Console.ResetColor();
            XY(x, y++); Console.ReadKey();
            Console.Clear();
        }

        public static void XY(int x, int y)
        {
            Console.SetCursorPosition(x, y);
        }

        public static void Continuar()
        {
            Console.Write("\n\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        public static void Error(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"ERROR: {mensaje}");
            Console.ResetColor();
        }
        #endregion
    }
}
