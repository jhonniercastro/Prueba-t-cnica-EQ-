using PruebaTecnicaEQ.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PruebaTecnicaEQ.Views
{
    public class VMpatron:BaseViewModel
    {        

        #region variables
        string _texto;
        #endregion


        #region Constructor
        public VMpatron(INavigation navigation)
        {
            Navigation = navigation;
        }
        #endregion



        #region Objetos
        public string Texto
        {
            get {return _texto;}
            set {SetValue(ref _texto, value);}
        }
        #endregion

        #region Procesos
        public async Task ProcesoAsync()
        {

        }
        public void ProcesoSimple()
        {

        }
        #endregion

        #region Comandos
        public ICommand ProcesoAsyncCommand => new Command(async() =>await ProcesoAsync());
        public ICommand ProcesoSimpleCommand => new Command(ProcesoSimple);
        #endregion
    
    }
}
