using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.PlayerLoop;
using UnityEngine.Pool;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager Instance;
    public static event Action BattleStarted;
    public static event Action PlayerTurnEnded;
    public static event Action PreparationPhaseStarted;
    public static event Action ExecutionPhaseStarted;
    public static event Action BattleVictory;
    public static event Action BattleDefeat;

    Animator anim;

    public ScriptableGameDefinitions gameDefinitions;
    public SkillDescriptionPanel skillDescPanel;
    public TextMeshProUGUI skillNameText;
    public GameObject turnOrderHUD;
    public Transform turnOrderIconsParent;
    public GameObject endTurnObj;
    public Image endTurnCircle;
    public GameObject helpTextParent;
    public GameObject helpTextGamepad;
    public GameObject helpTextKeyboard;
    public Transform damageTextParent;
    public DamageText damageTextPrefab;
    
    private Coroutine endTurnCoroutine;
    private ObjectPool<DamageText> pool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        SetPool();

        ControlsManager.EndTurnButtonPressed += EndTurnObj;
        ControlsManager.ControlTypeChanged += ChangeHelpTextType;
        BattleManager.TurnOffManagerUI += HideSkillDescriptionPanel;
        UnitStats.ShowDamageText += SpawnDamageText;
    }

    private void OnDestroy()
    {
        ControlsManager.EndTurnButtonPressed -= EndTurnObj;
        ControlsManager.ControlTypeChanged -= ChangeHelpTextType;
        BattleManager.TurnOffManagerUI -= HideSkillDescriptionPanel;
        UnitStats.ShowDamageText -= SpawnDamageText;
    }

    void SetPool()
    {
        pool = new ObjectPool<DamageText>(() =>
        {
            var text = Instantiate(damageTextPrefab, damageTextParent, true);
            text.gameObject.SetActive(false);
            return text;
        }, text =>
        {
            if (text == null  || text.gameObject == null) 
                Debug.Log("Text is NULL");
            text.gameObject.SetActive(true);
        }, text => {
            text.gameObject.SetActive(false);
        }, text => {
            
        }, false, 20,20);
        var elements = new DamageText[20];
        for (int i = 0; i < 20; i++)
        {
            elements[i] = pool.Get();
        }
        for (int i = 0; i < 20; i++)
        {
            pool.Release(elements[i]);
        }
    }

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ArrangeTurnOrderIcons(List<Unit> unitList)
    {
        turnOrderIconsParent.DetachChildren();
        List<Unit> unitListTemp = new List<Unit>();
        foreach (Unit unit in unitList) unitListTemp.Add(unit);
        unitListTemp.Reverse();

        foreach (Unit unit in unitListTemp)
        {
            unit.turnOrderIcon.transform.SetParent(turnOrderIconsParent);
        }
    }
    
    void SpawnDamageText(float value, Unit targetUnit)
    {
        var text = pool.Get();
        text.Init(KillDamageText);
        text.ShowDamageText(value, targetUnit);
    }

    void KillDamageText(DamageText damageText)
    {
        pool.Release(damageText);
    }

    void ChangeHelpTextType(ControlType newControlType)
    {
        helpTextGamepad.SetActive(false);
        helpTextKeyboard.SetActive(false);
        
        if (newControlType == ControlType.KeyboardMouse)
            helpTextKeyboard.SetActive(true);
        if (newControlType == ControlType.Gamepad)
            helpTextGamepad.SetActive(true);
    }
    
    public void ToggleTurnOrderHUD(bool active)
    {
        turnOrderHUD.SetActive(active);
    }

    public void ShowSkillDescriptionPanel(Unit unitToGetSkillInfo)
    {
        SkillStats skillStats = unitToGetSkillInfo.unitSkills.currentSelectedSkill.skillStats;
        string skillNameText = "<sprite name=" + skillStats.skillType.ToString() + ">" + " " + skillStats.skillName;
        string extraDescription = skillStats.SetSkillExtraInfo();
        
        skillDescPanel.SetPanelText(skillNameText, extraDescription);
        skillDescPanel.TogglePanelVisibility(true);
    }

    public void HideSkillDescriptionPanel()
    {
        skillDescPanel.TogglePanelVisibility(false);
    }

    public void PlayNewBattlePhaseAnim(BattlePhase phase)
    {
        if (phase == BattlePhase.ExecutionPhase) ExecutionPhaseAnim();
        else PreparationPhaseAnim();
    }

    void PreparationPhaseAnim()
    {
        anim.SetTrigger("PreparationPhase");
    }

    void ExecutionPhaseAnim()
    {
        anim.SetTrigger("ExecutionPhase");
    }

    public void PlayBattleVictoryAnim()
    {
        anim.SetTrigger("Victory");
    }

    public void PlayBattleDefeatAnim()
    {
        anim.SetTrigger("Defeat");
    }

    public void BattleStartedAnimEnd()
    {
        BattleStarted?.Invoke();
    }

    public void PreparationPhaseAnimEnd()
    {
        PreparationPhaseStarted?.Invoke();
    }

    public void ExecutionPhaseAnimEnd()
    {
        ExecutionPhaseStarted?.Invoke();
    }

    public void PlaySkillNameAnim(SkillStats skillStats)
    {
        skillNameText.text = "<sprite name=" + skillStats.skillType.ToString() + ">" + " " + skillStats.skillName;
        anim.SetTrigger("SkillNameStart");
    }

    public void PlaySkillNameEndAnim()
    {
        anim.SetTrigger("SkillNameEnd");
    }

    public void BattleVictoryAnimEnd()
    {
        BattleVictory?.Invoke();
    }

    public void BattleDefeatAnimEnd()
    {
        BattleDefeat?.Invoke();
    }

    public void EndTurnObj(bool showObj)
    {
        if (showObj)
        {
            endTurnObj.SetActive(true);
            endTurnCoroutine = StartCoroutine(EndTurnCircleFill());
        }
        else
        {
            StopCoroutine(endTurnCoroutine);
            endTurnObj.SetActive(false);
        }
    }

    IEnumerator EndTurnCircleFill()
    {
        float a = 0;
        float b = 1;
        float d = 0;
        float timeToComplete = gameDefinitions.endTurnButtonHoldTime;
        
        while (d < timeToComplete)
        {
            d += Time.deltaTime/timeToComplete;
            endTurnCircle.fillAmount = Mathf.Lerp(a, b, d);

            yield return null;
        }

        PlayerTurnEnded?.Invoke();
        BattleManager.Instance.ChangeBattlePhase(BattlePhase.ExecutionPhase);
        EndTurnObj(false);
    }
}
