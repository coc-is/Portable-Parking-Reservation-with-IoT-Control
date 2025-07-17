using System.Collections;
using System.Collections.Generic;
using UnityEngine.Android;
using UnityEngine;
using UnityEngine.UI;

public class BluetoothManager : MonoBehaviour
{
    
    // This function will be called by Java class to update BT connection status,
    // DO NOT CHANGE ITS NAME OR IT WILL NOT BE FOUND BY THE JAVA CLASS
    public void ConnectionStatus(string status)
    {
        bt.Toast("Connection Status: " + status);
        bt.isConnected = status == "connected";
    }

    // This function will be called by Java class whenever BT data is received,
    // DO NOT CHANGE ITS NAME OR IT WILL NOT BE FOUND BY THE JAVA CLASS
    public void ReadData(string data)
    {
        bt.ReadData(data);
    }

   
}
