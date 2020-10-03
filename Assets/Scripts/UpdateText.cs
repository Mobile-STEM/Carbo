using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateText : MonoBehaviour
{
    public Button nextButton;
    public Text welcomeText;
    // Start is called before the first frame update
    void Start()
    {
        nextButton.onClick.AddListener(OnButtonClicked);
        if (welcomeText == null)
            welcomeText = nextButton.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void OnButtonClicked()
    {
        Debug.Log("Button Next Clicked!");
        welcomeText.text = "The Carbon Cycle is such an abstract (out of the world) concept because it is hard to imagine that there are tiny carbon molecules around us!";
    }
}
