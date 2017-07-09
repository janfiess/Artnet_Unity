using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using UnityEngine.UI;

public class Art_Net : MonoBehaviour {
	private byte [] UDP;
	public InputField ip_textfield;


	// send DMX

	
	string targetIP = "192.168.0.99";
	private IPAddress target_IP;
	private int port = 6454;

	private IPEndPoint ipEndPoint;


	// receive DMX

	private Thread udpThread;
	private UdpClient udpClient;
	private bool running;
	private int universe = 0;


	void OnEnable () {
		targetIP = ip_textfield.text;
		if(ip_textfield.text == "") targetIP = ip_textfield.placeholder.GetComponent<Text>().text;
		// print own IP address
		Debug.Log (Network.player.ipAddress);

		// send DMX
		target_IP = IPAddress.Parse(targetIP);
	    ipEndPoint = new IPEndPoint (target_IP, port);

		// put Artnet header into UDP frame
		UDP = createEmptyArtnetPackage ();

		// receive DMX
		running = true;
		udpThread = new Thread (new ThreadStart(receive));
		udpThread.Start ();
	}

	// send DMX
	public byte getVal(int channel){
		return UDP [channel + 18];
	}

	/****************************************************************************
	 * send a single DMX value  
	 ****************************************************************************/

	public void send(int channel, byte value){
		UDP [channel + 18] = value;
	 
		Socket myClient = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

		myClient.SendTo (UDP, ipEndPoint);
		myClient.Close ();
	}

	/****************************************************************************
	 * send a whole received DMX package  
	 ****************************************************************************/

	public void send(byte [] values){
		Socket myClient = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

		myClient.SendTo (UDP, ipEndPoint);
		myClient.Close ();
	}


	public byte [] createEmptyArtnetPackage(){
		byte [] artnetFrame = new byte[530];

		// start value 0 for each channel in the UDP frame
		for (int i = 0; i < 530; i++) {
			artnetFrame [i] = 0;
		}

		artnetFrame [0] = 65;
		artnetFrame [1] = 114;
		artnetFrame [2] = 116;
		artnetFrame [3] = 45;
		artnetFrame [4] = 78;
		artnetFrame [5] = 101;
		artnetFrame [6] = 116;
		artnetFrame [7] = 0;
		artnetFrame [8] = 0;
		artnetFrame [9] = 80;
		artnetFrame [10] = 0;
		artnetFrame [11] = 14;
		artnetFrame [12] = 84;
		artnetFrame [13] = 0;
		artnetFrame [14] = 0;
		artnetFrame [15] = 0;
		artnetFrame [16] = 0;
		artnetFrame [17] = 24;
		return artnetFrame;
	}

	/****************************************************************************
	* receive DMX
	****************************************************************************/
	private void receive(){
		IPEndPoint RemoteIpEndPoint;

		try{
			udpClient = new UdpClient(port);
			RemoteIpEndPoint = new IPEndPoint(IPAddress.Any,0);
			while(running){
				if(udpClient.Available > 500){
					byte[] data;
					data = udpClient.Receive(ref RemoteIpEndPoint);

					if(data.Length >= 530){
						if(universe == data[14]){
							// Check if the incoming UDP package contains Artnet data (Thefirst 18 Bytes are equal in every Artnet package - the header)
							if(data [0] == 65 && data [1] == 114 && data [2] == 116 && data [3] == 45 && data [4] == 78 && data [5] == 101 
								&& data [6] == 116 && data [7] == 0 && data [8] == 0 && data [9] == 80 && data [10] == 0 && data [11] == 14 
								&& data [13] == 0 && data [14] == 0 && data [15] == 0 && data [16] == 0 && data [17] == 24 ){
					
								for(int i = 0;i < 512;i++){
									UDP[i + 18] = data[i+18];
								}
								send(UDP);
							}
						}
					}

					Thread.Sleep(20);
				}
				else{
					Thread.Sleep(20);
					// Debug.Log("No Message received");
				}
			}
		}
		catch{
			Debug.Log ("Port 6454 already reserved.");
		}
	}


	void OnApplicationQuit(){
		Debug.Log ("Quit Playmode");

		if (udpThread != null) {
			udpThread.Abort ();
		}
		if (udpClient != null) {
			udpClient.Close ();
		}
	}
}

// Inspiration zu UDP Send: https://www.youtube.com/watch?v=XflPAypxLwo