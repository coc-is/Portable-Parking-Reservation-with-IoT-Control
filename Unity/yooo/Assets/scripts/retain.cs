using System;
using UnityEngine;

[CreateAssetMenu(fileName = "retain", menuName = "Scriptable Objects/retain")]
public class retain : ScriptableObject
{
    public static String icnum="Bro you hackin";

    static int tick = 0;
    static int tickmax = 0;

    public static Vector2[] solardat = new Vector2[144];
    void OnEnable()
    {
        Application.targetFrameRate=30;

    }

    public static bool delay(float timesec, bool rst)
    {
        tickmax = (int)Math.Round(timesec * Application.targetFrameRate);
        tick = rst ? 0 : tick;

        return tick >= tickmax;


    }


}
