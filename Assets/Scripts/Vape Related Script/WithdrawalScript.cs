using UnityEngine;
using UnityEngine.UI;

public class WithdrawalScript : MonoBehaviour
{
    public Transform player; // Assign the player object in the inspector
    public Slider withdrawalSlider; // Reference to the withdrawal slider UI component
    public float maxWithdrawalAmount = 100f; // Maximum withdrawal amount
    public float decreaseRate = 1f; // Amount withdrawal decreases per second
    public float increaseRate = 5f; // Amount withdrawal increases per second when vaping

    private float currentWithdrawalAmount;

    private PlayerRespawn playerRespawn; // Reference to PlayerRespawn script

    void Start()
    {
        currentWithdrawalAmount = maxWithdrawalAmount; // Initialize to max withdrawal amount
        withdrawalSlider.maxValue = maxWithdrawalAmount; // Set max value for the slider
        withdrawalSlider.value = currentWithdrawalAmount; // Set initial slider value

        playerRespawn = player.GetComponent<PlayerRespawn>();
    }

    void Update()
    {
        // Decrease the withdrawal bar over time
        currentWithdrawalAmount -= decreaseRate * Time.deltaTime;
        withdrawalSlider.value = currentWithdrawalAmount;

        // Check if the player has died
        if (currentWithdrawalAmount <= 0)
        {
            PlayerDies();
        }
    }

    public void OnVape()
    {
        currentWithdrawalAmount += increaseRate * Time.deltaTime; // Increase the withdrawal bar
        currentWithdrawalAmount = Mathf.Clamp(currentWithdrawalAmount, 0, maxWithdrawalAmount); // Clamp to max value
        withdrawalSlider.value = currentWithdrawalAmount; // Update the slider
    }

    private void PlayerDies()
    {
        playerRespawn.RespawnWithDeathScreen();
        currentWithdrawalAmount = maxWithdrawalAmount;
        withdrawalSlider.maxValue = maxWithdrawalAmount;
        withdrawalSlider.value = currentWithdrawalAmount;
    }
}
