using AppExamen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppExamen.APIConsumer;
using System.Collections;

namespace AppExamen.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetalleDep : ContentPage
    {
        public Deportista deportista;
        public bool IsUpdateButtonVisible { get; set; }
        public bool IsAddButtonVisible { get; set; }
        public bool IsDeleteButtonVisible { get; set; }

        public DetalleDep()
        {
            InitializeComponent();
            IsUpdateButtonVisible = false;
            IsDeleteButtonVisible = false;
            IsAddButtonVisible = true;

            // Establecer el contexto de enlace de datos
            BindingContext = this;

            var lista = listaEntrenadores();
            List<string> nombresEntrenadores = lista.Select(x => $"{x.NombresEnt} {x.ApellidosEnt}").ToList();
            pickerEntrenadorDep.ItemsSource = nombresEntrenadores;
        }

        private string url = "https://apicompetencias.azurewebsites.net/api" + "/Deportista/";

        public Entrenador entrendorById(int id)
        {
            return APIConsumer<Entrenador>.SelectOne(url.Replace("Deportista", "Entrenador") + id.ToString());
        }

        public List<Entrenador> listaEntrenadores()
        {
            var lista = APIConsumer<Entrenador>.Select(url.Replace("Deportista", "Entrenador")).ToList();
            return lista;
        }

        public Entrenador entrenadorByNombre(string nombres)
        {
            var lista = APIConsumer<Entrenador>.Select(url.Replace("Deportista", "Entrenador")).
                Where(x => $"{x.NombresEnt} {x.ApellidosEnt}" == nombres).ToList();
            return lista[0];
        }

        public DetalleDep(Deportista deportista)
        {
            InitializeComponent();

            IsUpdateButtonVisible = true;
            IsDeleteButtonVisible = true;
            IsAddButtonVisible = false;

            // Establecer el contexto de enlace de datos
            BindingContext = this;

            this.deportista = deportista;
            if(deportista != null)
            {
                txtIdDep.Text = deportista.IdDep.ToString();
                txtNombresDep.Text = deportista.NombresDep;
                txtApellidosDep.Text = deportista.ApellidosDep;
                txtCedulaDep.Text = deportista.CedulaDep;

                pickerProvinciaDep.SelectedItem = deportista.ProvinciaDep;
                if (deportista.ActivoDep == true)
                {
                    pickerActivoDep.SelectedItem = "Activo";
                }
                else
                {
                    pickerActivoDep.SelectedItem = "Inactivo";
                }

                pickerCategoriaDep.SelectedItem = deportista.CategoriaDep;
                txtClubDep.Text = deportista.ClubDep;
                pickerGeneroDep.SelectedItem = deportista.GeneroDep;

                var lista = listaEntrenadores();
                List<string> nombresEntrenadores = lista.Select(x => $"{x.NombresEnt} {x.ApellidosEnt}").ToList();
                pickerEntrenadorDep.ItemsSource = nombresEntrenadores;

                Entrenador entrenador = entrendorById(deportista.IdEnt);
                pickerEntrenadorDep.SelectedItem = entrenador.NombresEnt + " " + entrenador.ApellidosEnt;
            }
        }

        private async void OnActualizarClicked(object sender, EventArgs e)
        {
            try
            {
                var datos = new Deportista
                {
                    IdDep = int.Parse(txtIdDep.Text),
                    NombresDep = txtNombresDep.Text,
                    ApellidosDep = txtApellidosDep.Text,
                    CedulaDep = txtCedulaDep.Text,
                    ActivoDep = pickerActivoDep.SelectedItem.ToString() == "Activo",
                    ProvinciaDep = pickerProvinciaDep.SelectedItem.ToString(),
                    CategoriaDep = pickerCategoriaDep.SelectedItem.ToString(),
                    ClubDep = txtClubDep.Text,
                    GeneroDep = pickerGeneroDep.SelectedItem.ToString(),
                    IdEnt = entrenadorByNombre(pickerEntrenadorDep.SelectedItem.ToString()).IdEnt,
                    IdEntNavigation = entrenadorByNombre(pickerEntrenadorDep.SelectedItem.ToString()),
                    DetalleCompetenciaList = deportista.DetalleCompetenciaList
                };

                APIConsumer<Deportista>.Update(url + txtIdDep.Text.ToString(), datos);

                await DisplayAlert("Éxito", "Los datos se han actualizado correctamente.", "Aceptar");
                await Navigation.PopToRootAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ha ocurrido un error al actualizar los datos: " + ex.Message, "Aceptar");
            }
        }

        private async void OnAgregarClicked(object sender, EventArgs e)
        {
            try
            {
                var datos = new Deportista
                {
                    IdDep = 0,
                    NombresDep = txtNombresDep.Text,
                    ApellidosDep = txtApellidosDep.Text,
                    CedulaDep = txtCedulaDep.Text,
                    ActivoDep = pickerActivoDep.SelectedItem.ToString() == "Activo",
                    ProvinciaDep = pickerProvinciaDep.SelectedItem.ToString(),
                    CategoriaDep = pickerCategoriaDep.SelectedItem.ToString(),
                    ClubDep = txtClubDep.Text,
                    GeneroDep = pickerGeneroDep.SelectedItem.ToString(),
                    IdEnt = entrenadorByNombre(pickerEntrenadorDep.SelectedItem.ToString()).IdEnt,
                };

                APIConsumer<Deportista>.Insert(url, datos);

                await DisplayAlert("Éxito", "Los datos se han ingresado correctamente.", "Aceptar");
                await Navigation.PopToRootAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ha ocurrido un error al ingresar los datos: " + ex.Message, "Aceptar");
            }
        }

        private async void OnEliminarClicked(object sender, EventArgs e)
        {
            try
            {
                APIConsumer<Deportista>.Delete(url + txtIdDep.Text.ToString());

                await DisplayAlert("Éxito", "Deportista Eliminado.", "Aceptar");
                await Navigation.PopToRootAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ha ocurrido un error al ingresar los datos: " + ex.Message, "Aceptar");
            }
        }
    }
}