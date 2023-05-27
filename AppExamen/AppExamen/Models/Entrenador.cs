using System;
using System.Collections.Generic;
using System.Text;

namespace AppExamen.Models
{
    public class Entrenador
    {
        public int IdEnt { get; set; }

        public string NombresEnt { get; set; }

        public string ApellidosEnt { get; set; }

        public string CedulaEnt { get; set; }

        public string ProvinciaEnt { get; set; }

        public bool? ActivoEnt { get; set; }

        public virtual ICollection<Deportista> DeportistaList { get; set; } = new List<Deportista>();
    }
}
