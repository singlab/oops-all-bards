package abl.actions;

import org.json.simple.JSONObject;
import server.Message;
import server.TCPServer;

// Wurguth questions an NPC.
// Args:
//  [0] - int characterId (Wurguth's ID)
//  [1] - String currentNpc (The name of the NPC to question)
public class QuestionLocals extends BaseAction {

    @Override
    public void execute(Object[] args) {
        int characterId = (int) args[0];
        String currentNpc = (String) args[1];

        System.out.println("Wurguth (ID " + characterId + ") is questioning NPC: " + currentNpc);

        // 1 -- combat action, 2 -- noncombat action
        int code = 2; // Assuming questioning is a non-combat action
        String msg = "QuestionLocals"; // Class name
        JSONObject data = new JSONObject();
        data.put("characterId", characterId);
        data.put("npcName", currentNpc);
        //Could add more to send here.

        Message toSend = new Message(code, msg, data);
        JSONObject jo = toSend.toJSON();
        TCPServer.getInstance().sendOutgoingMessage(jo);

        // In Unity, you would:
        // 1. Receive this message.
        // 2. Find the Wurguth GameObject (using characterId).
        // 3. Find the NPC GameObject (using npcName).
        // 4. Trigger a dialogue interaction between Wurguth and the NPC.
        //    This might involve:
        //    - Playing a "questioning" animation on Wurguth.
        //    - Starting a dialogue tree with the NPC.
        //    - The dialogue tree would likely involve skill checks
        //      and potentially update Wurguth's DELPEntity based on
        //      the NPC's responses (adding facts like knowsAbout(npc, player)
        //      or believesThreat(wurguth, player)).
    }
}
