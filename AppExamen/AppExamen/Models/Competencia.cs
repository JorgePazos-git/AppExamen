using System;
using System.Collections.Generic;
using System.Text;

namespace AppExamen.Models
{
    public class Competencia
    {
        public int IdCom { get; set; }

        public string NombreCom { get; set; }

        public DateTime FechaInicioCom { get; set; }

        public DateTime FechaFinCom { get; set; }

        public bool ActivoCom { get; set; }

        public string GeneroCom { get; set; }

        public string CategoriaCom { get; set; }

        public string SedeCom { get; set; }

        public virtual List<DetalleCompetencia> DetalleCompetenciaList { get; set; } = new List<DetalleCompetencia>();
    }
}
