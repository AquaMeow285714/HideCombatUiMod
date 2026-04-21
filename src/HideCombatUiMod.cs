using Godot.Bridge;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;

namespace HideCombatUiMod;

[ModInitializer(nameof(initialize))]
public class HideCombatUiMod
{
    public static void initialize()
    {
        Harmony harmony = new Harmony("HideCombatUiMod");
        harmony.PatchAll();

        ScriptManagerBridge.LookupScriptsInAssembly(typeof(HideCombatUiMod).Assembly);
    }
}