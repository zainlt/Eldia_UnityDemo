using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zain.Dialogue;
using DG.Tweening;

public class DialogUI : MonoBehaviour
{
    public GameObject dialogueBox;
    public Text dailogueText;
    public Image faceLeft, faceRight;
    public Text nameRight, nameLeft;
    public GameObject continueImage;

    private void Awake()
    {
        continueImage.SetActive(false);
    }

    private void OnEnable()
    {
        EventHandler.ShowDialogueEvent += OnShowDialogueEvent;
    }

    private void OnDisable()
    {
        EventHandler.ShowDialogueEvent -= OnShowDialogueEvent;
    }

    private void OnShowDialogueEvent(DialoguePiece dialoguePiece)
    {
        StartCoroutine(ShowDialogue(dialoguePiece));
    }
    
    private IEnumerator ShowDialogue(DialoguePiece piece)
    {
        if(piece != null)
        {
            piece.isDone = false;

            dialogueBox.SetActive(true);
            continueImage.SetActive(false);

            dailogueText.text = string.Empty;

            if(piece.name != string.Empty)
            {
                if (piece.onLeft)
                {
                    faceRight.gameObject.SetActive(false);
                    faceLeft.gameObject.SetActive(true);
                    faceLeft.sprite = piece.faceImage;
                    nameLeft.text = piece.name;
                }
                else
                {
                    faceLeft.gameObject.SetActive(false);
                    faceRight.gameObject.SetActive(true);
                    faceRight.sprite = piece.faceImage;
                    nameRight.text = piece.name;
                }
            }
            else
            {
                faceLeft.gameObject.SetActive(false);
                faceRight.gameObject.SetActive(false);
                nameLeft.gameObject.SetActive(false);
                nameRight.gameObject.SetActive(false);
            }

            //等待文字显示完之后
            yield return dailogueText.DOText(piece.dialogueText, 1f).WaitForCompletion();

            piece.isDone = true;

            if(piece.hasToPause && piece.isDone)
            {
                continueImage.SetActive(true);
            }
        }
        else
        {
            dialogueBox.SetActive(false);
            yield break;
        }
    }
}
