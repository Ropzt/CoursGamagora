using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class waveCounter : MonoBehaviour
{
    private Text waveDisplay ;
    private int waveCount = 1;

    private void Start()
    {
        waveDisplay = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        waveDisplay.text = "Wave : " + waveCount;
    }
}
