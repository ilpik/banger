using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWriter : MonoBehaviour
{
    public Text textbox;

    public float speed;

    private float score;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        score += speed * Time.deltaTime;
        textbox.text = ((int)score).ToString("0S00K0A");
    }
}
