using UnityEngine;
using UnityEngine.UI;

public class SliderVisibilityController : MonoBehaviour
{
    public Slider slider;               // Reference to the Slider component
    public float timeoutDuration = 3f;  // Duration of inactivity before hiding the slider
    private float inactivityTimer = 0f; // Timer to track inactivity

    private void Start()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }

        // Subscribe to the slider's value change event
        slider.onValueChanged.AddListener(OnSliderValueChanged);

        // Make sure the slider is visible at the start
        SetSliderVisible(true);
    }

    private void Update()
    {
        // Increment the timer if the slider is visible
        if (slider.gameObject.activeSelf)
        {
            inactivityTimer += Time.deltaTime;

            // Check if the timer exceeds the timeout duration
            if (inactivityTimer >= timeoutDuration)
            {
                SetSliderVisible(false);
            }
        }
    }

    private void OnSliderValueChanged(float value)
    {
        // Reset the inactivity timer and ensure the slider is visible when changed
        inactivityTimer = 0f;
        SetSliderVisible(true);
    }

    private void SetSliderVisible(bool visible)
    {
        // Toggle the slider's visibility by enabling/disabling its GameObject
        slider.gameObject.SetActive(visible);
    }

    private void OnDestroy()
    {
        // Unsubscribe from the slider's value change event to avoid memory leaks
        slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }
}
