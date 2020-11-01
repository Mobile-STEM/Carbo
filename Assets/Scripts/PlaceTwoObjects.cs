using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation.Samples
{
    [RequireComponent(typeof(ARRaycastManager))]
    public class PlaceTwoObjects : MonoBehaviour
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
        GameObject m_treePrefab;

        [SerializeField]
        GameObject m_mediumTreePrefab;

        [SerializeField]
        GameObject m_tallTreePrefab;

        /// <summary>
        /// The prefab to instantiate on touch.
        /// </summary>
        public GameObject treePrefab
        {
            get { return m_treePrefab; }
            set { m_treePrefab = value; }
        }

        public GameObject carbonModelPrefab
        {
            get { return m_carbonModelPrefab; }
            set { m_carbonModelPrefab = value; }
        }

        public GameObject mediumTreePrefab
        {
            get { return m_mediumTreePrefab; }
            set { m_mediumTreePrefab = value; }
        }

        public GameObject tallTreePrefab
        {
            get { return m_tallTreePrefab; }
            set { m_tallTreePrefab = value; }
        }

        [SerializeField]
        ARPlaneManager m_PlaneManager;

        public ARPlaneManager planeManager
        {
            get { return m_PlaneManager; }
            set { m_PlaneManager = value; }
        }

        /// <summary>
        /// The object instantiated as a result of a successful raycast intersection with a plane.
        /// </summary>
        public GameObject carbonModel { get; private set; }
        public GameObject spawnedObject { get; private set; }

        /// <summary>
        /// Invoked whenever an object is placed in on a plane.
        /// </summary>
        public static event Action onPlacedObject;

        ARRaycastManager m_RaycastManager;

        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        void Start()
        {
            nextButton.onClick.AddListener(() => OnNextButtonClicked());
            backButton.onClick.AddListener(() => OnBackButtonClicked());
            nextButton.gameObject.SetActive(false);
        }

        void OnNextButtonClicked()
        {
            arrayPosition++;
            text.text = textArray[arrayPosition];
        }

        /// <summary>
        /// If back button clicked goes to homepage
        /// </summary>
        void OnBackButtonClicked()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Molecules");
        }

        void Awake()
        {
            m_RaycastManager = GetComponent<ARRaycastManager>();
        }

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

        // waits a couple of seconds before giving other model
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

        void Update()
        {
            // on the first text box array, if no planes are found, continue letting them scan
            if (arrayPosition == 0)
            {
                if (PlanesFound())
                {
                    arrayPosition++;
                    text.text = textArray[arrayPosition];
                }
                return;
            }

            // if no position return
            if (!TryGetTouchPosition(out Vector2 touchPosition))
                return;

            // if the correct screen is active, lets put the carbon model up
            if (arrayPosition == 1)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
                    {
                        Pose hitPose = s_Hits[0].pose;

                        carbonModel = Instantiate(m_carbonModelPrefab, hitPose.position, hitPose.rotation);

                        if (onPlacedObject != null)
                        {
                            onPlacedObject();
                        }
                    }
                }
                arrayPosition++;
                text.text = textArray[arrayPosition];

                if (!enter)
                {
                    StartCoroutine(WaitOneSecond());
                }

            } else if (arrayPosition == 2 || arrayPosition == 5 || arrayPosition == 8)
            {
                nextButton.gameObject.SetActive(true);

                if (!enter)
                {
                    StartCoroutine(WaitOneSecond());
                }
            }
            else if (arrayPosition == 4 || arrayPosition == 7)
            {
                nextButton.gameObject.SetActive(false);
                if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
                {
                    // Raycast hits are sorted by distance, so the first one
                    // will be the closest hit.
                    var hitPose = s_Hits[0].pose;

                    if (spawnedObject == null)
                    {
                        spawnedObject = Instantiate(m_treePrefab, hitPose.position, hitPose.rotation);
                    }
                    else
                    {
                        if (!enter)
                        {
                            StartCoroutine(WaitOneSecond());
                        }
                        spawnedObject.transform.position = hitPose.position;

                        // must be in .2 x .2 x .2 area cube
                        if (arrayPosition == 7 && (Mathf.Abs(spawnedObject.transform.position.x - carbonModel.transform.position.x) < .3) && (Mathf.Abs(spawnedObject.transform.position.y - carbonModel.transform.position.y) < .3) && (Mathf.Abs(spawnedObject.transform.position.z - carbonModel.transform.position.z) < .3)) {
                            if (!enter)
                            {
                                StartCoroutine(WaitOneSecond());
                            }
                            arrayPosition++;
                            text.text = textArray[arrayPosition];
                        } else if (arrayPosition == 4)
                        {
                            arrayPosition++;
                            text.text = textArray[arrayPosition];
                        }
                    }
                }

            } else if (arrayPosition == 10)
            {
                Destroy(spawnedObject);
                spawnedObject = Instantiate(m_mediumTreePrefab, spawnedObject.transform.position, spawnedObject.transform.rotation);
            } else if (arrayPosition == 11)
            {
                Destroy(spawnedObject);
                spawnedObject = Instantiate(m_tallTreePrefab, spawnedObject.transform.position, spawnedObject.transform.rotation);
            }
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