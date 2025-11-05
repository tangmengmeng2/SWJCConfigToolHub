using MyUtility.Utility;
using SWJCTool.Entity;
using SWJCTool.Model;
using SWJCTool.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWJCTool.Service
{
    public class CreateSWJCService
    {
        public bool CreateSWJCConfig()
        {
            bool isRight = false;

            //获取码位表
            GetConfigService getConfigService = SingletonProvider<GetConfigService>.Instance;

            //临时写死路径
            string codeFile = Path.Combine("DevCfg", "站内室外监测码位表.ini");
            List<SWJCCodeEntity> _SWJCCodeEnts = new List<SWJCCodeEntity>();
            if (!getConfigService.ReadSWJCCode(codeFile, ref _SWJCCodeEnts))
            {
                return false;
            }

            #region 创建大排队（analog）

            MySWJCTemplateModel mySWJCTemplateModel = SingletonProvider<MySWJCTemplateModel>.Instance;

            List<CSMAnalogEnt> CSMAnalogEnts = new List<CSMAnalogEnt>();

            foreach (var item in _SWJCCodeEnts)
            {
                if (mySWJCTemplateModel.TryGetSWJCTemplateEntByKey(item.TempleTypeName, out SWJCTemplateEntity SWJCTemplateEnt))
                {
                    CreateAnalog(SWJCTemplateEnt, item, ref CSMAnalogEnts);
                }
                else
                {
                    Console.WriteLine("未找到" + item.Order);
                }
            }

            #endregion


            #region 拆分大排队

            Dictionary<int, List<FKCSMAnalogEnt>> typeToCSMAnalogKeyValuePairs = new Dictionary<int, List<FKCSMAnalogEnt>>();
            foreach (var item in CSMAnalogEnts)
            {
                //剔除标识为255的
                if (item.AnalogFlag == 255)
                {
                    continue;
                }
                FKCSMAnalogEnt fKCSMAnalogEnt = new FKCSMAnalogEnt()
                {
                    _CSMAnalogEnt = item,
                };
                if (typeToCSMAnalogKeyValuePairs.TryGetValue(item.AnalogTypeNum, out List<FKCSMAnalogEnt> csmAnlogTemps))
                {
                    csmAnlogTemps.Add(fKCSMAnalogEnt);
                }
                else
                {
                    csmAnlogTemps = new List<FKCSMAnalogEnt>();
                    typeToCSMAnalogKeyValuePairs.Add(item.AnalogTypeNum, csmAnlogTemps);
                    csmAnlogTemps.Add(fKCSMAnalogEnt);
                }
            }

            #endregion

            #region 写入

            int count = 0;
            foreach (var item in mySWJCTemplateModel.SWJCAnaloyCodeEnts)
            {
                string sectionName = "子项" + (count + 1).ToString();
                StringBuilder info = new StringBuilder();
                info.AppendLine($"名称=" + item.Name);
                info.AppendLine($"模拟量子类型码=" + item.AnaloyCode);
                info.AppendLine($"变化范围=0.05,0.01");
                info.Append($";;序号 = 名称，标志列，AD号，显示序号，AD最小，AD最大，系数，报警上限，报警下限，分路上限，分机号，单位，预留(开关量)，预留，预留(倍率)，预留");
                
                if (!typeToCSMAnalogKeyValuePairs.TryGetValue(item.AnaloyCode, out List<FKCSMAnalogEnt> csmAnlogTemps))
                {
                    csmAnlogTemps = new List<FKCSMAnalogEnt>();
                }

                int ou = 0;
                foreach (var csmAnlogTemp in csmAnlogTemps)
                {
                    csmAnlogTemp.Order = ou + 1;
                    csmAnlogTemp.ADOrder = ou;
                    ou++;
                }


                // 使用泛型版本
                var analogSections = new List<IniSection<FKCSMAnalogEnt>>
                {
                    new IniSection<FKCSMAnalogEnt>
                    { 
                        SectionName = sectionName,
                        DataList = csmAnlogTemps,
                        InfoLine = info.ToString(),
                        Formatter = entity => $"{entity.Order}=\t{entity._CSMAnalogEnt.AnalogName}," +
                        $"\t{entity._CSMAnalogEnt.AnalogFlag}," +
                        $"\t{entity.ADOrder}," +
                        $"\t{entity.ADOrder}," +
                        $"\t{entity._CSMAnalogEnt.AnalogRangeLower}," +
                        $"\t{entity._CSMAnalogEnt.AnalogRangeUpper}," +
                        $"\t{entity._CSMAnalogEnt.AnalogCoefficient}," +
                        $"\t{entity._CSMAnalogEnt.AnalogLimitUpper}," +
                        $"\t{entity._CSMAnalogEnt.AnalogLimitLower}," +
                        $"\t{entity._CSMAnalogEnt.AnalogShuntLimit}," +
                        $"\t{entity._CSMAnalogEnt.AnalogExtensionNum}," +
                        $"\t{entity._CSMAnalogEnt.AnalogUnit}," +
                        $"\t{entity._CSMAnalogEnt.Switch1}," +
                        $"\t{entity._CSMAnalogEnt.Switch2}," +
                        $"\t{entity._CSMAnalogEnt.Switch3}," +
                        $"\t{entity._CSMAnalogEnt.Switch4},",
                        UseParallelProcessing = true  // 启用并行处理
                    }
                };
                count++;
                // 分别写入不同的文件，或者合并到一个文件
                GenericIniWriter<FKCSMAnalogEnt>.WriteSectionsToFile(analogSections, "output.ini");

            }



            #endregion

            #region 创建Msg排队

            
            foreach (var item_MsgGatherTemplateEnt in mySWJCTemplateModel.MsgGatherTemplateEnts)
            {
                Dictionary<string, int[]> devToADKeyValuePairs = new Dictionary<string, int[]>();

                foreach (var item_Unit in item_MsgGatherTemplateEnt.MsgGatherTemplateUnits)
                {
                    //
                    if (typeToCSMAnalogKeyValuePairs.TryGetValue(item_Unit.TypeCode, out List<FKCSMAnalogEnt> fkEnts))
                    {
                        foreach (var item_FKCSMAnalogEnt in fkEnts)
                        {
                            if (item_FKCSMAnalogEnt._CSMAnalogEnt.AnalogName.EndsWith(item_Unit.KeyWord))
                            {
                                //提取名字
                                string deviceName = TextExtractPrefixDeal.ExtractPrefixAdvanced(item_FKCSMAnalogEnt._CSMAnalogEnt.AnalogName, item_Unit.KeyWord);
                                if (devToADKeyValuePairs.TryGetValue(deviceName, out int[] deviceADCodes))
                                {
                                    if (deviceADCodes.Count() >= item_Unit.Order)
                                    {
                                        deviceADCodes[item_Unit.Order - 1] = item_FKCSMAnalogEnt.ADOrder;
                                    }
                                }
                                else
                                {
                                    deviceADCodes = Enumerable.Repeat(-1, item_MsgGatherTemplateEnt.Num).ToArray();
                                    deviceADCodes[item_Unit.Order - 1] = item_FKCSMAnalogEnt.ADOrder;
                                    devToADKeyValuePairs.Add(deviceName, deviceADCodes);
                                }
                                //break;
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                //转换
                List<MsgDetailTemplate> msgDetailTemplates = new List<MsgDetailTemplate>();
                int order = 1;
                foreach (var item in devToADKeyValuePairs)
                {
                    MsgDetailTemplate msgDetailTemplateTemp = new MsgDetailTemplate(order, item.Key, item.Value);
                    order++;
                    msgDetailTemplates.Add(msgDetailTemplateTemp);
                }
                


                #region 开始写入

                // 使用泛型版本
                var attribSections = new List<IniSection<MsgGatherTemplateUnit>>
                {
                    new IniSection<MsgGatherTemplateUnit>
                    {
                        SectionName = item_MsgGatherTemplateEnt.Name + "属性",
                        DataList = item_MsgGatherTemplateEnt.MsgGatherTemplateUnits,
                        InfoLine = $";;测试工具创建",
                        Formatter = entity => $"{entity.Order}=\t{entity.Name}," +
                        $"\t{entity.UnitName}," +
                        $"\t{entity.TypeCode}," +
                        $"\t{entity.UnitFlag}," +
                        $"\t{entity.ReportHType}," +
                        $"\t{entity.ReportLType}," +
                        $"\t{entity.ReportFlag}," +
                        $"\t{entity.ReportCloumn1}," +
                        $"\t{entity.ReportCloumn2}," +
                        $"\t{entity.ReportCloumn3}," +
                        $"\t{entity.Source}," +
                        $"\t{entity.SourceInterface}," +
                        $"\t{entity.SourceInterfaceName}," +
                        $"\t{entity.KeyWord}",
                        UseParallelProcessing = true  // 启用并行处理
                    }
                };
                // 分别写入不同的文件，或者合并到一个文件
                GenericIniWriter<MsgGatherTemplateUnit>.WriteSectionsToFile(attribSections, "outputtes.ini");

                // 使用泛型版本
                var detailSections = new List<IniSection<MsgDetailTemplate>>
                {
                    new IniSection<MsgDetailTemplate>
                    {
                        SectionName = item_MsgGatherTemplateEnt.Name + "路数",
                        DataList = msgDetailTemplates,
                        InfoLine = $";;测试工具创建",
                        Formatter = entity => $"{entity.Order}=\t{entity.Name}," +
                        $"\t{entity.DetailValue}",
                        UseParallelProcessing = true  // 启用并行处理
                    }
                };
                // 分别写入不同的文件，或者合并到一个文件
                GenericIniWriter<MsgDetailTemplate>.WriteSectionsToFile(detailSections, "outputtes.ini");


                #endregion

                int qq = 0;
            }


            #endregion


            return isRight;
        }


        private bool CreateAnalog(SWJCTemplateEntity SWJCTemplateEnt, SWJCCodeEntity SWJCCodeEnt, ref List<CSMAnalogEnt> CSMAnalogEnts)
        {
            if (CSMAnalogEnts == null)
            {
                return false;
            }

            for (int i = 0; i < SWJCTemplateEnt.SWJCTemplateUnits.Count; i++)
            {
                try
                {
                    SWJCTemplateUnit sWJCTemplateUnit = SWJCTemplateEnt.SWJCTemplateUnits[i];

                    string analogName = "";
                    if ("DUMMY".Equals(sWJCTemplateUnit.UnitName.ToUpper()))
                    {
                        analogName = "DUMMY";
                    }
                    else
                    {
                        analogName = SWJCCodeEnt.Name + "-" + sWJCTemplateUnit.UnitName;
                    }

                    CSMAnalogEnt csmAnalogEnt = new CSMAnalogEnt()
                    {
                        AnalogName = analogName,
                        AnalogFlag = sWJCTemplateUnit.AnalogTag,
                        AnalogOrder = i,
                        AnalogType = sWJCTemplateUnit.AnalogType.ToString(),
                        AnalogTypeNum = sWJCTemplateUnit.AnalogType,
                        AnalogRangeLower = (float)sWJCTemplateUnit.RangeLower,
                        AnalogRangeUpper = (float)sWJCTemplateUnit.RangeUpper,
                        AnalogCoefficient = 1.0f,
                        AnalogLimitUpper = (float)sWJCTemplateUnit.RangeUpper,
                        AnalogLimitLower = (float)sWJCTemplateUnit.RangeLower,
                        AnalogShuntLimit = 0.0f,
                        AnalogExtensionNum = SWJCCodeEnt.ExtNum,
                        AnalogUnit = sWJCTemplateUnit.AnalogUnit,
                        Switch1 = "65535",
                        Switch2 = "65535",
                        Switch3 = sWJCTemplateUnit.AnalogFactor.ToString("0.0000"),
                        Switch4 = "65535",
                    };
                    CSMAnalogEnts.Add(csmAnalogEnt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("CreateAnalog" + ex);
                }

            }

            return true;
        }


        private bool WriteSWJC()
        {
            bool isRight = false;

            //创建文件夹 output/test-日期


            //创建文件


            //写入



            return isRight;
        }


    }
}
