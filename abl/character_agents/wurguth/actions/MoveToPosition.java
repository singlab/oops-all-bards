package abl.actions;

import org.json.simple.JSONObject;
import server.Message;
import server.TCPServer;

/**
 * Triggers the MoveBehavior on a character in Unity.
 * Args:
 * [0] - int characterId
 * [1] - float x
 * [2] - float y
 * [3] - float z
 */
public class MoveToPosition extends BaseAction {

    @Override
    public void execute(Object[] args) {
        int characterId = (int) args[0];
        // Note: ABL numbers can be Long or Double, casting is important.
        float x = ((Number) args[1]).floatValue();
        float y = ((Number) args[2]).floatValue();
        float z = ((Number) args[3]).floatValue();

        int code = 2;
        String msg = "moveToPosition";

        JSONObject data = new JSONObject();
        data.put("characterId", characterId);
        data.put("x", x);
        data.put("y", y);
        data.put("z", z);

        Message toSend = new Message(code, msg, data);
        JSONObject jo = toSend.toJSON();
        TCPServer.getInstance().sendOutgoingMessage(jo);
    }
}
