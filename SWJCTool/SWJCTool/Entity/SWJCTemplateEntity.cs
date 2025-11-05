using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWJCTool.Entity
{
    public class SWJCTemplateEntity
    {
        public int Order { get; private set; } // 序号

        public string Name { get; private set; } // 名称

        public int Num { get; private set; }  // 总数

        public string DeviceTypeCode { get; private set; } // 名称

        public string DeviceTypeSubCode { get; private set; } // 名称

        public List<SWJCTemplateUnit> SWJCTemplateUnits = new List<SWJCTemplateUnit>();

        public SWJCTemplateEntity(int order, string name, int num, string deviceTypeCode, string deviceTypeSubCode) : this(order, name, num)
        {
            DeviceTypeCode = deviceTypeCode;
            DeviceTypeSubCode = deviceTypeSubCode;
        }

        public SWJCTemplateEntity(int order, string name, int num)
        {
            Order = order;
            Name = name;
            Num = num;
        }

        public void AddUnit(SWJCTemplateUnit swjcTemplateUnit)
        {
            if (swjcTemplateUnit != null)
            {
                SWJCTemplateUnits.Add(swjcTemplateUnit);
                Num++;
            }

        }

    }



    public class SWJCTemplateUnit
    {
        public int Order { get; private set; }
        public string UnitName { get; private set; }
        public int AnalogTag { get; set; } //标记
        public int AnalogType { get; set; } //类型
        public double RangeLower { get; set; }  //量程下限
        public double RangeUpper { get; set; } //量程上限

        public string AnalogUnit { get; set; } // 模拟量单位
        public double AnalogFactor { get; set; } //模拟量倍率

        public SWJCTemplateUnit(int order, string unitName)
        {
            Order = order;
            UnitName = unitName;
        }


    }


    public class SWJCAnaloyCodeEntity
    {
        public int Order { get; private set; } // 序号

        public string Name { get; private set; } // 名称

        public int AnaloyCode { get; private set; } // 模拟量子类型码

        public SWJCAnaloyCodeEntity(int order, string name, int analoyCode)
        {
            Order = order;
            Name = name;
            AnaloyCode = analoyCode;
        }
    }



}
