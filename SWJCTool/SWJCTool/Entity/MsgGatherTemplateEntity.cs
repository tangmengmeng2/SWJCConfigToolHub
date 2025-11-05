using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWJCTool.Entity
{
    public class MsgGatherTemplateEntity
    {
        public int Order { get; set; }
        public string Name { get; set; }
        public int Num { get; set; }

        public List<MsgGatherTemplateUnit> MsgGatherTemplateUnits = new List<MsgGatherTemplateUnit>();

        public MsgGatherTemplateEntity(int order, string name)
        {
            Order = order;
            Name = name;
        }

        public void AddUnit(MsgGatherTemplateUnit msgGatherTemplateUnit)
        {
            Num++;
            MsgGatherTemplateUnits.Add(msgGatherTemplateUnit);
        }

    }


    public class MsgGatherTemplateUnit
    {
        public int Order { get; set; }
        public string Name { get; set; }
        public string UnitName { get; set; }
        public int TypeCode { get; set; }
        public int UnitFlag { get; set; }
        public string ReportHType { get; set; }
        public string ReportLType { get; set; }
        public int ReportFlag { get; set; }
        public string ReportCloumn1 { get; set; }
        public string ReportCloumn2 { get; set; }
        public string ReportCloumn3 { get; set; }
        public string Source { get; set; }
        public string SourceInterface { get; set; }
        public string SourceInterfaceName { get; set; }
        public string KeyWord { get; set; }


    }

    public class MsgDetailTemplate
    {
        public int Order { get; set; }
        public string Name { get; set; }
        public string DetailValue { get; set; }

        public MsgDetailTemplate(int order, string name, int[] detail)
        {
            Order = order;
            Name = name;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in detail)
            {
                stringBuilder.Append("\t" + item.ToString() + ",");
            }
            DetailValue = stringBuilder.ToString();
        }
    }



}
