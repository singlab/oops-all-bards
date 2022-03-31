package abl.actions;

import abl.runtime.PrimitiveAction;
/**
 * For this toy domain, all actions complete immediately. 
 * 
 * @author Ben Weber 3-7-11
 */
public abstract class BaseAction extends PrimitiveAction {

	/**  
	 * Performs the physcial act. 
	 */
	abstract public void execute(Object[] args);

	/**
	 * Returns that the action has completed successfully. 
	 */
    public synchronized int getCompletionStatus() {
    	return completionStatus = SUCCESS;
    }
}
