using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject[] menus;
    [SerializeField] Animator anim;
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] bool menu;

    void Awake()
    {
        if (menu)
        {
            scoreManager.p1Score = 0;
            scoreManager.p2Score = 0;
        }
    }

    public void Transition()
    {
        anim.SetBool("Round Over", true);
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("Level");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Unpause()
    {
        Time.timeScale = 1;
    }

    public void ToMain()
    {
        menus[0].SetActive(true);
        menus[1].SetActive(false);
        menus[2].SetActive(false);
    }

    public void ToKeybinds()
    {
        menus[0].SetActive(false);
        menus[1].SetActive(true);
        menus[2].SetActive(false);
    }

    public void ToCustomization()
    {
        menus[0].SetActive(false);
        menus[1].SetActive(false);
        menus[2].SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
