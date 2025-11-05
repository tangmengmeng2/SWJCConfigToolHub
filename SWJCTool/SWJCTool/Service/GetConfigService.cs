using System;
using System.Collections.Generic;
using System.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using SWJCTool.Entity;
using SWJCTool.Utility;
using MyUtility.Utility;

namespace SWJCTool.Service
{
    public class GetConfigService
    {
        private string filePath = "";
        public string ScriptServerIP { get; set; }

        public int ScriptServerPort { get; set; }

        public int LocalPort { get; set; }
        public string LocalIP { get; set; }
        public int CommType { get; set; }

        private const string TDProctrol_Old = "1";
        private const string HHProctrol_Old = "2";
        private const string InterconnArg = "3";

        public GetConfigService()
        {
            filePath = System.AppDomain.CurrentDomain.BaseDirectory;
        }

        public bool ReadScriptServiceConfig()
        {
            bool isRight = false;
            //判断是否存在配置文件
            string fileSysCfgPath = filePath + "CFG\\SysCfg.ini";
            if (File.Exists(fileSysCfgPath))
            {
                //读取IP和端口
                string ServerIPStr = INIHelper.Read("本机", "服务器IP", "-1", fileSysCfgPath);
                string ServerPortStr = INIHelper.Read("本机", "服务器port", "-1", fileSysCfgPath);


                if (CheckStrNum.CheckStrIsFourDot(ServerIPStr))
                {
                    ScriptServerIP = ServerIPStr;
                }
                else
                {
                    ScriptServerIP = "127.0.0.1";
                }
                if (CheckStrNum.CheckStrIsInteger(ServerPortStr) && !"-1".Equals(ServerPortStr))
                {
                    ScriptServerPort = Convert.ToInt32(ServerPortStr);
                }
                else
                {
                    ScriptServerPort = 6789;
                }
                isRight = true;
            }
            return isRight;
        }

        public bool ReadCommTypeConfig(ref int commType)
        {
            bool isRight = false;
            //获取程序所在路径
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory;
            //判断是否存在配置文件
            string fileSysCfgPath = filePath + "CFG\\SysCfg.ini";
            if (File.Exists(fileSysCfgPath))
            {
                //读取IP和端口
                string commTypeStr = INIHelper.Read("本机", "通信方式", "-1", fileSysCfgPath);

                if (CheckStrNum.CheckStrIsInteger(commTypeStr) && !"-1".Equals(commTypeStr))
                {
                    commType = Convert.ToInt32(commTypeStr);
                }
                else
                {
                    commType = 1;
                }

                isRight = true;
            }
            return isRight;
        }

        public bool ReadTCPConfig()
        {
            bool isRight = false;
            //获取程序所在路径
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory;
            //判断是否存在配置文件
            string fileSysCfgPath = filePath + "CFG\\SysCfg.ini";
            if (File.Exists(fileSysCfgPath))
            {
                //读取IP和端口
                string LocalIPStr = INIHelper.Read("本机", "本机IP", "-1", fileSysCfgPath);
                string LocalPortStr = INIHelper.Read("本机", "本机端口", "-1", fileSysCfgPath);
                string commType = INIHelper.Read("本机", "通信方式", "-1", fileSysCfgPath);

                if (CheckStrNum.CheckStrIsFourDot(LocalIPStr))
                {
                    LocalIP = LocalIPStr;
                }
                else
                {
                    LocalIP = "127.0.0.1";
                }
                if (CheckStrNum.CheckStrIsInteger(LocalPortStr) && !"-1".Equals(LocalPortStr))
                {
                    LocalPort = Convert.ToInt32(LocalPortStr);
                }
                else
                {
                    LocalPort = 5560;
                }


                if (CheckStrNum.CheckStrIsInteger(commType) && !"-1".Equals(commType))
                {
                    CommType = Convert.ToInt32(commType);
                }
                else
                {
                    CommType = 0;
                }

                isRight = true;
            }
            return isRight;
        }

        private bool ReadTCPAndComConfig(string filePath, string sectionKey, string IPKey, string PortKey, int type, ref string qkServerIP, ref int qkServerPort)
        {
            bool isRight = false;

            //读取IP和端口
            string qkServerIPStr = INIHelper.Read(sectionKey, IPKey, "-1", filePath);
            string qkServerPortStr = INIHelper.Read(sectionKey, PortKey, "-1", filePath);

            if (!CheckStrNum.CheckStrIsInteger(qkServerPortStr) ||
                "-1".Equals(qkServerIPStr) || "-1".Equals(qkServerPortStr))
            {
                isRight = false;
            }
            else
            {
                if (type == 1 && !CheckStrNum.CheckStrIsFourDot(qkServerIPStr))
                {
                    //校验为网路通信时，名称为4个点
                    isRight = false;
                }
                else
                {
                    isRight = true;
                }
            }

            if (isRight)
            {
                qkServerIP = qkServerIPStr;
                qkServerPort = Convert.ToInt32(qkServerPortStr);
            }


            return isRight;
        }

