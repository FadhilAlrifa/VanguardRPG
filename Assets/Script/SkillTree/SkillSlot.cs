using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSlot : MonoBehaviour
{
    public SkillSO skillSO; 
    public int currentLevel;
    public bool isUnlocked;
    
    [Header("UI References")]
    public Image skillIcon;
    public TMP_Text skillLevelText;
    public Button skillButton;

    [Header("Prerequisites")]
    public List<SkillSlot> prerequisiteSkills; 

    public static event Action<SkillSlot> onAbilityPointSpent;
    public static event Action<SkillSlot> onSkillMaxed;

    private void OnValidate()
    {
        if (skillSO != null && skillLevelText != null && skillIcon != null && skillButton != null)
        {
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        if (skillSO != null)
        {
            skillIcon.sprite = skillSO.skillIcon;
        }
        
        if (isUnlocked)
        {
            skillLevelText.text = currentLevel.ToString() + "/" + skillSO.maxLevel.ToString();
            skillIcon.color = Color.white;
            if (skillButton != null) skillButton.interactable = true;
        }
        else
        {
            skillLevelText.text = "Locked"; 
            skillIcon.color = Color.grey;
            if (skillButton != null) skillButton.interactable = false;
        }
    }

    public void tryUpgradeSkill()
    {
        if (isUnlocked && currentLevel < skillSO.maxLevel)
        {
            currentLevel++;
            UpdateUI();

            onAbilityPointSpent?.Invoke(this);

            if (currentLevel >= skillSO.maxLevel)
            {
                onSkillMaxed?.Invoke(this);
            }
        }
    }

    public void unlock()
    {
        isUnlocked = true;
        UpdateUI();
    }

    public bool canUnlockSkill()
    {
        foreach (SkillSlot slot in prerequisiteSkills)
        {
            if (!slot.isUnlocked || slot.currentLevel < slot.skillSO.maxLevel)
            {
                return false; 
            }
        }
        return true;
    }
}