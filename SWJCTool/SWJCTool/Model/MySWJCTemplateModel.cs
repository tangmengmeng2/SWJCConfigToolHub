using MyUtility.Utility;
using SWJCTool.Entity;
using SWJCTool.Service;
using SWJCTool.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWJCTool.Model
{
    class MySWJCTemplateModel
    {
        private List<SWJCTemplateEntity> _SWJCTemplateEnts = new List<SWJCTemplateEntity>();
        public List<SWJCTemplateEntity> SWJCTemplateEnts
        {
            get
            {
                return _SWJCTemplateEnts;
            }
        }


        private List<SWJCAnaloyCodeEntity> _SWJCAnaloyCodeEnts = new List<SWJCAnaloyCodeEntity>();
        public List<SWJCAnaloyCodeEntity> SWJCAnaloyCodeEnts
        {
            get
            {
                return _SWJCAnaloyCodeEnts;
            }
        }

        private List<MsgGatherTemplateEntity> msgGatherTemplateEnts = new List<MsgGatherTemplateEntity>();
        public List<MsgGatherTemplateEntity> MsgGatherTemplateEnts
        {
            get
            {
                return msgGatherTemplateEnts;
            }
        }


        public MySWJCTemplateModel()
        {
            Init();
        }

        private void Init()
        {
            string templePath = Path.Combine("DevCfg", "站内室外监测模板.ini");
            string msgPath = Path.Combine("DevCfg", "站内室外监测Msg模板.ini");
            //读取服务
            GetConfigService getConfigService = SingletonProvider<GetConfigService>.Instance;

            try
            {
                List<SWJCAnaloyCodeEntity> swjcAnaloyCodeEnts = new List<SWJCAnaloyCodeEntity>();
                if (getConfigService.ReadAnalogCode(templePath, ref swjcAnaloyCodeEnts))
                {
                    if (swjcAnaloyCodeEnts.Count > 0)
                    {
                        _SWJCAnaloyCodeEnts = swjcAnaloyCodeEnts;
                    }
                }

                int i = 0;
                List<SWJCTemplateEntity> swjcTemplateEnts = new List<SWJCTemplateEntity>();
                if (getConfigService.ReadTemplate(templePath, ref swjcTemplateEnts))
                {
                    for (int j = 0; j < swjcTemplateEnts.Count; j++)
                    {
                        SWJCTemplateEntity item = swjcTemplateEnts[j];
                        getConfigService.ReadTemplateUnit(templePath, ref item);
                    }
                    
                }
                if (swjcTemplateEnts.Count > 0)
                {
                    _SWJCTemplateEnts = swjcTemplateEnts;
                }


                #region 读取MsgGather模板

                if (getConfigService.ReadMsgGatherTemplate(msgPath, out List<MsgGatherTemplateEntity> _msgGatherTemplateEnts))
                {
                    msgGatherTemplateEnts = _msgGatherTemplateEnts;
                }


                #endregion


                int ooo = 0;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MySWJCTemplateModel::Init", ex);
            }

            


        }

        public bool TryGetSWJCTemplateEntByKey(string name, out SWJCTemplateEntity SWJCTemplateEnt)
        {
            bool isRight = false;

            SWJCTemplateEnt = null;

            if (string.IsNullOrEmpty(name))
            {
                return isRight;
            }
            foreach (var item in SWJCTemplateEnts)
            {
                if (item.Name.Equals(name))
                {
                    isRight = true;
                    SWJCTemplateEnt = item;
                    break;
                }
            }

            return isRight;
        }


        public bool TryGetSWJCAnaloyCodeEntByKey(int code, out SWJCAnaloyCodeEntity SWJCAnaloyCodeEnt)
        {
            bool isRight = false;

            SWJCAnaloyCodeEnt = null;

            if (code == 0 || code == -1)
            {
                return isRight;
            }
            foreach (var item in SWJCAnaloyCodeEnts)
            {
                if (item.AnaloyCode == code)
                {
                    isRight = true;
                    SWJCAnaloyCodeEnt = item;
                    break;
                }
            }

            return isRight;
        }

    }
}
