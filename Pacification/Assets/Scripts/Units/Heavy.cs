﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heavy : Attacker
{
    public Heavy(Player owner)
    {
        this.owner = owner;
        type = UnitType.HEAVY;
        hasMadeAction = false;
        this.level = owner.UnitLevel[2];

        upgradeHP = 20;
        hp = (isUpgraded() ? 350 : 120) - upgradeHP + ((level - (isUpgraded() ? 10 : 0)) * upgradeHP);
        maxHP = hp;

        upgradeATK = 10;
        defaultATK = (isUpgraded() ? 210 : 100) - upgradeATK + ((level - (isUpgraded() ? 10 : 0)) * upgradeATK);

        range = (isUpgraded() ? 3 : 2);
        mvtSPD = 1;

        dmgMultCity = (isUpgraded() ? 2f : 1.75f);
        dmgMult = new Dictionary<UnitType, float>()
        {
            { UnitType.SETTLER, 0.5f },
            { UnitType.WORKER, 0.5f },
            { UnitType.REGULAR, 0.5f },
            { UnitType.RANGED, 0.5f },
            { UnitType.HEAVY, 1f },
        };
    }

    public void LevelUp()
    {
        if (isMaxed())
            return;
        /*else if (level == 10)
        {
            this.SetGraphics(hexUnit.Grid.unitPrefab[(int)type + 4]);
            range = 2;
            hp = 350;
            maxHP = 350;

            defaultATK = 210;

            dmgMultCity = 2f;
        }*/
        else
        {
            maxHP += upgradeHP;
            hp += upgradeHP;
            defaultATK += upgradeATK;
        }

        level++;
    }
}