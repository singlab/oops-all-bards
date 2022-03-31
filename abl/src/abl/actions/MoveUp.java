package abl.actions;

import game.Bot;
import game.GameEngine;

import java.awt.Point;
/**
 * Sets the trajectory of the chaser. 
 * 
 * @author Ben Weber 3-7-11
 */
public class MoveUp extends BaseAction {

	/**
	 * Sets the trajectory of the player to move up.
	 * args[0] - bot id
	 */
	public void execute(Object[] args) {
		for(Bot b:GameEngine.getInstance().getBots()) {
			if(b.getId() == (Integer)args[0]) {
				b.setTrajectory(new Point(0, -GameEngine.BotSpeed));
			}
		}
	}
}
