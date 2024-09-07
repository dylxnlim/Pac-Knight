using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    public Transform box;
    public CanvasGroup shadow;
    public GameObject group;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnEnable()
    {
        audioManager.PlaySFX(audioManager.buttonUI);

        shadow.alpha = 0;
        shadow.LeanAlpha(1, 0.5f);

        box.localPosition = new Vector2(0, -Screen.height);
        box.LeanMoveLocalY(0, 0.5f).setEaseOutExpo().delay = 0.1f;
    }

    public void ClosePopup()
    {
        audioManager.PlaySFX(audioManager.buttonUI);

        shadow.LeanAlpha(0, 0.5f);
        box.LeanMoveLocalY(-Screen.height, 0.5f).setEaseInExpo().setOnComplete(OnComplete);
    }

    void OnComplete()
    {
        group.SetActive(false);
    }
}
