using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPointsProgressBar : ProgressBar
{
    private int maxActionPoints;
    private int currentActionPoints;
    
    public void Init(int inMaxActionPoints)
    {
        base.Init();
        maxActionPoints = inMaxActionPoints;
        currentActionPoints = inMaxActionPoints;
        ConfigureValues(currentActionPoints, 0);
    }

    public void SetOnlyActionPoints(int value)
    {
        value = Math.Clamp(value, 0, maxActionPoints);
        var oldLostPoints = maxActionPoints - currentActionPoints;
        var diff = value - currentActionPoints;
        var newLostPoints = Math.Max(oldLostPoints - diff,0);
        
        SetValues(value, newLostPoints);
    }
    
    public void SetOnlyMaxActionPoints(int value)
    {
        var diff = value - maxActionPoints;
        if(diff > 0)
            SetValues(currentActionPoints + diff, maxActionPoints - currentActionPoints);
        else
        {
            var oldLostActionPoints = maxActionPoints - currentActionPoints;
            var newLostActionPoints = Math.Max(oldLostActionPoints + diff, 0);
            var pointsPart = oldLostActionPoints + diff;
            var newPoints = (pointsPart < 0) ? currentActionPoints + pointsPart : currentActionPoints;
            SetValues(newPoints, newLostActionPoints);
        }
    }

    private void SetValues(int actionPoints, int lostPoints)
    {
        ChangeValues((0, actionPoints), (1, lostPoints));
        currentActionPoints = actionPoints;
        maxActionPoints = currentActionPoints + lostPoints;
    }
}
