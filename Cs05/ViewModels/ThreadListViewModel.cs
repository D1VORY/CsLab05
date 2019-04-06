using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Cs05.Models;
using Cs05.Tools;

namespace Cs05.ViewModels
{
    class ThreadListViewModel : BaseModel
    {
        private ObservableCollection<ProcessThread> _threads;

        public ObservableCollection<ProcessThread> Threads
        {
            get => _threads;
            set => _threads = value;
        }


        public ThreadListViewModel(Process pr)
        {
            
            try
            {
                Threads = new ObservableCollection<ProcessThread>(pr.Threads.Cast<ProcessThread>());
               
            }catch(SystemException )
            {
                MessageBox.Show("System not supported");
                
            }

            

        }

    }
}
