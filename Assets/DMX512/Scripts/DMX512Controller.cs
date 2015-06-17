using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
	
public class DMX512Controller : MonoBehaviour {
	public string address = "255.255.255.255";
	public int port = 6454;
	public int universe = 0;

	public bool blackOut;
	
	private bool showSliders = true;
	
	// Debug Window	properties
	protected static int windowIDSeed = 20000;
	protected Rect windowRect = new Rect(160, 20, 120, 50);
	protected int windowId;
	public bool debugWindow = true;
	
	protected int page = 0;
	
	private byte[] channelData = new byte[512];
	private byte[] oldChannelData = new byte[512];

	private UdpClient udpClient = new UdpClient();

	public virtual void Awake() {
		windowRect.x = PlayerPrefs.GetFloat("dmx.window.pos." + gameObject.name + ".x", Screen.width - 400.0f);
		windowRect.y = PlayerPrefs.GetFloat("dmx.window.pos." + gameObject.name + ".y", 20.0f);
		debugWindow = 1 == PlayerPrefs.GetInt("dmx.window." + gameObject.name + ".debug", 1);
		showSliders = 1 == PlayerPrefs.GetInt("dmx.window." + gameObject.name + ".showSliders", 0);
		windowRect.width = 400;
		windowId = windowIDSeed;
		windowIDSeed++;
	}
	
	protected virtual void OnDrawGUIWindow(int windowID) {
		GUILayout.BeginVertical();
		
		GUILayout.BeginHorizontal();
		
		showSliders = GUILayout.Toggle(showSliders, "Show sliders");
		blackOut = GUILayout.Toggle(blackOut, "Black out");
		
		GUILayout.EndHorizontal();
		
		if (showSliders) {
			string[] pages= new string[8];
			for (int i = 0; i < pages.Length; i++) {
				pages[i] = string.Format("{0}-{1}", i * 64 + 1, i * 64 + 64);
			}
			page = GUILayout.SelectionGrid(page, pages, pages.Length);
			
			for (int i = 0; i < 2; i++) {
				GUILayout.BeginHorizontal();
				
				for (int j = 0; j < 32; j++) {
					float num = channelData[page * 64 + i * 32 + j] / 255.0f;
					num = GUILayout.VerticalSlider(num, 1.0f, 0.0f);
					channelData[page * 64 + i * 32 + j] = (byte)(num * 255.0f);
				}
				
				GUILayout.EndHorizontal();
			}
		}
		
		GUI.DragWindow();
		GUILayout.EndVertical();
	}
	
	public virtual void OnGUI() {
		if(debugWindow) {
			windowRect = GUILayout.Window(windowId, windowRect, OnDrawGUIWindow, name, GUILayout.Width(200));
		}
	}
	
	public virtual void OnApplicationQuit() {
		PlayerPrefs.SetFloat("dmx.window.pos." + gameObject.name + ".x", windowRect.x);
		PlayerPrefs.SetFloat("dmx.window.pos." + gameObject.name + ".y", windowRect.y);
		PlayerPrefs.SetInt("dmx.window." + gameObject.name + ".debug", (debugWindow ? 1 : 0));
		PlayerPrefs.SetInt("dmx.window." + gameObject.name + ".showSliders", (showSliders ? 1 : 0));
	}

	void Update () {
		if (!channelData.SequenceEqual(oldChannelData)) {
			ArtNetSend();
			
			Buffer.BlockCopy(channelData, 0, oldChannelData, 0, 512);
		}
	}

	void ArtNetSend () {
		byte[] dgram = new byte[512 + 18];
		dgram[0] = (byte)'A';
		dgram[1] = (byte)'r';
		dgram[2] = (byte)'t';
		dgram[3] = (byte)'-';
		dgram[4] = (byte)'N';
		dgram[5] = (byte)'e';
		dgram[6] = (byte)'t';
		dgram[7] = 0;
		dgram[8] = 0;
		dgram[9] = 0x50;
		dgram[10] = 0;
		dgram[11] = 14;
		dgram[12] = 0;
		dgram[13] = 0;
		dgram[14] = 0;
		dgram[15] = 0;
		dgram[16] = 2;
		dgram[17] = 0;
		
		Buffer.BlockCopy(channelData, 0, dgram, 18, 512);
		
		udpClient.Send(dgram, dgram.Length, address, port);
	}
/*
	void ESPSend () {
		byte[] dgram = new byte[512 + 9];
		dgram [0] = (byte)'E';
		dgram [1] = (byte)'S';
		dgram [2] = (byte)'D';
        dgram [3] = (byte)'D';
		dgram [4] = (byte)universe;
		dgram [5] = 0;
        dgram[6] = 1;
        dgram[7] = 2;
        dgram[8] = 0;

		Buffer.BlockCopy(channelData, 0, dgram, 9, 512);

		udpClient.Send(dgram, dgram.Length, address, port);

		dgram [0] = (byte)'E';
		dgram [1] = (byte)'S';
		dgram [2] = (byte)'P';
		dgram [3] = (byte)'P';
		dgram [4] = 1;

		udpClient.Send(dgram, 5, address, port);
	}
*/
}
