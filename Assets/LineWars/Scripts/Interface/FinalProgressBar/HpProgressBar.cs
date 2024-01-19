using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpProgressBar: ProgressBar
{
    private int maxHp;
    private int currentHp;
    private int armor;
    
    public void Init(int inMaxHp, int inArmor)
    {
        maxHp = inMaxHp;
        currentHp = inMaxHp;
        armor = inMaxHp;
        ConfigureValues(maxHp, 0, inArmor);
    }

    public void SetOnlyHp(int value)
    {
        value = Math.Clamp(value, 0, maxHp);
        var oldLostHp = maxHp - currentHp;
        var diff = value - currentHp;
        var newLostHp = Math.Max(oldLostHp - diff,0);
        SetValues(value, newLostHp, armor);
    }
    
    public void SetOnlyMaxHp(int value)
    {
        var diff = value - maxHp;
        if(diff > 0)
            SetValues(currentHp + diff, maxHp - currentHp, armor);
        else
        {
            var oldLostHp = maxHp - currentHp;
            var newLostHp = Math.Max(oldLostHp + diff, 0);
            var hpPart = oldLostHp + diff;
            var newHp = (hpPart < 0) ?currentHp + hpPart : currentHp;
            SetValues(newHp, newLostHp, armor);
        }
    }

    public void SetOnlyArmor(int value)
    {
        ChangeValue(2, value);
        armor = value;
    }
    
    private void SetValues(int hp, int lostHp, int inArmor)
    {
        ChangeValues((0, hp), (1, lostHp), (2, inArmor));
        currentHp = hp;
        maxHp = hp + lostHp;
        armor = inArmor;
    }
}
