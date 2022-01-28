using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnOrderIcon : MonoBehaviour
{
    public Color AllyColor;
    public Color EnemyColor;
    public Color NeutralColor;

    public Image border;

    public void ChangeBorderColor(UnitType unitType)
    {
        switch(unitType)
        {
            case UnitType.AllyUnit: 
                border.color = AllyColor;
                break;
            case UnitType.EnemyUnit:
                border.color = EnemyColor;
                break;
            case UnitType.NeutralUnit:
                border.color = NeutralColor;
                break;
        }
    }
}