        public bool ReadTCPClientConfig(ref string qkServerIP, ref int qkServerPort)
        {
            bool isRight = false;
            //获取程序所在路径
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory;
            //判断是否存在配置文件
            string fileSysCfgPath = filePath + "CFG\\SysCfg.ini";
            if (File.Exists(fileSysCfgPath))
            {
                //读取IP和端口
                string qkServerIPStr = INIHelper.Read("本机", "缺口服务器IP", "-1", fileSysCfgPath);
                string qkServerPortStr = INIHelper.Read("本机", "缺口服务器端口", "-1", fileSysCfgPath);

                if (!CheckStrNum.CheckStrIsFourDot(qkServerIPStr))
                {
                    qkServerIP = "127.0.0.1";
                }
                else
                {
                    qkServerIP = qkServerIPStr;
                }

                if (!CheckStrNum.CheckStrIsInteger(qkServerPortStr) || "-1".Equals(qkServerPortStr))
                {
                    qkServerPort = 5011;
                }
                else
                {
                    qkServerPort = Convert.ToInt32(qkServerPortStr);
                }

                isRight = true;
            }
            return isRight;
        }

        public bool ReadTelCode(out string telCode)
        {
            bool isRight = false;
            telCode = "";
            //判断是否存在配置文件
            string fileSysCfgPath = filePath + "CFG\\SysCfg.ini";
            if (File.Exists(fileSysCfgPath))
            {
                //读取IP和端口
                telCode = INIHelper.Read("本机", "站名", "-1", fileSysCfgPath);

                if (string.IsNullOrEmpty(telCode) || "-1".Equals(telCode))
                {
                    return isRight;
                }

                isRight = true;
            }
            return isRight;
        }

        public bool ReadIdentCode(out string identCode)
        {
            bool isRight = false;
            identCode = "";
            //判断是否存在配置文件
            string fileSysCfgPath = filePath + "CFG\\SysCfg.ini";
            if (File.Exists(fileSysCfgPath))
            {
                //读取IP和端口
                identCode = INIHelper.Read("本机", "识别码", "-1", fileSysCfgPath);

                if (string.IsNullOrEmpty(identCode) || "-1".Equals(identCode))
                {
                    return isRight;
                }
                identCode = Regex.Replace(identCode, @"\s", "");
                isRight = true;
            }
            return isRight;
        }

        public bool ReadExtNumCode(out string extNumStr)
        {
            bool isRight = false;
            extNumStr = "";
            //判断是否存在配置文件
            string fileSysCfgPath = filePath + "CFG\\SysCfg.ini";
            if (File.Exists(fileSysCfgPath))
            {
                //读取IP和端口
                extNumStr = INIHelper.Read("本机", "分机号", "-1", fileSysCfgPath);

                if (string.IsNullOrEmpty(extNumStr) || "-1".Equals(extNumStr))
                {
                    return isRight;
                }
                extNumStr = Regex.Replace(extNumStr, @"\s", "");
                isRight = true;
            }
            return isRight;
        }

