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
public class GameAgent_ArgumentStepExecute {
   static public Object[] argumentExecute0(int __$stepID, final Object[] __$behaviorFrame, final BehavingEntity __$thisEntity) {
      switch (__$stepID) {
         case 1: {
            // lookForAllyAgent_1Step2
            final Object[] args = new Object[2];
            args[0] = ((ParallelBehaviorWME)__$behaviorFrame[1]);
            args[1] = ((AllyWME)__$behaviorFrame[0]);
            return args;
         }
         case 4: {
            // manageAllyAgent_1Step1
            final Object[] args = new Object[1];
            args[0] = ((AllyWME)__$behaviorFrame[0]);
            return args;
         }
         case 6: {
            // manageCombat_1Step2
            final Object[] args = new Object[1];
            args[0] = ((AllyWME)__$behaviorFrame[0]);
            return args;
         }
         case 7: {
            // manageCombat_1Step3
            final Object[] args = new Object[1];
            args[0] = ((AllyWME)__$behaviorFrame[0]);
            return args;
         }
         case 10: {
            // waitForTurn_1Step3
            final Object[] args = __$behaviorFrame;
            return args;
         }
         case 11: {
            // waitForTurn-4->AnonymousStep10_1Step1
            final Object[] args = new Object[1];
            args[0] = ((AllyWME)__$behaviorFrame[0]);
            return args;
         }
         case 14: {
            // lookToAssist_1Step3
            final Object[] args = __$behaviorFrame;
            return args;
         }
         case 15: {
            // lookToAssist-6->AnonymousStep14_1Step1
            final Object[] args = new Object[2];
            args[0] = ((AllyWME)__$behaviorFrame[0]);
            args[1] = ((AllyWME)__$behaviorFrame[1]);
            return args;
         }
      default:
         throw new AblRuntimeError("Unexpected stepID " + __$stepID);
      }
   }
}
