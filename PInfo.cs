using HarmonyLib;

namespace StSStuffMod
{
    public static class PInfo
    {
        // each loaded plugin needs to have a unique GUID. usually author+generalCategory+Name is good enough
        public const string GUID = "intoxicatedkid.stsstuffmod";
        public const string Name = "StS Stuff Mod";
        public const string version = "1.1.3";
        public static readonly Harmony harmony = new Harmony(GUID);

    }
}
