package abl.actions;

import org.json.simple.JSONObject;
import server.Message;
import server.TCPServer;

/**
 * Triggers the AggressiveConfrontationBehavior on a character in Unity.
 * Args:
 * [0] - int characterId
 * [1] - int targetId
 */
public class AggressiveConfrontation extends BaseAction {

    @Override
    public void execute(Object[] args) {
        int characterId = (int) args[0];
        int targetId = (int) args[1];

        int code = 2;
        String msg = "aggressiveConfrontation";

        JSONObject data = new JSONObject();
        data.put("characterId", characterId);
        data.put("targetId", targetId);

        Message toSend = new Message(code, msg, data);
        JSONObject jo = toSend.toJSON();
        TCPServer.getInstance().sendOutgoingMessage(jo);
    }
}