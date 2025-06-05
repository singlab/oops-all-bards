package abl.actions;

import org.json.simple.JSONObject;
import server.Message;
import server.TCPServer;

/**
 * Commands a character to request assistance.
 * Args:
 * [0] - int characterId (the actor)
 */
public class RequestAssistance extends BaseAction {

	@Override
	public void execute(Object[] args) {
		int characterId = (int) args[0];

		int code = 1;
		String msg = "RequestAssistance";

		JSONObject data = new JSONObject();
		data.put("characterId", characterId);

		Message toSend = new Message(code, msg, data);
		JSONObject jo = toSend.toJSON();
		TCPServer.getInstance().sendOutgoingMessage(jo);
	}
}