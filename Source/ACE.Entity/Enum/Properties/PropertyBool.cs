using System.ComponentModel;

namespace ACE.Entity.Enum.Properties
{
    // No properties are sent to the client unless they featured an attribute.
    // SendOnLogin gets sent to players in the PlayerDescription event
    // AssessmentProperty gets sent in successful appraisal
    public enum PropertyBool : ushort
    {
        Undef = 0,
        [Ephemeral]
        Stuck = 1,
        [AssessmentProperty]
        [Ephemeral]
        Open = 2,
        [AssessmentProperty]
        Locked = 3,
        RotProof = 4,
        AllegianceUpdateRequest = 5,
        AiUsesMana = 6,
        AiUseHumanMagicAnimations = 7,
        AllowGive = 8,
        CurrentlyAttacking = 9,
        AttackerAi = 10,
        IgnoreCollisions = 11,
        ReportCollisions = 12,
        Ethereal = 13,
        GravityStatus = 14,
        LightsStatus = 15,
        ScriptedCollision = 16,
        Inelastic = 17,
        [Ephemeral]
        Visibility = 18,
        Attackable = 19,
        SafeSpellComponents = 20,
        [SendOnLogin]
        AdvocateState = 21,
        Inscribable = 22,
        DestroyOnSell = 23,
        UiHidden = 24,
        IgnoreHouseBarriers = 25,
        HiddenAdmin = 26,
        PkWounder = 27,
        PkKiller = 28,
        NoCorpse = 29,
        UnderLifestoneProtection = 30,
        ItemManaUpdatePending = 31,
        [Ephemeral]
        GeneratorStatus = 32,
        [Ephemeral]
        ResetMessagePending = 33,
        DefaultOpen = 34,
        DefaultLocked = 35,
        DefaultOn = 36,
        OpenForBusiness = 37,
        IsFrozen = 38,
        DealMagicalItems = 39,
        LogoffImDead = 40,
        ReportCollisionsAsEnvironment = 41,
        AllowEdgeSlide = 42,
        AdvocateQuest = 43,
        [SendOnLogin]
        [Ephemeral]
        IsAdmin = 44,
        [SendOnLogin]
        [Ephemeral]
        IsArch = 45,
        [SendOnLogin]
        [Ephemeral]
        IsSentinel = 46,
        [SendOnLogin]
        IsAdvocate = 47,
        CurrentlyPoweringUp = 48,
        [Ephemeral]
        GeneratorEnteredWorld = 49,
        NeverFailCasting = 50,
        VendorService = 51,
        AiImmobile = 52,
        DamagedByCollisions = 53,
        IsDynamic = 54,
        IsHot = 55,
        IsAffecting = 56,
        AffectsAis = 57,
        SpellQueueActive = 58,
        [Ephemeral]
        GeneratorDisabled = 59,
        IsAcceptingTells = 60,
        LoggingChannel = 61,
        OpensAnyLock = 62,
        [AssessmentProperty]
        UnlimitedUse = 63,
        GeneratedTreasureItem = 64,
        IgnoreMagicResist = 65,
        IgnoreMagicArmor = 66,
        AiAllowTrade = 67,
        [SendOnLogin]
        SpellComponentsRequired = 68,
        [AssessmentProperty]
        IsSellable = 69,
        IgnoreShieldsBySkill = 70,
        NoDraw = 71,
        ActivationUntargeted = 72,
        HouseHasGottenPriorityBootPos = 73,
        [Ephemeral]
        GeneratorAutomaticDestruction = 74,
        HouseHooksVisible = 75,
        HouseRequiresMonarch = 76,
        HouseHooksEnabled = 77,
        HouseNotifiedHudOfHookCount = 78,
        AiAcceptEverything = 79,
        IgnorePortalRestrictions = 80,
        RequiresBackpackSlot = 81,
        DontTurnOrMoveWhenGiving = 82,
        NpcLooksLikeObject = 83,
        IgnoreCloIcons = 84,
        [AssessmentProperty]
        AppraisalHasAllowedWielder = 85,
        ChestRegenOnClose = 86,
        LogoffInMinigame = 87,
        PortalShowDestination = 88,
        PortalIgnoresPkAttackTimer = 89,
        NpcInteractsSilently = 90,
        [AssessmentProperty]
        Retained = 91,
        IgnoreAuthor = 92,
        Limbo = 93,
        [AssessmentProperty]
        AppraisalHasAllowedActivator = 94,
        ExistedBeforeAllegianceXpChanges = 95,
        IsDeaf = 96,
        [SendOnLogin]
        [Ephemeral]
        IsPsr = 97,
        Invincible = 98,
        [AssessmentProperty]
        Ivoryable = 99,
        [AssessmentProperty]
        Dyable = 100,
        CanGenerateRare = 101,
        CorpseGeneratedRare = 102,
        NonProjectileMagicImmune = 103,
        [SendOnLogin]
        ActdReceivedItems = 104,
        Unknown105 = 105,
        [Ephemeral]
        FirstEnterWorldDone = 106,
        RecallsDisabled = 107,
        [AssessmentProperty]
        RareUsesTimer = 108,
        ActdPreorderReceivedItems = 109,
        [Ephemeral]
        Afk = 110,
        IsGagged = 111,
        ProcSpellSelfTargeted = 112,
        IsAllegianceGagged = 113,
        EquipmentSetTriggerPiece = 114,
        Uninscribe = 115,
        WieldOnUse = 116,
        ChestClearedWhenClosed = 117,
        NeverAttack = 118,
        SuppressGenerateEffect = 119,
        TreasureCorpse = 120,
        EquipmentSetAddLevel = 121,
        BarberActive = 122,
        TopLayerPriority = 123,
        [SendOnLogin]
        NoHeldItemShown = 124,
        [SendOnLogin]
        LoginAtLifestone = 125,
        OlthoiPk = 126,
        [SendOnLogin]
        Account15Days = 127,
        HadNoVitae = 128,
        NoOlthoiTalk = 129,
        [AssessmentProperty]
        AutowieldLeft = 130,

