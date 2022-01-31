using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitUI : MonoBehaviour
{
    public event Action PlayDeathAnim;

    [Header("References")]
    public Canvas canvas;
    public Bar barParent;
    public GameObject skillTextParent;
    public TextMeshProUGUI skillText;

    private float barPercentage;
    private Coroutine currentCoroutine = null;

    bool unitIsDead;

    void OnEnable()
    {
        ControlsManager.ShowDetailsButtonPressedState += ToggleUI;
        BattleManager.TurnOffUnitUI += ToggleUI;
    }

    void OnDisable()
    {
        ControlsManager.ShowDetailsButtonPressedState -= ToggleUI;
        BattleManager.TurnOffUnitUI -= ToggleUI;
    }

    public void ChangeSortingOrder(int newSortingOrder)
    {
        canvas.sortingOrder = newSortingOrder;
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth, float healthChange)
    {
        if (currentHealth <= 0) unitIsDead = true;

        barPercentage = currentHealth / maxHealth;
        barParent.gameObject.SetActive(true);

        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        if (healthChange < 0)  currentCoroutine = StartCoroutine(BarLoss());
        else currentCoroutine = StartCoroutine(BarGain());
    }

    IEnumerator BarLoss()
    {
        barParent.bar.fillAmount = barPercentage;
        float t = 0;
        float a = barParent.barLoss.fillAmount;

        while (barParent.barLoss.fillAmount > barPercentage)
        {
            t += Time.deltaTime;
            barParent.barLoss.fillAmount = Mathf.Lerp(a, barPercentage, t);

            yield return null;
        }
        yield return new WaitForSeconds(1);
        barParent.gameObject.SetActive(false);

        if (unitIsDead)
        {
            unitIsDead = false;
            PlayDeathAnim?.Invoke();
        }
    }

    IEnumerator BarGain()
    {
        barParent.barLoss.fillAmount = barPercentage;
        float t = 0;
        float a = barParent.bar.fillAmount;

        while (barParent.bar.fillAmount < barPercentage)
        {
            t += Time.deltaTime;
            barParent.bar.fillAmount = Mathf.Lerp(a, barPercentage, t);

            yield return null;
        }

        yield return new WaitForSeconds(1);
        barParent.gameObject.SetActive(false);
    }

    public void ToggleUI(bool newState)
    {
        barParent.gameObject.SetActive(newState);
        skillTextParent.SetActive(newState);
    }

    public void UpdateSkillText(string skillName, SkillType skillType)
    {
        skillText.text = "<sprite name=" + skillType.ToString() + ">" + " " + skillName;
    }
}
