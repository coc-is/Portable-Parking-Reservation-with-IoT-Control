using UnityEngine;
using UnityEngine.UI;

public class wrapperright : MonoBehaviour
{
    void Start()
    {
        Button btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(inc);
    }

void inc()
    {
        button.barrieractive = false;
        bt.increment();
    }
}
