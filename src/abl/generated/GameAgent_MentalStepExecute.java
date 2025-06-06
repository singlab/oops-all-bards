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
         case 2: {
            // lookForVivCommands_1Step2
            ((VivWME)__$behaviorFrame[0]).setOnTree(true);
            break;
         }
         case 3: {
            // lookForVivCommands_1Step3
            ((__ValueTypes.IntVar)__$behaviorFrame[2]).i = ((VivWME)__$behaviorFrame[0]).getID();
            break;
         }
         case 6: {
            // lookForVivCommands_1Step5
            ((GameAgent)__$thisEntity).dict.addCharacter(((__ValueTypes.IntVar)__$behaviorFrame[2]).i , ((VivWME)__$behaviorFrame[0]));
            break;
         }
         case 8: {
            // manageVivCharacter_1Step1
            abl.wmes.ParallelBehaviorWME me = getBehaviorWME();
            System.out.println("ABL: Linking manager behavior " + me.getID() + " to character " + ((__ValueTypes.IntVar)__$behaviorFrame[0]).i);
            addWME(new CharacterManagerWME(((__ValueTypes.IntVar)__$behaviorFrame[0]).i , me.getID()));
            break;
         }
         case 9: {
            // processSpawnGoals_1Step1
            __$behaviorFrame[1] = ((GameAgent)__$thisEntity).dict.getCharacter(((__ValueTypes.IntVar)__$behaviorFrame[0]).i);
            break;
         }
         case 12: {
            // processSpawnGoals-4->ConditionalStep11_IF_MentalStep_GoalStep_1Step1
            ((__ValueTypes.IntVar)__$behaviorFrame[4]).i = ((VivWME)__$behaviorFrame[1]).getTargetIdForGoal("investigateSuspiciousActivity");
            break;
         }
         case 15: {
            // processSpawnGoals-4->ConditionalStep14_IF_MentalStep_GoalStep_1Step1
            ((__ValueTypes.IntVar)__$behaviorFrame[4]).i = ((VivWME)__$behaviorFrame[1]).getTargetIdForGoal("neutralizeThreat");
            break;
         }
         case 18: {
            // processSpawnGoals-4->ConditionalStep17_IF_MentalStep_GoalStep_1Step1
            ((__ValueTypes.IntVar)__$behaviorFrame[4]).i = ((VivWME)__$behaviorFrame[1]).getTargetIdForGoal("gatherInformation");
            break;
         }
         case 21: {
            // processSpawnGoals-4->ConditionalStep20_IF_MentalStep_GoalStep_1Step1
            ((__ValueTypes.IntVar)__$behaviorFrame[4]).i = ((VivWME)__$behaviorFrame[1]).getTargetIdForGoal("maintainGuildSecrecy");
            break;
         }
         case 23: {
            // processSpawnGoals_1Step7
            System.out.println("ABL: Finished processing spawn goals, deleting command WME.");
            deleteWME(((VivWME)__$behaviorFrame[1]));
            break;
         }
         case 24: {
            // investigateSuspiciousActivity_1Step1
            System.out.println("ABL Tactic: Character " + ((__ValueTypes.IntVar)__$behaviorFrame[0]).i + " is investigating " + ((__ValueTypes.IntVar)__$behaviorFrame[1]).i);
            break;
         }
         case 29: {
            // neutralizeThreat_1Step1
            System.out.println("ABL Tactic: Character " + ((__ValueTypes.IntVar)__$behaviorFrame[0]).i + " is neutralizing threat " + ((__ValueTypes.IntVar)__$behaviorFrame[1]).i);
            break;
         }
         case 32: {
            // gatherInformation_1Step1
            System.out.println("ABL Tactic: Character " + ((__ValueTypes.IntVar)__$behaviorFrame[0]).i + " is gathering info on " + ((__ValueTypes.IntVar)__$behaviorFrame[1]).i);
            break;
         }
         case 35: {
            // maintainGuildSecrecy_1Step1
            System.out.println("ABL Tactic: Character " + ((__ValueTypes.IntVar)__$behaviorFrame[0]).i + " is maintaining secrecy from " + ((__ValueTypes.IntVar)__$behaviorFrame[1]).i);
            break;
         }
      default:
         throw new AblRuntimeError("Unexpected stepID " + __$stepID);
      }
   }
}
