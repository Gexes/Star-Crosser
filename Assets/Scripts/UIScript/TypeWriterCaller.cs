using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeWriterCaller : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI aboutText; // Reference to the TextMeshProUGUI component
    [SerializeField] private TypeWriterScript typeWriter; // Reference to the TypeWriterScript
    [SerializeField][TextArea(3, 10)] private string fullText = "Default text to display"; // Editable in the Inspector

    public void DisplayText()
    {
        typeWriter.StartTypewriterEffect(aboutText, fullText);
    }
}
