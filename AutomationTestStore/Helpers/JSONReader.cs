using AutomationTestStore.Contants;
using AutomationTestStore.DataModels;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationTestStore.Helpers
{
    internal class JSONReader
    {
        /// <summary>
        /// Get configuration in json file
        /// </summary>
        /// <param name="jsonFile"></param>
        /// <returns></returns>
        public static ConfigurationModel? GetConfigure(string jsonFile)
        {
            string jsonTxt = File.ReadAllText(jsonFile);
            return JsonConvert.DeserializeObject<ConfigurationModel>(jsonTxt);
        }

        /// <summary>
        /// Get only value of browser field
        /// </summary>
        /// <returns></returns>
        public static string GetBrowserFromJSON()
        {
            string browser = GetConfigure(GeneralConstants.CONFIGURATION_FILE_PATH).Browser;
            return String.IsNullOrEmpty(browser) ? "" : browser;
        }

        /// <summary>
        /// Get only value of url field
        /// </summary>
        /// <returns></returns>
        public static string GetUrlFromJSON()
        {
            string url = GetConfigure(GeneralConstants.CONFIGURATION_FILE_PATH).URL;
            return String.IsNullOrEmpty(url) ? "" : url; ;
        }
    }
}
