package abl.generated;

import abl.runtime.*;
import wm.WME;
import wm.WorkingMemorySet;
import wm.WMEIndex;
import wm.TrackedWorkingMemory;
import java.util.*;
import java.lang.reflect.Method;
import java.lang.reflect.Field;
import abl.learning.*;
import abl.wmes.*;
import abl.actions.*;
import abl.sensors.*;
import abl.util.*;
public class GameAgent_MentalStepExecute {
   static public void mentalExecute0(int __$stepID, final Object[] __$behaviorFrame, final BehavingEntity __$thisEntity, MentalStep __$thisStep) {
      switch (__$stepID) {
         case 0: {
            // lookForAllyAgent_1Step1
            System.out.println("Looking for ally agent...");
            break;
         }
         case 2: {
            // lookForAllyAgent_1Step3
            System.out.println("AllyWME has been added to memory. Setting on tree.");
            ((AllyWME)__$behaviorFrame[0]).setOnTree(true);
            break;
         }
         case 4: {
            // lookForAllyAgent-0->ConditionalStep3_IF_MentalStep_GoalStep_1Step1
            System.out.println("New ally being added to ally manager.");
            break;
         }
         case 6: {
            // lookForAllyAgent_1Step5
            System.out.println("Updating ally wme in dictionary.");
            ((GameAgent)__$thisEntity).dict.addCharacter(((AllyWME)__$behaviorFrame[0]).getID() , ((AllyWME)__$behaviorFrame[0]));
            break;
         }
         case 10: {
            // manageCombat_1Step2
            System.out.println("Ally is in combat.");
            break;
         }
         case 13: {
            // waitForTurn_1Step1
            System.out.println("I am waiting to take my turn.");
            break;
         }
         case 16: {
            // lookToAssist_1Step1
            System.out.println("I am looking to assist.");
            break;
         }
         case 19: {
            // takeTurn_1Step1
            System.out.println("I am taking my turn.");
            break;
         }
         case 20: {
            // assistAlly_1Step1
            ((__ValueTypes.IntVar)__$behaviorFrame[2]).i = ((AllyWME)__$behaviorFrame[0]).getID();
            ((__ValueTypes.IntVar)__$behaviorFrame[3]).i = ((AllyWME)__$behaviorFrame[1]).getID();
            System.out.println("I am assisting.");
            break;
         }
         case 22: {
            // GameAgent_RootCollectionBehaviorStep1
            System.out.println("Starting game agent...");
            break;
         }
      default:
         throw new AblRuntimeError("Unexpected stepID " + __$stepID);
      }
   }
}