using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject[] menus;
    [SerializeField] Animator anim;

    public void Transition()
    {
        anim.SetBool("Round Over", true);
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("Level");
    }

    public void ToSettings()
    {
        menus[0].SetActive(false);
        menus[1].SetActive(true);
    }

    public void ToMain()
    {
        menus[1].SetActive(false);
        menus[0].SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
