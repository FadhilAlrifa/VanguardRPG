using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillTreeManager : MonoBehaviour
{
    [Header("Skill Slots Setup")]
    public SkillSlot[] skillSlots; 
    
    [Header("Points Setup")]
    public TMP_Text pointsText;
    public int availablePoints;

    private bool isProcessingUpgrade = false;

    private void OnEnable()
    {
        SkillSlot.onAbilityPointSpent += handleAbilityPointSpent;
        SkillSlot.onSkillMaxed += handleSkillMaxed;
    }

    private void OnDisable()
    {
        SkillSlot.onAbilityPointSpent -= handleAbilityPointSpent;
        SkillSlot.onSkillMaxed -= handleSkillMaxed;
    }

    private void Start()
    {
        updateAbilityPoints(0);

        foreach (SkillSlot slot in skillSlots)
        {
            if (slot.skillButton != null)
            {
                slot.skillButton.onClick.RemoveAllListeners();
                slot.skillButton.onClick.AddListener(() => checkAvailablePoints(slot));
            }
        }
    }

    private void checkAvailablePoints(SkillSlot slot)
    {
        if (isProcessingUpgrade) return;

        if (availablePoints > 0 && slot.currentLevel < slot.skillSO.maxLevel)
        {
            isProcessingUpgrade = true;
            slot.tryUpgradeSkill();
            isProcessingUpgrade = false;
        }
    }

    private void handleAbilityPointSpent(SkillSlot slot)
    {
        updateAbilityPoints(-1); 
    }

    private void handleSkillMaxed(SkillSlot slot)
    {
        foreach (SkillSlot s in skillSlots)
        {
            if (!s.isUnlocked && s.canUnlockSkill())
            {
                s.unlock();
            }
        }
    }

    public void updateAbilityPoints(int amount)
    {
        availablePoints += amount;
        if (pointsText != null)
        {
            pointsText.text = "Points: " + availablePoints.ToString();
        }
    }
}