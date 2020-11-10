using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/*
 * Script for the Fossil Screen of the AR Application
 */
namespace UnityEngine.XR.ARFoundation.Samples
{
    [RequireComponent(typeof(ARRaycastManager))]
    public class Fossils : MonoBehaviour
    {
        public Button backButton;
        public Button nextButton;
        public Text text;
        public string[] textArray;
        public int arrayPosition = 0;
        private bool enter = false;

        [SerializeField]
        GameObject m_carbonModelPrefab;

        [SerializeField]
        GameObject m_factoryPrefab;

        [SerializeField]
        GameObject m_dirtPrefab;

        [SerializeField]
        ARPlaneManager m_PlaneManager;

        /// <summary>
        /// The prefab to instantiate on touch.
        /// </summary>
        public GameObject factoryPrefab
        {
            get { return m_factoryPrefab; }
            set { m_factoryPrefab = value; }
        }

        public GameObject carbonModelPrefab
        {
            get { return m_carbonModelPrefab; }
            set { m_carbonModelPrefab = value; }
        }

        public GameObject dirtPrefab
        {
            get { return m_dirtPrefab; }
            set { m_dirtPrefab = value; }
        }

        public ARPlaneManager planeManager
        {
            get { return m_PlaneManager; }
            set { m_PlaneManager = value; }
        }

        /// <summary>
        /// The object instantiated as a result of a successful raycast intersection with a plane.
        /// </summary>
        public GameObject carbonModel { get; private set; }
        public GameObject dirtModel { get; private set; }
        public GameObject factoryModel { get; private set; }

        ARRaycastManager m_RaycastManager;

        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        /// <summary>
        /// On start, add listeners to next and back button
        /// </summary>
        void Start()
        {
            nextButton.onClick.AddListener(() => OnNextButtonClicked());
            backButton.onClick.AddListener(() => OnBackButtonClicked());
            nextButton.gameObject.SetActive(false);

            //TODO: Make sure it checks for the field always.
        }

        /// <summary>
        /// When Next button is clicked, increase array position and text
        /// </summary>
        void OnNextButtonClicked()
        {
            increaseArrayPosition();
        }

        /// <summary>
        /// If back button clicked goes to homepage
        /// </summary>
        void OnBackButtonClicked()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Molecules");
        }

        /// <summary>
        /// As script is awaked, get raycast manager
        /// </summary>
        void Awake()
        {
            m_RaycastManager = GetComponent<ARRaycastManager>();
        }

        /// <summary>
        /// Get touch position for every update
        /// </summary>
        /// <param name="touchPosition">position of touch</param>
        /// <returns> If touch is found or not </returns>
        bool TryGetTouchPosition(out Vector2 touchPosition)
        {
            if (Input.touchCount > 0)
            {
                touchPosition = Input.GetTouch(0).position;
                return true;
            }

            touchPosition = default;
            return false;
        }

        /// <summary>
        /// waits a couple of seconds before giving other model
        /// </summary>
        /// <returns></returns>
        IEnumerator WaitOneSecond()
        {
            enter = true;
            yield return new WaitForSeconds(2);
            enter = false;
        }

        /// <summary>
        /// Checks if there are any planes
        /// </summary>
        /// <returns> boolean </returns>
        bool PlanesFound()
        {
            if (planeManager == null)
                return false;

            return planeManager.trackables.count > 0;
        }

        /*        IEnumerator FadeOut(GameObject deletablePrefab)
                {
                    while (deletablePrefab.material.color.a > 0)
                    {
                        Color colorObject = deletablePrefab.material.color;
                        float fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

                        objectColor = new ConsoleColor(ObjectColor.r, objectColor.g, objectColor.b, fadeAmount);
                        deletablePrefab.material.color = objectColor;
                        yield return null;
                    }
                    spawnedObject = Instantiate(m_mediumTreePrefab, spawnedObject.transform.position, spawnedObject.transform.rotation);
                    Destroy(deletablePrefab);
                }

                IEnumerator FadeIn(GameObject createPrefab)
                {
                    while (createPrefab.material.color.a < 1)
                    {
                        Color colorObject = createPrefab.material.color;
                        float fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

                        objectColor = new ConsoleColor(ObjectColor.r, objectColor.g, objectColor.b, fadeAmount);
                        createPrefab.material.color = objectColor;
                        yield return null;
                    }
                }*/

        /// <summary>
        /// Increases the array position throughout the interaction
        /// </summary>
        void increaseArrayPosition()
        {
            arrayPosition++;
            text.text = textArray[arrayPosition];
        }

