using AppExamen.APIConsumer;
using AppExamen.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppExamen.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntrenadorPage : ContentPage
    {
        private string url = "https://apicompetencias.azurewebsites.net/api" + "/Entrenador/";

        public EntrenadorPage()
        {
            InitializeComponent();
            List<Entrenador> listaEntrenadores = ReadAllEntrenadores(); // Obtén la lista de deportistas desde tu proceso de obtención de datos

            entrenadoresListView.ItemsSource = listaEntrenadores;
        }

        private List<Entrenador> ReadAllEntrenadores()
        {
            var entrenadores = APIConsumer<Entrenador>.Select(url);
            var lista = entrenadores.Select(f => new Entrenador
            {
                IdEnt = f.IdEnt,
                NombresEnt = f.NombresEnt,
                ApellidosEnt = f.ApellidosEnt,
                CedulaEnt = f.CedulaEnt,
                ActivoEnt = f.ActivoEnt,
                ProvinciaEnt = f.ProvinciaEnt
            }).ToList();

            return lista;

        }

        private void Label_Tapped(object sender, EventArgs e)
        {
            // Obtén el objeto o la información completa relacionada con el elemento seleccionado
            var selectedItem = ((Label)sender).BindingContext;

            Navigation.PushAsync(new DetalleEnt((Entrenador)selectedItem)); // Ejemplo en Xamarin.Forms
        }

        private void AgregarButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DetalleEnt());
        }
    }
}