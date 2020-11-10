using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickCycle : MonoBehaviour
{
    public Button goButton;
    public Button nextButton;
    public Button backButton;
    int currentCarbonCycle;
    public Transform[] views;
    public float transitionSpeed;
    Transform currentView;
    public Text moleculeText;

    public string[] textArray = { "Hello friend! I am a CO2 molecule that absolutely loves plants! I am feeling lonely and want to visit my plant friends. Do you want to go on that journey?",
        "Hello friend! I am a CO2 molecule that loves working in factories! I am not in the mood to be lazy! Do you want to follow my journey?" };

    // Start is called before the first frame update
    void Start()
    {
        goButton.onClick.AddListener(() => OnGoButtonClicked());
        nextButton.onClick.AddListener(() => OnNextButtonClicked());
        backButton.onClick.AddListener(() => OnBackButtonClicked());
        backButton.gameObject.SetActive(false);
    }

    void Update()
    {
        if (currentCarbonCycle == 0)
        {
            currentView = views[0];
        } else
        {
            currentView = views[1];
        }
    }

    // Update is called once per frame
    void OnGoButtonClicked()
    {
        if (currentCarbonCycle == 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Plants");
        } else if (currentCarbonCycle == 1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Fossils");
        }
    }

    void OnNextButtonClicked()
    {
        backButton.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(false);
        currentCarbonCycle++;
        moleculeText.text = textArray[1];
    }

    void OnBackButtonClicked()
    {
        backButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(true);
        currentCarbonCycle--;
        moleculeText.text = textArray[0];
    }

    void LateUpdate()
    {

        //Lerp position
        transform.position = Vector3.Lerp(transform.position, currentView.position, Time.deltaTime * transitionSpeed);

        Vector3 currentAngle = new Vector3(
         Mathf.LerpAngle(transform.rotation.eulerAngles.x, currentView.transform.rotation.eulerAngles.x, Time.deltaTime * transitionSpeed),
         Mathf.LerpAngle(transform.rotation.eulerAngles.y, currentView.transform.rotation.eulerAngles.y, Time.deltaTime * transitionSpeed),
         Mathf.LerpAngle(transform.rotation.eulerAngles.z, currentView.transform.rotation.eulerAngles.z, Time.deltaTime * transitionSpeed));

        transform.eulerAngles = currentAngle;

    }
}
