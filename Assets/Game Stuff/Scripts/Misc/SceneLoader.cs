using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] string scene;

    void OnTriggerEnter(Collider trigger)
    {
        SceneManager.LoadScene(scene);
    }
}
