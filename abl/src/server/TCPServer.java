package server;

import java.lang.reflect.Field;


import java.io.*;
import java.net.*;
import java.nio.charset.StandardCharsets;
import java.util.Date;

import org.json.simple.JSONArray;
import org.json.simple.JSONObject;
import org.json.simple.JSONValue;

import abl.generated.GameAgent;
/**
 * This program demonstrates a simple TCP/IP socket server.
 *
 * @author www.codejava.net
 * ADD more docs if need be?
 */


public class TCPServer {
	private long data = -1;
	
	
	private static TCPServer server;
	
	public static TCPServer getInstance() {
		return server;
	}
	
	
	static public class Response {
		public int code;
		public String msg;
		public String data;
		
		public Response(int code, String msg, String data) {
			this.code = code;
			this.msg = msg;
			this.data = data;
		}
		
		private JSONObject toJSON() {
			JSONObject jo = new JSONObject();
			jo.put("code", code);
			jo.put("msg", msg);
			jo.put("data", data);
			return jo;
		}
	}
	
	static class Message {
		public int code;
		public String msg;
		public String data;
	
		public Message(int code, String msg, String data) {
			this.code = code;
			this.msg = msg;
			this.data = data;
		}
		private JSONObject toJSON() {
			JSONObject jo = new JSONObject();
			jo.put("code", code);
			jo.put("msg", msg);
			jo.put("data", data);
			return jo;
		}
	}
	
	public void startAgent() {
		GameAgent agent = new GameAgent();
		//agent.startBehaving();
	}
	void startServer() {
		
//    	new Thread() {
//    		public void run() {
//    			while (true) {
//    				try {
//    					startAgent();
//    					Thread.sleep(50);
//    				}
//    				catch (Exception e) {}
//    			}
//    		}
//    	}.start();
		
		int port = 5000;
        try (ServerSocket serverSocket = new ServerSocket(port)) {
        	
        	Response res = new Response(0, "success", "data");
        	JSONObject jo = res.toJSON();

            System.out.println("Server is listening on port " + port);
            while (true) {
                Socket socket = serverSocket.accept();
 
                System.out.println("New client connected");
 
                InputStream input = socket.getInputStream();
                BufferedReader in = new BufferedReader(new InputStreamReader(input));
  
                OutputStream output = socket.getOutputStream();
                PrintWriter writer = new PrintWriter(output, true);
                writer.println(jo);
                String line = "";
                while ((line = in.readLine()) != null) {
                	JSONObject obj = (JSONObject) JSONValue.parse(line);
                	System.out.println(obj.get("code"));
                	System.out.println(obj.get("msg"));
                	System.out.println(obj.get("data"));     
                	// TODO: Figure out how ABL passes in strings so 
                	// We can work with strings rather than just primitives.
                	this.data = (long) obj.get("code");
                }
 
            }
 
        } catch (IOException ex) {
            System.out.println("Server exception: " + ex.getMessage());
            ex.printStackTrace();
        }
	}
	
	// TODO: We are transforming multiple times, probably too much.
    public static void main(String[] args) {
    	server = new TCPServer();
    	server.startServer();

    }
    //TODO: Maybe add a setter. 
    // Also how to scale this for more than one WME type?
    public long getTestWME() {
    	return this.data;
    }
}