        public bool ReadAnalogCode(string _filePath, ref List<SWJCAnaloyCodeEntity> _SWJCAnaloyCodeEnts)
        {
            string fileCfgPath = Path.Combine(filePath, _filePath);
            if (!File.Exists(fileCfgPath))
                throw new FileNotFoundException($"INI文件不存在: {fileCfgPath}");
            //总数
            string sectionNumStr = INIHelper.Read("模拟量子类型", "总数", "", fileCfgPath);
            if (string.IsNullOrEmpty(sectionNumStr))
            {
                return false;
            }

            if (int.TryParse(sectionNumStr, out int sectionNum))
            {
                for (int i = 0; i < sectionNum; i++)
                {
                    string line = INIHelper.Read("模拟量子类型", (i + 1).ToString(), "", fileCfgPath);
                    //去除空格
                    line = Regex.Replace(line, @"\s", "");
                    var match = Regex.Match(line, @"^(.+?)\s*,\s*(0x[0-9A-Fa-f]+|\d+)$");
                    if (match.Success)
                    {
                        try
                        {
                            int order = i;
                            string name = match.Groups[1].Value.Trim();
                            string codeValue = match.Groups[2].Value.Trim();

                            int analoyCode = codeValue.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
                                ? Convert.ToInt32(codeValue, 16)
                                : int.Parse(codeValue);

                            _SWJCAnaloyCodeEnts.Add(new SWJCAnaloyCodeEntity(order, name, analoyCode));
                        }
                        catch (Exception ex)
                        {
                            throw new FormatException($"解析行失败: {line}", ex);
                        }
                    }
                }
            }
            else
            {
                return false;
            }

            if (_SWJCAnaloyCodeEnts.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool ReadTemplate(string _filePath, ref List<SWJCTemplateEntity> swjcTemplateEnts)
        {
            bool isRight = false;

            string fileCfgPath = Path.Combine(filePath, _filePath);
            if (!File.Exists(fileCfgPath))
                throw new FileNotFoundException($"INI文件不存在: {fileCfgPath}");

            if (swjcTemplateEnts == null)
            {
                swjcTemplateEnts = new List<SWJCTemplateEntity>();
            }

            //总数
            string sectionNumStr = INIHelper.Read("模板类型", "总数", "", fileCfgPath);
            if (string.IsNullOrEmpty(sectionNumStr))
            {
                return false;
            }

            if (int.TryParse(sectionNumStr, out int sectionNum))
            {
                for (int i = 0; i < sectionNum; i++)
                {
                    string line = INIHelper.Read("模板类型", (i + 1).ToString(), "", fileCfgPath);
                    //去除空格
                    line = Regex.Replace(line, @"\s", "");
                    var match = Regex.Match(line, @"^(.+?)\s*,\s*(0x[0-9A-Fa-f]+|\d+),\s*(0x[0-9A-Fa-f]+|\d+)$");
                    if (match.Success)
                    {
                        try
                        {
                            int order = i;
                            string name = match.Groups[1].Value.Trim();
                            string codeValue = match.Groups[2].Value.Trim();
                            string subCodeValue = match.Groups[3].Value.Trim();

                            swjcTemplateEnts.Add(new SWJCTemplateEntity(order, name, 0, codeValue, subCodeValue));
                        }
                        catch (Exception ex)
                        {
                            throw new FormatException($"解析行失败: {line}", ex);
                        }
                    }
                }
            }
            else
            {
                return false;
            }

            if (swjcTemplateEnts.Count > 0)
            {
                isRight = true;
            }
            return isRight;
        }



        public bool ReadTemplateUnit(string _filePath, ref SWJCTemplateEntity swjcTemplateEnt)
        {
            bool isRight = false;

            string fileCfgPath = Path.Combine(filePath, _filePath);
            if (!File.Exists(fileCfgPath))
                throw new FileNotFoundException($"INI文件不存在: {fileCfgPath}");

            if (swjcTemplateEnt == null)
            {
                return false;
            }

            // 正则表达式匹配
            string pattern = @"^(\d+)=([^,]+?),([^,]+?),([^,]+?),([^,]+?),([^,]+?),(.*?),([^,]+?)$";

            //读取全部字段内容
            List<string> lines = ReadIni.ReadSectionAllLine(fileCfgPath, swjcTemplateEnt.Name, "");
            if (lines != null)
            {
                foreach (var line in lines)
                {
                    //去除空格
                    string trimmedLine = Regex.Replace(line, @"\s", "");
                    if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("[") || trimmedLine.StartsWith(";") || trimmedLine.StartsWith("#"))
                    {
                        continue;
                    }

                    Match match = Regex.Match(trimmedLine, pattern);
                    if (match.Success)
                    {
                        try
                        {

                            int order = int.Parse(match.Groups[1].Value);
                            string unitName = match.Groups[2].Value.Trim();
                            int analogTag = int.Parse(match.Groups[3].Value);
                            int analogType = int.Parse(match.Groups[4].Value);
                            double rangeLower = double.Parse(match.Groups[5].Value);
                            double rangeUpper = double.Parse(match.Groups[6].Value);
                            string analogUnit = match.Groups[7].Value.Trim();
                            double analogFactor = double.Parse(match.Groups[8].Value);

                            SWJCTemplateUnit swjcTemplateUnit = new SWJCTemplateUnit(order, unitName)
                            {
                                AnalogTag = analogTag,
                                AnalogType = analogType,
                                RangeLower = rangeLower,
                                RangeUpper = rangeUpper,
                                AnalogUnit = analogUnit,
                                AnalogFactor = analogFactor,
                            };

                            swjcTemplateEnt.AddUnit(swjcTemplateUnit);
                        }
                        catch (Exception ex)
                        {
                            throw new FormatException($"解析行失败: {line}", ex);
                        }
                    }
                    else
                    {
                        Console.WriteLine("匹配失败:" + trimmedLine);
                    }

                }

            }

            if (swjcTemplateEnt.SWJCTemplateUnits.Count > 0)
            {
                isRight = true;
            }
            return isRight;
        }

        /// <summary>
        /// 读取MsgGather模板
        /// </summary>
        /// <param name="_filePath"></param>
        /// <param name="MsgGatherTemplateEnts"></param>
        /// <returns></returns>
        public bool ReadMsgGatherTemplate(string _filePath, out List<MsgGatherTemplateEntity> MsgGatherTemplateEnts)
        {
            bool isRight = false;

            string fileCfgPath = Path.Combine(filePath, _filePath);
            if (!File.Exists(fileCfgPath))
                throw new FileNotFoundException($"INI文件不存在: {fileCfgPath}");

            MsgGatherTemplateEnts = new List<MsgGatherTemplateEntity>();

            //总数
            string sectionNumStr = INIHelper.Read("设备类型", "总数", "", fileCfgPath);
            if (string.IsNullOrEmpty(sectionNumStr))
            {
                return false;
            }

            if (int.TryParse(sectionNumStr, out int sectionNum))
            {
                for (int i = 0; i < sectionNum; i++)
                {
                    string line = INIHelper.Read("设备类型", (i + 1).ToString(), "", fileCfgPath);
                    //去除空格
                    line = Regex.Replace(line, @"\s", "");
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    //创建模板对象
                    MsgGatherTemplateEntity msgGatherTemplateEnt = new MsgGatherTemplateEntity(i + 1, line);
                    
                    //读取详细配置
                    string sectionSubName = line + "属性";
                    //总数
                    string unitSumStr = INIHelper.Read(sectionSubName, "总数", "", fileCfgPath);
                    if (string.IsNullOrEmpty(unitSumStr) || !int.TryParse(unitSumStr, out int unitSum))
                    {
                        continue;
                    }

                    for (int j = 0; j < unitSum; j++)
                    {
                        string unitLine = INIHelper.Read(sectionSubName, (j + 1).ToString(), "", fileCfgPath);
                        //去除空行
                        if (string.IsNullOrEmpty(unitLine))
                        {
                            continue;
                        }

                        try
                        {
                            string[] fields = unitLine.Split(',');

                            // 确保有足够的字段
                            if (fields.Length < 14)
                            {
                                continue;
                            }
                            // 解析序号
                            int order = j + 1;

                            MsgGatherTemplateUnit msgGatherTemplateUnitTemp = new MsgGatherTemplateUnit
                            {
                                Order = order,
                                Name = fields[0].Trim(),
                                UnitName = fields[1].Trim(),
                                TypeCode = HexToInt(fields[2].Trim()),
                                UnitFlag = SafeParseInt(fields[3].Trim()),
                                ReportHType = fields[4].Trim(),
                                ReportLType = fields[5].Trim(),
                                ReportFlag = SafeParseInt(fields[6].Trim()),
                                ReportCloumn1 = fields[7].Trim(),
                                ReportCloumn2 = fields[8].Trim(),
                                ReportCloumn3 = fields[9].Trim(),
                                Source = fields[10].Trim(),
                                SourceInterface = fields[11].Trim(),
                                SourceInterfaceName = fields[12].Trim(),
                                KeyWord = fields[13].Trim()
                            };

                            msgGatherTemplateEnt.AddUnit(msgGatherTemplateUnitTemp);
                        }
                        catch (Exception ex)
                        {
                            throw new FormatException($"解析行时出错: {line}", ex);
                        }

                    }

                    //
                    if (msgGatherTemplateEnt.MsgGatherTemplateUnits.Count > 0)
                    {
                        MsgGatherTemplateEnts.Add(msgGatherTemplateEnt);
                    }
                }
            }
            else
            {
                return false;
            }

            if (MsgGatherTemplateEnts.Count > 0)
            {
                isRight = true;
            }
            return isRight;
        }

        private int HexToInt(string hexString)
        {
            if (hexString.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                return Convert.ToInt32(hexString, 16);
            }
            return SafeParseInt(hexString);
        }

        private int SafeParseInt(string value)
        {
            if (int.TryParse(value, out int result))
                return result;
            return 0; // 或者根据需求返回默认值
        }

        public bool ReadSWJCCode(string _filePath, ref List<SWJCCodeEntity> _SWJCCodeEnts)
        {
            bool isRight = false;

            string fileCfgPath = Path.Combine(filePath, _filePath);
            if (!File.Exists(fileCfgPath))
                throw new FileNotFoundException($"INI文件不存在: {fileCfgPath}");

            if (_SWJCCodeEnts == null)
            {
                return false;
            }

            // 正则表达式匹配
            string pattern = @"^(\d+)=([^,]+?),([^,]+?),([^,]+?),([^,]+?),([^,]+?),([^,]+?),([^,]+?)$";

            //读取全部字段内容
            List<string> lines = ReadIni.ReadSectionAllLine(fileCfgPath, "区段", "");
            if (lines != null)
            {
                foreach (var line in lines)
                {
                    //去除空格
                    string trimmedLine = Regex.Replace(line, @"\s", "");
                    if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("[") || trimmedLine.StartsWith(";") || trimmedLine.StartsWith("#"))
                    {
                        continue;
                    }

                    Match match = Regex.Match(trimmedLine, pattern);
                    if (match.Success)
                    {
                        try
                        {

                            int order = int.Parse(match.Groups[1].Value);
                            int channelId = int.Parse(match.Groups[2].Value);
                            int addressId = int.Parse(match.Groups[3].Value);
                            string name = match.Groups[4].Value.Trim();
                            int trackTypeNum = int.Parse(match.Groups[5].Value);
                            string templeTypeName = match.Groups[6].Value.Trim();
                            string deviceTypeName = match.Groups[7].Value.Trim();
                            int extNum = int.Parse(match.Groups[8].Value);

                            SWJCCodeEntity sWJCCodeEntity = new SWJCCodeEntity()
                            {

                                Order = order,
                                ChannelId = channelId,
                                AddressId = addressId,
                                Name = name,
                                TrackTypeNum = trackTypeNum,
                                TempleTypeName = templeTypeName,
                                DeviceTypeName = deviceTypeName,
                                ExtNum = extNum,
                            };

                            _SWJCCodeEnts.Add(sWJCCodeEntity);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("解析行失败:" + line);
                            throw new FormatException($"解析行失败: {line}", ex);
                        }
                    }
                    else
                    {
                        Console.WriteLine("匹配失败:" + trimmedLine);
                    }

                }

            }

            if (_SWJCCodeEnts.Count > 0)
            {
                isRight = true;
            }
            return isRight;
        }



