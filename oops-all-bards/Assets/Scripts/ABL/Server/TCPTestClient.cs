
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCPTestClient : MonoBehaviour {  	
	#region private members 	
	private TcpClient socketConnection; 	
	private Thread clientReceiveThread; 	
	#endregion  	
	// Use this for initialization 	
	void Start () {
		ConnectToTcpServer();
	}  	
	// Update is called once per frame	
	/// <summary> 	
	/// Setup socket connection. 	
	/// </summary> 	
	private void ConnectToTcpServer () { 		
		try {  			
			clientReceiveThread = new Thread (new ThreadStart(ListenForData)); 			
			clientReceiveThread.IsBackground = true; 			
			clientReceiveThread.Start();  		
		} 		
		catch (Exception e) { 			
			Debug.Log("On client connect exception " + e); 		
		} 	
	}  	
	/// <summary> 	
	/// Runs in background clientReceiveThread; Listens for incomming data. 	
	/// </summary>     
	private void ListenForData() { 		
		try { 			
			socketConnection = new TcpClient("localhost", 5000);  			
			Byte[] bytes = new Byte[1024];             
			while (true) { 				
				// Get a stream object for reading 				
				using (NetworkStream stream = socketConnection.GetStream()) { 					
					int length; 					
					// Read incomming stream into byte arrary. 					
					while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) 
					{ 						
						var incomingData = new byte[length]; 						
						Array.Copy(bytes, 0, incomingData, 0, length);

						// TODO: ADD MROE DOCS
						string result = System.Text.Encoding.UTF8.GetString(incomingData);
						ABLResponse response = JsonUtility.FromJson<ABLResponse>(result);
						Debug.Log(response.code);
						Debug.Log(response.msg);
						Debug.Log(response.data);
						SendMessage(response);
					} 				
				} 			
			}         
		}         
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		}     
	}	
	/// <summary> 	
	/// Send message to server using socket connection. 	
	/// </summary> 	
	// TODO: Change out the ALBResposne to something more generic.
	private void SendMessage(ABLResponse response) { 
		// TODO: Send WME to ABL server.
		// TODO: SEND OVER THE BOTWME TO ABL SERVER.        
		if (socketConnection == null) {             
			return;         
		}  	
		try { 			
			// Get a stream object for writing. 			
			NetworkStream stream = socketConnection.GetStream(); 			
			if (stream.CanWrite) {                 
								
				string json = JsonUtility.ToJson(response);
				// Convert string message to byte array.                 
				//	The reason for the newline is because of how Java's readLine
				//	works. It needs a newline to consider the stream complete.
				byte[] msgAsByteArray = Encoding.ASCII.GetBytes(json+'\n'); 				
				// Write byte array to socketConnection stream.                 
				stream.Write(msgAsByteArray, 0, msgAsByteArray.Length);                 
				Debug.Log("Client sent their message - should be received by server");             
			}         
		} 		
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		}     
	}

	// public void CreateTestAllyWME() 
	// {

	// } 
}