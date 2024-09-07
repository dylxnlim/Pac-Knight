using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtonHandler : MonoBehaviour
{
    [SerializeField] private string gameLevel = "level";
    [SerializeField] private string titleScreen = "title";
    public CanvasGroup shadow;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void StartGameButton()
    {
        audioManager.PlaySFX(audioManager.startButtonUI);
        StartCoroutine(FadeToBlack(0.5f, gameLevel));
    }

    public void ContinueGameButton()
    {
        audioManager.PlaySFX(audioManager.startButtonUI);
        StartCoroutine(FadeToBlack(0.5f, titleScreen));
    }

    private void Update()
    {
        //Close Game
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
    IEnumerator FadeToBlack(float delay, string scene)
    {
        shadow.LeanAlpha(0, delay);
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(scene);
    }
}
