using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace MotionConfigManager
{
    public class MotionConfigHandler
    {
        private readonly string _motionWebcontrolUrl;
        private readonly string _motionConfigParh;

        private readonly HttpClient _httpClient;

        //Config Params
        private const string ConfigSetParam = "/config/set?";


        //Commands
        private const string DetectionStatusCommand = "/detection/status";
        private const string DetectionStartCommand = "/detection/start";
        private const string DetectionStopCommand = "/detection/pause";

        private const string DetectionStatusOn = "ACTIVE";
        private const string DetectionStatusOff = "PAUSE";


        public MotionConfigHandler(string motionWebcontrolUrl, string motionConfigParh)
        {
            _motionWebcontrolUrl = motionWebcontrolUrl;
            _motionConfigParh = motionConfigParh;
            _httpClient = new HttpClient();
        }

        public async Task<MotionConfig> ReadConfiguration(string filePath)
        {
            if (File.Exists(filePath))
            {
                var reader = new StreamReader(filePath);
                var content = await reader.ReadToEndAsync();
                reader.Close();

                var config = new MotionConfig();

                var motionConfigProperties = config.GetType().GetProperties();
                var fields = motionConfigProperties
                    .Select(p => new {p, attr = p.GetCustomAttribute<MotionConfigProperty>()})
                    .Where(p => p.attr != null);

                foreach (var field in fields)
                {
                    var propertyName = field.attr.PropertyName;
                    var fieldValue = BetweenStrings(content, $"\n{propertyName} ", "\n\n#");
                    Console.WriteLine($"{propertyName}={fieldValue}");

                    var motionConfigProperty = motionConfigProperties.FirstOrDefault(rp =>
                        rp.CustomAttributes.FirstOrDefault(ca =>
                            ca.ConstructorArguments.Select(coa =>
                                coa.Value).ToList().Contains(propertyName)) != null);

                    if (motionConfigProperty != null)
                        motionConfigProperty.SetValue(config, fieldValue);
                }

                config.RecordMotion = await IsRecording();
                return config;
            }

            return null;
        }


        public async Task SaveConfiguration(MotionConfig config)
        {
            var currentConfig = await ReadConfiguration(_motionConfigParh);
            if (config.RecordMotion != currentConfig.RecordMotion)
            {
                await SendRecordStateChange(config.RecordMotion);
            }

            var configParams = new List<string>();
            var motionConfigProperties = config.GetType().GetProperties();
            foreach (var property in motionConfigProperties)
            {
                foreach (var customAttribute in property.CustomAttributes)
                {
                    var attributeName = customAttribute.ConstructorArguments.Select(ca => ca.Value).FirstOrDefault();
                    var value = property.GetValue(config);
                    configParams.Add($"{attributeName}={value}");
                }
            }

            await SendConfigParams(configParams);
        }

        private async Task SendRecordStateChange(bool record)
        {
            var command = record ? DetectionStartCommand : DetectionStopCommand;
            var response = await _httpClient.GetAsync(_motionWebcontrolUrl + command);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Failed to change recording state. Command={command}");
                Debug.WriteLine(content);
            }
        }

        private async Task<bool> IsRecording()
        {
            var response = await _httpClient.GetAsync(_motionWebcontrolUrl + DetectionStatusCommand);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return content.Contains(DetectionStatusOn);

            Debug.WriteLine($"Failed check recording status. Command={DetectionStatusCommand}");
            Debug.WriteLine(content);
            return false;
        }

        private async Task SendConfigParams(IEnumerable<string> configParams)
        {
            var urls = configParams.Select(cp => _motionWebcontrolUrl + ConfigSetParam + cp);
            foreach (var url in urls)
            {
                var response = await _httpClient.GetAsync(_motionWebcontrolUrl + DetectionStatusCommand);
                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"Set config failed. Url={url}");
                }
            }
        }

        private static string BetweenStrings(string text, string start, string end)
        {
            var p1 = text.IndexOf(start, StringComparison.Ordinal) + start.Length;
            var p2 = text.IndexOf(end, p1, StringComparison.Ordinal);
            return end == "" ? text.Substring(p1) : text.Substring(p1, p2 - p1);
        }
    }
}