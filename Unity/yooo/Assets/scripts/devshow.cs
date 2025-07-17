using TMPro;
using UnityEngine;

public class devshow : MonoBehaviour
{
    private TextMeshProUGUI text;
    
    void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        text.text = bt.devname;
    }
}
