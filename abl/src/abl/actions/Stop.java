package abl.actions;

import game.Bot;
import game.GameEngine;

import java.awt.Point;
/**
 * Stops the chaser. 
 * 
 * @author Ben Weber 3-7-11
 */
public class Stop extends BaseAction {

	/**
	 * Stops the chaser.
	 * args[0] - bot id
	 */
	public void execute(Object[] args) {
		for(Bot b:GameEngine.getInstance().getBots()) {
			if(b.getId() == (int)args[0]) {
				b.setTrajectory(new Point(0, 0));
			}
		}
	}
}
