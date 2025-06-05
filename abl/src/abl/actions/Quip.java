package abl.actions;

import org.json.simple.JSONObject;
import server.Message;
import server.TCPServer;

/**
 * Commands a character to make a quip.
 * Args:
 * [0] - int characterId (the actor)
 * [1] - bool inCombat
 */
public class Quip extends BaseAction {

	@Override
	public void execute(Object[] args) {
		int characterId = (int) args[0];
		boolean inCombat = (boolean) args[1];

		int code = 3;
		String msg = "Quip";

		JSONObject data = new JSONObject();
		data.put("characterId", characterId);
		data.put("inCombat", inCombat);

		Message toSend = new Message(code, msg, data);
		JSONObject jo = toSend.toJSON();
		TCPServer.getInstance().sendOutgoingMessage(jo);
	}
}