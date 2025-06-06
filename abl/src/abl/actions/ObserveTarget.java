package abl.actions;

import org.json.simple.JSONObject;
import server.Message;
import server.TCPServer;

/**
 * Triggers the ObserveBehavior on a character in Unity.
 * Args:
 * [0] - int characterId
 * [1] - int targetId
 * [2] - float duration
 */
public class ObserveTarget extends BaseAction {

    @Override
    public void execute(Object[] args) {
        int characterId = (int) args[0];
        int targetId = (int) args[1];
        float duration = ((Number) args[2]).floatValue();

        int code = 2;
        String msg = "observeTarget";

        JSONObject data = new JSONObject();
        data.put("characterId", characterId);
        data.put("targetId", targetId);
        data.put("duration", duration);

        Message toSend = new Message(code, msg, data);
        JSONObject jo = toSend.toJSON();
        TCPServer.getInstance().sendOutgoingMessage(jo);
    }
}