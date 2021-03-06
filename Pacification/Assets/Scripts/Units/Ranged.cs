using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : Attacker
{
    public Ranged(Player owner)
    {
        Owner = owner;
        type = UnitType.RANGED;
        hasMadeAction = false;
        level = owner.unitLevel[1];
        maxLevel = 20;

        upgradeHP = 25;
        hp = (IsUpgraded() ? 400 : 100) - upgradeHP + ((level - (IsUpgraded() ? 10 : 0)) * upgradeHP);
        MaxHP = hp;

        upgradeATK = 20;
        defaultATK = (IsUpgraded() ? 300 : 80) - upgradeATK + ((level - (IsUpgraded() ? 10 : 0)) * upgradeATK);

        range = 2;
        mvtSPD = 3;
        currMVT = 0;

        dmgMultCity = (IsUpgraded() ? 0.5f : 0.25f);
        dmgMult = new Dictionary<UnitType, float>()
        {
            { UnitType.SETTLER, (IsUpgraded() ? 2f : 1.5f) },
            { UnitType.WORKER, (IsUpgraded() ? 2f : 1.5f) },
            { UnitType.REGULAR, (IsUpgraded() ? 2f : 1.5f) },
            { UnitType.RANGED, (IsUpgraded() ? 2f : 1.5f) },
            { UnitType.HEAVY, (IsUpgraded() ? 0.5f : 0.25f) },
        };
    }

    public void NetworkLevelUp()
    {
        if(IsMaxed())
            return;

        MaxHP += upgradeHP;
        hp += upgradeHP;
        defaultATK += upgradeATK;
        level++;
    }
}