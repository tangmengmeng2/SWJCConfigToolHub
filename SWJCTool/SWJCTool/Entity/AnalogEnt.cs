using MyUtility.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWJCTool.Utility;

namespace SWJCTool.Entity
{
    public class CSMAnalogEnt
    {
        public long Id { get; set; }
        public string AnalogName { get; set; }
        public int AnalogFlag { get; set; }
        public int AnalogOrder { get; set; }
        public string AnalogType { get; set; }
        public int AnalogTypeNum { get; set; }
        public float AnalogRangeLower { get; set; }
        public float AnalogRangeUpper { get; set; }
        public float AnalogCoefficient { get; set; }
        public float AnalogLimitUpper { get; set; }
        public float AnalogLimitLower { get; set; }
        public float AnalogShuntLimit { get; set; }
        public int AnalogExtensionNum { get; set; }
        public string AnalogUnit { get; set; }
        public string Switch1 { get; set; }
        public string Switch2 { get; set; }
        public string Switch3 { get; set; }
        public string Switch4 { get; set; }

        public CSMAnalogEnt()
        {
            Id = CreateGuid.GuidToLongID();
        }

        public CSMAnalogEnt(string analogName, int analogFlag, int analogOrder, string analogType, float analogRangeLower, float analogRangeUpper, float analogCoefficient, float analogLimitUpper, float analogLimitLower, float analogShuntLimit, int analogExtensionNum, string analogUnit, string switch1, string switch2, string switch3, string switch4)
        {
            Id = CreateGuid.GuidToLongID();
            AnalogName = analogName;
            AnalogFlag = analogFlag;
            AnalogOrder = analogOrder;
            AnalogType = analogType;
            AnalogRangeLower = analogRangeLower;
            AnalogRangeUpper = analogRangeUpper;
            AnalogCoefficient = analogCoefficient;
            AnalogLimitUpper = analogLimitUpper;
            AnalogLimitLower = analogLimitLower;
            AnalogShuntLimit = analogShuntLimit;
            AnalogExtensionNum = analogExtensionNum;
            AnalogUnit = analogUnit;
            Switch1 = switch1;
            Switch2 = switch2;
            Switch3 = switch3;
            Switch4 = switch4;
        }

        public override string ToString()
        {
            return base.ToString();
        }

    }

    public class FKCSMAnalogEnt
    {
        public CSMAnalogEnt _CSMAnalogEnt { get; set; }
        public int Order { get; set; }
        public int ADOrder { get; set; }


    }

}
