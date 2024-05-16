using PruebaTecnicaEQ.Models;
using PruebaTecnicaEQ.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Data.SqlClient;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using Xamarin.Forms.Xaml;

namespace PruebaTecnicaEQ.ViewModel
{
    public class VMHome : BaseViewModel
    {
        #region variables
        string _Mensaje;
        public List<MListaTareas> listaTareas { get; set; }
        string rutaArchivo = "";
        string nombreArchivo = "datos.json";
        public bool IsRefreshing { get; set; }
        public ICommand RefreshCommand { get; set; }
        #endregion


        #region Constructor
        public VMHome(INavigation navigation)
        {
            listaTareas = new List<MListaTareas>();
            Navigation = navigation;
            rutaArchivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), nombreArchivo);
            if (!File.Exists(rutaArchivo))
            {
                File.WriteAllText(rutaArchivo, "");
            }
            CargarTareas();
            RefreshCommand = new Command(async () => await RefreshItems());
        }
        #endregion

  

        #region Objetos
        public string Mensaje
        {
            get { return _Mensaje; }
            set { SetValue(ref _Mensaje, value); }
        }
        #endregion

        #region Procesos

      
        public async Task Alerta()
        {
            await Navigation.PushAsync(new Page1());
            //await DisplayAlert("Titulo", _Mensaje, "Ok");
        }
        public async Task Eliminar(MListaTareas tarea)
        {
            listaTareas.RemoveAll(t => t.id == tarea.id);
            string json = JsonConvert.SerializeObject(listaTareas);
            File.WriteAllText(rutaArchivo, json);
            await DisplayAlert("Eliminado!!", $"Se ha eliminado la tarea {tarea.nombre}", "OK");
        }

        public async Task ChechBoxChanged(MListaTareas tarea)
        {
            await DisplayAlert("Eliminado!!", $"Se ha eliminado la tarea {tarea.nombre}", "OK");
        }

        public void CargarTareas()
        {
            try
            {
                string jsonLeido = File.ReadAllText(rutaArchivo);
                if (jsonLeido != "" || jsonLeido != "[]")
                {
                    listaTareas = JsonConvert.DeserializeObject<List<MListaTareas>>(jsonLeido).OrderBy(x => x.categoriaid).ToList();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void ViewAddTarea()
        {
           Navigation.PushAsync(new AddTarea());
        }


        private async Task RefreshItems()
        {
            IsRefreshing = true; // Establece IsRefreshing en true para mostrar el indicador de actividad
            CargarTareas();
            // Aquí colocas la lógica para recargar los datos de tu lista
            // Por ejemplo, puedes volver a cargar tus elementos desde una base de datos o servicio web

            // Luego, detén el indicador de actividad cuando hayas terminado de actualizar los datos
            IsRefreshing = false;
        }
        #endregion

        #region Comandos 
        public ICommand AlertaCommand => new Command(async () => await Alerta());
        //public ICommand ProcesoSimpleCommand => new Command(ProcesoSincrono);
        //public ICommand Alertacommand => new Command<Musuario>(async (p) => await Alerta(p));
        public ICommand EliminarCommand => new Command<MListaTareas>(async (p) => await Eliminar(p));
        public ICommand ViewRegistroCommand => new Command(ViewAddTarea);
        public ICommand ChechBoxChangedCommand_ => new Command<MListaTareas>(async (p) => await ChechBoxChanged(p));
        #endregion
    }
}
