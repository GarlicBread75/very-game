using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] float sceneLoadDelay;
    bool canChangeScene;

    void FixedUpdate()
    {
        if (canChangeScene)
        {
            canChangeScene = false;
            Invoke("ChangeScene", sceneLoadDelay);
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SetBool(bool thing)
    {
        canChangeScene = thing;
    }
}
