using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Cs05.ViewModels;

namespace Cs05.Views
{
    /// <summary>
    /// Логика взаимодействия для TaskDispatcher2.xaml
    /// </summary>
    public partial class TaskDispatcher2 : UserControl
    {
        public TaskDispatcher2()
        {
            InitializeComponent();
         
            //CollectionViewSource itemCollectionViewSource = (CollectionViewSource)(FindResource("ItemCollectionViewSource"));

            DataContext = new TaskDispatcherViewModel();
            
           
        }
    }
}
