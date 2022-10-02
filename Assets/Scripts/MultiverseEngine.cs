using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MultiverseEngine : MonoBehaviour
{
    public List<GameObject> universes;

    private int currentUniverse;

    public float timer = 10.0f;
    private float timeLeft;

    public TMP_Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        float timeLeftInt = Mathf.Ceil(timeLeft);
        timerText.text = (timeLeftInt<10?"0":"") + timeLeftInt;

        if (timeLeft <= 0 || Input.GetButtonDown("Fire3"))
        {
            timeLeft = timer;
            SwapUniverse();
        }
    }

    public void Reset()
    {
        currentUniverse = 0;
        timeLeft = timer;

        // Set only first universe as active
        universes[0].SetActive(true);
        for(int i=1; i<universes.Count; i++)
        {
            universes[i].SetActive(false);
        }
    }

    void SwapUniverse(int nextUniverse=-1)
    {
        universes[currentUniverse].SetActive(false);

        if (nextUniverse == -1)
        {
            currentUniverse++;
            if (currentUniverse >= universes.Count)
                currentUniverse = 0;
        }
        else
        {
            currentUniverse = nextUniverse;
        }

        universes[currentUniverse].SetActive(true);
    }
}
