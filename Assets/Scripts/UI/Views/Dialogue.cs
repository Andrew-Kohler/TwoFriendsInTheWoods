using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : View
{
    [SerializeField] private TextMeshProUGUI speechBubble;
    [SerializeField] private GameObject _leftBubbleConnector;
    [SerializeField] private GameObject _rightBubbleConnector;

    private float _yLevel = 260.4f;
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

    public void setBubbleConnector(bool left, float xCoord)
    {
        if (left)
        {
            _leftBubbleConnector.SetActive(true);
            _rightBubbleConnector.SetActive(false);

            _leftBubbleConnector.transform.localPosition = new Vector2(xCoord, _yLevel);
        }
        else
        {
            _leftBubbleConnector.SetActive(false);
            _rightBubbleConnector.SetActive(true);

            _rightBubbleConnector.transform.localPosition = new Vector2(xCoord, _yLevel);
        }
    }
}
