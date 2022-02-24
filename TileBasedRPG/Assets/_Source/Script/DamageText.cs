using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textUI;
    private Animator anim;
    private int play = Animator.StringToHash("Play");

    private Action<DamageText> KillAction;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void ShowDamageText(float value, Unit targetUnit)
    {
        string valueInString = value.ToString();
        string textToShow = value >= 0 ? "+" + valueInString : valueInString;

        transform.position = targetUnit.transform.position;
        SetDamageText(textToShow);   
        ToggleTextVisibility(true);
        anim.SetTrigger(play);
    }
    
    void SetDamageText(string text)
    {
        textUI.text = text;
    }

    void ToggleTextVisibility(bool state)
    {
        textUI.gameObject.SetActive(state);
    }

    public void Init(Action<DamageText> killAction)
    {
        KillAction = killAction;
    }
    
    public void Deactivate()
    {
        ToggleTextVisibility(false);
        KillAction(this);
    }
}
