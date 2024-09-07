using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Level level;
    public ProgressBar bar;
    public Ghost[] ghosts;
    public Player player;
    public Transform collectibles;
    public int score { get; private set; }
    public int lives { get; private set; }
    public int ghostMult { get; private set; } = 1;
    public Transform itemPanel;
    private int rounds;
    public TMP_Text itemName;
    public TMP_Text itemDesc;
    public TMP_Text roundsUI;
    public TMP_Text livesUI;
    public TMP_Text scoreUI;
    public TMP_Text gameStatus;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        HideItemPanel();
        GameOver();
        NewGame();
    }

    private void Update()
    {
        //if Game Over + UI
        if(this.lives <= 0)
        {
            if(rounds > 0){ gameStatus.SetText("Game Over!"); }
            else{ SceneManager.LoadScene("win"); }
            
            roundsUI.SetText("press any key to retry");
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene("title");
            }
        }
        //if Game Running + UI
        else
        {
            roundsUI.SetText("Round " + Mathf.Abs(rounds - 4).ToString());
        }
        livesUI.SetText("x " + this.lives.ToString());
        scoreUI.SetText(": " + this.score.ToString());

        //if ESC to Main Menu
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene("title");
        }
    }

    private void NewGame()
    {
        gameStatus.SetText("Game Stats");
        SetScore(0);
        SetLives(2);
        rounds = 3;
        NewRound();
    }

    private void NewRound()
    {
        this.level.Generate(Mathf.Abs(rounds - 4));

        foreach (Transform collect in collectibles)
        {
            collect.gameObject.SetActive(true);
        }
        ResetState();
    }

    private void ResetState()
    {
        ResetGhostMult();
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].ResetState();
        } 

        this.player.ResetState();
    }

    private void GameOver()
    {
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(false);
        } 

        this.player.gameObject.SetActive(false);
    }

    private void SetScore(int score)
    {
        this.score = score;
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
    }

    public void GhostEaten(Ghost ghost)
    {
        audioManager.PlaySFX(audioManager.sword);
        SetScore(this.score + ghost.points * this.ghostMult);
        this.ghostMult++;
    }

    public void PlayerEaten()
    {
        audioManager.PlaySFX(audioManager.death);
        this.player.gameObject.SetActive(false);
        SetLives(this.lives - 1);

        if(this.lives > 0)
        {
            Invoke(nameof(ResetState), 3.0f);
        }
        else
        {
            GameOver();
        }
    }

    public void CollectibleEaten(Collectible collect)
    {
        audioManager.PlaySFX(audioManager.coin);
        collect.gameObject.SetActive(false);
        SetScore(this.score + collect.points);

        if(!HasRemainingCollectibles())
        {
            this.player.gameObject.SetActive(false);
            rounds = rounds - 1;
            if(rounds > 0)
            {
                Invoke(nameof(NewRound), 3.0f);
            }
            else
            {
                audioManager.PlaySFX(audioManager.win);
                SetLives(0);
                GameOver();
            }
        }
    }

    public void ItemEaten(PowerPellet item, string name)
    {
        audioManager.PlaySFX(audioManager.item);

        if (name == "sword")
        {
            StartCoroutine(ShowItemPanel(2.0f, name));

            for (int i = 0; i < this.ghosts.Length; i++)
            {
                this.ghosts[i].frightened.Enable(item.duration);
            }

            CancelInvoke();
            Invoke(nameof(ResetGhostMult), item.duration);
            SetProgressBar(item.duration);
        }
        else if(name == "boot")
        {
            StartCoroutine(ShowItemPanel(2.0f, name));

            this.player.movement.speed = this.player.movement.speed * 1.20f;
        }
        else if (name == "clock")
        {
            StartCoroutine(ShowItemPanel(2.0f, name));

            for (int i = 0; i < this.ghosts.Length; i++)
            {
                this.ghosts[i].movement.speed -= this.ghosts[i].movement.initialSpeed / 2;
            }
        }
        else if (name == "heart")
        {
            StartCoroutine(ShowItemPanel(2.0f, name));
            this.lives += 1;

        }
        CollectibleEaten(item);
    }

    private bool HasRemainingCollectibles()
    {
        foreach (Transform collect in collectibles)
        {
            if(collect.gameObject.activeSelf)
            {
                return true;
            }
        }
        
        return false;
    }

    private void ResetGhostMult()
    {
        this.ghostMult = 1;
    }

    private void SetProgressBar(float duration)
    {
        bar.current = 0.0f;
        bar.maximum = duration;
        
        StartCoroutine(FillProgressBar(bar.current, bar.maximum));
    }

    private void HideItemPanel()
    {
        this.itemPanel.transform.localScale = Vector3.zero;
    }
    IEnumerator ShowItemPanel(float duration, string name)
    {
        //Change Text
        itemName.SetText(name);
        if(name == "sword"){ itemDesc.SetText("Its your turn to attack!"); }
        else if (name == "boot"){ itemDesc.SetText("Run faster!"); }
        else if (name == "clock"){ itemDesc.SetText("Enemies are slower."); }
        else if (name == "heart") { itemDesc.SetText("An Extra Life!"); }


        //Show Item Panel
        this.itemPanel.transform.LeanScale(Vector3.one, 0.5f).setEaseOutQuart();
        yield return new WaitForSeconds(duration);
        this.itemPanel.transform.LeanScale(Vector3.zero, 0.5f).setEaseOutQuart();
    }

    IEnumerator FillProgressBar(float current, float maximum)
    {
        
        while(current < maximum)
        {
            current += Time.deltaTime;
            
            bar.current = current;
            yield return null;
        }
    }
}
