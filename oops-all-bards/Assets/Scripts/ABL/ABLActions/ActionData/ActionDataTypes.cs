using System;

[Serializable]
public class ConfrontationData : ActionEventData
{
    public int targetId;
}

[Serializable]
public class ObserveTargetData : ActionEventData
{
    public int targetId;
    public float duration;
}

[Serializable]
public class MoveToPositionData : ActionEventData
{
    public float x;
    public float y;
    public float z;
}

[Serializable]
public class ProtectData : ActionEventData
{
    public int targetId;
}

[Serializable]
public class RequestAssistanceData : ActionEventData
{ }

[Serializable]
public class QuipData : ActionEventData
{
    public bool inCombat;
}