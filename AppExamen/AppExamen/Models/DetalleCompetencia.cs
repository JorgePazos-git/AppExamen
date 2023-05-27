using System;
using System.Collections.Generic;
using System.Text;

namespace AppExamen.Models
{
    public class DetalleCompetencia
    {
        public int IdDetalle { get; set; }

        public string Puntaje { get; set; }

        public int IdDep { get; set; }

        public int IdCom { get; set; }

        public virtual Competencia IdComNavigation { get; set; }

        public virtual Deportista IdDepNavigation { get; set; }
    }
}
