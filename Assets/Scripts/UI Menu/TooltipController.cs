using UnityEngine;

public class TooltipController : MonoBehaviour
{
    public GameObject vapeTooltip; // Reference to the tooltip GameObject
    private bool gamePaused = true; // Control whether the game is paused

    private void Start()
    {
        // Show the tooltip at the start
        vapeTooltip.SetActive(true);
        Time.timeScale = 0; // Pause the game
    }

    private void Update()
    {
        // Check for left-click to perform the vape action
        if (Input.GetMouseButtonDown(0) && gamePaused)
        {
            // Here you would call the vape action from your VapeScript
            PerformVapeAction();
        }
    }

    private void PerformVapeAction()
    {
        // Assuming you have a reference to VapeScript and it has a method to handle vaping
        VapeScript vapeScript = FindObjectOfType<VapeScript>();

        if (vapeScript != null)
        {
            // Call your vaping logic (you might want to rename this method)
            vapeScript.StartVaping(); // Start vaping
            HideTooltip(); // Hide the tooltip

            // Unpause the game
            gamePaused = false;
            Time.timeScale = 1; // Resume game time
        }
    }

    public void ShowTooltip()
    {
        vapeTooltip.SetActive(true);
    }

    public void HideTooltip()
    {
        vapeTooltip.SetActive(false);
    }
}
