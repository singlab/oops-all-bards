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
public class GameAgent_ArgumentStepExecute {
   static public Object[] argumentExecute0(int __$stepID, final Object[] __$behaviorFrame, final BehavingEntity __$thisEntity) {
      switch (__$stepID) {
         case 5: {
            // lookForAllyAgent-0->ConditionalStep3_IF_MentalStep_GoalStep_1Step2
            final Object[] args = new Object[2];
            args[0] = ((ParallelBehaviorWME)__$behaviorFrame[1]);
            args[1] = ((AllyWME)__$behaviorFrame[0]);
            return args;
         }
         case 8: {
            // manageAllyAgent_1Step1
            final Object[] args = new Object[1];
            args[0] = ((AllyWME)__$behaviorFrame[0]);
            return args;
         }
         case 11: {
            // manageCombat_1Step3
            final Object[] args = new Object[1];
            args[0] = ((AllyWME)__$behaviorFrame[0]);
            return args;
         }
         case 12: {
            // manageCombat_1Step4
            final Object[] args = new Object[1];
            args[0] = ((AllyWME)__$behaviorFrame[0]);
            return args;
         }
         case 15: {
            // waitForTurn_1Step3
            final Object[] args = new Object[1];
            args[0] = ((AllyWME)__$behaviorFrame[0]);
            return args;
         }
         case 18: {
            // lookToAssist_1Step3
            final Object[] args = new Object[2];
            args[0] = ((AllyWME)__$behaviorFrame[0]);
            args[1] = ((AllyWME)__$behaviorFrame[1]);
            return args;
         }
         case 21: {
            // assistAlly_1Step2
            final Object[] args = new Object[2];
            args[0] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[2]).i);
            args[1] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[3]).i);
            return args;
         }
      default:
         throw new AblRuntimeError("Unexpected stepID " + __$stepID);
      }
   }
}
