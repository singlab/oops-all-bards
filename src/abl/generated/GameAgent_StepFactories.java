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
public class GameAgent_StepFactories {
   static public Step stepFactory0(int __$stepID, Behavior __$behaviorParent, final Object[] __$behaviorFrame) {
      final Method __$stepFactory = GameAgent.__$stepFactory0_rfield;
      switch (__$stepID) {
         case -3: {
            // default wait step
            return new WaitStepDebug(-3, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, null, null, null);
         }
         case -2: {
            // default fail step
            return new FailStepDebug(-2, __$stepFactory, __$behaviorParent, false, false, false, (short)-32768, (short)0, false, null, null);
         }
         case -1: {
            // default succeed step
            return new SucceedStepDebug(-1, __$stepFactory, __$behaviorParent, false, false, (short)-32768, (short)0, false, null, null);
         }
         case 0: {
            // vivAgentRoot_1Step1
            return new WaitStepDebug(0, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, GameAgent.__$successTest0_rfield, null, null);
         }
         case 1: {
            // lookForVivCommands_1Step1
            return new WaitStepDebug(1, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, GameAgent.__$successTest0_rfield, null, null);
         }
         case 2: {
            // lookForVivCommands_1Step2
            return new MentalStepDebug(2, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "lookForVivCommands_1Step2");
         }
         case 3: {
            // lookForVivCommands_1Step3
            return new MentalStepDebug(3, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "lookForVivCommands_1Step3");
         }
         case 5: {
            // lookForVivCommands-1->ConditionalStep4_IF_GoalStep_1Step1
            return new GoalStepDebug(5, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, "manageVivCharacter(int)", null, (short)2);
         }
         case 4: {
            // lookForVivCommands_1Step4
            return new ConditionalStepDebug(4, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, null, null, null, GameAgent.__$conditionalTest0_rfield, null, "lookForVivCommands-1->ConditionalStep4_IF_GoalStep()", null);
         }
         case 6: {
            // lookForVivCommands_1Step5
            return new MentalStepDebug(6, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "lookForVivCommands_1Step5");
         }
         case 7: {
            // lookForVivCommands_1Step6
            return new GoalStepDebug(7, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, "processSpawnGoals(int)", null, (short)0);
         }
         case 8: {
            // manageVivCharacter_1Step1
            return new MentalStepDebug(8, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "manageVivCharacter_1Step1");
         }
         case 9: {
            // processSpawnGoals_1Step1
            return new MentalStepDebug(9, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "processSpawnGoals_1Step1");
         }
         case 10: {
            // processSpawnGoals_1Step2
            return new WaitStepDebug(10, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, GameAgent.__$successTest0_rfield, null, null);
         }
         case 12: {
            // processSpawnGoals-4->ConditionalStep11_IF_MentalStep_GoalStep_1Step1
            return new MentalStepDebug(12, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "processSpawnGoals-4->ConditionalStep11_IF_MentalStep_GoalStep_1Step1");
         }
         case 13: {
            // processSpawnGoals-4->ConditionalStep11_IF_MentalStep_GoalStep_1Step2
            return new GoalStepDebug(13, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, "investigateSuspiciousActivity(int, int)", null, (short)2);
         }
         case 11: {
            // processSpawnGoals_1Step3
            return new ConditionalStepDebug(11, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, null, null, null, GameAgent.__$conditionalTest0_rfield, null, "processSpawnGoals-4->ConditionalStep11_IF_MentalStep_GoalStep()", null);
         }
         case 15: {
            // processSpawnGoals-4->ConditionalStep14_IF_MentalStep_GoalStep_1Step1
            return new MentalStepDebug(15, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "processSpawnGoals-4->ConditionalStep14_IF_MentalStep_GoalStep_1Step1");
         }
         case 16: {
            // processSpawnGoals-4->ConditionalStep14_IF_MentalStep_GoalStep_1Step2
            return new GoalStepDebug(16, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, "neutralizeThreat(int, int)", null, (short)2);
         }
         case 14: {
            // processSpawnGoals_1Step4
            return new ConditionalStepDebug(14, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, null, null, null, GameAgent.__$conditionalTest0_rfield, null, "processSpawnGoals-4->ConditionalStep14_IF_MentalStep_GoalStep()", null);
         }
         case 18: {
            // processSpawnGoals-4->ConditionalStep17_IF_MentalStep_GoalStep_1Step1
            return new MentalStepDebug(18, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "processSpawnGoals-4->ConditionalStep17_IF_MentalStep_GoalStep_1Step1");
         }
         case 19: {
            // processSpawnGoals-4->ConditionalStep17_IF_MentalStep_GoalStep_1Step2
            return new GoalStepDebug(19, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, "gatherInformation(int, int)", null, (short)2);
         }
         case 17: {
            // processSpawnGoals_1Step5
            return new ConditionalStepDebug(17, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, null, null, null, GameAgent.__$conditionalTest0_rfield, null, "processSpawnGoals-4->ConditionalStep17_IF_MentalStep_GoalStep()", null);
         }
         case 21: {
            // processSpawnGoals-4->ConditionalStep20_IF_MentalStep_GoalStep_1Step1
            return new MentalStepDebug(21, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "processSpawnGoals-4->ConditionalStep20_IF_MentalStep_GoalStep_1Step1");
         }
         case 22: {
            // processSpawnGoals-4->ConditionalStep20_IF_MentalStep_GoalStep_1Step2
            return new GoalStepDebug(22, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, "maintainGuildSecrecy(int, int)", null, (short)2);
         }
         case 20: {
            // processSpawnGoals_1Step6
            return new ConditionalStepDebug(20, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, null, null, null, GameAgent.__$conditionalTest0_rfield, null, "processSpawnGoals-4->ConditionalStep20_IF_MentalStep_GoalStep()", null);
         }
         case 23: {
            // processSpawnGoals_1Step7
            return new MentalStepDebug(23, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "processSpawnGoals_1Step7");
         }
         case 24: {
            // investigateSuspiciousActivity_1Step1
            return new MentalStepDebug(24, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "investigateSuspiciousActivity_1Step1");
         }
         case 25: {
            // investigateSuspiciousActivity_1Step2
            return new PrimitiveStepDebug(25, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, new MoveToPosition(), null, "moveToPosition");
         }
         case 26: {
            // investigateSuspiciousActivity_1Step3
            return new WaitStepDebug(26, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, GameAgent.__$successTest0_rfield, null, null);
         }
         case 27: {
            // investigateSuspiciousActivity_1Step4
            return new PrimitiveStepDebug(27, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, new ObserveTarget(), null, "observeTarget");
         }
         case 28: {
            // investigateSuspiciousActivity_1Step5
            return new WaitStepDebug(28, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, GameAgent.__$successTest0_rfield, null, null);
         }
         case 29: {
            // neutralizeThreat_1Step1
            return new MentalStepDebug(29, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "neutralizeThreat_1Step1");
         }
         case 30: {
            // neutralizeThreat_1Step2
            return new PrimitiveStepDebug(30, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, new AggressiveConfrontation(), null, "aggressiveConfrontation");
         }
         case 31: {
            // neutralizeThreat_1Step3
            return new WaitStepDebug(31, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, GameAgent.__$successTest0_rfield, null, null);
         }
         case 32: {
            // gatherInformation_1Step1
            return new MentalStepDebug(32, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "gatherInformation_1Step1");
         }
         case 33: {
            // gatherInformation_1Step2
            return new PrimitiveStepDebug(33, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, new ObserveTarget(), null, "observeTarget");
         }
         case 34: {
            // gatherInformation_1Step3
            return new WaitStepDebug(34, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, GameAgent.__$successTest0_rfield, null, null);
         }
         case 35: {
            // maintainGuildSecrecy_1Step1
            return new MentalStepDebug(35, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "maintainGuildSecrecy_1Step1");
         }
         case 36: {
            // maintainGuildSecrecy_1Step2
            return new PrimitiveStepDebug(36, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, new CalmConfrontation(), null, "calmConfrontation");
         }
         case 37: {
            // maintainGuildSecrecy_1Step3
            return new WaitStepDebug(37, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, GameAgent.__$successTest0_rfield, null, null);
         }
         case 38: {
            // GameAgent_RootCollectionBehaviorStep1
            return new GoalStepDebug(38, __$stepFactory, __$behaviorParent, true, false, false, false, false, false, (short)-32768, (short)0, false, null, null, null, null, null, "vivAgentRoot()", null, (short)0);
         }
         case 39: {
            // GameAgent_RootCollectionBehaviorStep2
            return new GoalStepDebug(39, __$stepFactory, __$behaviorParent, true, false, false, false, false, false, (short)-32768, (short)0, false, null, null, null, null, null, "lookForVivCommands()", null, (short)0);
         }
      default:
         throw new AblRuntimeError("Unexpected stepID " + __$stepID);
      }
   }
}
