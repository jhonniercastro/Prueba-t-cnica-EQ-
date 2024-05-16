using Newtonsoft.Json;
using PruebaTecnicaEQ.Models;
using PruebaTecnicaEQ.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PruebaTecnicaEQ.ViewModel
{
    public class VMAddtarea : BaseViewModel
    {
        public List<MCategorias> categorias { get; set; }
        public MCategorias categoriaSeleccionada { get; set; }
        public string NombreTarea { get; set; }
        public string DescripcionTarea { get; set; }

        #region Constructor
        public VMAddtarea(INavigation navigation)
        {
            categorias = new List<MCategorias>();
            CargarCategorias();
            Navigation = navigation;
        }
        #endregion


        public void CargarCategorias()
        {
            categorias = new List<MCategorias>();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync("https://eqtools.eqtax.com:8585/api/TechnicalTest").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string content = response.Content.ReadAsStringAsync().Result;
                        categorias = JsonConvert.DeserializeObject<List<MCategorias>>(content);
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar el error
                Console.WriteLine("Error al consumir la API: " + ex.Message);
            }
        }


        public void GuardarTarea()
        {
            if (string.IsNullOrEmpty(NombreTarea) || string.IsNullOrEmpty(DescripcionTarea) || categoriaSeleccionada== null)
            {
                DisplayAlert("Adevertencia!!", $"Existen campos vacios", "OK");
                return;
            }


            string rutaArchivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "datos.json");
            string jsonLeido = File.ReadAllText(rutaArchivo);
            List<MListaTareas> listaTareas = new List<MListaTareas>();
            if (jsonLeido != "")
            {
                listaTareas = JsonConvert.DeserializeObject<List<MListaTareas>>(jsonLeido).OrderBy(x => x.categoriaid).ToList();
            }
            int ultimoId = 0;
            if (listaTareas.Count() > 0)
            {
                ultimoId = listaTareas.Max(t => t.id);
            }

            listaTareas.Add(new MListaTareas
            {
                id = ultimoId + 1,
                nombre = NombreTarea,
                descripcion = DescripcionTarea,
                estado = false,
                categoriaid = categoriaSeleccionada.id,
                descripcionCategoria = categoriaSeleccionada.category
            });

            string json = JsonConvert.SerializeObject(listaTareas);
            File.WriteAllText(rutaArchivo, json);
            DisplayAlert("Exito!!", $"Tarea creada correctamente", "OK");
        }

        public ICommand GuardarTareaCommand => new Command(GuardarTarea);



    }


}
