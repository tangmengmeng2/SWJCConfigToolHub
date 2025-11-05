using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWJCTool.Utility
{
    public static class FlexibleFileWriter
    {
        public static void WriteEntitiesWithHeader<T>(List<T> entities, int code, string codeName, string filePath,
            string sectionHeader, Func<T, string> lineFormatter, Action<StreamWriter> writeHeader = null)
        {
            if (entities == null || entities.Count == 0)
                return;

            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                // 写入节头
                if (!string.IsNullOrEmpty(sectionHeader))
                {
                    writer.WriteLine($"[{sectionHeader}]");
                }

                // 写入总数行
                writer.WriteLine($"总路数={entities.Count}");
                // 写入类型码
                writer.WriteLine($"名称={codeName}");
                // 写入类型码
                writer.WriteLine($"模拟量子类型码={code}");

                // 自定义头部写入
                writeHeader?.Invoke(writer);


                // 写入数据行
                foreach (var entity in entities)
                {
                    writer.WriteLine(lineFormatter(entity));
                }
            }
        }
    }
}
