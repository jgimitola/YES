using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    bool timerActive = true;
    float currentTime = 0;
    public int startMinutes;

    private Text currentTimeText;   

    public void StartTimer()
    {
        timerActive = true;
    }

    public void StopTimer()
    {
        timerActive = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentTimeText = GameManager.Instance.currentTimeText;
        currentTime = startMinutes * 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
        {
            currentTime = currentTime + Time.deltaTime;
        }
        currentTimeText.text = "Lap: " + currentTime.ToString();
    }
}
