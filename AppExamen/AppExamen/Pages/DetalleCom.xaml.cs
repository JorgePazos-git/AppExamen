using AppExamen.APIConsumer;
using AppExamen.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppExamen.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetalleCom : ContentPage
    {
        public Competencia competencia;
        private ObservableCollection<DeportistaPuntaje> deportistas;
        public bool IsUpdateButtonVisible { get; set; }
        public bool IsAddButtonVisible { get; set; }
        public bool IsDeleteButtonVisible { get; set; }

        public bool IspuntajeButtonVisible { get; set; }
        public bool IsAddDetButtonVisible { get; set; }

        public DetalleCom()
        {
            InitializeComponent();
            IsUpdateButtonVisible = false;
            IsDeleteButtonVisible = false;
            IsAddButtonVisible = true;
            IsAddDetButtonVisible = true;
            IspuntajeButtonVisible = true;

            // Establecer el contexto de enlace de datos
            BindingContext = this;

            // Evento SelectedIndexChanged para los Pickers de Categoría y Género
            EventHandler pickerSelectedIndexChanged = (sender, e) =>
            {
                if (pickerCategoriaCom.SelectedItem != null && pickerGeneroCom.SelectedItem != null)
                {
                    var lista = listaDeportistasByCatGen(pickerCategoriaCom.SelectedItem.ToString(), pickerGeneroCom.SelectedItem.ToString());
                    List<string> nombresDeportistas = lista.Select(x => $"{x.NombresDep} {x.ApellidosDep}").ToList();
                    pickerDetalleCom.ItemsSource = nombresDeportistas;
                }
            };

            // Asignar el mismo evento a ambos Pickers
            pickerCategoriaCom.SelectedIndexChanged += pickerSelectedIndexChanged;
            pickerGeneroCom.SelectedIndexChanged += pickerSelectedIndexChanged;
            deportistas = new ObservableCollection<DeportistaPuntaje>();
        }

        private string url = "https://apicompetencias.azurewebsites.net/api" + "/Competencia/";

        public DetalleCom(Competencia competencia)
        {
            InitializeComponent();
            IsUpdateButtonVisible = true;
            IsDeleteButtonVisible = true;
            IsAddButtonVisible = false;
            IsAddDetButtonVisible = false;
            IspuntajeButtonVisible = false;

            // Establecer el contexto de enlace de datos
            BindingContext = this;

            this.competencia = competencia;

            if (competencia != null)
            {
                txtIdCom.Text = competencia.IdCom.ToString();
                txtNombreCom.Text = competencia.NombreCom;
                txtFechaInicioCom.Date = competencia.FechaInicioCom;
                txtFechaInicioCom.Format = "d/MM/yyyy";
                txtFechaFinCom.Date = competencia.FechaFinCom;
                txtFechaFinCom.Format = "d/MM/yyyy";
                txtSedeCom.Text = competencia.SedeCom;

                if (competencia.ActivoCom == true)
                {
                    pickerActivoCom.SelectedItem = "Activo";
                }
                else
                {
                    pickerActivoCom.SelectedItem = "Inactivo";
                }

                pickerCategoriaCom.SelectedItem = competencia.CategoriaCom;
                pickerGeneroCom.SelectedItem = competencia.GeneroCom;

                var lista3 = listaDeportistasByCatGen(pickerCategoriaCom.SelectedItem.ToString(), pickerGeneroCom.SelectedItem.ToString());
                List<string> nombresDeportistas2 = lista3.Select(x => $"{x.NombresDep} {x.ApellidosDep}").ToList();
                pickerDetalleCom.ItemsSource = nombresDeportistas2;

                // Evento SelectedIndexChanged para los Pickers de Categoría y Género
                EventHandler pickerSelectedIndexChanged = (sender, e) =>
                {
                    if (pickerCategoriaCom.SelectedItem != null && pickerGeneroCom.SelectedItem != null)
                    {
                        var lista = listaDeportistasByCatGen(pickerCategoriaCom.SelectedItem.ToString(), pickerGeneroCom.SelectedItem.ToString());
                        List<string> nombresDeportistas = lista.Select(x => $"{x.NombresDep} {x.ApellidosDep}").ToList();
                        pickerDetalleCom.ItemsSource = nombresDeportistas;
                    }
                };

                // Asignar el mismo evento a ambos Pickers
                pickerCategoriaCom.SelectedIndexChanged += pickerSelectedIndexChanged;
                pickerGeneroCom.SelectedIndexChanged += pickerSelectedIndexChanged;

                var lista2 = ListaDeportistasDet(competencia.IdCom);
                deportistas = new ObservableCollection<DeportistaPuntaje>(transformar(lista2, int.Parse(txtIdCom.Text)));
                deportistasListView.ItemsSource = deportistas;
            }
        }

        public List<Deportista> listaDeportistasByCatGen(string categoria, string genero)
        {
            var lista = APIConsumer<Deportista>.Select(url.Replace("Competencia", "Deportista"))
                .Where(x => x.CategoriaDep == categoria && x.GeneroDep == genero).ToList();
            return lista;
        }

        public List<DeportistaPuntaje> transformar(List<Deportista> lista, int idCom)
        {
            var lista2 = new List<DeportistaPuntaje>();

            DeportistaPuntaje deportistaPuntaje = new DeportistaPuntaje();


            foreach (Deportista deportista in lista)
            {
                deportistaPuntaje.Deportista = deportista;
                deportistaPuntaje.Puntaje = puntaje(idCom, deportista.IdDep);
                // Agregar el deportista a la lista de deportistas seleccionados
                lista2.Add(deportistaPuntaje);
            }

            return lista2;
        }

        public string puntaje(int compe, int dep)
        {
            return (APIConsumer<DetalleCompetencia>.Select(url.Replace("Competencia", "DetalleCompetencia"))
                .Where(x => x.IdCom == compe && x.IdDep == dep)).ToList()[0].Puntaje;

            
        }
        public List<DetalleCompetencia> listaDeportistasCom(int id)
        {
            var lista = APIConsumer<DetalleCompetencia>.Select(url.Replace("Competencia", "DetalleCompetencia"))
                .Where(x => x.IdCom == id).ToList();
      
            return lista;
        }

        public List<Deportista> ListaDeportistasDet(int id)
        {
            List<DetalleCompetencia> lista = listaDeportistasCom(id).ToList();

            List<Deportista> lista2 = new List<Deportista>();

            foreach (var item in lista)
            {
                lista2.Add(DeportistasById(item.IdDep));
            }

            return lista2;
        }

        public Deportista DeportistasById(int id)
        {
            var lista = APIConsumer<Deportista>.SelectOne(url.Replace("Competencia", "Deportista") + id.ToString());

            return lista;
        }

        public Deportista deportistaByNombre(string nombres)
        {
            var lista = APIConsumer<Deportista>.Select(url.Replace("Competencia", "Deportista")).
                Where(x => $"{x.NombresDep} {x.ApellidosDep}" == nombres).ToList();
            return lista[0];
        }

        private void ADDdetalle_Clicked(object sender, EventArgs e)
        {
            Deportista deportistaSeleccionado = deportistaByNombre(pickerDetalleCom.SelectedItem.ToString());
            DeportistaPuntaje deportistaPuntaje = new DeportistaPuntaje();
            deportistaPuntaje.Deportista = deportistaSeleccionado;
            deportistaPuntaje.Puntaje = txtPuntaje.Text;

            // Agregar el deportista a la lista de deportistas seleccionados
            deportistas.Add(deportistaPuntaje);

            // Actualizar la fuente de datos del ListView
            deportistasListView.ItemsSource = deportistas;
            txtPuntaje.Text = "";
            pickerDetalleCom.SelectedItem = null;
        }


        private void DeshacerButton_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            DeportistaPuntaje deportista = (DeportistaPuntaje)button.BindingContext;

            deportistas.Remove(deportista);
            deportistasListView.ItemsSource = deportistas;
        }

        private async void OnAgregarClicked(object sender, EventArgs e)
        {

            try
            {
                // Obtener la lista de DeportistaPuntaje desde el ListView
                var deportistasPuntajes = this.deportistas;
                // Validar que se haya obtenido la lista correctamente
                if (deportistasPuntajes != null)
                {
                    var datos = new Competencia
                    {
                        IdCom = 0,
                        NombreCom = txtNombreCom.Text,
                        ActivoCom = pickerActivoCom.SelectedItem.ToString() == "Activo",
                        CategoriaCom = pickerCategoriaCom.SelectedItem.ToString(),
                        GeneroCom = pickerGeneroCom.SelectedItem.ToString(),
                        FechaFinCom = txtFechaFinCom.Date,
                        FechaInicioCom = txtFechaInicioCom.Date,
                        SedeCom = txtSedeCom.Text

                    };

                    var detalles = new List<DetalleCompetencia>();

                    APIConsumer<Competencia>.Insert(url, datos);

                    var competencias = APIConsumer<Competencia>.Select(url);
                    var ultimaCompetencia = competencias.OrderByDescending(c => c.IdCom).FirstOrDefault();


                    // Iterar sobre la lista de DeportistaPuntaje para obtener los detalles
                    foreach (var deportistaPuntaje in deportistasPuntajes)
                    {
                        var detalle = new DetalleCompetencia
                        {
                            IdDetalle = 0,
                            Puntaje = deportistaPuntaje.Puntaje, // Obtener el puntaje del DeportistaPuntaje
                            IdCom = ultimaCompetencia.IdCom,
                            IdDep = deportistaPuntaje.Deportista.IdDep
                        };

                        APIConsumer<DetalleCompetencia>.Insert(url.Replace("Competencia", "DetalleCompetencia"), detalle);
                        detalles.Add(detalle);
                    }

                    await DisplayAlert("Éxito", "Los datos se han ingresado correctamente.", "Aceptar");
                    await Navigation.PopToRootAsync();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ha ocurrido un error al ingresar los datos: " + ex.Message, "Aceptar");
            }
        }

        private async void OnActualizarClicked(object sender, EventArgs e)
        {
            try
            {
                // Obtener la lista de DeportistaPuntaje desde el ListView
                var deportistasPuntajes = this.deportistas;
                // Validar que se haya obtenido la lista correctamente
                if (deportistasPuntajes != null)
                {
                    var datos = new Competencia
                    {
                        IdCom = int.Parse(txtIdCom.Text),
                        NombreCom = txtNombreCom.Text,
                        ActivoCom = pickerActivoCom.SelectedItem.ToString() == "Activo",
                        CategoriaCom = pickerCategoriaCom.SelectedItem.ToString(),
                        GeneroCom = pickerGeneroCom.SelectedItem.ToString(),
                        FechaFinCom = txtFechaFinCom.Date,
                        FechaInicioCom = txtFechaInicioCom.Date,
                        SedeCom = txtSedeCom.Text

                    };

                    APIConsumer<Competencia>.Update(url + datos.IdCom.ToString(), datos);

                    await DisplayAlert("Éxito", "Los datos se han actualizado correctamente.", "Aceptar");
                    await Navigation.PopToRootAsync();
                }
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
                // Obtener la lista de DeportistaPuntaje desde el ListView
                var deportistasPuntajes = this.deportistas;
                // Validar que se haya obtenido la lista correctamente
                if (deportistasPuntajes != null)
                {
                    var datos = new Competencia
                    {
                        IdCom = int.Parse(txtIdCom.Text),
                        NombreCom = txtNombreCom.Text,
                        ActivoCom = false,
                        CategoriaCom = pickerCategoriaCom.SelectedItem.ToString(),
                        GeneroCom = pickerGeneroCom.SelectedItem.ToString(),
                        FechaFinCom = txtFechaFinCom.Date,
                        FechaInicioCom = txtFechaInicioCom.Date,
                        SedeCom = txtSedeCom.Text

                    };

                    APIConsumer<Competencia>.Update(url + datos.IdCom.ToString(), datos);

                    await DisplayAlert("Éxito", "Competencia Deshabilitada.", "Aceptar");
                    await Navigation.PopToRootAsync();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ha ocurrido un error : " + ex.Message, "Aceptar");
            }
        }
    }
}