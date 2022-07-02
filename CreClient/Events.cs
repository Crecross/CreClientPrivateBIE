using System;

namespace CreClient
{
    public static class Events
    {
        public static event Action UILoaded
        {
#if MELONLOADER
            add => Managers.RMCUIManager.UILoaded += value;
            remove => Managers.RMCUIManager.UILoaded -= value;
#elif BEPINEX
            add => KiraiMod.Core.UI.LegacyGUIManager.OnLoad += value;
            remove => KiraiMod.Core.UI.LegacyGUIManager.OnLoad -= value;
#endif
        }

        public static event Action Update
        {
#if MELONLOADER
            add => Mod.Update += value;
            remove => Mod.Update -= value;
#elif BEPINEX
            add => KiraiMod.Core.Events.Update += value;
            remove => KiraiMod.Core.Events.Update -= value;
#endif
        }
    }
}
