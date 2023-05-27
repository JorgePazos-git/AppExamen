using System;
using System.Collections.Generic;
using System.Text;

namespace AppExamen.Models
{
    public class Deportista
    {
        public int IdDep { get; set; }

        public string NombresDep { get; set; }

        public string ApellidosDep { get; set; }

        public string CedulaDep { get; set; }

        public bool ActivoDep { get; set; }

        public string ClubDep { get; set; }

        public string GeneroDep { get; set; }

        public string CategoriaDep { get; set; }

        public string ProvinciaDep { get; set; }

        public int IdEnt { get; set; }

        public virtual ICollection<DetalleCompetencia> DetalleCompetenciaList { get; set; } = new List<DetalleCompetencia>();

        public virtual Entrenador IdEntNavigation { get; set; }
    }
}
