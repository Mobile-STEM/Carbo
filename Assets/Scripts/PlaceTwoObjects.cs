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

        [SerializeField]
        [Tooltip("Instantiates this prefab on a plane at the touch location.")]
        GameObject m_carbonModelPrefab;

        [SerializeField]
        [Tooltip("Instantiates this prefab on a plane at the touch location.")]
        GameObject m_PlacedPrefab;

        /// <summary>
        /// The prefab to instantiate on touch.
        /// </summary>
        public GameObject placedPrefab
        {
            get { return m_PlacedPrefab; }
            set { m_PlacedPrefab = value; }
        }

        public GameObject carbonModelPrefab
        {
            get { return m_carbonModelPrefab; }
            set { m_carbonModelPrefab = value; }
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
            yield return new WaitForSecondsRealtime(4);
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
                StopCoroutine(WaitOneSecond());    // Interrupt in case it's running
                StartCoroutine(WaitOneSecond());
            } else if (arrayPosition == 2 || arrayPosition == 5 || arrayPosition == 9)
            {
                nextButton.gameObject.SetActive(true);
            }
            else if (arrayPosition == 4 || arrayPosition == 8)
            {
                nextButton.gameObject.SetActive(false);
                StopCoroutine(WaitOneSecond());    // Interrupt in case it's running
                StartCoroutine(WaitOneSecond());
                if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
                {
                    // Raycast hits are sorted by distance, so the first one
                    // will be the closest hit.
                    var hitPose = s_Hits[0].pose;

                    if (spawnedObject == null)
                    {
                        spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
                    }
                    else
                    {
                        spawnedObject.transform.position = hitPose.position;

                        if ((Mathf.Abs(spawnedObject.transform.position.x - carbonModel.transform.position.x) < 1) && (Mathf.Abs(spawnedObject.transform.position.y - carbonModel.transform.position.y) < 1) && (Mathf.Abs(spawnedObject.transform.position.z - carbonModel.transform.position.z) < 1)) {
                            StopCoroutine(WaitOneSecond());    // Interrupt in case it's running
                            StartCoroutine(WaitOneSecond());
                            arrayPosition++;
                            text.text = textArray[arrayPosition];
                        }
                    }
                }

            }
            else if (arrayPosition == (textArray.Length - 1))
            {
                nextButton.gameObject.SetActive(false);
            }
        }
    }
}