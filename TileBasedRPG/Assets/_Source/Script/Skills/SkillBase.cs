using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillBase : MonoBehaviour
{
    [HideInInspector] public Unit unit;
    [HideInInspector] public SkillStats skillStats;
    
    [Header("Skill Visual Effects")]
    public List<AnimationEvents> skillVFX;

    [SerializeField]
    protected int rowsToAffect;

    protected List<AnimationEvents> vfxsToPlay;
    protected List<BattleTile> animatedHexes;

    void Awake()
    {
        animatedHexes = new List<BattleTile>();
    }

    public virtual void StartSkill()
    {
        Debug.Log(unit.unitStats.unitName + " Starting Skill: " + skillStats.skillName);
        InterfaceManager.Instance.PlaySkillNameAnim(skillStats);

        StartCoroutine(PauseUntilExecution());
    }

    public virtual void SkillInitialSetup(Unit myUnit)
    {
        unit = myUnit;
        skillStats = GetComponent<SkillStats>();
        skillStats.InitialSetup();
        
        SetSkillValues();
    }

    public virtual void SetSkillValues()
    {
        
    }

    public virtual void SetSkillDescription(out string skillDescription)
    {
        skillDescription = "Base Skill";
    }

    public virtual IEnumerator PauseUntilExecution()
    {
        yield return new WaitForSeconds(0.5f);
        ExecuteSkill();
    }

    protected virtual void ExecuteSkill()
    {
        Debug.Log(unit.unitStats.unitName + " Executed Skill: " + skillStats.skillName);
        unit.SkillExecutionEndedEvent();
    }

    protected virtual bool CheckIfBackStab(Unit playerUnit, Unit enemyUnit)
    {
        if (enemyUnit == null) return false;

        if (playerUnit.CurrentTile.hexPos.column == enemyUnit.CurrentTile.hexPos.column &&
            playerUnit.facingSide == enemyUnit.facingSide) return true;
        
        if (enemyUnit.CurrentTile.hexPos.column > playerUnit.CurrentTile.hexPos.column &&
            enemyUnit.facingSide == FacingSide.FacingRight) return true;

        if (enemyUnit.CurrentTile.hexPos.column < playerUnit.CurrentTile.hexPos.column &&
            enemyUnit.facingSide == FacingSide.FacingLeft) return true;

        return false;
    }

    //when a unit's skill changes, check if skill modifies 
    //something during the planning phase
    public virtual void PlanningPhaseMethod()
    {

    }

    //Start visual aids during planning phase
    public virtual void VisualAidBegin()
    {

    }

    //End all visual aids
    public virtual void VisualAidEnd()
    {

    }
}
