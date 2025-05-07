using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : View
{
    [SerializeField] private TextMeshProUGUI speechBubble;
    [SerializeField] private GameObject _leftBubbleConnector;
    [SerializeField] private GameObject _rightBubbleConnector;
    [Header("Dialogue Move Forward Indicator")]
    [SerializeField] private GameObject _spaceBar;
    [SerializeField] private GameObject _gamerControls;
    [SerializeField] private GameObject _casualControls;

    private float _yLevel = 260.4f;
    public override void Initialize()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.IsGamerControls)
        {
            _gamerControls.SetActive(true);
            _casualControls.SetActive(false);
        }
        else
        {
            _gamerControls.SetActive(false);
            _casualControls.SetActive(true);
        }
    }

    public void setText(string text)
    {
        speechBubble.text = text;
    }

    public void setSpaceBar(bool truth)
    {
        _spaceBar.SetActive(truth);
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
