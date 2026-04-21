using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Nodes.Debug;
using Godot;

namespace YourMod.Patches;

[HarmonyPatch]
public static class HideCombatUiPatch
{
    private static readonly FieldInfo DebugReleaseInfoField =
        AccessTools.Field(typeof(NDebugInfoLabelManager), "_releaseInfo");

    private static readonly FieldInfo DebugModdedWarningField =
        AccessTools.Field(typeof(NDebugInfoLabelManager), "_moddedWarning");

    private static readonly FieldInfo DebugSeedField =
        AccessTools.Field(typeof(NDebugInfoLabelManager), "_seed");

    private static readonly FieldInfo DebugModWarningContainerField =
        AccessTools.Field(typeof(NDebugInfoLabelManager), "_modWarningContainer");

    private static readonly FieldInfo DebugModWarningLabelField =
        AccessTools.Field(typeof(NDebugInfoLabelManager), "_modWarningLabel");

    [HarmonyPatch(typeof(NCombatUi), "_Ready")]
    [HarmonyPostfix]
    public static void CombatUi_Ready_Postfix(NCombatUi __instance)
    {
        ForceHideCombatUi(__instance);
    }

    [HarmonyPatch(typeof(NCombatUi), "Activate")]
    [HarmonyPostfix]
    public static void CombatUi_Activate_Postfix(NCombatUi __instance)
    {
        ForceHideCombatUi(__instance);
    }

    [HarmonyPatch(typeof(NTopBar), "_Ready")]
    [HarmonyPostfix]
    public static void TopBar_Ready_Postfix(NTopBar __instance)
    {
        ForceHideTopBar(__instance);
    }

    [HarmonyPatch(typeof(NTopBar), "Initialize")]
    [HarmonyPostfix]
    public static void TopBar_Initialize_Postfix(NTopBar __instance)
    {
        ForceHideTopBar(__instance);
    }

    [HarmonyPatch(typeof(NRelic), "_Ready")]
    [HarmonyPostfix]
    public static void Relic_Ready_Postfix(NRelic __instance)
    {
        ForceHideRelic(__instance);
    }

    [HarmonyPatch(typeof(NDebugInfoLabelManager), "_Ready")]
    [HarmonyPostfix]
    public static void DebugInfo_Ready_Postfix(NDebugInfoLabelManager __instance)
    {
        ForceHideDebugInfo(__instance);
    }

    [HarmonyPatch(typeof(NDebugInfoLabelManager), "UpdateText")]
    [HarmonyPostfix]
    public static void DebugInfo_UpdateText_Postfix(NDebugInfoLabelManager __instance)
    {
        ForceHideDebugInfo(__instance);
    }

    [HarmonyPatch(typeof(NDebugInfoLabelManager), "_Input")]
    [HarmonyPostfix]
    public static void DebugInfo_Input_Postfix(NDebugInfoLabelManager __instance)
    {
        ForceHideDebugInfo(__instance);
    }

    private static void ForceHideCombatUi(NCombatUi ui)
    {
        if (ui == null)
            return;

        // 保留手牌，不隐藏
        SetTransparent(ui.EnergyCounterContainer);
        SetTransparent(ui.EndTurnButton);
        SetTransparent(ui.PlayQueue);
        SetTransparent(ui.PlayContainer);
        SetTransparent(ui.CardPreviewContainer);
        SetTransparent(ui.MessyCardPreviewContainer);

        // 只隐藏抽牌堆和弃牌堆
        SetTransparent(ui.DrawPile);
        SetTransparent(ui.DiscardPile);

        // 如果你也想隐藏 ExhaustPile，把下一行取消注释
        // SetTransparent(ui.ExhaustPile);

        // Hand 故意不处理，保持可见
        // SetTransparent(ui.Hand);
    }

    private static void ForceHideTopBar(NTopBar topBar)
    {
        if (topBar == null)
            return;

        // 不再用 AnimHide()，因为它会把 top bar 移走/禁用部分交互
        SetTransparent(topBar);

        // 双保险：把常见子节点也一起透明
        SetTransparent(topBar.Map);
        SetTransparent(topBar.Deck);
        SetTransparent(topBar.Pause);
        SetTransparent(topBar.PotionContainer);
        SetTransparent(topBar.RoomIcon);
        SetTransparent(topBar.FloorIcon);
        SetTransparent(topBar.BossIcon);
        SetTransparent(topBar.Gold);
        SetTransparent(topBar.Hp);
        SetTransparent(topBar.Portrait);
        SetTransparent(topBar.PortraitTip);
        SetTransparent(topBar.Timer);
        SetTransparent(topBar.TrailContainer as CanvasItem);
    }

    private static void ForceHideRelic(NRelic relic)
    {
        if (relic == null)
            return;

        SetTransparent(relic);
        SetTransparent(relic.Icon);
        SetTransparent(relic.Outline);
    }

    private static void ForceHideDebugInfo(NDebugInfoLabelManager manager)
    {
        if (manager == null)
            return;

        SetTransparent(DebugReleaseInfoField?.GetValue(manager) as CanvasItem);
        SetTransparent(DebugModdedWarningField?.GetValue(manager) as CanvasItem);
        SetTransparent(DebugSeedField?.GetValue(manager) as CanvasItem);
        SetTransparent(DebugModWarningContainerField?.GetValue(manager) as CanvasItem);
        SetTransparent(DebugModWarningLabelField?.GetValue(manager) as CanvasItem);
    }

    private static void SetTransparent(CanvasItem item)
    {
        if (item == null)
            return;

        item.Modulate = Colors.Transparent;
        item.Visible = true;
    }
}