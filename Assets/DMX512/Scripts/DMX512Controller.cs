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

	public byte[] data = new byte[512];
	private byte[] oldData = new byte[512];

	private UdpClient udpClient = new UdpClient();

	void Update () {
		if (!data.SequenceEqual(oldData)) {
			ArtNetSend();
			
			Buffer.BlockCopy(data, 0, oldData, 0, 512);
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
		dgram[14] = (byte)(universe & 0xff);
		dgram[15] = (byte)((universe >> 8) & 0x7f);
		dgram[16] = 2;
		dgram[17] = 0;
		
		Buffer.BlockCopy(data, 0, dgram, 18, 512);
		
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
