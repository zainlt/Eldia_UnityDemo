using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zain.Save
{
    public class DataSlot
    {
        /// <summary>
        /// 进度条，string是GUID
        /// </summary>
        public Dictionary<string, GameSaveData> dataDict = new Dictionary<string, GameSaveData>();

        #region UI显示情况
        public string DataTime
        {
            get
            {
                var key = TimeManager.Instance.GUID;

                if (dataDict.ContainsKey(key))
                {
                    var timeData = dataDict[key];

                    return "第" + timeData.timeDict["gameYear"] + "年/" + (Season)timeData.timeDict["gameSeason"] + "/" + timeData.timeDict["gameMonth"] + "月/" + timeData.timeDict["gameDay"] + "日/";
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
                        "01.Field" => "村庄",
                        "02.Home" => "家",
                        "03.Start" => "海边",
                        _ => string.Empty
                    };
                }
                else return string.Empty;
            }
        }



        #endregion



    }
}