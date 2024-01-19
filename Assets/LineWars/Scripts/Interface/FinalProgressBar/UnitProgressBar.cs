using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitProgressBar : MonoBehaviour
{
    [SerializeField] private HpProgressBar hpProgressBar;
    [SerializeField] private ActionPointsProgressBar actionPointsProgressBar;

    public void Init(int maxHp, int armor, int maxActionPoints)
    {
        hpProgressBar.Init(maxHp, armor);
        actionPointsProgressBar.Init(maxActionPoints);
    }

    public void SetHp(int value)
    {
        hpProgressBar.SetOnlyHp(value);
    }

    public void SetMaxHp(int value)
    {
        hpProgressBar.SetOnlyMaxHp(value);
    }

    public void SetArmor(int value)
    {
        hpProgressBar.SetOnlyArmor(value);
    }

    public void SetActionPoints(int value)
    {
        actionPointsProgressBar.SetOnlyActionPoints(value);
    }
    
    public void SetMaxActionPoints(int value)
    {
        actionPointsProgressBar.SetOnlyMaxActionPoints(value);
    }
}
