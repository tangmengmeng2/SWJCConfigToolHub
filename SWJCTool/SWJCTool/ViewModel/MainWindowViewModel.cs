using MyUtility.Utility;
using SWJCTool.Model;
using SWJCTool.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWJCTool.ViewModel
{
    class MainWindowViewModel
    {
        private GetConfigService getConfigService = SingletonProvider<GetConfigService>.Instance;
        private MySWJCTemplateModel mySWJCTemplateModel = SingletonProvider<MySWJCTemplateModel>.Instance;


        private CreateSWJCService _CreateSWJCService = SingletonProvider<CreateSWJCService>.Instance; 


        public MainWindowViewModel()
        {
        }


        public void CreateCodeFiles()
        {
            _CreateSWJCService.CreateSWJCConfig();
        }

    }
}
