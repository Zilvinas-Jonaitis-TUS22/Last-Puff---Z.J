using UnityEngine;
using TMPro; // Required for TextMeshPro
using UnityEngine.UI;

public class KeyUIManager : MonoBehaviour
{
    public TextMeshProUGUI keyCountText; // Reference to the TextMeshPro UI element
    public Animator keyAnimator; // Reference to the Animator component
    private PlayerInventory playerInventory; // Reference to the PlayerInventory for key count

    private void Start()
    {
        // Find the PlayerInventory in the scene
        playerInventory = FindObjectOfType<PlayerInventory>();

        // Update the UI immediately at start
        UpdateKeyUI();
    }

    private void Update()
    {
        // Update the UI every frame
        UpdateKeyUI();
    }

    private void UpdateKeyUI()
    {
        if (playerInventory != null)
        {
            // Update the key count display in TextMeshPro
            keyCountText.text = playerInventory.keys.ToString();

            // Set the "HasKey" animator parameter based on key count
            bool hasKey = playerInventory.keys > 0;
            keyAnimator.SetBool("HasKey", hasKey);
        }
    }
}
