using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public CropDetails cropDetails;
    public TileDetails tileDetails;
    private int harvestActionCount;

    public bool CanHarvest => tileDetails.growthDays >= cropDetails.TotalGrowthDays;

    private Animator anim;

    private Transform PlayerTransform => FindObjectOfType<Player>().transform;


    public void ProcessToolAction(ItemDetails tool, TileDetails Tile)
    {
        tileDetails = Tile;

        //����ʹ�ô���
        int requireActionCount = cropDetails.GetTotalRequireCount(tool.itemID);
        if (requireActionCount == -1) return;

        anim = GetComponentInChildren<Animator>();

        //���������
        if (harvestActionCount < requireActionCount)
        {
            harvestActionCount++;

            //�ж��Ƿ�����Ч����
            if(anim != null && cropDetails.hasAnimation)
            {
                Debug.Log("111");
                //���󵹻�������
                if (PlayerTransform.position.x < transform.position.x)
                    anim.SetTrigger("RotateRight");
                else
                    anim.SetTrigger("RotateLeft");
            }

            //��������
            //��������
            if (cropDetails.soundEffect != SoundName.none)
            {
                EventHandler.CallPlaySoundEvent(cropDetails.soundEffect);
            }



        }

        if (harvestActionCount >= requireActionCount)
        {
            if (cropDetails.generateAtPlayerPosition || !cropDetails.hasAnimation)
            {
                //����ũ����
                SpawnHarvestItems();
            }
            else if(cropDetails.hasAnimation)
            {
                if (PlayerTransform.position.x < transform.position.x)
                    anim.SetTrigger("FallingRight");
                else
                    anim.SetTrigger("FallingLeft");

                EventHandler.CallPlaySoundEvent(SoundName.TreeFalling);

                StartCoroutine(HarvestAfterAnimation());
            }
        }
    }

    //Э�� ��ľ����֮��ִ��
    private IEnumerator HarvestAfterAnimation()
    {
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("END"))
        {
            //����ѭ��
            yield return null;
        }

        SpawnHarvestItems();

        //ת��������
        if(cropDetails.transferItemID > 0)
        {
            CreateTransferCrop();
        }
    }

    /// <summary>
    /// ����ת��������
    /// </summary>
    private void CreateTransferCrop()
    {
        tileDetails.seedItemID = cropDetails.transferItemID;
        tileDetails.daysSinceLastHarvest = -1;
        tileDetails.growthDays = 0;

        EventHandler.CallRefreshCurrentMap();
    }


    /// <summary>
    /// ���ɹ�ʵ
    /// </summary>
    public void SpawnHarvestItems()
    {
        for (int i = 0; i < cropDetails.producedItemID.Length; i++)
        {
            int amountToProduce;

            if (cropDetails.producedMinAmount[i] == cropDetails.producedMaxAmount[i])
            {
                //����ֻ����ָ��������
                amountToProduce = cropDetails.producedMinAmount[i];
            }
            else    //��Ʒ�������
            {
                amountToProduce = Random.Range(cropDetails.producedMinAmount[i], cropDetails.producedMaxAmount[i] + 1);
            }

            //ִ������ָ����������Ʒ
            for (int j = 0; j < amountToProduce; j++)
            {
                if (cropDetails.generateAtPlayerPosition)
                {
                    EventHandler.CallHarvestAtPlayerPosition(cropDetails.producedItemID[i]);

                }
                else    //�����ͼ��������Ʒ
                {
                    //�ж�Ӧ�����ɵ���Ʒ����
                    var dirX = transform.position.x > PlayerTransform.position.x ? 1 : -1;
                    //һ����Χ�ڵ����
                    var spawnPos = new Vector3(transform.position.x + Random.Range(dirX, cropDetails.spawnRadius.x * dirX),
                    transform.position.y + Random.Range(-cropDetails.spawnRadius.y, cropDetails.spawnRadius.y), 0);

                    EventHandler.CallInstantiateItemInSecene(cropDetails.producedItemID[i], spawnPos);
                }
            }
        }

        if (tileDetails != null)
        {
            tileDetails.daysSinceLastHarvest++;

            //�Ƿ�����ظ�����
            if (cropDetails.daysToRegrow > 0 && tileDetails.daysSinceLastHarvest < cropDetails.regrowTimes - 1)
            {
                tileDetails.growthDays = cropDetails.TotalGrowthDays - cropDetails.daysToRegrow;
                //ˢ������
                EventHandler.CallRefreshCurrentMap();
            }
            else    //�����ظ�����
            {
                tileDetails.daysSinceLastHarvest = -1;
                tileDetails.seedItemID = -1;
                //FIXME:�Լ����
                // tileDetails.daysSinceDug = -1;
            }

            Destroy(gameObject);
        }
    }
}
