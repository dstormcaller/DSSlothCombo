using Dalamud.Game.ClientState.Keys;
using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Game.ClientState.Party;
using FFXIVClientStructs.FFXIV.Component.GUI;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System;
using XIVSlothCombo.Services;
using FFXIVClientStructs.FFXIV.Client.Game;
using XIVSlothCombo.CustomComboNS.Functions;
using XIVSlothCombo.Combos.PvE;
using System.Runtime.InteropServices;
using XIVSlothCombo.Combos.PvP;
using XIVSlothCombo.Core;
using XIVSlothCombo.Combos;
using System.Diagnostics;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using ECommons.DalamudServices;

namespace XIVSlothCombo.CustomComboNS
{
    /// <summary> Base class for each combo. </summary>
    internal abstract partial class CustomCombo : CustomComboFunctions
    {
        #region Boss Check
        // Rank 0 - Trash
        // Rank 1 - Hunt Target (S/A/B). Eureka Pazuzu is also Rank 1. Fate bosses might also be R1. Haven't checked.
        // Rank 2 - Final Dungeon Boss, Trial Boss, Raid Boss, Alliance Raid Boss.
        // Rank 3 - Trash
        // Rank 4 - Raid Trash (Alexander)
        // Rank 5 - There is no Rank 5
        // Rank 6 - First 2 bosses in dungeons.
        // Rank 7 - PvP stuff? 
        // Rank 8 - A puppy.
        // 32, 33 34, 35, 36, 37 - Old Diadem mobs.
        // Rank checks are very reliable. The HP check is done for unsynced content mainly.
        // There are probably better ways of doing than relying on HP for unsync stuff but this works.
        // For how to use, obviously just add a BossCheck() on dots for example. (Higanbana would be the perfect use case since the dot takes so long to be worth using)
        protected static uint DataId()
        {
            if (CurrentTarget is not IBattleChara chara)
                return 0;
            return chara.DataId;
        }
        public static bool BossCheck()
        {
            double maxHealth = LocalPlayer.MaxHp;
            var rank = Svc.Data.GetExcelSheet<Lumina.Excel.GeneratedSheets.BNpcBase>()?.GetRow((uint)DataId());

            return rank != null && EnemyHealthMaxHp() >= maxHealth * 11 && (rank.Rank == 2 || rank.Rank == 6);
        }
        public static bool BossCheckLast()
        {
            double maxHealth = LocalPlayer.MaxHp;
            var rank = Svc.Data.GetExcelSheet<Lumina.Excel.GeneratedSheets.BNpcBase>()?.GetRow((uint)DataId());

            return rank != null && EnemyHealthMaxHp() >= maxHealth * 11 && (rank.Rank == 2);
        }
        #endregion
    }
}