﻿#if MELONLOADER

using MelonLoader;
using System;
using System.Runtime.CompilerServices;

#elif BEPINEX

global using KiraiMod.Core;
global using KiraiMod.Core.ModuleAPI;
using BepInEx;
using BepInEx.IL2CPP;

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

            Managers.DependencyManager.LoadReModCore();

            RuntimeHelpers.RunClassConstructor(typeof(CreClient).TypeHandle);

            Managers.ModuleManager.LoadModules();
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

            KiraiMod.Core.Managers.ModuleManager.Register();
        }
    }

#endif

    public static class CreClient
    {
        internal static HttpClient http = new();

        // Entry point for both loaders
        static CreClient()
        {

        }
    }
}
