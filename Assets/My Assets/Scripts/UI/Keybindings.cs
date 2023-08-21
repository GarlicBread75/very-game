using UnityEngine;

[CreateAssetMenu(fileName = "InputManager", menuName = "Input Manager")]
public class Keybindings : ScriptableObject
{
    public KeyCode p1Jump, p1Down, p1Left, p1Right, p1Dash, p1Shoot, p2Jump, p2Down, p2Left, p2Right, p2Dash, p2Shoot;
}
