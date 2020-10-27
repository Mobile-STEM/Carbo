using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARTextUpdate : MonoBehaviour
{
    public Button backButton;
    public Button nextButton;
    public Text text;

    public string[] textArray;
    public int arrayPosition = 0;

    // Start is called before the first frame update
    void Start()
    {
        nextButton.onClick.AddListener(() => OnNextButtonClicked());
        backButton.onClick.AddListener(() => OnBackButtonClicked());
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnNextButtonClicked()
    {
        arrayPosition++;
        text.text = textArray[arrayPosition];

        if (arrayPosition == (textArray.Length - 1))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Molecules");
        }
    }

    void OnBackButtonClicked()
    {
        if (arrayPosition == 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Molecules");
        }
        else
        {
            arrayPosition--;
            text.text = textArray[arrayPosition];
        }
    }
}