        /* Custom Properties */
        LinkedPortalOneSummon = 9001,
        LinkedPortalTwoSummon = 9002,
        HouseEvicted = 9003,
        UntrainedSkills = 9004,
        [Ephemeral]
        IsEnvoy = 9005,
        UnspecializedSkills = 9006,
        FreeSkillResetRenewed = 9007,
        FreeAttributeResetRenewed = 9008,
        SkillTemplesTimerReset = 9009,
        FreeMasteryResetRenewed = 9010,

        // CustomDM
        [Ephemeral]
        IsEncounterGenerator = 9011,
        IsModified = 9012,
        VendorSellsSalvage = 9013,
        VendorSellsSpecialItems = 9014,
        VendorPKOnly = 9015,
        UseXpOverride = 9016,
        HasBeenResurrected = 9017,
        UseSmartSalvageSystem = 9018,
        SmartSalvageAvoidInscripted = 9019,
        SmartSalvageIsWhitelist = 9020,
        IsLightWeapon = 9021,
        Exploration1LandblockReached = 9022,
        Exploration2LandblockReached = 9023,
        Exploration3LandblockReached = 9024,
        UsesWeightAsGeneratorProbabilities = 9025,
        AiOmnidirectional = 9026,
        AiIncapableOfAnyMotion = 9027,
        IsVendorGeneratedItem = 9028,
        BlockSpellExtraction = 9029,

        /* Elite implementation */
        EliteTrigger = 9107,
        IsElite = 9108,
        SplitMod = 9109,
        DiscoMod = 9110,
        IsMini = 9111,
        IsRare = 9112,
        BeefyMod = 9113,
        Warded = 9114,
        TogglePhys = 9115,
        ToggleMis = 9116,
        ToggleSpell = 9117,
        ToggleRNG = 9118,
        MeteorMod = 9119,
        TeleMod = 9120,
        NovaMod = 9121,
        SupportMod = 9122,
        MirrorMod = 9123,
        MirrorMob = 9124,

        /* Arena Props */
        IsArenaObserver = 9300,
        IsPendingArenaObserver = 9301,
        HasArenaRareDmgBuff = 9302,
        HasArenaRareDmgReductionBuff = 9303,

        /* other */
        IsGhost = 9401,
    }

    public static class PropertyBoolExtensions
    {
        public static string GetDescription(this PropertyBool prop)
        {
            var description = prop.GetAttributeOfType<DescriptionAttribute>();
            return description?.Description ?? prop.ToString();
        }
    }
}

