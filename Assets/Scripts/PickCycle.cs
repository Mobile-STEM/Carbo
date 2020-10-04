using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickCycle : MonoBehaviour
{
    public Button plantButton;

    // Start is called before the first frame update
    void Start()
    {
        plantButton.onClick.AddListener(() => OnPlantButtonClicked());
    }

    // Update is called once per frame
    void OnPlantButtonClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Plants");
    }
}
