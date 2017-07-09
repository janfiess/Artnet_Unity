using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ArtNet;
using UnityEngine.UI;

public class DMXout : MonoBehaviour
{
    public byte[] DMXData = new byte[512];
    public InputField ip_textfield;

    ArtNet.Engine ArtEngine;

    void Start()
    {
        for (int i = 0; i < DMXData.Length; i++)
        {
            DMXData[i] = (byte)(0);
        }

        // Artnet sender / client
        string ipAddress = ip_textfield.text;
        if(ip_textfield.text == "") ipAddress = ip_textfield.placeholder.GetComponent<Text>().text;
        print(ipAddress);
        ArtEngine = new ArtNet.Engine("Open DMX Etheret", ipAddress);
        ArtEngine.Start();
    }

	// UI Button
	public void TurnRed(){
		DMXData[21] = (byte)(255);
        DMXData[22] = (byte)(255);
        DMXData[23] = (byte)(0);
        DMXData[24] = (byte)(0);
        print("TurnRed");
	}

	// UI Button
	public void TurnBlue(){
		DMXData[21] = (byte)(255);
        DMXData[22] = (byte)(0);
        DMXData[23] = (byte)(0);
        DMXData[24] = (byte)(255);
        print("TurnBlue");
	}
    void Update()
    {
        sendDmxData();
    }

    void sendDmxData()
    {
        ArtEngine.SendDMX(0, DMXData, DMXData.Length);
    }
}