#if MELONLOADER

using ReMod.Core.UI.QuickMenu;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using VRC.UI.Core;

namespace CreClient.Managers
{
    [Module]
    public static class RMCUIManager
    {
        static RMCUIManager() => MelonLoader.MelonCoroutines.Start(WaitForUiManager());

        public static Dictionary<string, ReMenuPage> pages = new();

        public static event Action UILoaded;

        private static IEnumerator WaitForUiManager()
        {
            while (GameObject.Find("UserInterface")?.GetComponentInChildren<VRC.UI.Elements.QuickMenu>(true) == null) yield return null;

            UILoaded?.Invoke();

            pages[""] = ReMenuPage.Create("CreClient", true);
            ReTabButton.Create("CreClient", "Open the CreClient menu", "CreClient", null);

            MemberAttribute.Added += HandleMember;
            MemberAttribute.All.ForEach(HandleMember);
        }

        private static void HandleMember(MemberAttribute member)
        {
            string[] sections = member.Section.Split('.');
            string current = null;
            ReMenuPage parent = pages[""];

            foreach (string section in sections)
            {
                if (current == null)
                    current = section;
                else current += "." + section;

                if (!pages.TryGetValue(current, out ReMenuPage page))
                    page = pages[current] = parent.AddMenuPage(section, "Open the submenu");
                parent = page;
            }

            if (member is InteractAttribute interactive)
                parent.AddButton(interactive.Name, interactive.Description, interactive.Invoke);
            else if (member is ConfigureAttribute<bool> toggle)
                parent.AddToggle(toggle.Name, toggle.Description, value => toggle.Value = value, toggle.Value);
        }
    }
}

#endif
