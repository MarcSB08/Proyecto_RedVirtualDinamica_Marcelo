using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_RedVirtualDinamica_Marcelo
{
    public class Red
    {
        public List<Subred> Subredes { get; set; }
        public List<Mensaje> Mensajes { get; set; }

        public Red()
        {
            Subredes = new List<Subred>();
            Mensajes = new List<Mensaje>();
        }

        public void ConfigurarRed()  // Opción 1
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("=== CONFIGURACIÓN DE LA RED ===");
            Console.ResetColor();

            int cantidadSubredes = 0;
            string opcion = "";
            bool entradaInvalida = false;

            // Verificar si ya existe una red configurada
            if (Subredes.Count != 0)
            {
                do
                {
                    Console.WriteLine("\nYa hay una red existente, desea eliminarla y crear una nueva?");
                    Console.Write("-Opción (Si/No): ");

                    opcion = Console.ReadLine().Trim().ToLower();
                    if (opcion == "si")
                    {
                        Subredes.Clear();
                        Mensajes.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nRed eliminada exitosamente!");
                        Console.ResetColor();
                    }
                    else if (opcion == "no")
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nRed no eliminada, volviendo al menú principal...");
                        Console.ResetColor();
                        Interfaz.Continuar();
                        return;
                    }
                    else
                    {
                        Interfaz.Error("Opción no válida");
                    }
                } while (opcion != "si" && opcion != "no");
            }

            // Solicitar cantidad de subredes
            do
            {
                try
                {
                    Console.Write("\nIngrese cantidad de subredes (mínimo 1): ");
                    cantidadSubredes = int.Parse(Console.ReadLine());

                    if (cantidadSubredes < 1)
                    {
                        Interfaz.Error("La cantidad de subredes debe ser al menos 1.\n");
                        entradaInvalida = true;
                    }
                    else
                    {
                        entradaInvalida = false;
                    }
                }
                catch (FormatException)
                {
                    entradaInvalida = true;
                    Interfaz.Error("Entrada no válida. Debe ingresar un número entero.\n");
                }
            } while (entradaInvalida);

            // Configurar cada subred
            for (int i = 1; i <= cantidadSubredes; i++)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"\nConfiguración de la SubRed #{i}:");
                Console.ResetColor();

                string numeroRed;
                bool redValida;

                do
                {
                    Console.Write("Ingrese número de red para la subred (ej: 180): ");
                    numeroRed = Console.ReadLine().Trim();

                    redValida = ValidarNumeroRed(numeroRed);

                    if (redValida && Subredes.Any(s => s.Enrutador.IP.StartsWith(numeroRed + ".")))
                    {
                        Interfaz.Error("Esta red ya existe\n\n");
                        redValida = false;
                    }

                } while (!redValida);

                string idSubred = i.ToString("00");
                string ipRouter = $"{numeroRed}.0";
                string ipPC = $"{numeroRed}.01";

                Subredes.Add(new Subred(idSubred, numeroRed, this));

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nSubRed {i} configurada:");
                Console.ResetColor();
                Console.WriteLine($"- Router: {ipRouter} (R{idSubred})");
                Console.WriteLine($"- PC: {ipPC} (PC{idSubred})");
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n¡Red configurada exitosamente!");
            Console.ResetColor();
            Interfaz.Continuar();
        }

        private bool ValidarNumeroRed(string numeroRed)
        {
            if (string.IsNullOrWhiteSpace(numeroRed))
            {
                Interfaz.Error("El número de red no puede estar vacío\n\n");
                return false;
            }

            if (!int.TryParse(numeroRed, out int red) || red < 0 || red > 255)
            {
                Interfaz.Error("El número de red debe ser entre 0 y 255\n\n");
                return false;
            }

            return true;
        }

        public void CrearMensaje()  //Opción 2
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("════ CREAR MENSAJE ════");
            Console.ResetColor();

            // Verificar que haya al menos 2 subredes
            if (Subredes.Count < 2)
            {
                Interfaz.Error("Se necesitan mínimo 2 subredes para crear mensajes\n");
                Interfaz.Continuar();
                return;
            }

            try
            {
                // Mostrar PCs disponibles
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nPCs disponibles en la red:");
                Console.ResetColor();

                foreach (var subred in Subredes)
                {
                    Console.WriteLine($"- PC: {subred.Computadora.IP} ({subred.Computadora.Nombre})");
                }

                // Seleccionar PC origen
                Console.Write("\nIngrese IP de origen (formato X.01): ");
                string ipOrigen = Console.ReadLine().Trim();
                var pcOrigen = Subredes.Select(s => s.Computadora).FirstOrDefault(pc => pc.IP == ipOrigen);

                if (pcOrigen == null)
                {
                    Interfaz.Error("No existe un PC con esa IP origen");
                    Interfaz.Continuar();
                    return;
                }

                // Seleccionar PC destino
                Console.Write("\nIngrese IP de destino (formato X.01): ");
                string ipDestino = Console.ReadLine().Trim();

                if (ipDestino == ipOrigen)
                {
                    Interfaz.Error("No puede enviar mensajes al mismo PC origen");
                    Interfaz.Continuar();
                    return;
                }

                var pcDestino = Subredes.Select(s => s.Computadora).FirstOrDefault(pc => pc.IP == ipDestino);

                if (pcDestino == null)
                {
                    Interfaz.Error("No existe un PC con esa IP destino");
                    Interfaz.Continuar();
                    return;
                }

                // Ingresar contenido del mensaje (con límite de 10 caracteres)
                string contenido;
                do
                {
                    Console.Write("\nIngrese el mensaje a enviar (máx 10 caracteres): ");
                    contenido = Console.ReadLine().Trim();

                    if (string.IsNullOrEmpty(contenido))
                    {
                        Interfaz.Error("El mensaje no puede estar vacío");
                    }
                    else if (contenido.Length > 10)
                    {
                        Interfaz.Error("El mensaje no puede tener más de 10 caracteres");
                        contenido = ""; // Forzar repetición del bucle
                    }

                } while (string.IsNullOrEmpty(contenido));

                // Generar ID del mensaje
                string idMensaje = $"M{Mensajes.Count + 1:00}";

                // Crear el mensaje
                Mensaje mensaje = new Mensaje(idMensaje, ipOrigen, ipDestino, contenido);

                // Agregar paquetes a la cola de envío del PC origen
                NodoLista<Paquete> actual = mensaje.Paquetes.Cabeza;
                while (actual != null)
                {
                    pcOrigen.ColaEnvio.InsertarFinal(actual.Datos);
                    actual = actual.Siguiente;
                }

                // Agregar mensaje a la lista de mensajes
                Mensajes.Add(mensaje);

                // Mostrar resultados
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nMensaje creado y paquetes encolados correctamente!");
                Console.ResetColor();
                Console.WriteLine($"ID Mensaje: {idMensaje}");
                Console.WriteLine($"Origen: {ipOrigen} | Destino: {ipDestino}");
                Console.WriteLine($"Contenido: {contenido}");
                Console.WriteLine($"Total de paquetes generados: {mensaje.Paquetes.Count}");
            }
            catch (Exception ex)
            {
                Interfaz.Error($"Error: {ex.Message}");
            }

            Interfaz.Continuar();
        }

        public void Enviar()  //Opcion 3
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("════ ENVIAR PAQUETE ════");
            Console.ResetColor();

            try
            {
                // 1. Seleccionar equipo origen
                Console.Write("\nIngrese la IP del equipo que enviará el paquete: ");
                string ip = Console.ReadLine().Trim();
                Dispositivo equipo = BuscarDispositivoPorIP(ip);

                if (equipo == null)
                {
                    Interfaz.Error("No existe un equipo con esa IP.\n");
                    Interfaz.Continuar();
                    return;
                }

                // 2. Verificar si hay paquetes para enviar
                if (equipo.ColaEnvio.Count == 0)
                {
                    Interfaz.Error("El equipo no tiene paquetes en su cola de envío.\n");
                    Interfaz.Continuar();
                    return;
                }

                // 3. Obtener el paquete a enviar
                Paquete paquete = equipo.ColaEnvio.EliminarInicio();
                paquete.Estado = EstadoPaquete.Enviado;

                Subred subredOrigen = BuscarSubredPorIP(equipo.IP);
                Dispositivo destino = null;

                // 4. Lógica de envío según tipo de dispositivo
                if (equipo is PC pcOrigen)
                {
                    // Envío desde PC a Router
                    destino = subredOrigen?.Enrutador;

                    if (destino == null)
                    {
                        Interfaz.Error("No se encontró el router asociado al PC.\n");
                        pcOrigen.ColaEnvio.InsertarFinal(paquete);
                        Interfaz.Continuar();
                        return;
                    }

                    if (destino.ColaEnvio.Count >= 4) // Capacidad máxima del router
                    {
                        Interfaz.Error("El router está lleno. Paquete será reenviado al final de la cola del PC.\n");
                        pcOrigen.ColaEnvio.InsertarFinal(paquete);
                        Interfaz.Continuar();
                        return;
                    }

                    destino.ColaEnvio.InsertarFinal(paquete);
                    paquete.AgregarTraza("Router", destino.IP);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nPaquete enviado exitosamente!");
                    Console.ResetColor();
                    Console.WriteLine($"Paquete: {paquete.IDPaquete} enviado a: {destino.IP}\n");
                }
                else if (equipo is Router routerOrigen)
                {
                    // Envío desde Router
                    Subred subredDestino = BuscarSubredPorIP(paquete.IPDestino);

                    if (subredDestino == null)
                    {
                        Interfaz.Error("No se encontró la subred destino.\n");
                        routerOrigen.ColaEnvio.InsertarFinal(paquete);
                        Interfaz.Continuar();
                        return;
                    }

                    if (subredDestino.Enrutador.IP == routerOrigen.IP)
                    {
                        // El router actual es el destino final, enviar al PC
                        PC pcDestino = subredDestino.Computadora;

                        if (pcDestino.ColaRecibidos.Count >= 10) // Capacidad máxima del PC
                        {
                            Interfaz.Error("La cola de recepción del PC destino está llena.\n");
                            routerOrigen.ColaEnvio.InsertarFinal(paquete);
                            Interfaz.Continuar();
                            return;
                        }

                        pcDestino.ColaRecibidos.InsertarFinal(paquete);
                        paquete.AgregarTraza("PC", pcDestino.IP);
                        paquete.Estado = EstadoPaquete.Recibido;

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nPaquete entregado exitosamente al PC destino!");
                        Console.ResetColor();
                        Console.WriteLine($"Paquete: {paquete.IDPaquete} recibido por: {pcDestino.IP}\n");
                    }
                    else
                    {
                        // Enviar a otro router intermedio
                        Router routerDestino = subredDestino.Enrutador;

                        if (routerDestino.ColaEnvio.Count >= 4) // Capacidad máxima del router
                        {
                            Interfaz.Error("El router destino está lleno. Intentando reenviar por otra ruta...\n");

                            // Buscar un router alternativo
                            var routerAlternativo = Subredes
                                .Select(s => s.Enrutador)
                                .FirstOrDefault(r => r.IP != routerOrigen.IP &&
                                                   r.IP != routerDestino.IP &&
                                                   r.ColaEnvio.Count < 4);

                            if (routerAlternativo != null)
                            {
                                routerAlternativo.ColaEnvio.InsertarFinal(paquete);
                                paquete.AgregarTraza("Router (Alternativo)", routerAlternativo.IP);

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("\nPaquete reenviado por ruta alternativa!");
                                Console.ResetColor();
                                Console.WriteLine($"Paquete: {paquete.IDPaquete} enviado a router: {routerAlternativo.IP}\n");
                            }
                            else
                            {
                                Interfaz.Error("No hay routers disponibles. Paquete será reenviado al final de la cola del router emisor.\n");
                                routerOrigen.ColaEnvio.InsertarFinal(paquete);
                                Interfaz.Continuar();
                                return;
                            }
                        }
                        else
                        {
                            routerDestino.ColaEnvio.InsertarFinal(paquete);
                            paquete.AgregarTraza("Router", routerDestino.IP);

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nPaquete enviado exitosamente al router destino intermedio!");
                            Console.ResetColor();
                            Console.WriteLine($"Paquete: {paquete.IDPaquete} enviado a router: {routerDestino.IP}\n");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Interfaz.Error($"{ex.Message}\n");
            }

            Interfaz.Continuar();
        }

        private Dispositivo BuscarDispositivoPorIP(string ip)
        {
            foreach (var subred in Subredes)
            {
                if (subred.Computadora.IP == ip) return subred.Computadora;
                if (subred.Enrutador.IP == ip) return subred.Enrutador;
            }
            return null;
        }

        private Subred BuscarSubredPorIP(string ip)
        {
            return Subredes.FirstOrDefault(s =>
                s.Computadora.IP == ip ||
                s.Enrutador.IP == ip ||
                s.Computadora.IP.StartsWith(ip.Split('.')[0]) || // Busca por número de red
                s.Enrutador.IP.StartsWith(ip.Split('.')[0]));
        }

        public List<Router> ObtenerRouters()
        {
            return Subredes.Select(s => s.Enrutador).ToList();
        }

        public void MostrarStatusRed()  //Opcion 5
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("════ ESTADO DE LA RED ════");
            Console.ResetColor();

            // Verificar si hay red configurada
            if (Subredes.Count == 0)
            {
                Interfaz.Error("No hay ninguna red configurada.\n");
                Interfaz.Continuar();
                return;
            }

            // Mostrar estado de cada subred
            int subredIndex = 1;
            foreach (var subred in Subredes)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("════ ESTADO DE LA RED ════");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\nSubRed {subred.ID}:");
                Console.ResetColor();

                // Mostrar estado del Router
                Console.WriteLine($"Router ({subred.Enrutador.IP}) - Cola de Envío:");
                if (subred.Enrutador.ColaEnvio.Count == 0)
                {
                    Console.WriteLine("  (Vacía)");
                }
                else
                {
                    foreach (var paquete in subred.Enrutador.ColaEnvio.ToList())
                    {
                        Console.WriteLine($"  {paquete.IDPaquete} - '{paquete.Dato}' - Estado: {paquete.Estado}");
                    }
                }

                // Mostrar estado del PC (Cola de Envío)
                Console.WriteLine($"\nPC ({subred.Computadora.IP}) - Cola de Envío:");
                if (subred.Computadora.ColaEnvio.Count == 0)
                {
                    Console.WriteLine("  (Vacía)");
                }
                else
                {
                    foreach (var paquete in subred.Computadora.ColaEnvio.ToList())
                    {
                        Console.WriteLine($"  {paquete.IDPaquete} - '{paquete.Dato}' - Estado: {paquete.Estado}");
                    }
                }

                // Mostrar estado del PC (Cola de Recibidos)
                Console.WriteLine($"\nPC ({subred.Computadora.IP}) - Cola de Recibidos:");
                if (subred.Computadora.ColaRecibidos.Count == 0)
                {
                    Console.WriteLine("  (Vacía)");
                }
                else
                {
                    var paquetes = subred.Computadora.ColaRecibidos.ToList();
                    var idsMensajes = paquetes.Select(p => p.IDPaquete.Substring(0, 3)).Distinct();

                    foreach (var idMensaje in idsMensajes)
                    {
                        var mensajeOriginal = Mensajes.FirstOrDefault(m =>
                            m.IDMensaje == idMensaje &&
                            m.IPDestino == subred.Computadora.IP);

                        if (mensajeOriginal == null)
                        {
                            Console.WriteLine($"  Mensaje {idMensaje} no reconocido.");
                            continue;
                        }

                        var recibidos = paquetes.Where(p => p.IDPaquete.StartsWith(idMensaje)).ToList();

                        bool estaCompleto = recibidos.Count == mensajeOriginal.Paquetes.Count;
                        bool ordenCorrecto = true;
                        for (int i = 0; i < recibidos.Count; i++)
                        {
                            if (recibidos[i].NumeroSecuencia != i + 1)
                            {
                                ordenCorrecto = false;
                                break;
                            }
                        }

                        string reconstruido = new string(recibidos.Select(p => p.Dato).ToArray());

                        if (estaCompleto && ordenCorrecto)
                        {
                            mensajeOriginal.Estado = EstadoMensaje.Recibido;
                            Console.WriteLine($"  MENSAJE COMPLETO RECIBIDO: '{reconstruido}' - Estado: {mensajeOriginal.Estado}");
                        }
                        else
                        {
                            mensajeOriginal.Estado = EstadoMensaje.Dañado;
                            Console.WriteLine($"  MENSAJE DAÑADO: '{reconstruido}' - Estado: {mensajeOriginal.Estado}");
                        }
                    }
                }

                subredIndex++;
                Interfaz.Continuar();
            }
        }
    }
}
