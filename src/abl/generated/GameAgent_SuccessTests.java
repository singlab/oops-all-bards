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
public class GameAgent_SuccessTests {
   static public boolean successTest0(int __$stepID, final Object[] __$behaviorFrame, final BehavingEntity __$thisEntity) {
      switch (__$stepID) {
         case 0: {
            // vivAgentRoot_1Step1
               if (
                  true
               )

               {
                  return true;
               }


            return false;
         }
         case 1: {
            // lookForVivCommands_1Step1
               List wmeList0;
               ListIterator wmeIter0;
               wmeList0 = BehavingEntity.getBehavingEntity().lookupWME("VivWME");
               wmeIter0 = wmeList0.listIterator();
               while(wmeIter0.hasNext()) {
                  VivWME wme__0 = (VivWME)wmeIter0.next();
                  __$behaviorFrame[0] = wme__0;
                  if (
                     ( wme__0.getOnTree() == false )
                  )

                  {
                        List wmeList1;
                        ListIterator wmeIter1;
                        wmeList1 = BehavingEntity.getBehavingEntity().lookupReflectionWMEBySignature("ParallelBehaviorWME", "vivAgentRoot()");
                        wmeIter1 = wmeList1.listIterator();
                        while(wmeIter1.hasNext()) {
                           ParallelBehaviorWME wme__1 = (ParallelBehaviorWME)wmeIter1.next();
                           __$behaviorFrame[1] = wme__1;
                           if (
                              (wme__1.getSignature().equals("vivAgentRoot()"))
                           )

                           {
                              return true;
                           }

                        }


                  }

               }


            return false;
         }
         case 10: {
            // processSpawnGoals_1Step2
               List wmeList0;
               ListIterator wmeIter0;
               wmeList0 = BehavingEntity.getBehavingEntity().lookupWME("CharacterManagerWME");
               wmeIter0 = wmeList0.listIterator();
               while(wmeIter0.hasNext()) {
                  CharacterManagerWME wme__0 = (CharacterManagerWME)wmeIter0.next();
                  __$behaviorFrame[2] = wme__0;
                  if (
                     ( wme__0.getCharacterId() == ((__ValueTypes.IntVar)__$behaviorFrame[0]).i )
                  )

                  {
                        List wmeList1;
                        ListIterator wmeIter1;
                        wmeList1 = BehavingEntity.getBehavingEntity().lookupWME("ParallelBehaviorWME");
                        wmeIter1 = wmeList1.listIterator();
                        while(wmeIter1.hasNext()) {
                           ParallelBehaviorWME wme__1 = (ParallelBehaviorWME)wmeIter1.next();
                           __$behaviorFrame[3] = wme__1;
                           if (
                              ( wme__1.getBehaviorID() == ((CharacterManagerWME)__$behaviorFrame[2]).managerBehaviorId )
                           )

                           {
                              return true;
                           }

                        }


                  }

               }


            return false;
         }
         case 26: {
            // investigateSuspiciousActivity_1Step3
               List wmeList0;
               ListIterator wmeIter0;
               wmeList0 = BehavingEntity.getBehavingEntity().lookupWME("BehaviorStatusWME");
               wmeIter0 = wmeList0.listIterator();
               while(wmeIter0.hasNext()) {
                  BehaviorStatusWME wme__0 = (BehaviorStatusWME)wmeIter0.next();
                  if (
                     ( wme__0.getCharacterID() == ((__ValueTypes.IntVar)__$behaviorFrame[0]).i )
                     &&
                     (wme__0.getBehaviorName().equals("Move"))
                     &&
                     (wme__0.getStatus().equals("Success"))
                  )

                  {
                     return true;
                  }

               }


            return false;
         }
         case 28: {
            // investigateSuspiciousActivity_1Step5
               List wmeList0;
               ListIterator wmeIter0;
               wmeList0 = BehavingEntity.getBehavingEntity().lookupWME("BehaviorStatusWME");
               wmeIter0 = wmeList0.listIterator();
               while(wmeIter0.hasNext()) {
                  BehaviorStatusWME wme__0 = (BehaviorStatusWME)wmeIter0.next();
                  if (
                     ( wme__0.getCharacterID() == ((__ValueTypes.IntVar)__$behaviorFrame[0]).i )
                     &&
                     (wme__0.getBehaviorName().equals("Observe"))
                     &&
                     (wme__0.getStatus().equals("Success"))
                  )

                  {
                     return true;
                  }

               }


            return false;
         }
         case 31: {
            // neutralizeThreat_1Step3
               List wmeList0;
               ListIterator wmeIter0;
               wmeList0 = BehavingEntity.getBehavingEntity().lookupWME("BehaviorStatusWME");
               wmeIter0 = wmeList0.listIterator();
               while(wmeIter0.hasNext()) {
                  BehaviorStatusWME wme__0 = (BehaviorStatusWME)wmeIter0.next();
                  if (
                     ( wme__0.getCharacterID() == ((__ValueTypes.IntVar)__$behaviorFrame[0]).i )
                     &&
                     (wme__0.getBehaviorName().equals("AggressiveConfrontation"))
                     &&
                     (wme__0.getStatus().equals("Success"))
                  )

                  {
                     return true;
                  }

               }


            return false;
         }
         case 34: {
            // gatherInformation_1Step3
               List wmeList0;
               ListIterator wmeIter0;
               wmeList0 = BehavingEntity.getBehavingEntity().lookupWME("BehaviorStatusWME");
               wmeIter0 = wmeList0.listIterator();
               while(wmeIter0.hasNext()) {
                  BehaviorStatusWME wme__0 = (BehaviorStatusWME)wmeIter0.next();
                  if (
                     ( wme__0.getCharacterID() == ((__ValueTypes.IntVar)__$behaviorFrame[0]).i )
                     &&
                     (wme__0.getBehaviorName().equals("Observe"))
                     &&
                     (wme__0.getStatus().equals("Success"))
                  )

                  {
                     return true;
                  }

               }


            return false;
         }
         case 37: {
            // maintainGuildSecrecy_1Step3
               List wmeList0;
               ListIterator wmeIter0;
               wmeList0 = BehavingEntity.getBehavingEntity().lookupWME("BehaviorStatusWME");
               wmeIter0 = wmeList0.listIterator();
               while(wmeIter0.hasNext()) {
                  BehaviorStatusWME wme__0 = (BehaviorStatusWME)wmeIter0.next();
                  if (
                     ( wme__0.getCharacterID() == ((__ValueTypes.IntVar)__$behaviorFrame[0]).i )
                     &&
                     (wme__0.getBehaviorName().equals("CalmConfrontation"))
                     &&
                     (wme__0.getStatus().equals("Success"))
                  )

                  {
                     return true;
                  }

               }


            return false;
         }
      default:
         throw new AblRuntimeError("Unexpected stepID " + __$stepID);
      }
   }
}
