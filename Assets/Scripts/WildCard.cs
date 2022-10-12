using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildCard : Card, IEffectCard
{
    public WildCard(int color, int value) : base(color, value)
    {
    }

    public void PerformEffect()
    {}
}
