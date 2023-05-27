using AppExamen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppExamen.APIConsumer;

namespace AppExamen.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeportistaPage : ContentPage
    {

        private string url = "https://apicompetencias.azurewebsites.net/api" + "/Deportista/";

        public DeportistaPage()
        {
            InitializeComponent();
            List<Deportista> listaDeportistas = ReadAllDeportistas(); // Obtén la lista de deportistas desde tu proceso de obtención de datos

            deportistasListView.ItemsSource = listaDeportistas;
        }

        private List<Deportista> ReadAllDeportistas()
        {
            var deportistas = APIConsumer<Deportista>.Select(url);
            var lista = deportistas.Select(f => new Deportista
            {
                IdDep = f.IdDep,
                NombresDep = f.NombresDep,
                ApellidosDep = f.ApellidosDep,
                CedulaDep = f.CedulaDep,
                ActivoDep = f.ActivoDep,
                ClubDep = f.ClubDep,
                GeneroDep = f.GeneroDep,
                CategoriaDep = f.CategoriaDep,
                ProvinciaDep = f.ProvinciaDep,
                IdEnt = f.IdEnt
            }).ToList();

            return lista;

        }

        private void Label_Tapped(object sender, EventArgs e)
        {
            // Obtén el objeto o la información completa relacionada con el elemento seleccionado
            var selectedItem = ((Label)sender).BindingContext;

            Navigation.PushAsync(new DetalleDep((Deportista)selectedItem)); // Ejemplo en Xamarin.Forms
        }
        private void AgregarButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DetalleDep());
        }
    }
}