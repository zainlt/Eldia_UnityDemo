using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zain.Map;

namespace Zain.CropPlant
{
    public class CropGenerator : MonoBehaviour
    {
        private Grid currentGrid;

        public int seedItemID;
        public int growthDays;  //�Ѿ�����������


        private void Awake()
        {
            currentGrid = FindObjectOfType<Grid>();
        }

        private void OnEnable()
        {
            EventHandler.GenerateCropEvent += GenerateCrop;
        }

        private void OnDisable()
        {
            EventHandler.GenerateCropEvent -= GenerateCrop;
        }

        private void GenerateCrop()
        {
            //��������ת������������
            Vector3Int cropGridPos = currentGrid.WorldToCell(transform.position);

            if(seedItemID != 0)
            {
                var tile = GridMapManager.Instance.GetTileDetailsOnMousePosition(cropGridPos);

                if(tile == null)
                {
                    tile = new TileDetails();
                    tile.gridX = cropGridPos.x;
                    tile.gridY = cropGridPos.y;

                }

                tile.daysSinceWatered = -1;
                tile.seedItemID = seedItemID;
                tile.growthDays = growthDays;
                tile.isNPCObstacle = true;

                GridMapManager.Instance.UpdateTileDetails(tile);
            }
        }

    }
}

