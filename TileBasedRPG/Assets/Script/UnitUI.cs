using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitUI : MonoBehaviour
{
    public Canvas canvas;
    public TextMeshProUGUI skillText;
    public Bar bar;

    public Dictionary<SkillType, string> skillTextIcons;

    public void ChangeSortingOrder(int newSortingOrder)
    {
        canvas.sortingOrder = newSortingOrder;
    }

    public void UpdateHealthBar()
    {

    }

    public void UpdateSkillText(string skillName, SkillType skillType)
    {
        skillText.text = "<sprite name=" + skillType.ToString() + ">" + " " + skillName;
    }
}
