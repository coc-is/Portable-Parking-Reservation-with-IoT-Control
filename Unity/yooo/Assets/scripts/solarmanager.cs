using UnityEngine;
using TMPro;
using Radishmouse;

public class solarmanager : MonoBehaviour
{
    public static Transform bar;
    public static TextMeshProUGUI done;
    public static int count=0;

    private UILineRenderer rend;
    private float step = 1/144f;
        void Start()
    {
        bar = gameObject.GetComponent<Transform>();
        done = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        rend = GameObject.Find("/Canvas/LineRenderer").GetComponent<UILineRenderer>();

        
        updat();
    }

    // Update is called once per frame
    void Update()
    {
        if (count < 144) {
            bar.localScale = new Vector3(step*count, 1, 1);
                }
        else
        {
            done.text = "Done";
            bar.localScale = new Vector3(1, 1, 1);
        }

        if (button.retrieving && bt.receiveddata.Length>0)
        {
            filldata();
            updat();
        }
    }

    void filldata()
    {
        if (bt.receiveddata[0] == '1')
        {//736 -> 13.2 / opamp lm358 3v3 max swing 2.66 2.67
            retain.solardat[count] = new Vector2(count * 7, float.Parse(bt.receiveddata.Substring(1, 4)));
            count++;
            bt.cleardat();
            if (count >= 144)
            {
                button.retrieving = false;
            }
        }else if (bt.receiveddata.Equals("00000"))
        {
            bt.cleardat();
            bt.WriteData("A00002E");
            rst();
        }



    }

    private void updat()
    {
       for(int i = 0; i < retain.solardat.Length; i++)
        {
            rend.points[i] = retain.solardat[i];
        }
        rend.SetAllDirty();
    }
    public static void rst()
    {
        count = 0;
        done.text = "";

    }
}
