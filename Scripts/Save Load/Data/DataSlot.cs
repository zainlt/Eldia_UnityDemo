using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zain.Save
{
    public class DataSlot
    {
        /// <summary>
        /// ��������string��GUID
        /// </summary>
        public Dictionary<string, GameSaveData> dataDict = new Dictionary<string, GameSaveData>();

        #region UI��ʾ���
        public string DataTime
        {
            get
            {
                var key = TimeManager.Instance.GUID;

                if (dataDict.ContainsKey(key))
                {
                    var timeData = dataDict[key];

                    return "��" + timeData.timeDict["gameYear"] + "��/" + (Season)timeData.timeDict["gameSeason"] + "/" + timeData.timeDict["gameMonth"] + "��/" + timeData.timeDict["gameDay"] + "��/";
                }
                else return string.Empty;

            }
        }

        public string DataScene
        {
            get
            {
                var key = Transition.Transition.Instance.GUID;
                if (dataDict.ContainsKey(key))
                {
                    var transitionData = dataDict[key];
                    return transitionData.dataSceneName switch
                    {
                        "01.Field" => "��ׯ",
                        "02.Home" => "��",
                        "03.Start" => "����",
                        _ => string.Empty
                    };
                }
                else return string.Empty;
            }
        }



        #endregion



    }
}