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
            // lookForVivCommands-1->ConditionalStep4_IF_GoalStep_1Step1
            final Object[] args = new Object[2];
            args[0] = ((ParallelBehaviorWME)__$behaviorFrame[1]);
            args[1] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[2]).i);
            return args;
         }
         case 7: {
            // lookForVivCommands_1Step6
            final Object[] args = new Object[1];
            args[0] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[2]).i);
            return args;
         }
         case 13: {
            // processSpawnGoals-4->ConditionalStep11_IF_MentalStep_GoalStep_1Step2
            final Object[] args = new Object[3];
            args[0] = ((ParallelBehaviorWME)__$behaviorFrame[3]);
            args[1] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[0]).i);
            args[2] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[4]).i);
            return args;
         }
         case 16: {
            // processSpawnGoals-4->ConditionalStep14_IF_MentalStep_GoalStep_1Step2
            final Object[] args = new Object[3];
            args[0] = ((ParallelBehaviorWME)__$behaviorFrame[3]);
            args[1] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[0]).i);
            args[2] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[4]).i);
            return args;
         }
         case 19: {
            // processSpawnGoals-4->ConditionalStep17_IF_MentalStep_GoalStep_1Step2
            final Object[] args = new Object[3];
            args[0] = ((ParallelBehaviorWME)__$behaviorFrame[3]);
            args[1] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[0]).i);
            args[2] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[4]).i);
            return args;
         }
         case 22: {
            // processSpawnGoals-4->ConditionalStep20_IF_MentalStep_GoalStep_1Step2
            final Object[] args = new Object[3];
            args[0] = ((ParallelBehaviorWME)__$behaviorFrame[3]);
            args[1] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[0]).i);
            args[2] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[4]).i);
            return args;
         }
         case 25: {
            // investigateSuspiciousActivity_1Step2
            final Object[] args = new Object[4];
            args[0] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[0]).i);
            args[1] = new Float(10.0);
            args[2] = new Float(0.0);
            args[3] = new Float(25.5);
            return args;
         }
         case 27: {
            // investigateSuspiciousActivity_1Step4
            final Object[] args = new Object[3];
            args[0] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[0]).i);
            args[1] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[1]).i);
            args[2] = new Float(15.0);
            return args;
         }
         case 30: {
            // neutralizeThreat_1Step2
            final Object[] args = new Object[2];
            args[0] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[0]).i);
            args[1] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[1]).i);
            return args;
         }
         case 33: {
            // gatherInformation_1Step2
            final Object[] args = new Object[3];
            args[0] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[0]).i);
            args[1] = new Integer(((__ValueTypes.IntVar)__$behaviorFrame[1]).i);
            args[2] = new Float(30.0);
            return args;
         }
         case 36: {
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
