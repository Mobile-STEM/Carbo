using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleItems : MonoBehaviour
{
    public Button plusButton;
    public GameObject ItemLayouts;

    // Start is called before the first frame update
    void Start()
    {
        plusButton.onClick.AddListener(() => OnPlusButtonClicked());
        ItemLayouts.gameObject.SetActive(false);
    }

    void OnPlusButtonClicked()
    {
        if (ItemLayouts.gameObject.activeSelf == true)
        {
            ItemLayouts.gameObject.SetActive(false);
        } else
        {
            ItemLayouts.gameObject.SetActive(true);
        }
    }
}
