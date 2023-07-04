using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Zain.Transition
{
    public class Transition : MonoBehaviour
    {
        [SceneName]
        public string startSceneName = string.Empty;
        private CanvasGroup fadeCanvasGroup;
        public bool isFade = false;

        private IEnumerator Start()
        {
            fadeCanvasGroup = FindObjectOfType<CanvasGroup>();

            //为了让 CursorManager 在初始的时候就获得鼠标的具体位置
            yield return LoadSceneSetActive(startSceneName);
            EventHandler.CallAfterSceneLoadEvent();
        }

        private void OnEnable()
        {
            EventHandler.TransitionEvent += OnTransitionEvent;
        }

        private void OnDisable()
        {
            EventHandler.TransitionEvent -= OnTransitionEvent;
        }

        private void OnTransitionEvent(string sceneToGo, Vector3 positionToGo)
        {
            if (!isFade)
                StartCoroutine(Transitions(sceneToGo, positionToGo));
        }

        private IEnumerator Transitions(string sceneName, Vector3 targetPosition)
        {
            EventHandler.CallBeforeSceneUnloadEvent();

            yield return Fade(1f);

            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            yield return LoadSceneSetActive(sceneName);

            //
            EventHandler.CallMoveToPoistion(targetPosition);

            EventHandler.CallAfterSceneLoadEvent();

            yield return Fade(0);
        }

        /// <summary>
        /// 加载场景并设置为激活
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        private IEnumerator LoadSceneSetActive(string sceneName)
        {
            //异步加载场景,叠加模式
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

            SceneManager.SetActiveScene(newScene);
        }

        private IEnumerator Fade(float targetAlpha)
        {
            isFade = true;

            fadeCanvasGroup.blocksRaycasts = true;

            float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / Settings.fadeDuration;

            while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
            {
                fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
                yield return null;
            }

            fadeCanvasGroup.blocksRaycasts = false;

            isFade = false;
        }
    }

}
