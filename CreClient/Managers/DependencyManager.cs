#if MELONLOADER

using MelonLoader;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CreClient.Managers
{
    public static class DependencyManager
    {
        public const string ReModCorePath = "UserLibs/ReMod.Core.dll";
        public const string UpdaterPath = "Plugins/ReMod.Core.Updater.dll";

        public static void LoadReModCore()
        {
            if (AppDomain.CurrentDomain.GetAssemblies().Any(x => x.GetName().Name == "ReMod.Core"))
                return;


            if (File.Exists(ReModCorePath))
            {
                Utils.SmartLogger.Info("Loading ReMod.Core");

                Assembly.Load(File.ReadAllBytes(ReModCorePath));
            }
            else
            {
                Utils.SmartLogger.Warning("ReMod.Core does not exist");

                if (MelonHandler.Plugins.Any(plugin => plugin.Info.Name == "ReMod.Core.Updater"))
                    RMCUFailed();

                Utils.SmartLogger.Info("Downloading ReMod.Core.Updater");

                byte[] updater;

                try
                {
                    updater = CreClient.http.GetByteArrayAsync("https://github.com/PennyBunny/ReMod.Core.Updater/releases/latest/download/ReMod.Core.Updater.dll").Result;
                } 
                catch (Exception ex)
                {
                    Utils.SmartLogger.Debug(ex.ToString());
                    Utils.SmartLogger.Fatal("ReMod.Core is missing and the updater failed to download.");
                    for (; ; );
                }

                Utils.SmartLogger.Info("Saving ReMod.Core.Updater");

                File.WriteAllBytes(UpdaterPath, updater);

                Utils.SmartLogger.Info("Loading ReMod.Core.Updater");

                ((MelonPlugin)Activator.CreateInstance(Assembly.Load(updater).GetCustomAttribute<MelonInfoAttribute>().SystemType)).OnPreInitialization();

                Utils.SmartLogger.Info("Loading ReMod.Core");

                if (File.Exists(ReModCorePath))
                    Assembly.Load(File.ReadAllBytes(ReModCorePath));
                else RMCUFailed();
            }
        }

        private static void RMCUFailed()
        {
            Utils.SmartLogger.Fatal("ReMod.Core.Updater failed");
            for (; ; );
        }
    }
}

#endif
