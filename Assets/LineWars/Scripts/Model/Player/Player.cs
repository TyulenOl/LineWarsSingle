using UnityEngine;


public class Player : MonoBehaviour
{
    public static Player LocalPlayer { get; private set; }
    private INation nation;

    void Awake()
    {
    }

    private void Start()
    {
        LocalPlayer = this;
    }
}