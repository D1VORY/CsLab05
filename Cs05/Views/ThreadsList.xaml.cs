using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Cs05.ViewModels;

namespace Cs05.Views
{
    /// <summary>
    /// Логика взаимодействия для ThreadsList.xaml
    /// </summary>
    public partial class ThreadsList : Window
    {
        public ThreadsList()
        {
            InitializeComponent();
        }

        public ThreadsList(Process pr) : this()
        {


            DataContext = new ThreadListViewModel(pr);
        }
    }
}
