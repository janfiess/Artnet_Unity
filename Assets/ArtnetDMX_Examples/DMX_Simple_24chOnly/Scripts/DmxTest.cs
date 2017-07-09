/*
	This script sends artnet signals to control DMX devices.
	Press UI Buttons
	script attached to the Artnet_Ctrl gameobject which is always available in scene
 */

using UnityEngine;
using System.Collections;


public class DmxTest : MonoBehaviour {
	Art_Net artnet;
	

	void Start(){
		artnet = GetComponent<Art_Net>();

		// set all channels to black
		for (int i = 0; i < 512; i++) {
			artnet.send (i, 0);
		}
	}

	// Test light: if channel 19 (ChNr) is selected at the light, 
	// the master fader is ChNr + 3 (-> DMX 22)
	// R: ChNr + 4 (-> DMX 23),  G: ChNr + 5 (-> DMX 24),  B: ChNr + 6 (-> DMX 25)


	// UI Buttons
	public void TurnRed(){
		artnet.send (21, (byte) 255); // DMX 22
		artnet.send (22, (byte) 255); // DMX 23   (-> R)
		artnet.send (23, (byte) 0); // DMX 24   (-> G)
		artnet.send (24, (byte) 0); // DMX 25   (-> B ??? )
	}

	public void TurnBlue(){
		artnet.send (21, (byte) 255); // DMX 22
		artnet.send (22, (byte) 0); // DMX 23   (-> R)
		artnet.send (23, (byte) 0); // DMX 24   (-> G)
		artnet.send (24, (byte) 255); // DMX 25   (-> B ??? )
	}
}