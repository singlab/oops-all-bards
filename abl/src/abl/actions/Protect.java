package abl.actions;

import org.json.simple.JSONObject;

import server.Message;
import server.TCPServer;

// An acting character protects a target character by taking damage for them.
// Args [0] -- int actingCharacter (id field in wme)
// 		[1] -- int targetCharacter (id field in wme)
public class Protect extends BaseAction {

	@Override
	public void execute(Object[] args) {
		System.out.println("Acting character: " + args[0]);
		System.out.println("Target character: " + args[1]);
		// Codes: 1 -- combat action
		//		  2 -- noncombat action
		int code = 1;
		// Msg field must match name of java class in String format
		String msg = "Protect";
		JSONObject data = new JSONObject();
		data.put("actingCharacter", args[0]);
		data.put("targetCharacter", args[1]);
		
		
		Message toSend = new Message(code, msg, data);
		JSONObject jo = toSend.toJSON();
		TCPServer.getInstance().sendOutgoingMessage(jo);
	}
}
