using System;

namespace CreClient.Utils
{
    public static class SmartLogger
    {
        private static Action<string> _Debug;
        private static Action<string> _Info;
        private static Action<string> _Message;
        private static Action<string> _Warning;
        private static Action<string> _Error;
        private static Action<string> _Fatal;

#if MELONLOADER
        private static MelonLoader.MelonLogger.Instance logger;

        public static void SetupML(MelonLoader.MelonLogger.Instance logger)
        {
            SmartLogger.logger = logger;

            _Debug /*  */ = msg => Log(0, $"{msg}");
            _Info /*   */ = msg => Log(1, $"{msg}");
            _Message /**/ = msg => Log(2, $"{msg}");
            _Warning /**/ = msg => Log(3, $"{msg}");
            _Error /*  */ = msg => Log(4, $"{msg}");
            _Fatal /*  */ = msg => Log(5, $"{msg}");
        }

        private static void Log(int level, string message)
        {
            if (logger is null) return;

            if (level < 3)
                logger.Msg("[" + Levels[level].Item1.ToUpper() + "] " + message);
            else if (level == 3)
                logger.Warning(message);
            else logger.Error(message);

            Console.WriteLine("\x1B[1A\x1B[27C\x1B[0K" + Levels[level].Item2 + "[" + Levels[level].Item1 + "] " + message + "\x1b[0m");
        }

        private static readonly (string, string)[] Levels =
        {
            ("Debug", /*  */ "\x1b[37m"),
            ("Info", /*   */ "\x1b[36m"),
            ("Message", /**/ "\x1b[34m"),
            ("Warning", /**/ "\x1b[33m"),
            ("Error", /*  */ "\x1b[31m"),
            ("Fatal", /*  */ "\x1b[31m"),
        };
#elif BEPINEX
        public static void SetupBIE(BepInEx.Logging.ManualLogSource logger)
        {
            _Debug /*  */ = logger.LogDebug;
            _Info /*   */ = logger.LogInfo;
            _Message /**/ = logger.LogMessage;
            _Warning /**/ = logger.LogWarning;
            _Error /*  */ = logger.LogError;
            _Fatal /*  */ = logger.LogFatal;
        }
#endif

        public static void Debug(string message) => _Debug?.Invoke(message);
        public static void Info(string message) => _Info?.Invoke(message);
        public static void Message(string message) => _Message?.Invoke(message);
        public static void Warning(string message) => _Warning?.Invoke(message);
        public static void Error(string message) => _Error?.Invoke(message);
        public static void Fatal(string message) => _Fatal?.Invoke(message);

        internal static void PrintAll()
        {
            Debug("Test Debug");
            Info("Test Info");
            Message("Test Message");
            Warning("Test Warning");
            Error("Test Error");
            Fatal("Test Fatal");
        }
    }
}
