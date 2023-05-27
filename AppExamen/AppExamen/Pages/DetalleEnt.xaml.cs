using AppExamen.APIConsumer;
using AppExamen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;

namespace AppExamen.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetalleEnt : ContentPage
    {

        public Entrenador entrenador;

        public bool IsUpdateButtonVisible { get; set; }
        public bool IsAddButtonVisible { get; set; }
        public bool IsDeleteButtonVisible { get; set; }

        public DetalleEnt()
        {
            InitializeComponent();

            IsUpdateButtonVisible = false;
            IsDeleteButtonVisible = false;
            IsAddButtonVisible = true;

            // Establecer el contexto de enlace de datos
            BindingContext = this;
        }

        private string url = "https://apicompetencias.azurewebsites.net/api" + "/Entrenador/";

        public List<Deportista> listaDepEnt(int idEnt)
        {
            var lista = APIConsumer<Deportista>.Select(url.Replace("Entrenador", "Deportista")).Where(x => x.IdEnt == idEnt);
            return lista.ToList();
        }
        
        public DetalleEnt(Entrenador entrenador)
        {
            InitializeComponent();
            IsUpdateButtonVisible = true;
            IsDeleteButtonVisible = true;
            IsAddButtonVisible = false;

            // Establecer el contexto de enlace de datos
            BindingContext = this;

            this.entrenador = entrenador;

            if (entrenador != null)
            {
                txtIdEnt.Text = entrenador.IdEnt.ToString();
                txtNombresEnt.Text = entrenador.NombresEnt;
                txtApellidosEnt.Text = entrenador.ApellidosEnt;
                txtCedulaEnt.Text = entrenador.CedulaEnt;
                pickerProvinciaEnt.SelectedItem = entrenador.ProvinciaEnt;
                if(entrenador.ActivoEnt == true)
                {
                    pickerActivoEnt.SelectedItem = "Activo";
                }
                else
                {
                    pickerActivoEnt.SelectedItem = "Inactivo";
                }

                foreach (var dep in listaDepEnt(entrenador.IdEnt))
                {
                    txtDepEnt.Text += dep.NombresDep + " "  +dep.ApellidosDep + "\n\n"; 
                }            
            }
        }

        private async void OnActualizarClicked(object sender, EventArgs e)
        {
            try
            {
                var datos = new Entrenador
                {
                    IdEnt = int.Parse(txtIdEnt.Text),
                    NombresEnt = txtNombresEnt.Text,
                    ApellidosEnt = txtApellidosEnt.Text,
                    CedulaEnt = txtCedulaEnt.Text,
                    ActivoEnt = pickerActivoEnt.SelectedItem.ToString() == "Activo",
                    ProvinciaEnt = pickerProvinciaEnt.SelectedItem.ToString(),
                    DeportistaList = entrenador.DeportistaList
                };

                APIConsumer<Entrenador>.Update(url + txtIdEnt.Text.ToString(), datos);

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
                var datos = new Entrenador
                {
                    IdEnt = 0,
                    NombresEnt = txtNombresEnt.Text,
                    ApellidosEnt = txtApellidosEnt.Text,
                    CedulaEnt = txtCedulaEnt.Text,
                    ActivoEnt = pickerActivoEnt.SelectedItem.ToString() == "Activo",
                    ProvinciaEnt = pickerProvinciaEnt.SelectedItem.ToString()
                };

                APIConsumer<Entrenador>.Insert(url, datos);

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
                var datos = new Entrenador
                {
                    IdEnt = int.Parse(txtIdEnt.Text),
                    NombresEnt = txtNombresEnt.Text,
                    ApellidosEnt = txtApellidosEnt.Text,
                    CedulaEnt = txtCedulaEnt.Text,
                    ActivoEnt = false,         
                    ProvinciaEnt = pickerProvinciaEnt.SelectedItem.ToString(),
                    DeportistaList = entrenador.DeportistaList
                };

                APIConsumer<Entrenador>.Update(url + txtIdEnt.Text.ToString(), datos);


                await DisplayAlert("Éxito", "Entrenador Deshabilitado.", "Aceptar");
                await Navigation.PopToRootAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ha ocurrido un error al ingresar los datos: " + ex.Message, "Aceptar");
            }
        }
    }
}