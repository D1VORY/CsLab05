using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cs05.Tools;

namespace Cs05.Models
{
    class MyTask :BaseModel
    {
        private string _name;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        private int _id;

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            set => _isActive = value;
        }

        private int _cpu;

        public int Cpu
        {
            get => _cpu;
            set => _cpu = value;
        }

        private int _ram;

        public int Ram
        {
            get => _ram;
            set => _ram = value;
        }

        private int _threads;

        public int Threads
        {
            get => _threads;
            set => _threads = value;
        }

        private string _userName;

        public string UserName
        {
            get => _userName;
            set => _userName = value;
        }

        private string _path;

        public string Path
        {
            get => _path;
            set => _path = value;
        }

        public MyTask(string name,int id, bool isActive,int cpu,int ram,int th,string uName,string path)
        {
            Name = name;
            Id = id;
            IsActive = isActive;
            Cpu = cpu;
            Ram = ram;
            Threads = th;
            UserName = uName;
            Path = path;
        }


    }
}
