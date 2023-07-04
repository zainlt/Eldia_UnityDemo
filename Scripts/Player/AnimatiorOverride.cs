using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatiorOverride : MonoBehaviour
{
    private Animator[] animators;
    public SpriteRenderer holdItem;

    [Header("各部分动画列表")]
    public List<AnimatorType> animatorTypes;

    private Dictionary<string, Animator> animatorNameDict = new Dictionary<string, Animator>();

    private void Awake()
    {
        animators = GetComponentsInChildren<Animator>();
        foreach (var anim in animators)
        {
            animatorNameDict.Add(anim.name, anim);
        }
    }

    private void OnEnable()
    {
        EventHandler.ItemSelectedEvent += OnItemSeletedEvent;
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
    }

    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSeletedEvent;
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
    }

    private void OnBeforeSceneUnloadEvent()
    {
        holdItem.enabled = false;
        SwitchAnimator(PartType.None);
    }

    private void OnItemSeletedEvent(ItemDetails itemDetails, bool isSeleted)
    {
        PartType currentType = itemDetails.itemType switch
        {
            //WORKFLOW:不同的工具返回不同的动画在此补全
            ItemType.Seed => PartType.Carry,
            ItemType.Commodity => PartType.Carry,
            ItemType.HoeTool => PartType.Hoe,
            _ => PartType.None
        };

        if(isSeleted == false)
        {
            currentType = PartType.None;
            holdItem.enabled = false;
        }
        else
        {
            if(currentType == PartType.Carry)
            {
                holdItem.sprite = itemDetails.itemOnWorldSprite;
                holdItem.enabled = true;
            }
            else
            {
                holdItem.enabled = false;
            }
        }

        SwitchAnimator(currentType);
    }

    private void SwitchAnimator(PartType partType)
    {
        foreach(var item in animatorTypes)
        {
            if(item.partType == partType)
            {
                animatorNameDict[item.partName.ToString()].runtimeAnimatorController = item.overrideController;
            }
            else if (item.partType == PartType.None)
            {
                animatorNameDict[item.partName.ToString()].runtimeAnimatorController = item.overrideController;
            }
        }
    }
}
