using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_RedVirtualDinamica_Marcelo
{
    public class Red
    {
        #region Atributos
        public List<Subred> Subredes { get; set; }
        public List<Mensaje> Mensajes { get; set; }
        #endregion

        #region Metodos
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

            int cantidad_Subredes = 0;
            string opcion = "";
            bool entrada_Invalida = false;

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

            do
            {
                try
                {
                    Console.Write("\nIngrese cantidad de subredes (mínimo 1): ");
                    cantidad_Subredes = int.Parse(Console.ReadLine());

                    if (cantidad_Subredes < 1)
                    {
                        Interfaz.Error("La cantidad de subredes debe ser al menos 1.\n");
                        entrada_Invalida = true;
                    }
                    else
                    {
                        entrada_Invalida = false;
                    }
                }
                catch (FormatException)
                {
                    entrada_Invalida = true;
                    Interfaz.Error("Entrada no válida. Debe ingresar un número entero.\n");
                }
            } while (entrada_Invalida);

            for (int i = 1; i <= cantidad_Subredes; i++)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"\nConfiguración de la SubRed #{i}:");
                Console.ResetColor();

                string numero_Red;
                bool red_Valida;

                do
                {
                    Console.Write("Ingrese número de red para la subred (ej: 180): ");
                    numero_Red = Console.ReadLine().Trim();

                    red_Valida = ValidarNumeroRed(numero_Red);

                    if (red_Valida && Subredes.Any(s => s.Enrutador.IP.StartsWith(numero_Red + ".")))
                    {
                        Interfaz.Error("Esta red ya existe\n\n");
                        red_Valida = false;
                    }

                } while (!red_Valida);

                string id_Subred = i.ToString("00");
                string ip_Router = $"{numero_Red}.0";
                string ip_PC = $"{numero_Red}.01";

                Subredes.Add(new Subred(id_Subred, numero_Red, this));

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nSubRed {i} configurada:");
                Console.ResetColor();
                Console.WriteLine($"- Router: {ip_Router} (R{id_Subred})");
                Console.WriteLine($"- PC: {ip_PC} (PC{id_Subred})");
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n¡Red configurada exitosamente!");
            Console.ResetColor();
            Interfaz.Continuar();
        }

        private bool ValidarNumeroRed(string numero_Red)
        {
            if (string.IsNullOrWhiteSpace(numero_Red))
            {
                Interfaz.Error("El número de red no puede estar vacío\n\n");
                return false;
            }

            if (!int.TryParse(numero_Red, out int red) || red < 0 || red > 255)
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

            if (Subredes.Count < 2)
            {
                Interfaz.Error("Se necesitan mínimo 2 subredes para crear mensajes\n");
                Interfaz.Continuar();
                return;
            }

            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nPCs disponibles en la red:");
                Console.ResetColor();

                foreach (var subred in Subredes)
                {
                    Console.WriteLine($"- PC: {subred.Computadora.IP} ({subred.Computadora.Nombre})");
                }

                Console.Write("\nIngrese IP de origen (formato X.01): ");
                string ip_Origen = Console.ReadLine().Trim();
                var pc_Origen = Subredes.Select(s => s.Computadora).FirstOrDefault(pc => pc.IP == ip_Origen);

                if (pc_Origen == null)
                {
                    Interfaz.Error("No existe un PC con esa IP origen");
                    Interfaz.Continuar();
                    return;
                }

                Console.Write("\nIngrese IP de destino (formato X.01): ");
                string ip_Destino = Console.ReadLine().Trim();

                if (ip_Destino == ip_Origen)
                {
                    Interfaz.Error("No puede enviar mensajes al mismo PC origen");
                    Interfaz.Continuar();
                    return;
                }

                var pc_Destino = Subredes.Select(s => s.Computadora).FirstOrDefault(pc => pc.IP == ip_Destino);

                if (pc_Destino == null)
                {
                    Interfaz.Error("No existe un PC con esa IP destino");
                    Interfaz.Continuar();
                    return;
                }

                string contenido;
                do
                {
                    Console.Write("\nIngrese el mensaje a enviar: ");
                    contenido = Console.ReadLine().Trim();

                    if (string.IsNullOrEmpty(contenido))
                    {
                        Interfaz.Error("El mensaje no puede estar vacío");
                        contenido = "";
                    }
                } while (string.IsNullOrEmpty(contenido));

                string id_Mensaje = $"M{Mensajes.Count + 1:00}";

                Mensaje mensaje = new Mensaje(id_Mensaje, ip_Origen, ip_Destino, contenido);

                NodoLista<Paquete> actual = mensaje.Paquetes.Cabeza;
                while (actual != null)
                {
                    pc_Origen.ColaEnvio.InsertarFinal(actual.Datos);
                    actual = actual.Siguiente;
                }

                Mensajes.Add(mensaje);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nMensaje creado y paquetes encolados correctamente!");
                Console.ResetColor();
                Console.WriteLine($"ID Mensaje: {id_Mensaje}");
                Console.WriteLine($"Origen: {ip_Origen} | Destino: {ip_Destino}");
                Console.WriteLine($"Contenido: {contenido}");
                Console.WriteLine($"Total de paquetes generados: {mensaje.Paquetes.Count}");
            }
            catch (Exception ex)
            {
                Interfaz.Error($"Error: {ex.Message}");
            }

            Interfaz.Continuar();
        }

        public void Enviar()  //Opción 3
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("════ ENVIAR PAQUETE ════");
            Console.ResetColor();

            try
            {
                Console.Write("\nIngrese la IP del equipo que enviará el paquete: ");
                string ip = Console.ReadLine().Trim();
                Dispositivo equipo = BuscarDispositivoPorIP(ip);

                if (equipo == null)
                {
                    Interfaz.Error("No existe un equipo con esa IP.\n");
                    Interfaz.Continuar();
                    return;
                }

                if (equipo.ColaEnvio.Count == 0)
                {
                    Interfaz.Error("El equipo no tiene paquetes en su cola de envío.\n");
                    Interfaz.Continuar();
                    return;
                }

                Paquete paquete = equipo.ColaEnvio.EliminarInicio();
                paquete.Estado = EstadoPaquete.Enviado;

                Subred subred_Origen = BuscarSubredPorIP(equipo.IP);
                Dispositivo destino = null;

                if (equipo is PC pc_Origen)
                {
                    destino = subred_Origen?.Enrutador;

                    if (destino == null)
                    {
                        Interfaz.Error("No se encontró el router asociado al PC.\n");
                        pc_Origen.ColaEnvio.InsertarFinal(paquete);
                        Interfaz.Continuar();
                        return;
                    }

                    if (destino.ColaEnvio.Count >= 4) // Capacidad máxima del router
                    {
                        Interfaz.Error("El router está lleno. Paquete será reenviado al final de la cola del PC.\n");
                        pc_Origen.ColaEnvio.InsertarFinal(paquete);
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
                else if (equipo is Router router_Origen)
                {
                    Subred subred_Destino = BuscarSubredPorIP(paquete.IPDestino);

                    if (subred_Destino == null)
                    {
                        Interfaz.Error("No se encontró la subred destino.\n");
                        router_Origen.ColaEnvio.InsertarFinal(paquete);
                        Interfaz.Continuar();
                        return;
                    }

                    if (subred_Destino.Enrutador.IP == router_Origen.IP)
                    {
                        PC pc_Destino = subred_Destino.Computadora;

                        pc_Destino.ColaRecibidos.InsertarFinal(paquete);
                        paquete.AgregarTraza("PC", pc_Destino.IP);
                        paquete.Estado = EstadoPaquete.Recibido;

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nPaquete entregado exitosamente al PC destino!");
                        Console.ResetColor();
                        Console.WriteLine($"Paquete: {paquete.IDPaquete} recibido por: {pc_Destino.IP}\n");
                    }
                    else
                    {
                        Router router_Destino = subred_Destino.Enrutador;

                        if (router_Destino.ColaEnvio.Count >= 4) // Capacidad máxima del router
                        {
                            Interfaz.Error("El router destino está lleno. Intentando reenviar por otra ruta...\n");

                            var router_Alternativo = Subredes.Select(s => s.Enrutador).FirstOrDefault(r => r.IP != router_Origen.IP && r.IP != router_Destino.IP && r.ColaEnvio.Count < 4);

                            if (router_Alternativo != null)
                            {
                                router_Alternativo.ColaEnvio.InsertarFinal(paquete);
                                paquete.AgregarTraza("Router (Alternativo)", router_Alternativo.IP);

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("\nPaquete reenviado por ruta alternativa!");
                                Console.ResetColor();
                                Console.WriteLine($"Paquete: {paquete.IDPaquete} enviado a router: {router_Alternativo.IP}\n");
                            }
                            else
                            {
                                Interfaz.Error("No hay routers disponibles. Paquete será reenviado al final de la cola del router emisor.\n");
                                router_Origen.ColaEnvio.InsertarFinal(paquete);
                                Interfaz.Continuar();
                                return;
                            }
                        }
                        else
                        {
                            router_Destino.ColaEnvio.InsertarFinal(paquete);
                            paquete.AgregarTraza("Router", router_Destino.IP);

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nPaquete enviado exitosamente al router destino intermedio!");
                            Console.ResetColor();
                            Console.WriteLine($"Paquete: {paquete.IDPaquete} enviado a router: {router_Destino.IP}\n");
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
                s.Computadora.IP.StartsWith(ip.Split('.')[0]) ||
                s.Enrutador.IP.StartsWith(ip.Split('.')[0]));
        }

        public void VisualizarTrazaPaquete()  //Opción 4
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("════ VISUALIZAR TRAZA DE PAQUETE ════");
            Console.ResetColor();

            try
            {
                if (Subredes.Count == 0)
                {
                    Interfaz.Error("No hay ninguna red configurada.\n");
                    Interfaz.Continuar();
                    return;
                }

                Console.Write("\nIngrese el ID del paquete (ej: M01P01): ");
                string id_Paquete = Console.ReadLine().Trim().ToUpper();

                Paquete paquete = null;
                Dispositivo dispositivo_Actual = null;

                foreach (var subred in Subredes)
                {
                    var nodo = subred.Computadora.ColaEnvio.Cabeza;
                    while (nodo != null)
                    {
                        if (nodo.Datos.IDPaquete == id_Paquete)
                        {
                            paquete = nodo.Datos;
                            dispositivo_Actual = subred.Computadora;
                            break;
                        }
                        nodo = nodo.Siguiente;
                    }

                    if (paquete == null)
                    {
                        nodo = subred.Computadora.ColaRecibidos.Cabeza;
                        while (nodo != null)
                        {
                            if (nodo.Datos.IDPaquete == id_Paquete)
                            {
                                paquete = nodo.Datos;
                                dispositivo_Actual = subred.Computadora;
                                break;
                            }
                            nodo = nodo.Siguiente;
                        }
                    }

                    if (paquete != null) break;
                }

                if (paquete == null)
                {
                    foreach (var subred in Subredes)
                    {
                        var nodo = subred.Enrutador.ColaEnvio.Cabeza;
                        while (nodo != null)
                        {
                            if (nodo.Datos.IDPaquete == id_Paquete)
                            {
                                paquete = nodo.Datos;
                                dispositivo_Actual = subred.Enrutador;
                                break;
                            }
                            nodo = nodo.Siguiente;
                        }

                        if (paquete != null) break;
                    }
                }

                if (paquete == null)
                {
                    Interfaz.Error($"No se encontró ningún paquete con ID: {id_Paquete}\n");
                    Interfaz.Continuar();
                    return;
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nINFORMACIÓN DEL PAQUETE:");
                Console.ResetColor();
                Console.WriteLine($"ID: {paquete.IDPaquete}");
                Console.WriteLine($"Origen: {paquete.IPOrigen}");
                Console.WriteLine($"Destino: {paquete.IPDestino}");
                Console.WriteLine($"Dato: '{paquete.Dato}'");
                Console.WriteLine($"Número de secuencia: {paquete.NumeroSecuencia}");
                Console.WriteLine($"Estado actual: {paquete.Estado}");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nUBICACIÓN ACTUAL:");
                Console.ResetColor();
                Console.WriteLine($"Dispositivo: {(dispositivo_Actual is PC ? "PC" : "Router")}");
                Console.WriteLine($"Nombre: {dispositivo_Actual.Nombre}");
                Console.WriteLine($"IP: {dispositivo_Actual.IP}");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nHISTORIAL DE TRAZA (del más reciente al más antiguo):");
                Console.ResetColor();

                var traza = paquete.ObtenerTraza().ToList();
                if (traza.Count == 0)
                {
                    Console.WriteLine("  (No hay registros de traza)");
                }
                else
                {
                    int paso = 1;
                    foreach (var entrada in traza)
                    {
                        var partes = entrada.Split('|');
                        Console.WriteLine($"  {paso++}. Dispositivo: {partes[0]} - IP: {partes[1]}");
                    }
                }
            }
            catch (Exception ex)
            {
                Interfaz.Error($"Error al visualizar traza: {ex.Message}\n");
            }

            Interfaz.Continuar();
        }

        public void MostrarStatusRed()  //Opción 5
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("════ ESTADO DE LA RED ════");
            Console.ResetColor();

            if (Subredes.Count == 0)
            {
                Interfaz.Error("No hay ninguna red configurada.\n");
                Interfaz.Continuar();
                return;
            }

            int subred_Index = 1;
            foreach (var subred in Subredes)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("════ ESTADO DE LA RED ════");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\nSubRed {subred.ID}:");
                Console.ResetColor();

                Console.WriteLine($"Router ({subred.Enrutador.IP}) - Cola de Envío ({subred.Enrutador.ColaEnvio.Count}/4):");
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

                Console.WriteLine($"\nPC ({subred.Computadora.IP}) - Cola de Recibidos:");
                if (subred.Computadora.ColaRecibidos.Count == 0)
                {
                    Console.WriteLine("  (Vacía)");
                }
                else
                {
                    var paquetes = subred.Computadora.ColaRecibidos.ToList();
                    var ids_Mensajes = paquetes.Select(p => p.IDPaquete.Substring(0, 3)).Distinct();

                    foreach (var id_Mensaje in ids_Mensajes)
                    {
                        var mensaje_Original = Mensajes.FirstOrDefault(m =>
                            m.IDMensaje == id_Mensaje &&
                            m.IPDestino == subred.Computadora.IP);

                        if (mensaje_Original == null)
                        {
                            Console.WriteLine($"  Mensaje {id_Mensaje} no reconocido.");
                            continue;
                        }

                        var recibidos = paquetes.Where(p => p.IDPaquete.StartsWith(id_Mensaje)).ToList();

                        bool esta_Completo = recibidos.Count == mensaje_Original.Paquetes.Count;
                        bool orden_Correcto = true;
                        for (int i = 0; i < recibidos.Count; i++)
                        {
                            if (recibidos[i].NumeroSecuencia != i + 1)
                            {
                                orden_Correcto = false;
                                break;
                            }
                        }

                        string reconstruido = new string(recibidos.Select(p => p.Dato).ToArray());

                        if (esta_Completo && orden_Correcto)
                        {
                            mensaje_Original.Estado = EstadoMensaje.Recibido;
                            Console.WriteLine($"  MENSAJE COMPLETO RECIBIDO: '{reconstruido}' - Estado: {mensaje_Original.Estado}");
                        }
                        else
                        {
                            mensaje_Original.Estado = EstadoMensaje.Dañado;
                            Console.WriteLine($"  MENSAJE DAÑADO: '{reconstruido}' - Estado: {mensaje_Original.Estado}");
                        }
                    }
                }

                subred_Index++;
                Interfaz.Continuar();
            }
        }

        public void MostrarStatusSubred()  //Opción 6
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("════ ESTADO DE UNA SUBRED ════");
            Console.ResetColor();

            try
            {
                if (Subredes.Count == 0)
                {
                    Interfaz.Error("No hay ninguna red configurada.\n");
                    Interfaz.Continuar();
                    return;
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\nSubredes disponibles:");
                Console.ResetColor();

                foreach (var subred in Subredes)
                {
                    Console.WriteLine($"- Subred {subred.ID} (Router: {subred.Enrutador.IP}, PC: {subred.Computadora.IP})");
                }

                Console.Write("\nIngrese el ID de la subred que desea visualizar (ej: 01): ");
                string id_Subred = Console.ReadLine().Trim();

                var subred_Seleccionada = Subredes.FirstOrDefault(s => s.ID == id_Subred);
                if (subred_Seleccionada == null)
                {
                    Interfaz.Error($"No existe una subred con ID: {id_Subred}\n");
                    Interfaz.Continuar();
                    return;
                }

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("════ ESTADO DE UNA SUBRED ════");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\nSubred {subred_Seleccionada.ID}:");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"\n[ROUTER] {subred_Seleccionada.Enrutador.Nombre} ({subred_Seleccionada.Enrutador.IP}):");
                Console.ResetColor();
                Console.WriteLine($"Estado: {(subred_Seleccionada.Enrutador.ColaEnvio.Count >= 4 ? "LLENO" : "DISPONIBLE")}");
                Console.WriteLine($"Paquetes en cola: {subred_Seleccionada.Enrutador.ColaEnvio.Count}/4");

                if (subred_Seleccionada.Enrutador.ColaEnvio.Count > 0)
                {
                    Console.WriteLine("\nPaquetes en cola de envío:");
                    int contador = 1;
                    var nodo = subred_Seleccionada.Enrutador.ColaEnvio.Cabeza;
                    while (nodo != null)
                    {
                        Console.WriteLine($"  {contador++}. {nodo.Datos.IDPaquete} - '{nodo.Datos.Dato}' " +
                                         $"(Sec: {nodo.Datos.NumeroSecuencia}, Estado: {nodo.Datos.Estado})");
                        nodo = nodo.Siguiente;
                    }
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\n[PC] {subred_Seleccionada.Computadora.Nombre} ({subred_Seleccionada.Computadora.IP}):");
                Console.ResetColor();
                Console.WriteLine($"Paquetes en cola de envío: {subred_Seleccionada.Computadora.ColaEnvio.Count}");
                Console.WriteLine($"Paquetes en cola de recibidos: {subred_Seleccionada.Computadora.ColaRecibidos.Count}");

                if (subred_Seleccionada.Computadora.ColaEnvio.Count > 0)
                {
                    Console.WriteLine("\nPaquetes en cola de envío:");
                    int contador = 1;
                    var nodo = subred_Seleccionada.Computadora.ColaEnvio.Cabeza;
                    while (nodo != null)
                    {
                        Console.WriteLine($"  {contador++}. {nodo.Datos.IDPaquete} - '{nodo.Datos.Dato}' " +
                                         $"(Sec: {nodo.Datos.NumeroSecuencia}, Estado: {nodo.Datos.Estado})");
                        nodo = nodo.Siguiente;
                    }
                }

                if (subred_Seleccionada.Computadora.ColaRecibidos.Count > 0)
                {
                    Console.WriteLine("\nPaquetes en cola de recibidos:");
                    int contador = 1;
                    var nodo = subred_Seleccionada.Computadora.ColaRecibidos.Cabeza;
                    while (nodo != null)
                    {
                        Console.WriteLine($"  {contador++}. {nodo.Datos.IDPaquete} - '{nodo.Datos.Dato}' " +
                                         $"(Sec: {nodo.Datos.NumeroSecuencia}, Estado: {nodo.Datos.Estado})");
                        nodo = nodo.Siguiente;
                    }

                    var paquetes_Recibidos = subred_Seleccionada.Computadora.ColaRecibidos.ToList();
                    var ids_Mensajes = paquetes_Recibidos.Select(p => p.IDPaquete.Substring(0, 3)).Distinct();

                    Console.WriteLine("\nMensajes recibidos:");
                    foreach (var id_Mensaje in ids_Mensajes)
                    {
                        var mensaje_Original = Mensajes.FirstOrDefault(m => m.IDMensaje == id_Mensaje);
                        if (mensaje_Original != null)
                        {
                            var paquetes_Mensaje = paquetes_Recibidos.Where(p => p.IDPaquete.StartsWith(id_Mensaje)).ToList();
                            bool completo = paquetes_Mensaje.Count == mensaje_Original.Paquetes.Count;
                            bool orden_Correcto = !paquetes_Mensaje.Any(p => p.NumeroSecuencia != paquetes_Mensaje.IndexOf(p) + 1);

                            string estado = completo && orden_Correcto ? "COMPLETO" : "DAÑADO";
                            ConsoleColor color_Estado = estado == "COMPLETO" ? ConsoleColor.Green : ConsoleColor.Red;

                            string contenido = new string(paquetes_Mensaje.OrderBy(p => p.NumeroSecuencia)
                                                      .Select(p => p.Dato).ToArray());

                            Console.Write($"- {id_Mensaje}: ");
                            Console.ForegroundColor = color_Estado;
                            Console.Write($"{estado} ");
                            Console.ResetColor();
                            Console.WriteLine($"- '{contenido}' ({paquetes_Mensaje.Count}/{mensaje_Original.Paquetes.Count} paquetes)");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Interfaz.Error($"Error al mostrar estado de subred: {ex.Message}\n");
            }

            Interfaz.Continuar();
        }

        public void MostrarStatusEquipo()  //Opción 7
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("════ ESTADO DE UN EQUIPO ════");
            Console.ResetColor();

            try
            {
                if (Subredes.Count == 0)
                {
                    Interfaz.Error("No hay ninguna red configurada.\n");
                    Interfaz.Continuar();
                    return;
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\nEquipos disponibles:");
                Console.ResetColor();

                Console.WriteLine("\n[ROUTERS]");
                foreach (var subred in Subredes)
                {
                    Console.WriteLine($"- {subred.Enrutador.Nombre} ({subred.Enrutador.IP}) - Paquetes: {subred.Enrutador.ColaEnvio.Count}/4");
                }

                Console.WriteLine("\n[PCs]");
                foreach (var subred in Subredes)
                {
                    Console.WriteLine($"- {subred.Computadora.Nombre} ({subred.Computadora.IP}) - Envío: {subred.Computadora.ColaEnvio.Count} | Recibidos: {subred.Computadora.ColaRecibidos.Count}");
                }

                Console.Write("\nIngrese la IP del equipo que desea visualizar: ");
                string ipEquipo = Console.ReadLine().Trim();

                Dispositivo equipo = BuscarDispositivoPorIP(ipEquipo);
                if (equipo == null)
                {
                    Interfaz.Error("No existe un equipo con esa IP.\n");
                    Interfaz.Continuar();
                    return;
                }

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("════ ESTADO DE UN EQUIPO ════");
                Console.ResetColor();

                if (equipo is Router router)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"\n[ROUTER] {router.Nombre} ({router.IP})");
                    Console.ResetColor();
                    Console.WriteLine($"Capacidad: {router.ColaEnvio.Count}/4 paquetes");
                    Console.WriteLine($"Estado: {(router.ColaEnvio.Count >= 4 ? "LLENO" : "DISPONIBLE")}");

                    if (router.ColaEnvio.Count > 0)
                    {
                        Console.WriteLine("\nPaquetes en cola (orden de envío):");
                        int posicion = 1;
                        var nodo = router.ColaEnvio.Cabeza;
                        while (nodo != null)
                        {
                            Console.WriteLine($"  {posicion++}. {nodo.Datos.IDPaquete}");
                            Console.WriteLine($"     - Dato: '{nodo.Datos.Dato}'");
                            Console.WriteLine($"     - Secuencia: {nodo.Datos.NumeroSecuencia}");
                            Console.WriteLine($"     - Estado: {nodo.Datos.Estado}");
                            Console.WriteLine($"     - Origen: {nodo.Datos.IPOrigen}");
                            Console.WriteLine($"     - Destino: {nodo.Datos.IPDestino}");
                            nodo = nodo.Siguiente;
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nNo hay paquetes en cola.");
                    }
                }
                else if (equipo is PC pc)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"\n[PC] {pc.Nombre} ({pc.IP})");
                    Console.ResetColor();

                    Console.WriteLine($"\n[COLA DE ENVÍO] - {pc.ColaEnvio.Count} paquetes");
                    if (pc.ColaEnvio.Count > 0)
                    {
                        int posicion = 1;
                        var nodo = pc.ColaEnvio.Cabeza;
                        while (nodo != null)
                        {
                            Console.WriteLine($"  {posicion++}. {nodo.Datos.IDPaquete}");
                            Console.WriteLine($"     - Dato: '{nodo.Datos.Dato}'");
                            Console.WriteLine($"     - Secuencia: {nodo.Datos.NumeroSecuencia}");
                            Console.WriteLine($"     - Estado: {nodo.Datos.Estado}");
                            Console.WriteLine($"     - Destino: {nodo.Datos.IPDestino}");
                            nodo = nodo.Siguiente;
                        }
                    }
                    else
                    {
                        Console.WriteLine("  No hay paquetes en cola de envío.");
                    }

                    Console.WriteLine($"\n[COLA DE RECIBIDOS] - {pc.ColaRecibidos.Count} paquetes");
                    if (pc.ColaRecibidos.Count > 0)
                    {
                        int posicion = 1;
                        var nodo = pc.ColaRecibidos.Cabeza;
                        while (nodo != null)
                        {
                            Console.WriteLine($"  {posicion++}. {nodo.Datos.IDPaquete}");
                            Console.WriteLine($"     - Dato: '{nodo.Datos.Dato}'");
                            Console.WriteLine($"     - Secuencia: {nodo.Datos.NumeroSecuencia}");
                            Console.WriteLine($"     - Estado: {nodo.Datos.Estado}");
                            Console.WriteLine($"     - Origen: {nodo.Datos.IPOrigen}");
                            nodo = nodo.Siguiente;
                        }

                        Console.WriteLine("\n[RESUMEN DE MENSAJES]");
                        var paquetes_Recibidos = pc.ColaRecibidos.ToList();
                        var ids_Mensajes = paquetes_Recibidos.Select(p => p.IDPaquete.Substring(0, 3)).Distinct();

                        foreach (var id_Mensaje in ids_Mensajes)
                        {
                            var mensaje_Original = Mensajes.FirstOrDefault(m => m.IDMensaje == id_Mensaje);
                            if (mensaje_Original != null)
                            {
                                var paquetes_Mensaje = paquetes_Recibidos.Where(p => p.IDPaquete.StartsWith(id_Mensaje)).ToList();
                                bool completo = paquetes_Mensaje.Count == mensaje_Original.Paquetes.Count;
                                bool orden_Correcto = !paquetes_Mensaje.Any(p => p.NumeroSecuencia != paquetes_Mensaje.IndexOf(p) + 1);

                                string estado = completo && orden_Correcto ? "COMPLETO" : "DAÑADO";
                                ConsoleColor color_Estado = estado == "COMPLETO" ? ConsoleColor.Green : ConsoleColor.Red;

                                string contenido = new string(paquetes_Mensaje.OrderBy(p => p.NumeroSecuencia)
                                                          .Select(p => p.Dato).ToArray());

                                Console.Write($"- {id_Mensaje}: ");
                                Console.ForegroundColor = color_Estado;
                                Console.Write($"{estado} ");
                                Console.ResetColor();
                                Console.WriteLine($"- '{contenido}' ({paquetes_Mensaje.Count}/{mensaje_Original.Paquetes.Count} paquetes)");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("  No hay paquetes recibidos.");
                    }
                }
            }
            catch (Exception ex)
            {
                Interfaz.Error($"Error al mostrar estado del equipo: {ex.Message}\n");
            }

            Interfaz.Continuar();
        }

        public void EliminarPaquete()  //Opción 8
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("════ ELIMINAR PAQUETE DE COLA ════");
            Console.ResetColor();

            try
            {
                if (Subredes.Count == 0)
                {
                    Interfaz.Error("No hay ninguna red configurada.\n");
                    Interfaz.Continuar();
                    return;
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\nEquipos disponibles:");
                Console.ResetColor();

                Console.WriteLine("\n[ROUTERS]");
                foreach (var subred in Subredes)
                {
                    Console.WriteLine($"- {subred.Enrutador.IP} ({subred.Enrutador.Nombre}) - Paquetes: {subred.Enrutador.ColaEnvio.Count}/4");
                }

                Console.WriteLine("\n[PCs]");
                foreach (var subred in Subredes)
                {
                    Console.WriteLine($"- {subred.Computadora.IP} ({subred.Computadora.Nombre}) - Envío: {subred.Computadora.ColaEnvio.Count} | Recibidos: {subred.Computadora.ColaRecibidos.Count}");
                }

                Console.Write("\nIngrese la IP del equipo donde desea eliminar un paquete: ");
                string ip_Equipo = Console.ReadLine().Trim();

                Dispositivo equipo = BuscarDispositivoPorIP(ip_Equipo);
                if (equipo == null)
                {
                    Interfaz.Error("No existe un equipo con esa IP.\n");
                    Interfaz.Continuar();
                    return;
                }

                if (equipo.ColaEnvio.Count == 0 && (equipo is PC pc && pc.ColaRecibidos.Count == 0))
                {
                    Interfaz.Error("El equipo no tiene paquetes en ninguna cola.\n");
                    Interfaz.Continuar();
                    return;
                }

                ListaEnlazada<Paquete> cola_Seleccionada = null;
                string tipo_Cola = "";

                if (equipo is Router)
                {
                    cola_Seleccionada = equipo.ColaEnvio;
                    tipo_Cola = "envío del router";
                }
                else if (equipo is PC)
                {
                    Console.WriteLine("\nSeleccione la cola del PC:");
                    Console.WriteLine("1. Cola de envío");
                    Console.WriteLine("2. Cola de recibidos");
                    Console.Write("Opción: ");
                    string opcionCola = Console.ReadLine().Trim();

                    switch (opcionCola)
                    {
                        case "1":
                            cola_Seleccionada = equipo.ColaEnvio;
                            tipo_Cola = "envío del PC";
                            break;
                        case "2":
                            cola_Seleccionada = ((PC)equipo).ColaRecibidos;
                            tipo_Cola = "recibidos del PC";
                            break;
                        default:
                            Interfaz.Error("Opción no válida.\n");
                            Interfaz.Continuar();
                            return;
                    }
                }

                if (cola_Seleccionada.Count == 0)
                {
                    Interfaz.Error($"La cola de {tipo_Cola} está vacía.\n");
                    Interfaz.Continuar();
                    return;
                }

                Console.WriteLine($"\nPaquetes en la cola de {tipo_Cola}:");
                int posicion = 1;
                var nodo = cola_Seleccionada.Cabeza;
                while (nodo != null)
                {
                    Console.WriteLine($"{posicion++}. {nodo.Datos.IDPaquete} - " +
                                    $"'{nodo.Datos.Dato}' (Sec: {nodo.Datos.NumeroSecuencia})");
                    nodo = nodo.Siguiente;
                }

                Console.Write("\nIngrese el número de secuencia del paquete a eliminar: ");
                if (!int.TryParse(Console.ReadLine(), out int num_Secuencia) || num_Secuencia < 1)
                {
                    Interfaz.Error("Número de secuencia inválido.\n");
                    Interfaz.Continuar();
                    return;
                }

                bool paquete_Eliminado = false;
                nodo = cola_Seleccionada.Cabeza;
                while (nodo != null)
                {
                    if (nodo.Datos.NumeroSecuencia == num_Secuencia)
                    {
                        string id_Mensaje = nodo.Datos.IDPaquete.Substring(0, 3);
                        var mensaje = Mensajes.FirstOrDefault(m => m.IDMensaje == id_Mensaje);

                        cola_Seleccionada.Eliminar(nodo.Datos);
                        paquete_Eliminado = true;

                        if (mensaje != null && equipo is PC pc_Destino && cola_Seleccionada == pc_Destino.ColaRecibidos)
                        {
                            mensaje.Estado = EstadoMensaje.Dañado;
                        }

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\nPaquete {nodo.Datos.IDPaquete} eliminado correctamente.");
                        Console.ResetColor();
                        break;
                    }
                    nodo = nodo.Siguiente;
                }

                if (!paquete_Eliminado)
                {
                    Interfaz.Error($"No se encontró un paquete con número de secuencia {num_Secuencia}.\n");
                }
            }
            catch (Exception ex)
            {
                Interfaz.Error($"Error al eliminar paquete: {ex.Message}\n");
            }

            Interfaz.Continuar();
        }

        public void VisualizarMensajesRecibidos() //Opción 9
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("════ MENSAJES RECIBIDOS ════");
            Console.ResetColor();

            if (Subredes.Count == 0)
            {
                Interfaz.Error("No hay ninguna red configurada.\n");
                Interfaz.Continuar();
                return;
            }

            Console.WriteLine("\nPCs disponibles en la red:");
            foreach (var subred in Subredes)
            {
                Console.WriteLine($"- {subred.Computadora.IP} ({subred.Computadora.Nombre})");
            }

            Console.Write("\nIngrese la IP del PC para ver sus mensajes recibidos: ");
            string ip_PC = Console.ReadLine().Trim();

            var pc = Subredes.Select(s => s.Computadora).FirstOrDefault(p => p.IP == ip_PC);
            if (pc == null)
            {
                Interfaz.Error("No existe un PC con esa IP.\n");
                Interfaz.Continuar();
                return;
            }

            var paquetes_Recibidos = pc.ColaRecibidos.ToList();
            if (paquetes_Recibidos.Count == 0)
            {
                Console.WriteLine("\nEl PC no tiene mensajes recibidos.");
                Interfaz.Continuar();
                return;
            }

            var ids_Mensajes = paquetes_Recibidos.Select(p => p.IDPaquete.Substring(0, 3)).Distinct().OrderBy(id => id);

            var mensajes_Recibidos = new List<string>();
            var mensajes_Danados = new List<string>();

            foreach (var id_Mensaje in ids_Mensajes)
            {
                var mensaje_Original = Mensajes.FirstOrDefault(m => m.IDMensaje == id_Mensaje && m.IPDestino == pc.IP);
                if (mensaje_Original == null) continue;

                var paquetes_Mensaje = paquetes_Recibidos
                    .Where(p => p.IDPaquete.StartsWith(id_Mensaje))
                    .ToList();

                bool completo = paquetes_Mensaje.Count == mensaje_Original.Paquetes.Count;
                bool orden_Correcto = true;
                for (int i = 0; i < paquetes_Mensaje.Count; i++)
                {
                    if (paquetes_Mensaje[i].NumeroSecuencia != i + 1)
                    {
                        orden_Correcto = false;
                        break;
                    }
                }

                var paquetes_OrdenLlegada = paquetes_Recibidos.Where(p => p.IDPaquete.StartsWith(id_Mensaje)).ToList();
                string contenido = new string(paquetes_OrdenLlegada.Select(p => p.Dato).ToArray());

                if (completo && orden_Correcto)
                {
                    mensaje_Original.Estado = EstadoMensaje.Recibido;
                    mensajes_Recibidos.Add($"| {id_Mensaje,-8} | {contenido,-15} | {mensaje_Original.Estado,-9} |");
                }
                else
                {
                    mensaje_Original.Estado = EstadoMensaje.Dañado;
                    string motivo = "";
                    if (!completo) motivo += "Faltan paquetes, ";
                    if (!orden_Correcto) motivo += "Secuencia incorrecta";
                    mensajes_Danados.Add($"| {id_Mensaje,-8} | {contenido,-15} | {mensaje_Original.Estado,-9} | {motivo,-20} |");
                }
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"════ MENSAJES RECIBIDOS EN {pc.IP} ════");
            Console.ResetColor();

            if (mensajes_Recibidos.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nMENSAJES COMPLETOS:");
                Console.ResetColor();
                Console.WriteLine("+----------+-----------------+-----------+");
                Console.WriteLine("| ID       | Contenido       | Estado    |");
                Console.WriteLine("+----------+-----------------+-----------+");
                mensajes_Recibidos.ForEach(Console.WriteLine);
                Console.WriteLine("+----------+-----------------+-----------+");
            }

            if (mensajes_Danados.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nMENSAJES DAÑADOS:");
                Console.ResetColor();
                Console.WriteLine("+----------+-----------------+-----------+----------------------+");
                Console.WriteLine("| ID       | Contenido       | Estado    | Motivo               |");
                Console.WriteLine("+----------+-----------------+-----------+----------------------+");
                mensajes_Danados.ForEach(Console.WriteLine);
                Console.WriteLine("+----------+-----------------+-----------+----------------------+");
            }

            if (mensajes_Recibidos.Count == 0 && mensajes_Danados.Count == 0)
            {
                Console.WriteLine("\nNo se encontraron mensajes completos o dañados.");
            }

            Interfaz.Continuar();
        }

        public void ConsultarInformacionPaquete() //Opción 10
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("════ CONSULTAR INFORMACIÓN DE PAQUETE ════");
            Console.ResetColor();

            try
            {
                if (Subredes.Count == 0)
                {
                    Interfaz.Error("No hay ninguna red configurada.\n");
                    Interfaz.Continuar();
                    return;
                }

                Console.Write("\nIngrese el ID completo del paquete (ej: M01P01): ");
                string id_Paquete = Console.ReadLine().Trim().ToUpper();

                (Paquete paquete, Dispositivo dispositivo) = BuscarPaqueteEnRed(id_Paquete);

                if (paquete == null)
                {
                    Interfaz.Error($"No se encontró ningún paquete con ID: {id_Paquete}\n");
                    Interfaz.Continuar();
                    return;
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nINFORMACIÓN DEL PAQUETE:");
                Console.ResetColor();
                Console.WriteLine($"- ID: {paquete.IDPaquete}");
                Console.WriteLine($"- Origen: {paquete.IPOrigen}");
                Console.WriteLine($"- Destino: {paquete.IPDestino}");
                Console.WriteLine($"- Dato: '{paquete.Dato}'");
                Console.WriteLine($"- Número de secuencia: {paquete.NumeroSecuencia}");
                Console.WriteLine($"- Estado actual: {paquete.Estado}");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nUBICACIÓN ACTUAL:");
                Console.ResetColor();
                Console.WriteLine($"- Dispositivo: {(dispositivo is PC ? "PC" : "Router")}");
                Console.WriteLine($"- Nombre: {dispositivo.Nombre}");
                Console.WriteLine($"- IP: {dispositivo.IP}");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nHISTORIAL DE TRAZA:");
                Console.ResetColor();

                var traza = paquete.ObtenerTraza().ToList();
                if (traza.Count == 0)
                {
                    Console.WriteLine("  (No hay registros de traza)");
                }
                else
                {
                    Console.WriteLine("┌───────┬────────────┬──────────────┐");
                    Console.WriteLine("│ Orden │ Dispositivo│ Dirección IP │");
                    Console.WriteLine("├───────┼────────────┼──────────────┤");

                    int orden = 1;
                    foreach (var entrada in traza)
                    {
                        var partes = entrada.Split('|');
                        Console.WriteLine($"│ {orden,-5} │ {partes[0],-10} │ {partes[1],-12} │");
                        orden++;
                    }
                    Console.WriteLine("└───────┴────────────┴──────────────┘");
                }
            }
            catch (Exception ex)
            {
                Interfaz.Error($"Error al consultar información: {ex.Message}\n");
            }

            Interfaz.Continuar();
        }

        private (Paquete, Dispositivo) BuscarPaqueteEnRed(string id_Paquete)
        {
            foreach (var subred in Subredes)
            {
                var nodo_Router = subred.Enrutador.ColaEnvio.Cabeza;
                while (nodo_Router != null)
                {
                    if (nodo_Router.Datos.IDPaquete.Equals(id_Paquete, StringComparison.OrdinalIgnoreCase))
                    {
                        return (nodo_Router.Datos, subred.Enrutador);
                    }
                    nodo_Router = nodo_Router.Siguiente;
                }

                var nodo_PCEnvio = subred.Computadora.ColaEnvio.Cabeza;
                while (nodo_PCEnvio != null)
                {
                    if (nodo_PCEnvio.Datos.IDPaquete.Equals(id_Paquete, StringComparison.OrdinalIgnoreCase))
                    {
                        return (nodo_PCEnvio.Datos, subred.Computadora);
                    }
                    nodo_PCEnvio = nodo_PCEnvio.Siguiente;
                }

                var nodo_PCRecibidos = subred.Computadora.ColaRecibidos.Cabeza;
                while (nodo_PCRecibidos != null)
                {
                    if (nodo_PCRecibidos.Datos.IDPaquete.Equals(id_Paquete, StringComparison.OrdinalIgnoreCase))
                    {
                        return (nodo_PCRecibidos.Datos, subred.Computadora);
                    }
                    nodo_PCRecibidos = nodo_PCRecibidos.Siguiente;
                }
            }

            return (null, null);
        }

        public void VaciarColaDispositivo()  //Opción 11
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("════ VACIAR COLA DE DISPOSITIVO ════");
            Console.ResetColor();

            try
            {
                if (Subredes.Count == 0)
                {
                    Interfaz.Error("No hay ninguna red configurada.\n");
                    Interfaz.Continuar();
                    return;
                }

                Console.WriteLine("\nDispositivos disponibles:");
                Console.WriteLine("\n[ROUTERS]");
                foreach (var subred in Subredes)
                {
                    Console.WriteLine($"- {subred.Enrutador.Nombre} ({subred.Enrutador.IP}) - Paquetes: {subred.Enrutador.ColaEnvio.Count}/4");
                }

                Console.WriteLine("\n[PCs]");
                foreach (var subred in Subredes)
                {
                    Console.WriteLine($"- {subred.Computadora.Nombre} ({subred.Computadora.IP})");
                    Console.WriteLine($"  - Envío: {subred.Computadora.ColaEnvio.Count} paquetes");
                    Console.WriteLine($"  - Recibidos: {subred.Computadora.ColaRecibidos.Count} paquetes");
                }

                Console.Write("\nIngrese la IP del dispositivo: ");
                string ip_Dispositivo = Console.ReadLine().Trim();

                Dispositivo dispositivo = BuscarDispositivoPorIP(ip_Dispositivo);
                if (dispositivo == null)
                {
                    Interfaz.Error("No existe un dispositivo con esa IP.\n");
                    Interfaz.Continuar();
                    return;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"\n¿Está seguro que desea vaciar la(s) cola(s) de {dispositivo.Nombre} ({dispositivo.IP})? (S/N): ");
                Console.ResetColor();
                string confirmacion = Console.ReadLine().Trim().ToUpper();

                if (confirmacion != "S")
                {
                    Console.WriteLine("\nOperación cancelada.");
                    Interfaz.Continuar();
                    return;
                }

                if (dispositivo is Router router)
                {
                    int cantidad = router.ColaEnvio.Count;
                    router.ColaEnvio = new ListaEnlazada<Paquete>();
                    Console.WriteLine($"\nSe han eliminado {cantidad} paquetes del router {router.IP}");
                }
                else if (dispositivo is PC pc)
                {
                    Console.WriteLine("\nSeleccione qué cola desea vaciar:");
                    Console.WriteLine("1. Cola de envío");
                    Console.WriteLine("2. Cola de recibidos");
                    Console.WriteLine("3. Ambas colas");
                    Console.Write("Opción: ");
                    string opcion = Console.ReadLine().Trim();

                    int cantidad_Envio = 0;
                    int cantidad_Recibidos = 0;

                    switch (opcion)
                    {
                        case "1":
                            cantidad_Envio = pc.ColaEnvio.Count;
                            pc.ColaEnvio = new ListaEnlazada<Paquete>();
                            Console.WriteLine($"\nSe han eliminado {cantidad_Envio} paquetes de la cola de envío.");
                            break;
                        case "2":
                            cantidad_Recibidos = pc.ColaRecibidos.Count;
                            pc.ColaRecibidos = new ListaEnlazada<Paquete>();
                            Console.WriteLine($"\nSe han eliminado {cantidad_Recibidos} paquetes de la cola de recibidos.");
                            break;
                        case "3":
                            cantidad_Envio = pc.ColaEnvio.Count;
                            cantidad_Recibidos = pc.ColaRecibidos.Count;
                            pc.ColaEnvio = new ListaEnlazada<Paquete>();
                            pc.ColaRecibidos = new ListaEnlazada<Paquete>();
                            Console.WriteLine($"\nSe han eliminado {cantidad_Envio + cantidad_Recibidos} paquetes (envío: {cantidad_Envio}, recibidos: {cantidad_Recibidos}).");
                            break;
                        default:
                            Interfaz.Error("Opción no válida.\n");
                            break;
                    }
                }

                ActualizarEstadosMensajes();
            }
            catch (Exception ex)
            {
                Interfaz.Error($"Error al vaciar cola: {ex.Message}\n");
            }

            Interfaz.Continuar();
        }

        private void ActualizarEstadosMensajes()
        {
            foreach (var mensaje in Mensajes)
            {
                bool todos_Eliminados = true;
                var nodo = mensaje.Paquetes.Cabeza;

                while (nodo != null)
                {
                    if (nodo.Datos.Estado != EstadoPaquete.Dañado)
                    {
                        todos_Eliminados = false;
                        break;
                    }
                    nodo = nodo.Siguiente;
                }

                if (todos_Eliminados)
                {
                    mensaje.Estado = EstadoMensaje.Dañado;
                }
            }
        }

        public void EliminarSubredEspecifica()  //Opción 12
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("════ ELIMINAR SUBRED ESPECÍFICA ════");
            Console.ResetColor();

            try
            {
                if (Subredes.Count == 0)
                {
                    Interfaz.Error("No hay ninguna red configurada.\n");
                    Interfaz.Continuar();
                    return;
                }

                Console.WriteLine("\nSubredes existentes:");
                foreach (var subred in Subredes)
                {
                    Console.WriteLine($"- Subred {subred.ID}");
                    Console.WriteLine($"  Router: {subred.Enrutador.IP} ({subred.Enrutador.Nombre})");
                    Console.WriteLine($"  PC: {subred.Computadora.IP} ({subred.Computadora.Nombre})");
                    Console.WriteLine($"  Paquetes en router: {subred.Enrutador.ColaEnvio.Count}/4");
                    Console.WriteLine($"  Paquetes en PC (envío/recibidos): {subred.Computadora.ColaEnvio.Count}/{subred.Computadora.ColaRecibidos.Count}\n");
                }

                Console.Write("\nIngrese el ID de la subred a eliminar (ej: 01): ");
                string id_Subred = Console.ReadLine().Trim();

                var subred_Eliminar = Subredes.FirstOrDefault(s => s.ID == id_Subred);
                if (subred_Eliminar == null)
                {
                    Interfaz.Error($"No existe una subred con ID: {id_Subred}\n");
                    Interfaz.Continuar();
                    return;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nSe eliminará la siguiente subred:");
                Console.ResetColor();
                Console.WriteLine($"- ID: {subred_Eliminar.ID}");
                Console.WriteLine($"- Router: {subred_Eliminar.Enrutador.IP} ({subred_Eliminar.Enrutador.Nombre})");
                Console.WriteLine($"- PC: {subred_Eliminar.Computadora.IP} ({subred_Eliminar.Computadora.Nombre})");
                Console.WriteLine($"- Paquetes en router: {subred_Eliminar.Enrutador.ColaEnvio.Count}");
                Console.WriteLine($"- Paquetes en PC: {subred_Eliminar.Computadora.ColaEnvio.Count + subred_Eliminar.Computadora.ColaRecibidos.Count}");

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\n¿Está seguro que desea eliminar esta subred? (S/N): ");
                Console.ResetColor();
                string confirmacion = Console.ReadLine().Trim().ToUpper();

                if (confirmacion != "S")
                {
                    Console.WriteLine("\nOperación cancelada.");
                    Interfaz.Continuar();
                    return;
                }

                int mensajes_Eliminados = 0;
                for (int i = Mensajes.Count - 1; i >= 0; i--)
                {
                    var mensaje = Mensajes[i];
                    if (mensaje.IPOrigen == subred_Eliminar.Computadora.IP ||
                        mensaje.IPDestino == subred_Eliminar.Computadora.IP)
                    {
                        Mensajes.RemoveAt(i);
                        mensajes_Eliminados++;
                    }
                }

                Subredes.Remove(subred_Eliminar);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nSubred eliminada exitosamente!");
                Console.ResetColor();
                Console.WriteLine($"- Mensajes eliminados: {mensajes_Eliminados}");
                Console.WriteLine($"- Total subredes restantes: {Subredes.Count}");
            }
            catch (Exception ex)
            {
                Interfaz.Error($"Error al eliminar subred: {ex.Message}\n");
            }

            Interfaz.Continuar();
        }

        public void EliminarTodaLaRed() //Opción 13
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("════ ELIMINAR TODA LA RED ════");
            Console.ResetColor();

            try
            {
                if (Subredes.Count == 0)
                {
                    Interfaz.Error("No hay ninguna red configurada para eliminar.\n");
                    Interfaz.Continuar();
                    return;
                }

                Console.WriteLine("\nResumen de la red actual:");
                Console.WriteLine($"- Total subredes: {Subredes.Count}");
                Console.WriteLine($"- Total routers: {Subredes.Count}");
                Console.WriteLine($"- Total PCs: {Subredes.Count}");
                Console.WriteLine($"- Total mensajes en el sistema: {Mensajes.Count}");

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n¡ADVERTENCIA! Esta acción es irreversible.");
                Console.WriteLine("Se eliminarán todos los dispositivos y mensajes de la red.");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\n¿Está absolutamente seguro que desea eliminar toda la red? (S/N): ");
                Console.ResetColor();
                string confirmacion = Console.ReadLine().Trim().ToUpper();

                if (confirmacion != "S")
                {
                    Console.WriteLine("\nOperación cancelada.");
                    Interfaz.Continuar();
                    return;
                }

                int total_Subredes = Subredes.Count;
                int total_Mensajes = Mensajes.Count;

                Subredes.Clear();
                Mensajes.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n¡Red eliminada exitosamente!");
                Console.ResetColor();
                Console.WriteLine($"- Subredes eliminadas: {total_Subredes}");
                Console.WriteLine($"- Mensajes eliminados: {total_Mensajes}");
                Console.WriteLine("\nLa red ha sido completamente restablecida.");
            }
            catch (Exception ex)
            {
                Interfaz.Error($"Error al eliminar la red: {ex.Message}\n");
            }

            Interfaz.Continuar();
        }
        #endregion
    }
}
