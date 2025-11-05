using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SWJCTool.Utility
{
    public static class TextExtractPrefixDeal
    {
        public static string ExtractPrefixAdvanced(string input, string keyword)
        {
            try
            {
                string escapedKeyword = Regex.Escape(keyword);

                // 支持多种分隔符：-、空格、下划线等
                string pattern = $@"^(.*?)[\s\-_]?{escapedKeyword}$";

                Match match = Regex.Match(input, pattern);

                if (match.Success)
                {
                    string prefix = match.Groups[1].Value;

                    // 清理结果：去除末尾可能的分隔符
                    return Regex.Replace(prefix, @"[\s\-_]+$", "");
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"提取过程中出错: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
