using System.Collections;
using System.Collections.Generic;
using UnityEngine.Sprites;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    private GameObject menuCanvas;
    public GameObject menuPrefab;

    public Button showPauseButton;
    //private Image image;
    private Sprite tempImage;
    public GameObject pausePanel;
    public Slider volumeSlider;
    public Slider ambientSlider;
    public Button cancelBut;
    public Button exitBut;

    private void Awake()
    {
        showPauseButton.onClick.AddListener(TogglePausePanel);
        cancelBut.onClick.AddListener(TogglePausePanel);

        volumeSlider.value = AudioManager.Instance.GetMusicVolume();
        ambientSlider.value = AudioManager.Instance.GetAmbientVolume();

        volumeSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicMaterVolume);
        ambientSlider.onValueChanged.AddListener(AudioManager.Instance.SetAmbientVolume);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePausePanel();
        }
    }

    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadedEvent;
    }

    private void OnAfterSceneLoadedEvent()
    {
        if (menuCanvas.transform.childCount > 0)
            Destroy(menuCanvas.transform.GetChild(0).gameObject);
    }

    private void Start()
    {
        menuCanvas = GameObject.FindWithTag("MenuCanvas");
        Instantiate(menuPrefab, menuCanvas.transform);
        //image = showPauseButton.gameObject.GetComponent<Image>();
        


    }

    private void TogglePausePanel()
    {
        bool isOpen = pausePanel.activeInHierarchy;

        if (isOpen)
        {
            pausePanel.SetActive(false);
            //showPauseButton.enabled = false;
            tempImage = Resources.Load("stop", typeof(Sprite)) as Sprite;
            showPauseButton.image.sprite = tempImage;
            //image.sprite = tempImage;
            Time.timeScale = 1;
        }
        else
        {
            System.GC.Collect();
            //showPauseButton.enabled = true;
            tempImage = Resources.Load("continue", typeof(Sprite)) as Sprite;
            showPauseButton.image.sprite = tempImage;
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void ReturnMenuCanvas()
    {
        Time.timeScale = 1;
        StartCoroutine(BackToMenu());
    }

    private IEnumerator BackToMenu()
    {
        pausePanel.SetActive(false);
        EventHandler.CallEndGameEvent();
        yield return new WaitForSeconds(1f);
        Instantiate(menuPrefab, menuCanvas.transform);
    }
}
