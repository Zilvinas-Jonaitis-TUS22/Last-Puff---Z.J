using UnityEngine;

public class CollectableVape : MonoBehaviour
{
    public float refillAmount = 20f; // Amount to refill the vape bar
    public string vapeID; // Unique ID for this vape object

    private VapeScript vapeScript;
    public AudioSource collectSound;

    public Animator handAnimator; // Reference to the hand Animator component

    private bool isCollected = false; // Track collection status

    void Start()
    {
        vapeScript = FindObjectOfType<VapeScript>();

        if (collectSound == null)
        {
            collectSound = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true; // Mark as collected
            //Play Interaction Animation
            handAnimator.SetTrigger("Grabbing");

            // Refills the vape bar
            if (vapeScript != null)
            {
                vapeScript.vapeJuiceSlider.value += refillAmount;
                vapeScript.vapeJuiceSlider.value = Mathf.Clamp(vapeScript.vapeJuiceSlider.value, 0, vapeScript.vapeJuiceAmount);
            }

            // Play the collection sound
            if (collectSound != null && collectSound.clip != null)
            {
                collectSound.Play();
            }

            // Disable the object after collecting
            gameObject.SetActive(false);
        }
    }
}
