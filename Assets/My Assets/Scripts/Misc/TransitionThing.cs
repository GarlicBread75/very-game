using UnityEngine;
using UnityEngine.Events;

public class TransitionThing : MonoBehaviour
{
    [SerializeField] UnityEvent eventThingy;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ChangeScene()
    {
        eventThingy.Invoke();
    }

    public void Idle()
    {
        anim.SetBool("To Idle", true);
    }
}
