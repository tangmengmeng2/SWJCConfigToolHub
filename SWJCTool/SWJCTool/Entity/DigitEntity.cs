using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWJCTool.Utility;

namespace SWJCTool.Entity
{
    /// <summary>
    /// 开关量配置
    /// </summary>
    public class DigitEntity
    {
        public int DigitID { get; set; }
        public string DigitName { get; set; }
        public int DigitOrder { get; set; }
        public int DigitType { get; set; }
        public int DigitInverse { get; set; }
        public int DigitExtNum { get; set; }
        public string DigitNotes { get; set; }
        public string DigitAlarmDelay { get; set; }
        public string DigitRecoveryDelay { get; set; }
        public string DigitAlarmLevel { get; set; }
        public string DigitYL { get; set; }
        public string DigitAlarmInfo { get; set; }

        public DigitEntity(int digitID, string digitName, int digitOrder, int digitType, int digitInverse, int digitExtNum)
        {
            DigitID = digitID;
            DigitName = digitName;
            DigitOrder = digitOrder;
            DigitType = digitType;
            DigitInverse = digitInverse;
            DigitExtNum = digitExtNum;
        }

        public string ToCSMFormat()
        {
            return DigitName + ",\t" +
                DigitOrder + ",\t" +
                DigitType + ",\t" +
                DigitInverse + ",\t" +
                DigitExtNum + ",\t" +
                DigitNotes + ",\t" +
                DigitAlarmDelay + ",\t" +
                DigitRecoveryDelay + ",\t";
        }

        public override string ToString()
        {
            return "DigitEnt{" +
                    "digitID='" + DigitID + '\'' +
                    ", digitName='" + DigitName + '\'' +
                    ", digitOrder='" + DigitOrder + '\'' +
                    ", digitType='" + DigitType + '\'' +
                    ", digitInverse='" + DigitInverse + '\'' +
                    ", digitExtNum='" + DigitExtNum + '\'' +
                    ", digitNotes='" + DigitNotes + '\'' +
                    ", digitAlarmDelay='" + DigitAlarmDelay + '\'' +
                    ", digitRecoveryDelay='" + DigitRecoveryDelay + '\'' +
                    ", digitAlarmLevel='" + DigitAlarmLevel + '\'' +
                    ", digitYL='" + DigitYL + '\'' +
                    ", digitAlarmInfo='" + DigitAlarmInfo + '\'' +
                    '}';
        }
    }


}
