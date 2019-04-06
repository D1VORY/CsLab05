using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Security.Cryptography;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using Cs05.Models;
using Cs05.Tools;
using Cs05.Views;
using ProcessThread = System.Diagnostics.ProcessThread;
using ThreadState = System.Diagnostics.ThreadState;

namespace Cs05.ViewModels
{
    class TaskDispatcherViewModel :BaseModel
    {
        private ObservableCollection<MyTask> _tasks;
        private MyTask _selectedTask;

        private RelayCommand _showThreadsCommand;
        private RelayCommand _showModulesCommand;
        private RelayCommand _terminateCommand;
        private RelayCommand _showFolderCommand;

       

        public RelayCommand ShowFolderCommand => _showFolderCommand ??
                                                (_showFolderCommand = new RelayCommand(ShowFolderImplementation,
                                                    CanExecuteCommand));
        private void ShowFolderImplementation(object o)
        {
            try
            {
                Process.Start("explorer.exe", Process.GetProcessById(SelectedTask.Id).MainModule.FileName);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

                throw;
            }

        }

        public RelayCommand TerminateCommand => _terminateCommand ??
                                                  (_terminateCommand = new RelayCommand(TerminateImplementation,
                                                      CanExecuteCommand));
        private void TerminateImplementation(object o)
        {
            Process.GetProcessById(SelectedTask.Id).Kill();
        }


        public RelayCommand ShowThreadsCommand => _showThreadsCommand ??
                                                  (_showThreadsCommand = new RelayCommand(ShowThreadsImplementation,
                                                      CanExecuteCommand));
        private void ShowThreadsImplementation(object o)
        {

           
                try
                {
                    //TODO MAke state to be showed
                    ThreadsList tl = new ThreadsList(Process.GetProcessById(SelectedTask.Id));
                    tl.Show();
                }
                catch (ArgumentException )
                {


                }
            

        }


        public RelayCommand ShowModulesCommand => _showModulesCommand ??
                                                  (_showModulesCommand = new RelayCommand(ShowModulesImplementation,
                                                      CanExecuteCommand));
        private  void ShowModulesImplementation(object o)
        {
          
                try
                {
                    
                    ModulesList ml = new ModulesList(Process.GetProcessById(SelectedTask.Id));
                    ml.Show();
                }
                catch (ArgumentException )
                {


                }
            
        }

        public ObservableCollection<MyTask> Tasks
        {
            get => _tasks;
            set => _tasks = value;
        }

        public MyTask SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                
            }

        }


       public TaskDispatcherViewModel()
        {
           
            Tasks = new ObservableCollection<MyTask>();
            
            
                foreach (Process pr in Process.GetProcesses())
                {
                    try
                    {

                    Tasks.Add(new MyTask(pr.ProcessName, pr.Id, pr.Responding,CalculateCpu(pr) ,CalculateRam(pr)
                        ,
                         pr.Threads.Count, pr.MachineName, pr.MainModule.FileName));
                    }
                    catch (Exception e )
                    {
                        //MessageBox.Show(e.Message);
                    }
                }

            Thread myThread = new Thread(new ThreadStart(UpdateList));
            myThread.Start();


            Thread updatePropertiesThread = new Thread(UpdateProperties);
            updatePropertiesThread.Start();

            
        }



        private void UpdateProperties()
        {

            while (true)
            {  
                    lock (Tasks)
                    {

                        Process pr;
                            
                        for (int i = 0; i < Tasks.Count; ++i)
                        {
                            try
                            {
                                pr = Process.GetProcessById(Tasks[i].Id);
                            }
                            catch (ArgumentException )
                            {
                               // MessageBox.Show("Not found " + i);
                                Tasks.RemoveAt(i);
                                --i;
                                continue;
                            }

                            Tasks[i].Cpu = CalculateCpu(pr);
                            Tasks[i].Ram = CalculateRam(pr);
                            Tasks[i].IsActive = pr.Responding;
                            Tasks[i].Threads = pr.Threads.Count;

                           
                        }

                        Application.Current.Dispatcher.BeginInvoke(new Func<ObservableCollection<MyTask>>(() => Tasks));
                    }
                   
                Thread.Sleep(2000);
            }
        }


        private async void Check1()
        {
            await Task.Run((() =>
            {
                bool found ;
                for (int i = 0; i < Tasks.Count; ++i)
                {
                    found = false;
                    foreach (Process pr in Process.GetProcesses())
                    {
                        if (Tasks[i].Id == pr.Id)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        Application.Current.Dispatcher.Invoke(() => { Tasks.RemoveAt(i); });
                        //Tasks.RemoveAt(i);
                        --i;
                    }
                }
            }));
        }

        private async void Check2()
        {
            await Task.Run((() =>
            {
                bool found;
                foreach (Process pr in Process.GetProcesses())
                {
                    found = false;
                    for (int i = 0; i < Tasks.Count; ++i)
                    {
                        if (pr.Id == Tasks[i].Id)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        Application.Current.Dispatcher.Invoke(() => {  
                        Tasks.Add(new MyTask(pr.ProcessName, pr.Id, pr.Responding, 0,
                            (int)pr.PrivateMemorySize64, pr.Threads.Count, pr.MachineName,
                            "pr.MainModule.FileName"));
                        });
                    }

                }
            }));
        }

        private void UpdateList()
        {
            while (true)
            {
                Thread.Sleep(5000);

               
                lock (Tasks)
                {
                        Check1();
                        Check2();

                }

            }
        }


        private bool CanExecuteCommand(object obj)
        {
            return SelectedTask != null;
        }


        private int CalculateCpu(Process pr)
        {
            PerformanceCounter CpuCounter = new PerformanceCounter("Process", "% Processor Time", pr.ProcessName);
            return Convert.ToInt32(CpuCounter.NextValue() / Environment.ProcessorCount);
        }

        private int CalculateRam(Process pr)
        {
            PerformanceCounter RamCounter = new PerformanceCounter("Process", "Private Bytes", pr.ProcessName);
            return (int)Math.Round(RamCounter.NextValue() / (1024 * 1024), 2);
        }

       
    }
}
