using System.Collections;
using UnityEngine;
using TMPro; // For TextMeshPro

public class TextScript : MonoBehaviour
{
    public TextMeshProUGUI tooltipText;
    public float typingSpeed = 0.05f;
    public float removingSpeed = 0.001f;
    public string tooltipMessage = "Hey sweetie, dinner will be ready soon. Get home safe. Love ya";
    // Start is called before the first frame update
    void Start()
    {
        tooltipText.text = ""; // Clear the text initially
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TextSummoned()
    {
        StartCoroutine(TypeText(tooltipMessage));
    }

    public void TextHidden()
    {
        StartCoroutine(RemoveText());
    }

    IEnumerator TypeText(string message)
    {
        tooltipText.text = ""; // Clear the text initially
        foreach (char letter in message)
        {
            tooltipText.text += letter; // Add one letter at a time
            yield return new WaitForSeconds(typingSpeed); // Wait for the typing speed
        }
    }

    IEnumerator RemoveText()
    {
        // Wait for a while before starting the removal effect (optional)
        yield return new WaitForSeconds(0.002f); // Optional delay before text starts disappearing

        // Keep removing characters from the end of the current text
        while (tooltipText.text.Length > 0)
        {
            tooltipText.text = tooltipText.text.Substring(0, tooltipText.text.Length - 1); // Remove one character at a time
            yield return new WaitForSeconds(removingSpeed); // Wait for the typing speed before removing the next character
        }
    }

}
