using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MotionConfigHandler
{
    public class MotionConfigReader
    {
        public MotionConfigReader()
        {
        }

        public async Task<MotionConfig> ReadConfigurationFile(string filePath)
        {

            if (File.Exists(filePath))
            {
                var reader = new StreamReader(filePath);
                var content = await reader.ReadToEndAsync();
                reader.Close();

                var config = new MotionConfig();
                var realProperties = config.GetType().GetProperties();
                var properties = config.GetType().GetProperties()
                    .Select(p => new {p, attr = p.GetCustomAttribute<MotionConfigProperty>()})
                    .Where(p => p.attr != null);

                foreach (var p in properties)
                {
                    var propertyName = p.attr.PropertyName;
                    var value = betweenStrings(content, $"\r\n{propertyName} ", "\r\n");
                    Console.WriteLine($"{propertyName}={value}");
                }


                Debug.WriteLine(properties);
            }


            return null;
        }

        public static String betweenStrings(String text, String start, String end)
        {
            int p1 = text.IndexOf(start) + start.Length;
            int p2 = text.IndexOf(end, p1);

            if (end == "") return (text.Substring(p1));
            else return text.Substring(p1, p2 - p1);
        }
    }
}