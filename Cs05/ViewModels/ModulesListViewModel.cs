using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Cs05.ViewModels
{
    class ModulesListViewModel
    {

        private ObservableCollection<ProcessModule> _modules;

        public ObservableCollection<ProcessModule> Modules
        {
            get => _modules;
            set => _modules = value;
        }


        public ModulesListViewModel(Process pr)
        {
            
            try
            {
                Modules = new ObservableCollection<ProcessModule>(pr.Modules.Cast<ProcessModule>());
                
            }
            catch (SystemException )
            {
                MessageBox.Show("System not supported");

            }



        }
    }
}