        private bool ReadYP(string filePath)
        {
            bool isRight = false;

            Dictionary<string, int> keyValueYPTypePairs = new Dictionary<string, int>();
            Dictionary<int, List<CSMAnalogEnt>> keyValueYPAnalogPairs = new Dictionary<int, List<CSMAnalogEnt>>();

            string sectionNumStr = INIHelper.Read("模拟量项目", "子项数目", "-1", filePath);
            if (!"-1".Equals(sectionNumStr))
            {
                //判断是否为纯数字
                if (Regex.IsMatch(sectionNumStr, "[0-9]+"))
                {
                    int sectionNum = Convert.ToInt32(sectionNumStr);
                    //循环读取
                    for (int i = 0; i < sectionNum; i++)
                    {
                        int order = (i + 1);
                        string orderName = "子项" + order;
                        string name = INIHelper.Read(orderName, "名称", "-1", filePath);
                        string numStr = INIHelper.Read(orderName, "总路数", "-1", filePath);
                        int connectType = 0;
                        //存在该名称的移频类型，且读取到数目
                        if (keyValueYPTypePairs.ContainsKey(name) && Regex.IsMatch(numStr, "[0-9]+"))
                        {
                            connectType = keyValueYPTypePairs[name];
                            List<CSMAnalogEnt> AnalogValue = null;
                            if (keyValueYPAnalogPairs.ContainsKey(connectType))
                            {
                                AnalogValue = keyValueYPAnalogPairs[connectType];
                            }
                            else
                            {
                                AnalogValue = new List<CSMAnalogEnt>();
                                keyValueYPAnalogPairs.Add(connectType, AnalogValue);
                            }
                            //转为数值
                            int num = Convert.ToInt32(numStr);
                            //获取数据
                            for (int j = 0; j < num; j++)
                            {
                                string childKey = (j + 1).ToString();
                                string childValue = INIHelper.Read(orderName, childKey, "-1", filePath);
                                if (!"-1".Equals(childValue))
                                {
                                    //去除空格
                                    childValue = Regex.Replace(childValue, @"\s", "");
                                    //分割
                                    string[] childValues = childValue.Split(',', '，');
                                    if (childValues.Count() >= 16)
                                    {
                                        //判断模拟量是否为DUMMY或者类型码为255
                                        if ("DUMMY".Equals(childValues[0]) || "255".Equals(childValues[1]))
                                        {
                                            continue;
                                        }
                                        string analogName = childValues[0];

                                        int analogFlag = 0;
                                        if (CheckStrNum.CheckStrIsInteger(childValues[1]))
                                        {
                                            analogFlag = Convert.ToInt32(childValues[1]);
                                        }

                                        int analogOrder = 0;
                                        if (CheckStrNum.CheckStrIsInteger(childValues[2]))
                                        {
                                            analogOrder = Convert.ToInt32(childValues[2]);
                                        }

                                        string analogType = connectType.ToString();

                                        float analogRangeLower = 0.0f;
                                        if (CheckStrNum.CheckStrIsDecimals(childValues[4]))
                                        {
                                            analogRangeLower = Convert.ToSingle(childValues[4]);
                                        }

                                        float analogRangeUpper = 0.0f;
                                        if (CheckStrNum.CheckStrIsDecimals(childValues[5]))
                                        {
                                            analogRangeUpper = Convert.ToSingle(childValues[5]);
                                        }

                                        float analogCoefficient = 0.0f;
                                        if (CheckStrNum.CheckStrIsDecimals(childValues[6]))
                                        {
                                            analogCoefficient = Convert.ToSingle(childValues[6]);
                                        }

                                        float analogLimitUpper = 0.0f;
                                        if (CheckStrNum.CheckStrIsDecimals(childValues[7]))
                                        {
                                            analogLimitUpper = Convert.ToSingle(childValues[7]);
                                        }

                                        float analogLimitLower = 0.0f;
                                        if (CheckStrNum.CheckStrIsDecimals(childValues[8]))
                                        {
                                            analogLimitLower = Convert.ToSingle(childValues[8]);
                                        }

                                        float analogShuntLimit = 0.0f;
                                        if (CheckStrNum.CheckStrIsDecimals(childValues[9]))
                                        {
                                            analogShuntLimit = Convert.ToSingle(childValues[9]);
                                        }

                                        int analogExtensionNum = 0;
                                        if (CheckStrNum.CheckStrIsInteger(childValues[10]))
                                        {
                                            analogExtensionNum = Convert.ToInt32(childValues[10]);
                                        }
                                        string analogUnit = childValues[11];
                                        string switch1 = childValues[12];
                                        string switch2 = childValues[13];
                                        string switch3 = childValues[14];
                                        string switch4 = childValues[15];
                                        CSMAnalogEnt CSMAnalogEnt = new CSMAnalogEnt(analogName, analogFlag, analogOrder, analogType, analogRangeLower, analogRangeUpper, analogCoefficient, analogLimitUpper,
                                            analogLimitLower, analogShuntLimit, analogExtensionNum, analogUnit, switch1, switch2, switch3, switch4);
                                        AnalogValue.Add(CSMAnalogEnt);
                                    }
                                }

                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }



            }
            return isRight;
        }

        /// <summary>
        /// 读取指定分机号CSM监测标准分库配置
        /// </summary>
        /// <param name="telCode">电报码</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="extNum">分机号</param>
        /// <returns></returns>
        public List<DigitEntity> ReadCSMDigitCfgByExtNum(string telCode, string fileName, int extNum)
        {
            //开关量行的匹配字符
            string digitCfgLinePttern = @"\d+\s*=\s*(?<value>[^,]*(\,(?!,)[^,]*)*)";
            List<DigitEntity> digitValue = new List<DigitEntity>();
            //组包路径
            string digitFIlePath = Path.Combine(filePath, "CFG", telCode, fileName);
            if (!File.Exists(digitFIlePath))
            {
                return digitValue;
            }
            string sectionNumStr = INIHelper.Read("开关量", "总路数", "-1", digitFIlePath);
            if (!"-1".Equals(sectionNumStr))
            {
                //判断是否为纯数字
                if (Regex.IsMatch(sectionNumStr, "[0-9]+"))
                {
                    int sectionNum = Convert.ToInt32(sectionNumStr);
                    int order = 0;
                    //读取全部数据
                    List<string> digitAllLine = ReadIni.ReadSectionAllLine(digitFIlePath, "开关量", "");
                    if (digitAllLine != null)
                    {
                        for (int i = 0; i < digitAllLine.Count; i++)
                        {
                            if (string.IsNullOrEmpty(digitAllLine[i]))
                            {
                                continue;
                            }
                            //去除空格
                            string digitStr = Regex.Replace(digitAllLine[i], @"\s", "");
                            //检查匹配
                            Match match = Regex.Match(digitStr, digitCfgLinePttern);
                            if (!match.Success)
                            {
                                continue;
                            }
                            //分割
                            string[] digitStrs = digitStr.Split('=');
                            if (digitStrs.Count() != 2 || !CheckStrNum.CheckStrIsInteger(digitStrs[0]))
                            {
                                continue;
                            }
                            //分割
                            string[] digitInfoStrs = digitStrs[1].Split(',', '，');
                            if (digitInfoStrs.Count() < 5)
                            {
                                continue;
                            }
                            //判断分机号一致
                            int digitExtNum = Convert.ToInt32(digitInfoStrs[4]);
                            if (digitExtNum == extNum)
                            {
                                order++;
                                int digitID = order;
                                string digitName = digitInfoStrs[0];
                                int digitOrder = Convert.ToInt32(digitInfoStrs[1]);

                                int digitType;
                                if (CheckStrNum.CheckStrIsInteger(digitInfoStrs[2]))
                                {
                                    digitType = Convert.ToInt32(digitInfoStrs[2]);
                                }
                                else
                                {
                                    digitType = 128;
                                }

                                int digitInverse;
                                if (CheckStrNum.CheckStrIsInteger(digitInfoStrs[3]))
                                {
                                    digitInverse = Convert.ToInt32(digitInfoStrs[3]);
                                }
                                else
                                {
                                    digitInverse = 0;
                                }


                                DigitEntity digitEntity = new DigitEntity(digitID, digitName, digitOrder, digitType, digitInverse, digitExtNum);
                                if (digitInfoStrs.Count() >= 6)
                                {
                                    digitEntity.DigitNotes = digitInfoStrs[5];
                                }
                                //写入报警延迟，报警恢复
                                if (digitInfoStrs.Count() >= 7)
                                {
                                    digitEntity.DigitAlarmDelay = digitInfoStrs[6];
                                }
                                if (digitInfoStrs.Count() >= 8)
                                {
                                    digitEntity.DigitRecoveryDelay = digitInfoStrs[7];
                                }
                                digitValue.Add(digitEntity);
                            }
                        }
                    }
                }
            }
            return digitValue;
        }

        /// <summary>
        /// 读取CSM监测标准分库配置
        /// </summary>
        /// <param name="telCode">电报码</param>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        public List<DigitEntity> ReadCSMDigitCfg(string telCode, string fileName)
        {
            //开关量行的匹配字符
            string digitCfgLinePttern = @"\d+\s*=\s*(?<value>[^,]*(\,(?!,)[^,]*)*)";
            List<DigitEntity> digitValue = new List<DigitEntity>();
            //组包路径
            string digitFilePath = Path.Combine(filePath, "CFG", telCode, fileName);
            if (!File.Exists(digitFilePath))
            {
                return digitValue;
            }
            string sectionNumStr = INIHelper.Read("开关量", "总路数", "-1", digitFilePath);
            if (!"-1".Equals(sectionNumStr))
            {
                //判断是否为纯数字
                if (Regex.IsMatch(sectionNumStr, "[0-9]+"))
                {
                    int sectionNum = Convert.ToInt32(sectionNumStr);
                    int order = 0;
                    //读取全部数据
                    List<string> digitAllLine = ReadIni.ReadSectionAllLine(digitFilePath, "开关量", "");
                    if (digitAllLine != null)
                    {
                        for (int i = 0; i < digitAllLine.Count; i++)
                        {
                            if (string.IsNullOrEmpty(digitAllLine[i]))
                            {
                                continue;
                            }
                            //去除空格
                            string digitStr = Regex.Replace(digitAllLine[i], @"\s", "");
                            //检查匹配
                            Match match = Regex.Match(digitStr, digitCfgLinePttern);
                            if (!match.Success)
                            {
                                continue;
                            }
                            //分割
                            string[] digitStrs = digitStr.Split('=');
                            if (digitStrs.Count() != 2 || !CheckStrNum.CheckStrIsInteger(digitStrs[0]))
                            {
                                continue;
                            }
                            //分割
                            string[] digitInfoStrs = digitStrs[1].Split(',', '，');
                            if (digitInfoStrs.Count() < 5)
                            {
                                continue;
                            }
                            //判断分机号不大于255
                            int digitExtNum = Convert.ToInt32(digitInfoStrs[4]);
                            if (digitExtNum <= 255 && digitExtNum >= 0)
                            {
                                order++;
                                int digitID = order;
                                string digitName = digitInfoStrs[0];
                                int digitOrder = Convert.ToInt32(digitInfoStrs[1]);

                                int digitType;
                                if (CheckStrNum.CheckStrIsInteger(digitInfoStrs[2]))
                                {
                                    digitType = Convert.ToInt32(digitInfoStrs[2]);
                                }
                                else
                                {
                                    digitType = 128;
                                }

                                int digitInverse;
                                if (CheckStrNum.CheckStrIsInteger(digitInfoStrs[3]))
                                {
                                    digitInverse = Convert.ToInt32(digitInfoStrs[3]);
                                }
                                else
                                {
                                    digitInverse = 0;
                                }


                                DigitEntity digitEntity = new DigitEntity(digitID, digitName, digitOrder, digitType, digitInverse, digitExtNum);
                                if (digitInfoStrs.Count() >= 6)
                                {
                                    digitEntity.DigitNotes = digitInfoStrs[5];
                                }
                                //写入报警延迟，报警恢复
                                if (digitInfoStrs.Count() >= 7)
                                {
                                    digitEntity.DigitAlarmDelay = digitInfoStrs[6];
                                }
                                if (digitInfoStrs.Count() >= 8)
                                {
                                    digitEntity.DigitRecoveryDelay = digitInfoStrs[7];
                                }
                                digitValue.Add(digitEntity);
                            }
                        }
                    }
                }
            }
            return digitValue;
        }

        public bool WriteDigitInitCfg(string telCode, string section, List<DigitEntity> digitEnts)
        {
            bool isRight = false;


            if (digitEnts.Count == 0)
            {
                return isRight;
            }
            //组包路径
            string digitFilePath = filePath + "\\CFG\\" + telCode + "\\digit_init.ini";// INI文件的路径   

            // 检查默认文件是否存在，如果存在则删除  
            if (File.Exists(digitFilePath))
            {
                File.Delete(digitFilePath);
            }

            List<string> lines = new List<string>(); // 用于存储所有要写入文件的行  
            // 在文件前添加总路数 
            string numStr = "总路数=" + digitEnts.Count.ToString();
            lines.Add(numStr);

            // 构建要写入文件的行  
            for (int i = 1; i <= digitEnts.Count; i++)
            {
                string key = $"{i}";
                string value = digitEnts[i - 1].ToCSMFormat();
                lines.Add($"{key}={value}"); // 将键值对添加到列表中  
            }

            // 创建或覆盖文件并写入内容  
            using (StreamWriter writer = new StreamWriter(digitFilePath, false, Encoding.GetEncoding("GB2312")))
            {
                // 写入节  
                writer.WriteLine($"[{section}]");
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var line in lines)
                {
                    stringBuilder.AppendLine(line);
                }
                writer.Write(stringBuilder.ToString());

            }
            return isRight;
        }


        public bool ReadLKTypeCode(out int LKTypeCode)
        {
            bool isRight = false;
            LKTypeCode = -1;
            //判断是否存在配置文件
            string fileSysCfgPath = filePath + "CFG\\SysCfg.ini";
            if (File.Exists(fileSysCfgPath))
            {
                //读取IP和端口
                string LKTypeCodeStr = INIHelper.Read("本机", "模拟类型", "-1", fileSysCfgPath);

                if (string.IsNullOrEmpty(LKTypeCodeStr) || "-1".Equals(LKTypeCodeStr))
                {
                    return isRight;
                }
                LKTypeCode = Convert.ToInt32(LKTypeCodeStr);
                isRight = true;
            }
            return isRight;
        }


        public bool ReadDelayTime(ref int DealyTime)
        {
            bool isRight = false;
            string DealyTimeStr = "";
            //判断是否存在配置文件
            string fileConfigPath = filePath + "CFG\\config.ini";
            if (File.Exists(fileConfigPath))
            {
                //读取IP和端口
                DealyTimeStr = INIHelper.Read("网络控制", "默认延迟", "-1", fileConfigPath);

                if (string.IsNullOrEmpty(DealyTimeStr) || "-1".Equals(DealyTimeStr) || !CheckStrNum.CheckStrIsInteger(DealyTimeStr))
                {
                    DealyTime = 5;
                }
                else
                {
                    DealyTime = Convert.ToInt32(DealyTimeStr);
                }

                isRight = true;
            }

            return isRight;
        }

        /// <summary>
        /// 读取通用的XML文件类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="lKDevStatus"></param>
        /// <returns></returns>
        public bool ReadStandardXMLToClass<T>(string path, ref T lKDevStatus)
        {
            bool isRight = false;
            string fileCommPath = filePath + path;

            if (!File.Exists(fileCommPath))
            {
                return isRight;
            }

            try
            {
                lKDevStatus = MyUtility.Utility.DeserializeToXmlTool.DeserializeFromXml<T>(fileCommPath);
                isRight = true;
            }
            catch (Exception e)
            {
                LogHelper.WriteLog("ReadDevStatusXML", e);
            }

            return isRight;
        }


    }
}

