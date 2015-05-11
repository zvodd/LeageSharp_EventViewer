using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace EventViewer
{
    class Settings
    {
        public static Menu myMenu;

        public static Vector2 screenMax = new Vector2(1920, 1080);
        public static bool showEvents { get { return myMenu.Item("showEvents").GetValue<bool>(); } }
        public static GameEventId[] ignoredEvents = { GameEventId.OnSurrenderVote }; // list of useless events
        public static bool OnScreenEventsOnly { get { return myMenu.Item("onScreenEventsOnly").GetValue<bool>(); }  }
        public static bool showMouseScreenPos { get { return myMenu.Item("showMouseScreenPos").GetValue<bool>(); }  }
        public static bool showMouseWorldPos  { get { return myMenu.Item("showMouseWorldPos").GetValue<bool>(); } }

        public static void Load()
        {
            // not implemented
        }

        public static void Save()
        {
            // not implemented
        }


        public static void makeMenu()
        {
            myMenu = new Menu("Event Viewer", "eventview", true);
            myMenu.AddSubMenu(new Menu("Settings", "settings"));
            myMenu.SubMenu("settings").AddItem(new MenuItem("showEvents", "Show events").SetValue(true));
            myMenu.SubMenu("settings").AddItem(new MenuItem("onScreenEventsOnly", "Only show on-screen events").SetValue(true));
            myMenu.SubMenu("settings").AddItem(new MenuItem("showMouseScreenPos", "showMouseScreenPos").SetValue(false));
            myMenu.SubMenu("settings").AddItem(new MenuItem("showMouseWorldPos", "showMouseWorldPos").SetValue(false));
            //myMenu.AddItem(new MenuItem("track", "Track Game").SetValue(true));
            myMenu.AddToMainMenu();
        }
    }
}
