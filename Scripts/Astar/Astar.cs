using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zain.Map;

namespace Zain.Astar
{
    public class Astar : Singleton<Astar>
    {
        private GridNodes gridNodes;

        private Node startNode;
        private Node targetNode;
        private int gridWidth;
        private int gridHeight;
        private int originX;
        private int originY;

        private List<Node> openNodeList;        //��ǰѡ��Node�ܱߵ�8����

        private HashSet<Node> closedNodeList;      //���б�ѡ�еĵ�

        private bool pathFound;

        /// <summary>
        /// ����·������Stack��ÿһ��
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <param name="npcMovementStep"></param>
        public void BuildPath(string sceneName, Vector2Int startPos, Vector2Int endPos, Stack<MovementStep> npcMovementStep)
        {
            pathFound = false;

            //������������Ϣ�ɹ�����������в���
            if (GenerateGridNodes(sceneName, startPos, endPos))
            {
                //�������·��
                if (FindShortestPath())
                {
                    //����NPC�ƶ�·��
                    UpdatePathOnMovementStepStack(sceneName, npcMovementStep);
                }
            }
        }


        /// <summary>
        /// ������������Ϣ����ʼ�������б�
        /// </summary>
        /// <param name="sceneName">��������</param>
        /// <param name="startPos">��ʼ��</param>
        /// <param name="endPos">�յ�</param>
        /// <returns></returns>
        private bool GenerateGridNodes(string sceneName,Vector2Int startPos,Vector2Int endPos)
        {
            if (GridMapManager.Instance.GetGridDimensions(sceneName, out Vector2Int gridDimensions, out Vector2Int gridOrigin))
            {
                //������Ƭ��ͼ��Χ���������㷶Χ����
                gridNodes = new GridNodes(gridDimensions.x, gridDimensions.y);

                gridWidth = gridDimensions.x;
                gridHeight = gridDimensions.y;
                originX = gridOrigin.x;
                originY = gridOrigin.y;

                openNodeList = new List<Node>();
                closedNodeList = new HashSet<Node>();
            }
            else
                return false;

            //�������ʵ��λ�õ�����
            startNode = gridNodes.GetGridNode(startPos.x - originX, startPos.y - originY);
            targetNode = gridNodes.GetGridNode(endPos.x - originX, endPos.y - originY);

            for(int x = 0;x < gridWidth; ++x)
            {
                for(int y = 0;y < gridHeight; ++y)
                {
                    Vector3Int tilePos = new Vector3Int(x + originX, y + originY, 0);

                    var key = tilePos.x + "x" + tilePos.y + "y" + sceneName;

                    TileDetails tile = GridMapManager.Instance.GetTileDetails(key);
                    

                    if(tile != null)
                    {
                        Node node = gridNodes.GetGridNode(x, y);

                        if (tile.isNPCObstacle)
                            node.isObstacle = true;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// �ҵ����·�����н�㲢��ӵ�closeNodeList
        /// </summary>
        /// <returns></returns>
        private bool FindShortestPath()
        {
            //������
            openNodeList.Add(startNode);

            while (openNodeList.Count > 0)
            {
                //�������
                openNodeList.Sort();

                Node closeNode = openNodeList[0];

                openNodeList.RemoveAt(0);
                closedNodeList.Add(closeNode);

                if (closeNode == targetNode)
                {
                    pathFound = true;
                    break;
                }

                //������Χ8���㲹�䵽openList
                EvaluateNeighbourNodes(closeNode);

            }
            return pathFound;
        }

        /// <summary>
        /// ������Χ8���㲢���뵽openlist
        /// </summary>
        /// <param name="currentNode"></param>
        private void EvaluateNeighbourNodes(Node currentNode)
        {
            Vector2Int currentNodePos = currentNode.gridPosition;
            Node validNeighbourNode;

            for(int x = -1; x <= 1; ++x)
            {
                for(int y = -1; y <= 1; ++y)
                {
                    if (x == 0 && y == 0)
                        continue;

                    validNeighbourNode = GetValidNeighbourNode(currentNodePos.x + x, currentNodePos.y + y);

                    if(validNeighbourNode != null)
                    {
                        if (!openNodeList.Contains(validNeighbourNode))
                        {
                            validNeighbourNode.gCost = currentNode.gCost + GetDistance(currentNode, validNeighbourNode);
                            validNeighbourNode.hCost = GetDistance(validNeighbourNode, targetNode);

                            //���Ӹ��ڵ�
                            validNeighbourNode.parentNode = currentNode;
                            openNodeList.Add(validNeighbourNode);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// �õ���Ч��Node�����ϰ�����ѡ��ĵ�
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Node GetValidNeighbourNode(int x,int y)
        {
            if(x >= gridWidth || y >= gridHeight || x < 0 || y < 0)
            {
                return null;
            }

            Node neighbourNode = gridNodes.GetGridNode(x, y);

            //������ϰ������Ǹ��ڵ� �����ؿ�
            if (neighbourNode.isObstacle || closedNodeList.Contains(neighbourNode))
                return null;
            else
                return neighbourNode;
        }

        /// <summary>
        /// ��������ľ���
        /// </summary>
        /// <param name="nodeA"></param>
        /// <param name="nodeB"></param>
        /// <returns></returns>
        private int GetDistance(Node nodeA,Node nodeB)
        {
            int xDistance = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
            int yDistance = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);

            if(xDistance > yDistance)
            {
                return 10 * (xDistance - yDistance) + 14 * yDistance;
            }
            return 10 * (yDistance - xDistance) + 14 * xDistance;
        }

        /// <summary>
        /// ����ÿһ������ͳ�������
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="npcMovementStep"></param>
        private void UpdatePathOnMovementStepStack(string sceneName, Stack<MovementStep> npcMovementStep)
        {
            Node nextNode = targetNode;

            while (nextNode != null)
            {
                MovementStep newStep = new MovementStep();
                newStep.sceneName = sceneName;
                newStep.gridCoordinate = new Vector2Int(nextNode.gridPosition.x + originX, nextNode.gridPosition.y + originY);

                //ѹջ
                npcMovementStep.Push(newStep);
                nextNode = nextNode.parentNode;
            }
        }
    }
}

