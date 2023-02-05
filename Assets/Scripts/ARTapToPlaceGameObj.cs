using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

// [RequireComponents(typeOf(ARRaycastManager))]
public class ARTapToPlaceGameObj : MonoBehaviour
{
    // Public variable to store the prefab to be instantiated
    public GameObject gameObjectToInstantiate;

    // Private variable to store the instantiated game object
    private GameObject spawnedObject;
    // Private variable to store reference to ARRaycastManager component
    private ARRaycastManager _arRaycastManager;
    // Vector2 to store touch position
    private Vector2 touchPosition;

    // List of ARRaycastHit objects
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // Awake function is called before start
    private void Awake() 
    {
        // Get reference to ARRaycastManager component
        _arRaycastManager = GetComponent<ARRaycastManager>();    
    }

    // Function to get touch position
    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        // Check if there is touch input
        if (Input.touchCount > 0)
        {
            // Get touch position
            touchPosition = Input.GetTouch(0).position;
            // Return true as touch position was obtained
            return true;
        }

        // Set touchPosition to default value and return false
        // if there is no touch input
        touchPosition = default;
        return false;
    }

    // Update function is called once per frame
    void Update()
    {
        // Check if touch position is obtained
        if(!TryGetTouchPosition(out Vector2 touchPosition))
            return;
        // Check if raycast hits a plane within polygon
        if(_arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            // Get the pose (position and rotation) of the hit
            var hitPose = hits[0].pose;

            // Check if instantiated object exists
            if(spawnedObject == null)
            {
                // Instantiate the game object at the hit position and rotation
                spawnedObject = Instantiate(gameObjectToInstantiate, hitPose.position, hitPose.rotation); 
            }
            else
            {
                // Set the position of the instantiated object to the hit position
                spawnedObject.transform.position = hitPose.position;
            }
        }    
    }
}