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
public class GameAgent_MentalStepExecute {
   static public void mentalExecute0(int __$stepID, final Object[] __$behaviorFrame, final BehavingEntity __$thisEntity, MentalStep __$thisStep) {
      switch (__$stepID) {
         case 0: {
            // lookForAllyAgent_1Step1
            System.out.println("Looking for ally agent...");
            break;
         }
         case 3: {
            // lookForAllyAgent_1Step4
            System.out.println("Found new AllyWME");
            ((AllyWME)__$behaviorFrame[0]).setOnTree(true);
            break;
         }
         case 9: {
            // waitForTurn_1Step1
            System.out.println("I am waiting to take my turn.");
            break;
         }
         case 13: {
            // lookToAssist_1Step1
            System.out.println("I am looking to assist.");
            break;
         }
         case 17: {
            // takeTurn_1Step1
            System.out.println("I am taking my turn.");
            break;
         }
         case 18: {
            // assistAlly_1Step1
            System.out.println("I am going to assist.");
            break;
         }
         case 19: {
            // GameAgent_RootCollectionBehaviorStep1
            System.out.println("Starting game agent...");
            break;
         }
      default:
         throw new AblRuntimeError("Unexpected stepID " + __$stepID);
      }
   }
}
