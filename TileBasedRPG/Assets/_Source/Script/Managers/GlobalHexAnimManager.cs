using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalHexAnimManager : MonoBehaviour
{
    public static GlobalHexAnimManager Instance;

    [SerializeField] Animator globalHex;

    public static float NormalizedAnimTime => Instance.globalHex.GetCurrentAnimatorStateInfo(0).normalizedTime;

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
