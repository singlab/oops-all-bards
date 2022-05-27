package abl.actions;

import org.json.simple.JSONObject;

import server.Message;
import server.TCPServer;

// An acting character protects a target character by taking damage for them.
// Args [0] -- int actingCharacter (id field in wme)
public class RequestAssistance extends BaseAction {

	@Override
	public void execute(Object[] args) {
		System.out.println("Acting character: " + args[0]);
		// Codes: 1 -- combat action
		//		  2 -- noncombat action
		// 		  3 -- dialogue
		int code = 1;
		// Msg field must match name of java class in String format
		String msg = "RequestAssistance";
		JSONObject data = new JSONObject();
		data.put("actingCharacter", args[0]);
		
		Message toSend = new Message(code, msg, data);
		JSONObject jo = toSend.toJSON();
		TCPServer.getInstance().sendOutgoingMessage(jo);
	}
}
