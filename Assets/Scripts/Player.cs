using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PlayerNumber
    {
        One,
        Two,
        Three,
        Four
    };
    public PlayerNumber playerNumber { get; private set; }

    void Start()
    {
    }

    void Update()
    {
    }
}
