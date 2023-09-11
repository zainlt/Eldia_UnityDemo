using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Zain.Dialogue
{
    [RequireComponent(typeof(NPCMovement))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Dialoguecontroller : MonoBehaviour
    {

        public UnityEvent OnFinishEvent;
        private NPCMovement npc => GetComponent<NPCMovement>();

        public List<DialoguePiece> dialogueList = new List<DialoguePiece>();

        private Stack<DialoguePiece> dialogueStack;

        private bool canTalk;
        private bool isTalk;

        private GameObject uiSign;
        private GameObject uiSign1;


        private void Awake()
        {
            uiSign = transform.GetChild(1).gameObject;
            uiSign1 = transform.GetChild(2).gameObject;
            FillDialogueStack();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                canTalk = !npc.isMoving && npc.isInteractable;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                canTalk = false;
            }
        }

        private void Update()
        {
            uiSign.SetActive(canTalk);
            uiSign1.SetActive(canTalk);

            if(canTalk & Input.GetKeyDown(KeyCode.Space) && !isTalk)
            {
                StartCoroutine(DialogueRoutine());
            }
        }

        /// <summary>
        /// 构建对话堆栈
        /// </summary>
        private void FillDialogueStack()
        {
            dialogueStack = new Stack<DialoguePiece>();
            for(int i = dialogueList.Count - 1;i > -1; i--)
            {
                dialogueList[i].isDone = false;
                dialogueStack.Push(dialogueList[i]);
            }
        }

        private IEnumerator DialogueRoutine()
        {
            isTalk = true;
            if(dialogueStack.TryPop(out DialoguePiece result))
            {
                //传到UI显示内容
                EventHandler.CallShowDialogueEvent(result);
                EventHandler.CallUpdateGameStateEvent(GameState.Pause);
                yield return new WaitUntil(() => result.isDone == true);
                isTalk = false;
            }
            //对话完
            else
            {
                EventHandler.CallUpdateGameStateEvent(GameState.GamePlay);
                EventHandler.CallShowDialogueEvent(null);
                FillDialogueStack();
                isTalk = false;

                if (OnFinishEvent != null)
                {
                    OnFinishEvent.Invoke();
                    canTalk = false;
                }
            }
        }
    }
}
