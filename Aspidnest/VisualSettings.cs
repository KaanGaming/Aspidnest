using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modding;
using Aspidnest.Utils;

namespace Aspidnest
{
    public partial class Aspidnest : IMenuMod
    {
        public bool ToggleButtonInsideMenu => false;

        public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry)
        {
            var mk = new MenuMaker();

            return new List<IMenuMod.MenuEntry>()
            {
                mk.Entry("Teleport Enemies", "Automatically teleport the enemies to you when far away",
                v => stngs.enemytp = v == 1, () => stngs.enemytp == true ? 1 : 0,
                "Off", "On"),
                mk.Entry("Enemies Give Soul", "The immortal enemies give soul if this is on",
                v => stngs.enemysoul = v == 1, () => stngs.enemysoul == true ? 1 : 0,
                "Off", "On"),
                mk.FloatEntry("Enemy Scale", "How big the enemies are?",
                v => stngs.scaler = mk.GetFloat(v), () => mk.IdFromFloat(stngs.scaler)),
                mk.FloatEntry("Teleport Distance", "When Teleport Enemies is on, the distance required to teleport enemies back to the player",
                v => stngs.tpdist = mk.GetFloat(v), () => mk.IdFromFloat(stngs.tpdist)),
                mk.KeybindEntry("Toggle Bind", "Keybind to toggle enemies",
                v => stngs.togglebind = mk.GetKeybind(v), () => mk.IdFromKeybind(stngs.togglebind)),
                mk.Empty(),
                mk.Empty("Enemies:"),
                mk.IntEntry("Aspid Count", "Select how many primal aspids to spawn",
                v => stngs.aspidCount = v, () => stngs.aspidCount,
                0, 50),
                mk.IntEntry("C.Hunter Count", "Select how many crystal hunters to spawn",
                v => stngs.hunterCount = v, () => stngs.hunterCount,
                0, 50),
                mk.IntEntry("Loodle Count", "Select how many loodles/frogs to spawn",
                v => stngs.frogCount = v, () => stngs.frogCount,
                0, 50),
                mk.IntEntry("Petras Count", "Select how many mantis petras to spawn",
                v => stngs.petraCount = v, () => stngs.petraCount,
                0, 50),
                mk.IntEntry("H.Soldier Count", "Select how many hive soldiers to spawn",
                v => stngs.soldierCount = v, () => stngs.soldierCount,
                0, 50),
                mk.IntEntry("H.Guardian Count", "Select how many hive guardians to spawn",
                v => stngs.guardianCount = v, () => stngs.guardianCount,
                0, 50),
		        mk.IntEntry("Squit Count", "Select how many squits to spawn",
                v => stngs.squitCount = v, () => stngs.squitCount,
                0, 50),
		        mk.IntEntry("Flukefey Count", "Select how many flukefeys to spawn",
                v => stngs.flukeCount = v, () => stngs.flukeCount,
                0, 50),
		        mk.IntEntry("Mossy Count", "Select how many mossflies to spawn",
                v => stngs.mossyCount = v, () => stngs.mossyCount,
                0, 50),
		        mk.IntEntry("Lancer Count", "Select how many lance sentries to spawn",
                v => stngs.lanceCount = v, () => stngs.lanceCount,
                0, 50)
            };
        }
    }
}
