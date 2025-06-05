package abl.actions;

import org.json.simple.JSONObject;
import server.Message;
import server.TCPServer;

/**
 * Commands a character to protect an ally in combat.
 * Args:
 * [0] - int characterId (the actor)
 * [1] - int targetId (the character to protect)
 */
public class Protect extends BaseAction {

	@Override
	public void execute(Object[] args) {
		int characterId = (int) args[0];
		int targetId = (int) args[1];

		int code = 1;
		String msg = "Protect";

		JSONObject data = new JSONObject();
		data.put("characterId", characterId);
		data.put("targetId", targetId);

		Message toSend = new Message(code, msg, data);
		JSONObject jo = toSend.toJSON();
		TCPServer.getInstance().sendOutgoingMessage(jo);
	}
}