        //runs every time something new happens
        void Update()
        {
            // on the first text box array, if no planes are found, continue letting them scan
            if (arrayPosition == 0)
            {
                if (PlanesFound())
                {
                    increaseArrayPosition();
                }
                return;
            }

            // if no position return
            if (!TryGetTouchPosition(out Vector2 touchPosition))
                return;

            // if the correct screen is active, lets put the carbon model up
            if (arrayPosition == 1)
            {
                //hide button to not let the user continue
                if (nextButton.gameObject.activeSelf)
                {
                    nextButton.gameObject.SetActive(false);
                }

                //wait to avoid placing too fast
                StartCoroutine(WaitOneSecond());

                //add object
                if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = s_Hits[0].pose;
                    carbonModel = Instantiate(m_carbonModelPrefab, hitPose.position, hitPose.rotation);
                }

                //continue
                increaseArrayPosition();
            }
            else if (arrayPosition == 2 || arrayPosition == 5 || arrayPosition == 7)
            {
                nextButton.gameObject.SetActive(true);
                StartCoroutine(WaitOneSecond());
            }
            else if (arrayPosition == 3)
            {
                //hide button to not let the user continue
                if (nextButton.gameObject.activeSelf)
                {
                    nextButton.gameObject.SetActive(false);
                }

                //wait to avoid placing too fast
                StartCoroutine(WaitOneSecond());

                //add object
                if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = s_Hits[0].pose;
                    dirtModel = Instantiate(m_dirtPrefab, hitPose.position, hitPose.rotation);
                }

                //continue
                increaseArrayPosition();
            }
            else if (arrayPosition == 4)
            {
                //hide button to not let the user continue
                if (nextButton.gameObject.activeSelf)
                {
                    nextButton.gameObject.SetActive(false);
                }

                //wait to avoid placing too fast
                if (!enter)
                {
                    StartCoroutine(WaitOneSecond());
                }

                if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
                {
                    // Raycast hits are sorted by distance, so the first one
                    // will be the closest hit.
                    var hitPose = s_Hits[0].pose;
                    if (carbonModel == null)
                    {
                        carbonModel = Instantiate(carbonModelPrefab, hitPose.position, hitPose.rotation);
                    }
                    else
                    {
                        carbonModel.transform.position = hitPose.position;

                        // must be in .3 x .3 x .3 area cube
                        if ((Mathf.Abs(dirtModel.transform.position.x - carbonModel.transform.position.x) < .3) && (Mathf.Abs(dirtModel.transform.position.y - carbonModel.transform.position.y) < .3) && (Mathf.Abs(dirtModel.transform.position.z - carbonModel.transform.position.z) < .3))
                        {
                            if (!enter)
                            {
                                StartCoroutine(WaitOneSecond());
                            }
                            //continue
                            increaseArrayPosition();
                        }
                    }
                }
            }
            else if (arrayPosition == 6)
            {
                //hide button to not let the user continue
                if (nextButton.gameObject.activeSelf)
                {
                    nextButton.gameObject.SetActive(false);
                }

                //wait to avoid placing too fast
                StartCoroutine(WaitOneSecond());

                //add object
                if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = s_Hits[0].pose;
                    factoryModel = Instantiate(m_factoryPrefab, hitPose.position, hitPose.rotation);
                }

                //continue
                increaseArrayPosition();
            }
            else if (arrayPosition == 8)
            {
                //hide button to not let the user continue
                if (nextButton.gameObject.activeSelf)
                {
                    nextButton.gameObject.SetActive(false);
                }

                //wait to avoid placing too fast
                if (!enter)
                {
                    StartCoroutine(WaitOneSecond());
                }

                if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
                {
                    // Raycast hits are sorted by distance, so the first one
                    // will be the closest hit.
                    var hitPose = s_Hits[0].pose;
                    if (carbonModel == null)
                    {
                        carbonModel = Instantiate(carbonModelPrefab, hitPose.position, hitPose.rotation);
                    }
                    else
                    {
                        carbonModel.transform.position = hitPose.position;
                        // must be in .3 x .3 x .3 area cube
                        if ((Mathf.Abs(factoryModel.transform.position.x - carbonModel.transform.position.x) < .3) && (Mathf.Abs(factoryModel.transform.position.y - carbonModel.transform.position.y) < .3) && (Mathf.Abs(factoryModel.transform.position.z - carbonModel.transform.position.z) < .3))
                        {
                            if (!enter)
                            {
                                StartCoroutine(WaitOneSecond());
                            }
                            //continue
                            increaseArrayPosition();
                        }
                    }
                }
            }
            // Last position, only allow them to click the home button instead of next.. may need to be removed
            else if (arrayPosition == (textArray.Length - 1))
            {
                if (!enter)
                {
                    StartCoroutine(WaitOneSecond());
                }
                nextButton.gameObject.SetActive(false);
            }
        }
    }
}