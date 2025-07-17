using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Android;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "bt", menuName = "Scriptable Objects/bt")]


public class bt : ScriptableObject
{
    public static string receiveddata="";
    private static char[] temprecdata = new char[10];
    private static int dataindex=0;

    public static string devname="No Device";
    public static bool isConnected;

    private static AndroidJavaClass unity3dbluetoothplugin;
    public static AndroidJavaObject BluetoothConnector;

    private static ArrayList devlist = new ArrayList();
    public static int index = 0;

    // Start is called before the first frame update
    public static void init()
    {
        InitBluetooth();
        isConnected = false;

        GetPairedDevices();


        if (devlist.Count != 0)
        {
            StartConnection((devlist[0] as string).Split('+')[0]);
            devname = (devlist[0] as string).Split("+")[1].Replace("CARCRASHER","");
        }
    }

    public static void increment()
    {
        GetPairedDevices();

        index++;
        index = (index >= devlist.Count) ? 0 : index;

        if (devlist.Count != 0)
        {
            StopConnection();
            string tempname = (string)devlist[index];
            StartConnection(tempname.Split("+")[0]);
            devname = tempname.Split("+")[1].Replace("CARCRASHER", "");
        }

    }

    public static void decrement()
    {
        GetPairedDevices();

        index--;
        index = (index < 0) ? devlist.Count - 1 : index;

        if (devlist.Count != 0)
        {
            StopConnection();
            string tempname = (string)devlist[index];
            StartConnection(tempname.Split("+")[0]);
            devname = tempname.Split("+")[1].Replace("CARCRASHER", "");
        }
    }

    // creating an instance of the bluetooth class from the plugin 
    public static void InitBluetooth()
    {
        //if (Application.platform != RuntimePlatform.Android)
            //return;

        // Check BT and location permissions
        if (!Permission.HasUserAuthorizedPermission(Permission.CoarseLocation)
            || !Permission.HasUserAuthorizedPermission(Permission.FineLocation)
            || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_ADMIN")
            || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH")
            || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_SCAN")
            || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_ADVERTISE")
            || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_CONNECT"))
        {

            Permission.RequestUserPermissions(new string[] {
                        Permission.CoarseLocation,
                            Permission.FineLocation,
                            "android.permission.BLUETOOTH_ADMIN",
                            "android.permission.BLUETOOTH",
                            "android.permission.BLUETOOTH_SCAN",
                            "android.permission.BLUETOOTH_ADVERTISE",
                             "android.permission.BLUETOOTH_CONNECT"
                    });

        }


        unity3dbluetoothplugin = new AndroidJavaClass("com.example.unity3dbluetoothplugin.BluetoothConnector");
        BluetoothConnector = unity3dbluetoothplugin.CallStatic<AndroidJavaObject>("getInstance");


    }



    // Get paired devices from BT settings
    public static void GetPairedDevices()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        // This function when called returns an array of PairedDevices as "MAC+Name" for each device found
        string[] data = BluetoothConnector.CallStatic<string[]>("GetPairedDevices"); ;


        devlist.Clear();

        foreach (var d in data)
        {
            if (d.Contains("CARCRASHER"))
            {
                devlist.Add(d);
            }
        }
    }

    // Start BT connect using device MAC address "deviceAdd"
    public static void StartConnection(string devmac)
    {
       if (Application.platform != RuntimePlatform.Android)
            return;

        BluetoothConnector.CallStatic("StartConnection", devmac.ToUpper());
    }

    // Stop BT connetion
    public static void StopConnection()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        if (isConnected)
            BluetoothConnector.CallStatic("StopConnection");
    }

    // This function will be called by Java class whenever BT data is received,
    // DO NOT CHANGE ITS NAME OR IT WILL NOT BE FOUND BY THE JAVA CLASS
    public static void ReadData(string data)
    {

        for(int i=0; (i<data.Length) && (dataindex < temprecdata.Length); dataindex++)
        {
            temprecdata[dataindex] = data[i];
            i++;
        }
        if (!new string(temprecdata).Contains("A"))
        {
            cleardat();
            WriteData("A00000E");
        }
        else
        {
            fulldata();
        }
        
    }

    public static void fulldata()
    {
        string temp = new string(temprecdata);
        if ((dataindex >= temprecdata.Length)&& !temp.Contains("E"))
        {
            cleardat();
            WriteData("A00000E");
            
        }
        else if (temp.Contains("E"))
        {
                receiveddata = temp.Split("A")[1].Split("E")[0];
            if (receiveddata.Length == 5)
            {
                WriteData("A00003E");
            }
            else
            {
                cleardat();
                WriteData("A00000E");
            }
        }
 

    }

    public static void cleardat()
    {
        dataindex = 0;
        receiveddata = "";
        for (int i = 0; i < temprecdata.Length; i++)
        {
            temprecdata[i] = '\0';
        }
    }

    // Write data to the connected BT device
    public static void WriteData(string data)
    {
        if (Application.platform != RuntimePlatform.Android)
           return;

        if (isConnected)
            BluetoothConnector.CallStatic("WriteData", data);
    }

    public static void Toast(string data)
    {
       if (Application.platform != RuntimePlatform.Android)
            return;

        BluetoothConnector.CallStatic("Toast", data);
    }

}

