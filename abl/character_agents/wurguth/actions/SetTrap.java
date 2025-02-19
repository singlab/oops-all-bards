package abl.actions;

import org.json.simple.JSONObject;
import server.Message;
import server.TCPServer;

//Wurguth sets a trap at the provided location.
//Args:
// [0] - int characterId (Wurguth's ID)
// [1] - String trapLocation (The name of the location)
public class SetTrap extends BaseAction
{
	@Override
    public void execute(Object[] args) {
        int characterId = (int) args[0];
		String trapLocation = (String) args[1];

        System.out.println("Wurguth (ID " + characterId + ") is setting a trap at " + trapLocation);

        int code = 2; // Non-combat action
        String msg = "SetTrap";
        JSONObject data = new JSONObject();
        data.put("characterId", characterId);
		data.put("location", trapLocation);
		//Could add more to send here.

        Message toSend = new Message(code, msg, data);
        JSONObject jo = toSend.toJSON();
        TCPServer.getInstance().sendOutgoingMessage(jo);
    }
}
