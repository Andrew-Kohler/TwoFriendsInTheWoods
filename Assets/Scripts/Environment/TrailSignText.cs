using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrailSignText : MonoBehaviour
{
    [SerializeField] private int _signNum;
    [SerializeField] private TextMeshPro _text;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.IsGamerControls)
        {
            if (_signNum == 0)
            {
                _text.text = "HIKING TIP: Use WASD to walk the trail safely.";
            }
            else if (_signNum == 1)
            {
                _text.text = "Safe hikers use Q to tell fellow hikers to go ahead.";
            }
            else if (_signNum == 2)
            {
                _text.text = "When climbing the trail, hold SHIFT in a pinch.";
            }
            else if (_signNum == 3)
            {
                _text.text = "Safe hikers use the Space Bar to jump over obstacles.";
            }
            else if (_signNum == 4)
            {
                _text.text = "Only rangers that use SHIFT allowed beyond the fence.";
            }
        }
        else
        {
            if (_signNum == 0)
            {
                _text.text = "HIKING TIP: Use arrow keys to walk the trail safely.";
            }
            else if (_signNum == 1)
            {
                _text.text = "Safe hikers use Z to tell fellow hikers to go ahead.";
            }
            else if (_signNum == 2)
            {
                _text.text = "When climbing the trail, hold C in a pinch.";
            }
            else if (_signNum == 3)
            {
                _text.text = "Safe hikers use X to jump over obstacles.";
            }
            else if (_signNum == 4)
            {
                _text.text = "Only rangers that use C allowed beyond the fence.";
            }
        }
        
    }
}
