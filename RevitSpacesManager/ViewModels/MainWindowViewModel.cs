using RevitSpacesManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitSpacesManager.ViewModels
{
    internal class MainWindowViewModel
    {
        private readonly MainModel _MainModel;

        public MainWindowViewModel()
        {
            _MainModel = new MainModel();
        }
    }
}
