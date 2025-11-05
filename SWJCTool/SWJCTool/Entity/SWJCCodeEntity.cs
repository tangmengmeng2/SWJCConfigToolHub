using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWJCTool.Entity
{
    public class SWJCCodeEntity
    {
        public int Order { get; set; } // 序号
        public int ChannelId { get; set; } // 通道号,
        public int AddressId { get; set; } // 模块地址,

        public string Name { get; set; } // 区段名称,

        public int TrackTypeNum { get; set; }  // 轨道制式,

        public string TempleTypeName { get; set; } // 模板类型,

        public string DeviceTypeName { get; set; } // 预留,
        public int ExtNum { get; set; }  // 分机号




    }
}
