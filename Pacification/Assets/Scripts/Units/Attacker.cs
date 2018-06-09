﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Attacker : Unit
{
    protected int upgradeHP;
    protected int defaultATK;
    protected int upgradeATK;
    protected int range;

    protected Dictionary<Unit.UnitType, float> dmgMult;
    protected float dmgMultCity;

    public int UpgradeHP
    {
        get { return upgradeHP; }
        set { upgradeHP = value; }
    }

    public int DefaultATK
    {
        get { return defaultATK; }
        set { defaultATK = value; }
    }

    public int UpgradeATK
    {
        get { return upgradeATK; }
        set { upgradeATK = value; }
    }

    public int Range
    {
        get { return range; }
        set { range = value; }
    }

    public bool IsUpgraded()
    {
        return level > 10;
    }

    public bool IsMaxed()
    {
        return level == (IsUpgraded() ? 20 : 10);
    }

    public bool IsInRangeToAttack(HexCell target)
    {
        return HexUnit.location.coordinates.DistanceTo(target.coordinates) <= range;
    }
    
    public bool Attack(Unit target)
    {
        if (hasMadeAction)
            return false;

        hexUnit.Rotation(target.HexUnit.Location);
        Thread.Sleep(250);
        anim.animator.SetTrigger("ActionTrigger");

        float multiplier = 1f;
        dmgMult.TryGetValue(target.Type, out multiplier);
        int damage = (int)((float)((defaultATK - upgradeATK) + upgradeATK * level) * multiplier);

        if(GameManager.Instance.gamemode == GameManager.Gamemode.MULTI)
            Owner.client.Send("CUNI|UTD|" + target.HexUnit.location.coordinates.X + "#" + target.HexUnit.location.coordinates.Z + "#" + damage + "|" + target.Owner.name);
        else
            target.Owner.NetworkTakeDamageUnit(target.HexUnit.location.coordinates.X + "#" + target.HexUnit.location.coordinates.Z + "#" + damage);
        hasMadeAction = true;
        anim.animator.SetTrigger("IdleTrigger");
        return true;
    }

    public bool Attack(City target)
    {
        if (hasMadeAction)
            return false;

        hexUnit.Rotation(target.Location);
        Thread.Sleep(250);
        anim.animator.SetTrigger("ActionTrigger");

        int damage = (int)((float)((defaultATK - upgradeATK) + upgradeATK * level) * dmgMultCity);

        if(GameManager.Instance.gamemode == GameManager.Gamemode.MULTI)
            Owner.client.Send("CUNI|CTD|" + target.Location.coordinates.X + "#" + target.Location.coordinates.Z + "#" + damage + "|" + target.Owner.name);
        else
            target.Owner.NetworkTakeDamageCity(target.Location.coordinates.X + "#" + target.Location.coordinates.Z + "#" + damage);
        hasMadeAction = true;
        anim.animator.SetTrigger("IdleTrigger");
        return true;
    }

    public bool Attack(Resource target)
    {
        if(hasMadeAction)
            return false;

        hexUnit.Rotation(target.Location);
        Thread.Sleep(250);
        anim.animator.SetTrigger("ActionTrigger");

        int damage = (int)((float)((defaultATK - upgradeATK) + upgradeATK * level) * dmgMultCity);

        Owner.client.Send("CUNI|RTD|" + target.Location.coordinates.X + "#" + target.Location.coordinates.Z + "#" + damage + "|" + target.Owner.name);
        hasMadeAction = true;
        anim.animator.SetTrigger("IdleTrigger");
        return true;
    }
}