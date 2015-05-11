#region

using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

#endregion

namespace EventViewer
{
    class Program
    {

        private static Obj_AI_Hero Player { get { return ObjectManager.Player; } }

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnLoad;
        }


        private static void OnLoad(EventArgs args)
        {
            Game.OnNotify += OnGameNotify;
            Drawing.OnDraw += Drawing_OnDraw;
            Settings.makeMenu();
        }

        static void Drawing_OnDraw(EventArgs args)
        {
            float xpos = Settings.screenMax.X * 0.8645833333333333f;
            float ypos = Settings.screenMax.Y * 0.6296296296296296f;
            Vector3 gameCur = Game.CursorPos;
            Vector2 cursor = Drawing.WorldToScreen(gameCur);

            Drawing.DrawText(xpos, ypos, System.Drawing.Color.LightSkyBlue, "EventViewer Running.");
            if (Settings.showMouseScreenPos)
                Drawing.DrawText(xpos, ypos + 20, System.Drawing.Color.LightSkyBlue, "Cur X,Y: " + cursor.X+ ", " + cursor.Y);
            if (Settings.showMouseWorldPos)
                Drawing.DrawText(xpos, ypos + 40, System.Drawing.Color.LightSkyBlue, String.Format("Wrd X,Y: {0}, {1}, {2}", gameCur.X, gameCur.Y, gameCur.Z));
            //Drawing.DrawText(xpos, ypos + 60, System.Drawing.Color.LightSkyBlue, "Logger On: ");
        }

        private static void OnGameNotify(GameNotifyEventArgs args)
        {
            if (!Settings.showEvents)
                return;
            int netID = args.NetworkId;
            string eventname = args.EventId.ToString();
            GameObject obj = ObjectManager.GetUnitByNetworkId<GameObject>(netID);

            if (obj == null || !obj.IsValid)
                return;
            // filter unwanted events
            if (Settings.ignoredEvents.Any(x => (x == args.EventId)))
            {
                return;
            }

            Vector3 objPos = obj.Position;
            Vector2 objScreenPos = Drawing.WorldToScreen(obj.Position);
            string msg = null;
            ConsoleColor color = ConsoleColor.White;

            //If obj.Postition is applicable (AI objs) and event is not onscreen. Ignore event.
            if (Settings.OnScreenEventsOnly && typeof(Obj_AI_Base).IsInstanceOfType(obj))
            {
                if (!(objScreenPos.X <= Settings.screenMax.X && objScreenPos.X >= 0
                      && objScreenPos.Y <= Settings.screenMax.Y && objScreenPos.Y >= 0))
                {
                    return;
                }
            }
            switch (obj.Type)
            {
                case GameObjectType.obj_AI_Hero:
                    Obj_AI_Hero hero = obj as Obj_AI_Hero;

                    if (netID == Player.NetworkId)
                    {
                        color = ConsoleColor.Cyan;
                        msg = String.Format("Player : {0,19}", eventname);
                    }
                    else
                    {
                        string prefix;
                        if (hero.IsAlly)
                        {
                            color = ConsoleColor.Green;
                            prefix = "Ally   ";
                        }
                        else
                        {
                            color = ConsoleColor.Red;
                            prefix = "Enemy  ";
                        }
                        msg = String.Format("{0}: {1,19}: '{2}'",
                               prefix, eventname, hero.ChampionName);
                    }
                    break;
                case GameObjectType.obj_AI_Minion:
                    Obj_AI_Minion minion = obj as Obj_AI_Minion;                  

                        if (minion.IsAlly)
                            color = ConsoleColor.DarkYellow;
                        else
                            color = ConsoleColor.DarkMagenta;
                        msg = String.Format("Minion : {0,19}: '{1}'\n\t-'{2}': '{3}': {4},{5}",
                               eventname, minion.Name, minion.Type, minion.Flags, objScreenPos.X,
                               objScreenPos.Y);
                    
                    break;
                default:
                        msg = String.Format("Other  : {0,19}: '{1}'\n\t-'{2}': '{3}'",
                               eventname, obj.Name, obj.Type, obj.Flags);
                    break;
            }
            Console.ForegroundColor = color;
            if (msg != null) 
                Console.WriteLine(msg);
        }
    }
}
