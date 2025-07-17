using UnityEngine;
using UnityEngine.UI;

public class wrapperleft : MonoBehaviour
{

    void Start()
    {
        Button btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(dec);
    }

    void dec()
    {
        button.barrieractive = false;
        bt.decrement();
    }
}
