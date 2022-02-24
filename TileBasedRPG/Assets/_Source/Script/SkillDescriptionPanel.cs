using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillDescriptionPanel : MonoBehaviour
{
    [SerializeField] private GameObject panelObj;
    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private TextMeshProUGUI skillDescriptionText;
    [SerializeField] private TextMeshProUGUI skillExtraInfoText;
    [SerializeField] private TextMeshProUGUI skillCostText;

    public void TogglePanelVisibility(bool showPanel)
    {
        panelObj.SetActive(showPanel);
    }

    public void SetPanelText(string skillName, string skillDescription)
    {
        skillNameText.text = skillName;
        skillDescriptionText.text = skillDescription;
        //skillExtraInfoText.text = skillExtraInfo;
    }
}
