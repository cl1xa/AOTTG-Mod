using UnityEngine;

namespace Mod
{
    public class Main
    {
        public static Main instance;

        public static GameObject obj;

        public static bool 我讨厌黑人;

        public Main()
        {
            if (instance == null)
            {
                obj = new GameObject();

                //Local initialization of modded objects
                obj.AddComponent<DebugConsole>();
                obj.AddComponent<Menu>();

                Object.DontDestroyOnLoad(obj);
            }

            instance = this;
        }
    }
}