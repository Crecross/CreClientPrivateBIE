#if MELONLOADER

using CreClient.Managers;
using MelonLoader;
using System;
using System.Runtime.CompilerServices;

#elif BEPINEX

global using KiraiMod.Core;
global using KiraiMod.Core.ModuleAPI;
using BepInEx;
using BepInEx.IL2CPP;
using KiraiMod.Core.Managers;

#endif

using System.Net.Http;

#if MELONLOADER

[assembly: MelonInfo(typeof(CreClient.Mod), "CreClient", "2.0.0", "Crecross#9901")]
[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonColor(ConsoleColor.Blue)]

#endif

namespace CreClient
{

#if MELONLOADER

    public class Mod : MelonMod
    {
        public override void OnApplicationStart()
        {
            Utils.SmartLogger.SetupML(LoggerInstance);

            DependencyManager.LoadReModCore();

            RuntimeHelpers.RunClassConstructor(typeof(CreClient).TypeHandle);
        }
    }

#elif BEPINEX

    [BepInPlugin(CreClient.GUID, "CreClient", "2.0.0")]
    [BepInDependency(KiraiMod.Core.Plugin.GUID)]
    [BepInDependency(TypeScanner.TypeScanner.GUID)]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
            Utils.SmartLogger.SetupBIE(Log);

            typeof(CreClient).Initialize();
        }
    }

#endif

    public static class CreClient
    {
        public const string GUID = "com.github.Crecross.CreClient";

        internal static HttpClient http = new();

        // Entry point for both loaders
        static CreClient()
        {
            ModuleManager.Register();
        }
    }
}
