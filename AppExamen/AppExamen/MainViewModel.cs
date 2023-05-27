using System.Collections.Generic;
using System.ComponentModel;
using AppExamen.Models;
using Xamarin.Forms;

namespace AppExamen.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private bool isMenuVisible;
        private Frame menuFrame;

        public bool IsMenuVisible
        {
            get { return isMenuVisible; }
            set
            {
                if (isMenuVisible != value)
                {
                    isMenuVisible = value;
                    OnPropertyChanged(nameof(IsMenuVisible));

                    // Actualizar la visibilidad del marco del menú
                    if (menuFrame != null)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            menuFrame.IsVisible = isMenuVisible;
                        });
                    }
                }
            }
        }

        public List<MenuOption> MenuOptions { get; set; }

        public Frame MenuFrame
        {
            set { menuFrame = value; }
        }

        public MainViewModel()
        {
            // Inicializar las opciones del menú
            MenuOptions = new List<MenuOption>
        {
            new MenuOption { Title = "Deportista" },
            new MenuOption { Title = "Entrenador" },
            new MenuOption { Title = "Competencia" }
        };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}

