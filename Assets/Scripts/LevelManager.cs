using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    private int currentLevel;

    public static LevelManager _Instance;

    private bool canChangeLevel = false;
    private bool fadeIn = false;
    private bool fadeOut = false;
    private int fadeComplete = -1; // 0 = fadein, 1 = fadeout, -1 = N/A
    public float lerpSpeed = 0.1f;
    private Image fadeToBlackImg;

    private void Awake()
    {
        if (_Instance == null)
            _Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        fadeToBlackImg = GameObject.FindGameObjectWithTag("FadeToBlack").GetComponentInChildren<Image>();
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        FadeIn();
    }

    private void Update()
    {
        if (fadeIn && fadeToBlackImg.color.a < 0.96f)
        {
            fadeToBlackImg.color = Color.Lerp(fadeToBlackImg.color, new Color(0, 0, 0, 1), lerpSpeed);
        }
        else if (fadeIn)
        {
            fadeToBlackImg.color = new Color(0, 0, 0, 1);
            fadeIn = false;
            fadeComplete = 0;
        }

        if (fadeOut && fadeToBlackImg.color.a > 0.025f)
        {
            fadeToBlackImg.color = Color.Lerp(fadeToBlackImg.color, new Color(0, 0, 0, 0), lerpSpeed);
        }
        else if (fadeOut)
        {
            fadeToBlackImg.color = new Color(0, 0, 0, 0);
            fadeOut = false;
            fadeComplete = 1;
        }
    }

    public void NextLevel()
    {
        StartCoroutine(NewSceneRoutine());
        //SceneManager.LoadScene(currentLevel + 1);
    }

    public void Restart()
    {
        StartCoroutine(RestartRoutine());
    }

    public void FadeIn()
    {
        fadeToBlackImg.color = new Color(0, 0, 0, 1);
        fadeOut = true;
    }

    private IEnumerator RestartRoutine()
    {
        fadeIn = true;
        fadeOut = false;
        while (fadeComplete != 0)
        {
            yield return 0;
        }

        if (fadeComplete == 0)
        {
            SceneManager.LoadScene(currentLevel);
        }
    }

    private IEnumerator NewSceneRoutine()
    {
        fadeIn = true;
        fadeOut = false;
        while (fadeComplete != 0)
        {
            yield return 0;
        }

        if (fadeComplete == 0)
        {
            if (currentLevel + 1 < SceneManager.sceneCountInBuildSettings)
                SceneManager.LoadScene(currentLevel + 1);
            else
                SceneManager.LoadScene(0);
        }
    }
}