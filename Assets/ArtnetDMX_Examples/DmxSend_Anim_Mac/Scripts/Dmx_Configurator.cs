/*
	This script reads the color values of Unity software lights in realtime (Update function) 
	and translates the RGB values to DMX devices
	The Unity software lights' colors are driven by the Animator
 */


using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ArtNet;
using UnityEngine.UI;

public class Dmx_Configurator : MonoBehaviour
{
    public byte[] DMXData = new byte[512];
    public InputField ip_textfield;

    ArtNet.Engine ArtEngine;
	Animator animator;

	// LED1
	public Light softwareLight1; // this color will be transmitted to LED 1 (and 2)
	public GameObject softwareLight1_debug;
	int port_led_1_master = 21; // DMX22
	int port_led_1_r = 22; // DMX23
	int port_led_1_g = 23; // DMX24
	int port_led_1_b = 24; // DMX25 (-> B ??? Channel 25 and up cause issues)
	Color prevLed_1_Color;


	// Shaker
	public Light shaker; 
	public GameObject shaker_debug;
	int port_shaker = 0;            // DMX 1
	Color prevShaker_Value;



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

		animator = GetComponent<Animator> ();
    }

		// UI buttons
	public void DMX_RightCombination(){
		
		animator.SetTrigger ("trg_RightCombination");
	}

	public void DMX_WrongCombination(){
		animator.SetTrigger ("trg_WrongCombination");
	}

	void Update(){
	// First change DMX array values (DMX Port, value [0..255]));
	// Then send it away.
	// Pick the current color of the light in the scene and send it to real LEDs via Artnet

		if (softwareLight1.color != prevLed_1_Color) {
			// 1 RGB LED needs 3 DMX addresses for each channel 
			DMXData[port_led_1_master] = (byte)(255);
			DMXData[port_led_1_r] = (byte)(softwareLight1.color.r * 255);
			DMXData[port_led_1_g] = (byte)(softwareLight1.color.g * 255);
			DMXData[port_led_1_b] = (byte)(softwareLight1.color.b * 255);

			// see color also on a gameobject in scene
			softwareLight1_debug.GetComponent<Renderer>().material.color = softwareLight1.color;
			prevLed_1_Color = softwareLight1.color;
		}

		if (shaker.color != prevShaker_Value) {
			DMXData[port_shaker] = (byte)(shaker.color.a * 255);

			// see color also on a gameobject in scene
			shaker_debug.GetComponent<Renderer>().material.color = shaker.color;
			prevShaker_Value = shaker.color;
		}


		ArtEngine.SendDMX(0, DMXData, DMXData.Length);
	}

}









// using UnityEngine;
// using System.Collections;

// public class Dmx_Configurator : MonoBehaviour {
// 	Artnett artnet;
// 	Animator animator;
	

// 	public Light softwareLight1; // this color will be transmitted to LED 1 (and 2)
// 	public GameObject softwareLight1_debug;

// 	// DMX ports of RGB LED 1 (gets the color of softwareLight1)
// 	int port_led_1_r = 22; // DMX23
// 	int port_led_1_g = 23; // DMX24
// 	int port_led_1_b = 24; // DMX25 (-> B ??? Channel 25 and up cause issues)
// 	Color prevLed_1_Color;



// 	public Light shaker; 
// 	public GameObject shaker_debug;
// 	// DMX port of the shaker
// 	int port_shaker = 0;            // DMX 1
// 	Color prevShaker_Value;


// 	void Start(){
// 		artnet = GetComponent<Artnett>();
// 		animator = GetComponent<Animator> ();

// 		// set all channels to black
// 		for (int i = 0; i < 512; i++) {
// 			artnet.send (i, 0);
// 		}

// 		artnet.send (21, (byte) 255); // DMX 22
// 		// Test light: if channel 19 (ChNr) is selected at the light, 
// 		// the master fader is ChNr + 3 (-> DMX 22)
// 		// R: ChNr + 4 (-> DMX 23),  G: ChNr + 5 (-> DMX 24),  B: ChNr + 6 (-> DMX 25)
// 	}

// 	// UI buttons
// 	public void DMX_RightCombination(){
		
// 		animator.SetTrigger ("trg_RightCombination");
// 		print("doit");
// 	}

// 	public void DMX_WrongCombination(){
// 		animator.SetTrigger ("trg_WrongCombination");
// 	}
	


// 	void Update () {
// //		control LED 1
// 	// artnet.send (DMX Port, value [0..255]));
// 	// Pick the current color of the light in the scene and send it to real LEDs via Artnet

// 		if (softwareLight1.color != prevLed_1_Color) {
// 			// 1 RGB LED needs 3 DMX addresses for each channel 
// 			artnet.send (port_led_1_r, (byte)(softwareLight1.color.r * 255));
// 			artnet.send (port_led_1_g, (byte)(softwareLight1.color.g * 255));
// 			artnet.send (port_led_1_b, (byte)(softwareLight1.color.b * 255));

// 			// see color also on a gameobject in scene
// 			softwareLight1_debug.GetComponent<Renderer>().material.color = softwareLight1.color;
			
// 			prevLed_1_Color = softwareLight1.color;
// 		}

// 	//		control shaker
// 		if (shaker.color != prevShaker_Value) {
// 			// 1 RGB LED needs 3 DMX addresses for each channel 
// 			artnet.send (port_led_1_r, (byte)(softwareLight1.color.r * 255));
// 			artnet.send (port_led_1_g, (byte)(softwareLight1.color.g * 255));
// 			artnet.send (port_led_1_b, (byte)(softwareLight1.color.b * 255));

// 			artnet.send (port_shaker, (byte)(shaker.color.r * 255));

// 			// see color also on a gameobject in scene
// 			softwareLight1_debug.GetComponent<Renderer>().material.color = softwareLight1.color;
// 			shaker_debug.GetComponent<Renderer>().material.color = shaker.color;
			
// 			prevShaker_Value = shaker.color;
// 		}
// 	}
	
// }