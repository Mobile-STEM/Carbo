using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateText : MonoBehaviour
{
    public Button backButton;
    public Button nextButton;
    public Text nextButtonText;
    public Text welcomeText;


    public string[] textArray = { "Welcome to the CarbO App! CarbO is an AR (Augumented Reality) App intended to help you understand the Carbon Cycle and why it is important.",
        "The Carbon Cycle is such an abstract (out of the world) concept because it is hard to imagine that there are tiny carbon molecules around us!",
        "This app will help you visualize all of the carbon molecules that surround and exist in your environment.",
        "You will be able to go on journeys that carbon molecules go through and learn what interactions happen at each journey.",
        "So, are you ready for the journey?" };
    public int arrayPosition = 0;

    // Start is called before the first frame update
    void Start()
    {
        nextButton.onClick.AddListener(() => OnNextButtonClicked());
        backButton.onClick.AddListener(() => OnBackButtonClicked());

        if (arrayPosition == 0)
        {
            backButton.gameObject.SetActive(false);
        }

        if (nextButtonText == null)
        {
            nextButtonText = nextButton.GetComponentInChildren<Text>();
        }
    }

    void OnNextButtonClicked()
    {
        if (arrayPosition == 4)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Molecules");
        }

        arrayPosition++;

        if (arrayPosition == 4)
        {
            nextButtonText.text = "Yes";
        }

        if (arrayPosition != 0)
        {
            backButton.gameObject.SetActive(true);
        }

        welcomeText.text = textArray[arrayPosition];
    }

    void OnBackButtonClicked()
    {
        arrayPosition--;

        if (arrayPosition == 0)
        {
            backButton.gameObject.SetActive(false);
        }

        if (arrayPosition != 4)
        {
            nextButtonText.text = "Next";
        }

        welcomeText.text = textArray[arrayPosition];
    }
}
