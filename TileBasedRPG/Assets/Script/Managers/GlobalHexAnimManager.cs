using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalHexAnimManager : MonoBehaviour
{
    public static GlobalHexAnimManager Instance;

    public Animator globalHex;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        globalHex.Play("Danger");
    }
}
