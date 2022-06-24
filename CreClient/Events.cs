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
    }
}
