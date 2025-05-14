public static class OutcomeStrings
{
    // General Outcomes
    public const string General_Success = "Success";
    public const string General_Failure = "Failure";
    // ... other general outcomes

    public static class Assist
    {
        public const string Assist_GuildAllySuccess = "Assist_GuildAllySuccess";
    }

    public static class Attack
    {
        public const string Attack_Success_TargetGuildAffiliated = "Attack_Success_TargetGuildAffiliated";
    }

    public static class Dialogue
    {
        public const string Dialogue_Lie = "Dialogue_Lie";
        public const string Dialogue_Truth = "Dialogue_Truth";
        public const string Dialogue_GuildInfoSuccess = "Dialogue_GuildInfoSuccess";
        public const string Dialogue_PiggyRevealsMotivation = "Dialogue_PiggyRevealsMotivation";
        // ... other dialogue outcomes
    }

    public static class Observe
    {
        public const string Observe_SuspiciousAction_Sneaking = "Observe_SuspiciousAction_Sneaking";
    }

    public static class Combat
    {
        public const string Combat_Hit = "Combat_Hit";
        public const string Combat_Miss = "Combat_Miss";
        public const string Combat_PlayerInitiated_Piggy = "Combat_PlayerInitiated_Piggy";
        public const string Combat_PlayerInitiated_QuestNPC = "Combat_PlayerInitiated_QuestNPC";
        public const string Combat_WurguthAttacksPlayer = "Combat_WurguthAttacksPlayer";
        public const string Combat_LocationTrigger_Quinton = "Combat_LocationTrigger_Quinton";
        // ... other combat outcomes
    }

    public static class Reputation
    {
        public const string Reputation_Fame_High_Combat = "Reputation_Fame_High_Combat";
    }
    // ... other categories
}