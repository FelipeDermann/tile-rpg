using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    public static UpdateManager Instance;

    public float globalHexAnimationTime;

    float time = 0;

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
    }

    // Update is called once per frame
    void Update()
    {
        GlobalHexAnimationTimer();
    }

    void GlobalHexAnimationTimer()
    {
        time += Time.deltaTime / 1;
        globalHexAnimationTime = Mathf.Lerp(0, 1, time);

        if (globalHexAnimationTime >= 1)
        {
            time = 0;
            globalHexAnimationTime = 0;
            GlobalHexAnimationTimer();
        }
    }
}
