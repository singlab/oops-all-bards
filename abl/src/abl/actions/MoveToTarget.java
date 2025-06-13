// Place this in your abl.actions package
package abl.actions;

import org.json.simple.JSONObject;
import server.Message;
import server.TCPServer;

/**
 * Triggers a character to move towards a specific target entity in Unity.
 * Args:
 * [0] - int characterId (the actor)
 * [1] - int targetId (the entity to move towards)
 */
public class MoveToTarget extends BaseAction {

    @Override
    public void execute(Object[] args) {
        int characterId = (int) args[0];
        int targetId = (int) args[1];
        int code = 2;
        String msg = "moveToTarget";

        JSONObject data = new JSONObject();
        data.put("characterId", characterId);
        data.put("targetId", targetId);

        Message toSend = new Message(code, msg, data);
        JSONObject jo = toSend.toJSON();
        TCPServer.getInstance().sendOutgoingMessage(jo);
    }
}