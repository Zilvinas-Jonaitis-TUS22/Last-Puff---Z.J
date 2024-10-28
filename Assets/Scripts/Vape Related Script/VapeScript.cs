using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

public class VapeScript : MonoBehaviour
{
    public Slider vapeJuiceSlider; // Reference to the slider UI component
    public Animator animator; // Reference to the Animator component
    public Animator handAnimator; // Reference to the Animator component
    public Light vapeLight; // Reference to the Light component
    public AudioSource vapeAudioSource; // Reference to the AudioSource for vaping sound
    public AudioSource exhaleAudioSource; // Reference to the AudioSource for exhale sound
    public float vapeJuiceAmount = 100f; // Total amount of vape juice
    public float consumptionRate = 1f; // Amount of vape juice consumed per second

    private StarterAssetsInputs _Inputs;
    private WithdrawalScript _withdrawalScript;

    void Start()
    {
        vapeJuiceSlider.maxValue = vapeJuiceAmount; // Set the max value of the slider
        vapeJuiceSlider.value = vapeJuiceAmount; // Initialize the slider to full vape juice

        _Inputs = GetComponent<StarterAssetsInputs>();
        _withdrawalScript = GetComponent<WithdrawalScript>();

        // Ensure the light is off initially
        if (vapeLight != null)
        {
            vapeLight.enabled = false;
        }

        // Ensure vaping audio is set to loop and is not playing initially
        if (vapeAudioSource != null)
        {
            vapeAudioSource.loop = true;
            vapeAudioSource.Stop();
        }

        // Ensure exhale audio does not loop
        if (exhaleAudioSource != null)
        {
            exhaleAudioSource.loop = false;
        }
    }

    void Update()
    {
        if (_Inputs.vaping)
        {
            // Reduce the vape juice over time
            vapeJuiceSlider.value -= consumptionRate * Time.deltaTime;

            // Check if the vape juice is empty
            if (vapeJuiceSlider.value <= 0)
            {
                vapeJuiceSlider.value = 0;
                StopVaping(); // Stop vaping if the juice is finished
            }
            else
            {
                // Set the "Currently Vaping" parameter to true while vaping
                animator.SetBool("Currently Vaping", true);
                handAnimator.SetBool("Currently Vaping", true);
                _withdrawalScript.OnVape(); // Increase withdrawal when vaping

                // Enable the light when vaping
                if (vapeLight != null)
                {
                    vapeLight.enabled = true;
                }

                // Play vaping audio if it's not already playing
                if (vapeAudioSource != null && !vapeAudioSource.isPlaying)
                {
                    vapeAudioSource.Play();
                }
            }
        }
        else
        {
            // Set the "Currently Vaping" parameter to false when not vaping
            animator.SetBool("Currently Vaping", false);
            handAnimator.SetBool("Currently Vaping", false);

            // Disable the light when not vaping
            if (vapeLight != null)
            {
                vapeLight.enabled = false;
            }

            // Stop vaping audio and play exhale sound once when vaping stops
            if (vapeAudioSource != null && vapeAudioSource.isPlaying)
            {
                vapeAudioSource.Stop();

                // Play the exhale audio if available
                if (exhaleAudioSource != null)
                {
                    exhaleAudioSource.Play();
                }
            }
        }
    }

    public void StartVaping()
    {
        _Inputs.vaping = true;
    }

    public void StopVaping()
    {
        _Inputs.vaping = false;
    }
}
