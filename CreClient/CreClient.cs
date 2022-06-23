#if MELONLOADER

using MelonLoader;
using System;
using System.Runtime.CompilerServices;

#elif BEPINEX

global using KiraiMod.Core;
using BepInEx;
using BepInEx.IL2CPP;

#endif

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

            Utils.SmartLogger.PrintAll();

            RuntimeHelpers.RunClassConstructor(typeof(CreClient).TypeHandle);
        }
    }

#elif BEPINEX

    [BepInPlugin(GUID, "CreClient", "2.0.0")]
    [BepInDependency(KiraiMod.Core.Plugin.GUID)]
    [BepInDependency(TypeScanner.TypeScanner.GUID)]
    public class Plugin : BasePlugin
    {
        public const string GUID = "com.github.Crecross.CreClient";

        public override void Load()
        {
            Utils.SmartLogger.SetupBIE(Log);

            typeof(CreClient).Initialize();
        }
    }

#endif

    public static class CreClient
    {
        // Entry point for both loaders
        static CreClient()
        {

        }
    }
}
