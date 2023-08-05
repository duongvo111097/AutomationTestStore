using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationTestStore.Contants
{
    internal static class GeneralContants
    {
        public static string? ROOT_PATH = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName ;
        public static string CONFIGURATION_PATH = Path.Combine(ROOT_PATH, FolderContants.CONFIGURATOIN_FOLDER);
        public static string CONFIGURATION_FILE_PATH = Path.Combine(CONFIGURATION_PATH, FileContants.CONFIGURATION_FILE);
        public static int TIME_TO_WAIT_FOR_WEB_LOADING_IN_SECOND = 15;
        public static int TIME_TO_WAIT_FOR_ELEMENT_LOADING_IN_SECOND = 15;
        public static int TIME_TO_POLLING_INTERVAL_IN_MILLISECONDS = 500;
    }

    internal static class FolderContants
    {
        public static string CONFIGURATOIN_FOLDER = "Config";
    }
    internal static class FileContants
    {
        public static string CONFIGURATION_FILE = "Configuration.json";
    }
}
