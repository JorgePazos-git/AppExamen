using AppExamen.APIConsumer;
using AppExamen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppExamen.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompetenciaPage : ContentPage
    {
        private string url = "https://apicompetencias.azurewebsites.net/api" + "/Competencia/";

        public CompetenciaPage()
        {
            InitializeComponent();
            List<Competencia> listaCompetencias = ReadAllCompetencias(); // Obtén la lista de deportistas desde tu proceso de obtención de datos

            competenciasListView.ItemsSource = listaCompetencias;
        }

        private List<Competencia> ReadAllCompetencias()
        {
            var competencias = APIConsumer<Competencia>.Select(url);
            var lista = competencias.Select(f => new Competencia
            {
                IdCom = f.IdCom,
                NombreCom = f.NombreCom,
                CategoriaCom = f.CategoriaCom,
                FechaFinCom = f.FechaFinCom,
                FechaInicioCom = f.FechaInicioCom,
                ActivoCom = f.ActivoCom,
                SedeCom = f.SedeCom,
                GeneroCom = f.GeneroCom
            }).ToList();

            return lista;

        }

        private void Label_Tapped(object sender, EventArgs e)
        {
            // Obtén el objeto o la información completa relacionada con el elemento seleccionado
            var selectedItem = ((Label)sender).BindingContext;

            Navigation.PushAsync(new DetalleCom((Competencia)selectedItem)); // Ejemplo en Xamarin.Forms
        }

        private void AgregarButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DetalleCom());
        }
    }
}