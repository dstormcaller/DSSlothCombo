using Dalamud.Game.ClientState.Objects.Types;
using FFXIVClientStructs.FFXIV.Client.Game;
using XIVSlothCombo.CustomComboNS.Functions;

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

        #region Item Usage
        public unsafe void UseItem(uint itemId)
        {
            FFXIVClientStructs.FFXIV.Client.Game.ActionManager.Instance()->UseAction(FFXIVClientStructs.FFXIV.Client.Game.ActionType.Item, itemId, 0xE0000000, 65535, 0, 0, null);
        }
        #endregion
        #region Can use potion
        public unsafe static bool CanUse()
        {
            var PotionCDGroup = 68;
            bool canpot = ActionManager.Instance()->GetRecastGroupDetail(PotionCDGroup)->IsActive == 0;
            return canpot;
        }
        // Execution can be called with something like this if (CanUse()) UseItem(1038956); 
        // that's the code for a HQ Hyper-Potion. 
        // This does NOT replace the action on the hotbar. The item is just...used so the conditions must be quite strict, but it works.

        #endregion
    }
}