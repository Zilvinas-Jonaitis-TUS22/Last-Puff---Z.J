using StarterAssets; // Ensure this using directive is included
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Timeline : MonoBehaviour
{
    public string cinematicObjectsTag = "CinematicObject";
    public string cinematicTriggerTag = "CinematicTrigger";
    public string timelineTag = "MasterTimeline";

    public bool returnPlayerToPosition = true;
    public Vector3 playerReturnPosition;
    public Quaternion playerReturnRotation; // Exposed rotation variable

    private GameObject masterTimelineObject;
    private PlayableDirector timelineDirector;
    private List<GameObject> cinematicObjects = new List<GameObject>();
    private List<GameObject> playerObjects = new List<GameObject>();
    private GameObject cinematicTriggerObject;
    private bool timelineLogOnce = false;

    public GameObject playerObject;
    public GameObject UICanvas;
    public bool oneTimeUse = true;

    private int currentAnimationStateHash;
    private float currentAnimationTime;
    private Dictionary<string, bool> boolParameters = new Dictionary<string, bool>();

    private void Start()
    {
        // Find the MasterTimeline object by tag, including inactive objects
        masterTimelineObject = FindInactiveObjectByTag(timelineTag);
        if (masterTimelineObject == null)
        {
            //Debug.LogError("MasterTimeline GameObject not found with tag: " + timelineTag);
            return;
        }

        // Temporarily activate the MasterTimeline object to get its components
        bool wasActive = masterTimelineObject.activeSelf;
        if (!wasActive)
        {
            //Debug.Log("MasterTimeline is inactive, temporarily activating it.");
            masterTimelineObject.SetActive(true);
        }

        // Get the PlayableDirector component from MasterTimeline
        timelineDirector = masterTimelineObject.GetComponent<PlayableDirector>();
        if (timelineDirector == null || timelineDirector.playableAsset == null)
        {
            //Debug.LogError("PlayableDirector or playable asset not found on MasterTimeline.");
            return;
        }

        //Debug.Log("MasterTimeline and PlayableDirector found, deactivating MasterTimeline object at start.");

        // Deactivate MasterTimeline and all cinematic objects initially
        masterTimelineObject.SetActive(false);

        // Restore the original active state
        if (!wasActive)
        {
            masterTimelineObject.SetActive(false);
        }

        // Store references to all cinematic objects and the trigger object in lists
        Transform[] allTransforms = UnityEngine.Object.FindObjectsOfType<Transform>(true);
        foreach (Transform t in allTransforms)
        {
            if (t.CompareTag(cinematicObjectsTag))
            {
                cinematicObjects.Add(t.gameObject);
                t.gameObject.SetActive(false);
            }
            else if (t.CompareTag("Player"))
            {
                playerObjects.Add(t.gameObject);
            }
            else if (t.CompareTag(cinematicTriggerTag) && oneTimeUse)
            {
                cinematicTriggerObject = t.gameObject;
            }
        }
    }

    private GameObject FindInactiveObjectByTag(string tag)
    {
        Transform[] allTransforms = Resources.FindObjectsOfTypeAll<Transform>();
        foreach (Transform t in allTransforms)
        {
            if (t.CompareTag(tag) && t.gameObject.hideFlags == HideFlags.None)
            {
                return t.gameObject;
            }
        }
        return null;
    }

    public void ManualActivation()
    {
        if (UICanvas != null)
        {
            UICanvas.SetActive(false);
        }

        //Debug.Log("Player entered trigger, starting cinematic...");

        // Save the current animation state and time
        Animator playerAnimator = playerObject.GetComponent<Animator>();
        if (playerAnimator != null)
        {
            AnimatorStateInfo currentStateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
            currentAnimationStateHash = currentStateInfo.shortNameHash;
            currentAnimationTime = currentStateInfo.normalizedTime;

            // Save all Bool parameter states
            foreach (AnimatorControllerParameter parameter in playerAnimator.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Bool)
                {
                    boolParameters[parameter.name] = playerAnimator.GetBool(parameter.name);
                }
            }
        }
        else
        {
            //Debug.LogError("Player Animator not found.");
        }

        // Deactivate all Player-tagged objects
        foreach (GameObject player in playerObjects)
        {
            player.SetActive(false);
            //Debug.Log($"Player object deactivated: {player.name}");
        }

        // Reset input values (you'll need to customize this based on your input handling)
        ResetPlayerInputs();

        // Activate MasterTimeline and all cinematic objects
        masterTimelineObject.SetActive(true);
        //Debug.Log("MasterTimeline activated.");

        foreach (GameObject obj in cinematicObjects)
        {
            obj.SetActive(true);
            //Debug.Log($"Cinematic object activated: {obj.name}");
        }

        // Start the timeline
        timelineDirector.Play();
        //Debug.Log("Timeline playing...");

        // Start the coroutine to wait for the timeline to finish
        StartCoroutine(WaitForTimelineToFinish());

    }
    private void OnTriggerEnter(Collider other)
    {
        if (UICanvas != null)
        {
            UICanvas.SetActive(false);
        }

        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player entered trigger, starting cinematic...");

            // Save the current animation state and time
            Animator playerAnimator = other.GetComponent<Animator>();
            if (playerAnimator != null)
            {
                AnimatorStateInfo currentStateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
                currentAnimationStateHash = currentStateInfo.shortNameHash;
                currentAnimationTime = currentStateInfo.normalizedTime;

                // Save all Bool parameter states
                foreach (AnimatorControllerParameter parameter in playerAnimator.parameters)
                {
                    if (parameter.type == AnimatorControllerParameterType.Bool)
                    {
                        boolParameters[parameter.name] = playerAnimator.GetBool(parameter.name);
                    }
                }
            }
            else
            {
                //Debug.LogError("Player Animator not found.");
            }

            // Deactivate all Player-tagged objects
            foreach (GameObject player in playerObjects)
            {
                player.SetActive(false);
                //Debug.Log($"Player object deactivated: {player.name}");
            }

            // Reset input values (you'll need to customize this based on your input handling)
            ResetPlayerInputs();

            // Activate MasterTimeline and all cinematic objects
            masterTimelineObject.SetActive(true);
            //Debug.Log("MasterTimeline activated.");

            foreach (GameObject obj in cinematicObjects)
            {
                obj.SetActive(true);
                //Debug.Log($"Cinematic object activated: {obj.name}");
            }

            // Start the timeline
            timelineDirector.Play();
            //Debug.Log("Timeline playing...");

            // Start the coroutine to wait for the timeline to finish
            StartCoroutine(WaitForTimelineToFinish());
        }
    }

    private void ResetPlayerInputs()
    {
        //Debug.Log("Resetting player inputs to zero.");

        // Ensure you have the right reference to the FirstPersonController or Input script
        // Example for Starter Assets:
        var playerController = FindObjectOfType<StarterAssets.FirstPersonController>();
        if (playerController != null)
        {
            // Disable the player controller input handling
            playerController.enabled = false;

            // Reset the input values
            var starterAssetsInputs = playerController.GetComponent<StarterAssetsInputs>();
            if (starterAssetsInputs != null)
            {
                starterAssetsInputs.move = Vector2.zero;
                starterAssetsInputs.look = Vector2.zero;
                starterAssetsInputs.jump = false;
                starterAssetsInputs.sprint = false;
            }

            // Reset the player's velocity
            var characterController = playerController.GetComponent<CharacterController>();
            if (characterController != null)
            {
                characterController.Move(Vector3.zero);
                characterController.velocity.Set(0, 0, 0); // Ensure velocity is reset
            }
        }
    }

    private IEnumerator WaitForTimelineToFinish()
    {
        //Debug.Log("Started coroutine to wait for timeline to finish.");

        // Wait until the timeline finishes playing
        while (timelineDirector.state == PlayState.Playing)
        {
            if (!timelineLogOnce)
            {
                //Debug.Log("Timeline still playing...");
                timelineLogOnce = true; // Log this message only once
            }
            yield return null;  // Keep checking the timeline state
        }

        // Timeline has finished playing, now proceed with the next steps
        //Debug.Log("Timeline finished!");

        // Deactivate cinematic objects after the timeline finishes
        foreach (GameObject obj in cinematicObjects)
        {
            obj.SetActive(false);
            //Debug.Log($"Cinematic object deactivated: {obj.name}");
        }

        // Deactivate the CinematicTrigger GameObject
        if (cinematicTriggerObject != null)
        {
            cinematicTriggerObject.SetActive(false);
            //Debug.Log("CinematicTrigger deactivated.");
        }

        // Reactivate player objects and place them at the specified position
        //Debug.Log($"Found {playerObjects.Count} player objects to reactivate.");
        if (returnPlayerToPosition)
        {
            playerObject.transform.position = playerReturnPosition;
            playerObject.transform.rotation = playerReturnRotation;
        }
        
        foreach (GameObject player in playerObjects)
        {
            //player.transform.position = playerReturnPosition;
            //player.transform.rotation = playerReturnRotation;

            // Reset Rigidbody if present
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            player.SetActive(true);
            //Debug.Log($"Player object reactivated and positioned: {player.name}");

            // Ensure the player object is active before accessing the Animator
            if (player.activeInHierarchy)
            {
                Animator playerAnimator = player.GetComponent<Animator>();
                if (playerAnimator != null)
                {
                    playerAnimator.Play(currentAnimationStateHash, 0, currentAnimationTime);

                    // Restore all Bool parameter states
                    foreach (var parameter in boolParameters)
                    {
                        playerAnimator.SetBool(parameter.Key, parameter.Value);
                    }
                }
                else
                {
                    //Debug.LogError("Player Animator not found on reactivation.");
                }
            }
            else
            {
                //Debug.LogError("Player object is not active in hierarchy.");
            }
        }

        // Reactivate the player controller after cinematic
        var playerController = FindObjectOfType<StarterAssets.FirstPersonController>();
        if (playerController != null)
        {
            playerController.enabled = true;

            // Reset the input values again to ensure no residual inputs
            var starterAssetsInputs = playerController.GetComponent<StarterAssetsInputs>();
            if (starterAssetsInputs != null)
            {
                starterAssetsInputs.move = Vector2.zero;
                starterAssetsInputs.look = Vector2.zero;
                starterAssetsInputs.jump = false;
                starterAssetsInputs.sprint = false;
            }
        }

        // Deactivate the MasterTimeline GameObject
        masterTimelineObject.SetActive(false);
        //Debug.Log("MasterTimeline deactivated after timeline finishes.");

        if (UICanvas != null)
        {
            UICanvas.SetActive(true);
        }
    }


}