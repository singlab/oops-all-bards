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
import java.util.HashMap;
public class GameAgent_ArgumentStepExecute {
   static public Object[] argumentExecute0(int __$stepID, final Object[] __$behaviorFrame, final BehavingEntity __$thisEntity) {
      switch (__$stepID) {
         case 6: {
            // lookForVivCommands-0->ConditionalStep4_IF_MentalStep_GoalStep_1Step2
            final Object[] args = new Object[3];
            args[0] = ((ParallelBehaviorWME)__$behaviorFrame[1]);
            args[1] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[2]).i);
            args[2] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[3]).i);
            return args;
         }
         case 9: {
            // lookForVivCommands-0->ConditionalStep7_IF_MentalStep_GoalStep_1Step2
            final Object[] args = new Object[3];
            args[0] = ((ParallelBehaviorWME)__$behaviorFrame[1]);
            args[1] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[2]).i);
            args[2] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[3]).i);
            return args;
         }
         case 12: {
            // lookForVivCommands-0->ConditionalStep10_IF_MentalStep_GoalStep_1Step2
            final Object[] args = new Object[3];
            args[0] = ((ParallelBehaviorWME)__$behaviorFrame[1]);
            args[1] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[2]).i);
            args[2] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[3]).i);
            return args;
         }
         case 15: {
            // lookForVivCommands-0->ConditionalStep13_IF_MentalStep_GoalStep_1Step2
            final Object[] args = new Object[3];
            args[0] = ((ParallelBehaviorWME)__$behaviorFrame[1]);
            args[1] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[2]).i);
            args[2] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[3]).i);
            return args;
         }
         case 19: {
            // investigateSuspiciousActivity_1Step2
            final Object[] args = new Object[2];
            args[0] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[0]).i);
            args[1] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[1]).i);
            return args;
         }
         case 21: {
            // investigateSuspiciousActivity_1Step4
            final Object[] args = new Object[3];
            args[0] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[0]).i);
            args[1] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[1]).i);
            args[2] = new Float(15.0);
            return args;
         }
         case 24: {
            // neutralizeThreat_1Step2
            final Object[] args = new Object[2];
            args[0] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[0]).i);
            args[1] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[1]).i);
            return args;
         }
         case 27: {
            // gatherInformation_1Step2
            final Object[] args = new Object[3];
            args[0] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[0]).i);
            args[1] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[1]).i);
            args[2] = new Float(30.0);
            return args;
         }
         case 30: {
            // maintainGuildSecrecy_1Step2
            final Object[] args = new Object[2];
            args[0] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[0]).i);
            args[1] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[1]).i);
            return args;
         }
      default:
         throw new AblRuntimeError("Unexpected stepID " + __$stepID);
      }
   }
}
