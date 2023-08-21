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

    public void Invoke()
    {
        eventThingy.Invoke();
    }

    public void Idle()
    {
        anim.SetBool("To Idle", true);
    }
}
