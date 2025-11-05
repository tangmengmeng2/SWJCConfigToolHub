using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWJCTool.Utility
{
    public static class GenericIniWriter<T>
    {
        public static void WriteSectionsToFile(List<IniSection<T>> sections, string filePath)
        {
            using (var writer = new StreamWriter(filePath, true, Encoding.UTF8, 65536))
            {
                foreach (var section in sections)
                {
                    WriteSection(writer, section);
                    writer.WriteLine();
                }
            }
        }

        private static void WriteSection(StreamWriter writer, IniSection<T> section)
        {
            if (section.DataList == null)
                return;

            writer.WriteLine($"[{section.SectionName}]");
            writer.WriteLine($"{section.InfoLine}");
            writer.WriteLine($"总路数={section.DataList.Count}");

            // 使用并行处理提高性能（如果格式化操作很耗时）
            if (section.UseParallelProcessing)
            {
                WriteSectionParallel(writer, section);
            }
            else
            {
                WriteSectionSequential(writer, section);
            }
        }

        private static void WriteSectionSequential(StreamWriter writer, IniSection<T> section)
        {
            foreach (var item in section.DataList)
            {
                string line = section.Formatter(item);
                writer.WriteLine(line);
            }
        }

        private static void WriteSectionParallel(StreamWriter writer, IniSection<T> section)
        {
            // 先并行格式化所有行
            var lines = new string[section.DataList.Count];
            Parallel.For(0, section.DataList.Count, i =>
            {
                lines[i] = section.Formatter(section.DataList[i]);
            });

            // 然后顺序写入文件
            foreach (string line in lines)
            {
                writer.WriteLine(line);
            }
        }
    }

    public class IniSection<T>
    {
        public string SectionName { get; set; }
        public string InfoLine { get; set; }
        public List<T> DataList { get; set; }
        public Func<T, string> Formatter { get; set; }
        public bool UseParallelProcessing { get; set; } = false;
    }
}
