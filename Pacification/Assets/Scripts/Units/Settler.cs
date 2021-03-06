using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settler : Unit
{
    public Settler(Player owner)
    {
        Owner = owner;
        type = UnitType.SETTLER;
        hasMadeAction = false;
        mvtSPD = 3;
        currMVT = 0;
        hp = 100;
        MaxHP = hp;
        // TODO : couleur du joueur
    }

    public void Settle()
    {
        if (hasMadeAction)
            return;

        anim.animator.SetTrigger("ActionTrigger");
        Owner.client.Send("CUNI|CIT|" + (int)type + "#" + HexUnit.location.coordinates.X + "#" + HexUnit.location.coordinates.Z);
        hasMadeAction = true;
        anim.animator.SetTrigger("IdleTrigger");
    }
}