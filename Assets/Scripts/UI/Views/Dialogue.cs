using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : View
{
    [SerializeField] private TextMeshProUGUI speechBubble;
    [SerializeField] private TriTest tri;
    public override void Initialize()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setText(string text)
    {
        speechBubble.text = text;
    }

    public void setBubbleConnector(Vector2 point)
    {
        tri.SetPoint1(point);
    }
}
