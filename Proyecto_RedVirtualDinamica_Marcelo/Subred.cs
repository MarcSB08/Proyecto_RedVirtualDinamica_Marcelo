using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_RedVirtualDinamica_Marcelo
{
    public class Subred
    {
        #region Atributos
        public string ID { get; set; }
        public PC Computadora { get; set; }
        public Router Enrutador { get; set; }
        #endregion

        #region Metodos
        public Subred(string id, string ipBase, Red red)
        {
            ID = id;
            Enrutador = new Router($"{ipBase}.0", $"R{id}", red);
            Computadora = new PC($"{ipBase}.01", $"PC{id}");
        }
        #endregion
    }
}
