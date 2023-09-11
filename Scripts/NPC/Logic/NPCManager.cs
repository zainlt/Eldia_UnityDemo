using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : Singleton<NPCManager>
{
    public SceneRouteDataList_SO sceneRouteData;
    public List<NPCPosition> npcPositionList;

    private Dictionary<string, SceneRoute> sceneRouteDict = new Dictionary<string, SceneRoute>();

    protected override void Awake()
    {
        base.Awake();

        InitSceneRouteDict();
    }

    private void OnEnable()
    {
        EventHandler.StartNewGameEvent += OnStartNewGameEvent;
    }

    private void OnDisable()
    {
        EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
    }

    private void OnStartNewGameEvent(int obj)
    {
        foreach(var character in npcPositionList)
        {
            character.npc.position = character.position;
            character.npc.GetComponent<NPCMovement>().StartScene = character.startScene;
        }
    }

    /// <summary>
    /// ��ʼ���ֵ�
    /// </summary>
    private void InitSceneRouteDict()
    {
        if (sceneRouteData.sceneRouteList.Count > 0)
        {
            foreach(SceneRoute sceneRoute in sceneRouteData.sceneRouteList)
            {
                var key = sceneRoute.fromSceneName + sceneRoute.gotoSceneName;

                if (sceneRouteDict.ContainsKey(key))
                    continue;
                else
                    sceneRouteDict.Add(key, sceneRoute);
            }
        }
    }

    /// <summary>
    /// ����·��
    /// </summary>
    /// <param name="fromSceneName"></param>
    /// <param name="gotoSceneName"></param>
    /// <returns></returns>
    public SceneRoute GetSceneRoute(string fromSceneName,string gotoSceneName)
    {
        return sceneRouteDict[fromSceneName + gotoSceneName];
    }
}
