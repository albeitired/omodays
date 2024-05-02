using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    private GameManager gm;
    private AchievementManager am;
    public int desperation, leakState, money;
    public bool isProtected, wearBoxers, wearJeans, isSoaked, isWatched, roomieWatch;
    public bool firstTimeBuyDiaper, firstTimeUseDiaper, roommateFirstWitnessDiaper, bedProtected;
    public int wetTimes, caughtTimes, omoScenarioUnlocked;
    public bool shooterUnlocked, horrorUnlocked, omogeUnlocked;
    public int[] itemStock;
    public bool scnDesperatePeeing, scnDiaperPee, scnDiaperLeak, scnCaughtWetPants, scnCaughtWearingDiapers,
                scnCaughtPeeingSelf, scnRoomieIntoOmo, scnPlayWithClothes, scnWorkPee, scnRewetting,
                scnRoomieWorkPee, scnPeeTryWork, scnWorkPeeSoaked, scnShooterWetting, scnShooterHold, 
                scnDrinkWhilePee, scnDiaperWhilePee, scnBuyDiaperWhilePee, scnFreePeeing, scnRoomieIntoDiaper,
                scnPlayWithPee, scnRoomieAwakening, scnDescribeLeakyDiapers, scnDescribeDiaperPee, scnPeeOnBed,
                scnDiaperLeakOnBed, scnDiaperPeeOnBed, scnWaterBottlePee, scnHalfMadeIt, scnWetTheBed,
                scnPreventWetBed, scnMorningPeeOnBed;

    public PlayerData(GameManager gm, AchievementManager am) {
        desperation = gm.desperation;
        leakState = gm.leakState;
        money = gm.money;
        isProtected = gm.isProtected;
        wearBoxers = gm.wearBoxers;
        wearJeans = gm.wearJeans;
        isSoaked = gm.isSoaked;
        isWatched = gm.isWatched;
        roomieWatch = gm.roomieWatch;
        firstTimeBuyDiaper = gm.firstTimeBuyDiaper;
        firstTimeUseDiaper = gm.firstTimeUseDiaper;
        roommateFirstWitnessDiaper = gm.roommateFirstWitnessDiaper;
        bedProtected = gm.bedProtected;
        wetTimes = gm.wetTimes;
        caughtTimes = gm.caughtTimes;
        omoScenarioUnlocked = gm.omoScenarioUnlocked;
        shooterUnlocked = gm.shooterUnlocked;
        horrorUnlocked = gm.horrorUnlocked;
        omogeUnlocked = gm.omogeUnlocked;

        itemStock = new int[gm.itemStock.Length];
        for(int i=0; i<gm.itemStock.Length; i++) {
            itemStock[i] = gm.itemStock[i];
        }

        scnDesperatePeeing = am.scnDesperatePeeing;
        scnDiaperPee = am.scnDiaperPee;
        scnDiaperLeak = am.scnDiaperLeak;
        scnCaughtWetPants = am.scnCaughtWetPants; 
        scnCaughtWearingDiapers = am.scnCaughtWearingDiapers;
        scnCaughtPeeingSelf = am.scnCaughtPeeingSelf;
        scnRoomieIntoOmo = am.scnRoomieIntoOmo;
        scnPlayWithClothes = am.scnPlayWithClothes;
        scnWorkPee = am.scnWorkPee;
        scnRewetting = am.scnRewetting;
        scnRoomieWorkPee = am.scnRoomieWorkPee;
        scnPeeTryWork = am.scnPeeTryWork;
        scnWorkPeeSoaked = am.scnWorkPeeSoaked;
        scnShooterWetting = am.scnShooterWetting;
        scnShooterHold = am.scnShooterHold;
        scnDrinkWhilePee = am.scnDrinkWhilePee;
        scnDiaperWhilePee = am.scnDiaperWhilePee;
        scnBuyDiaperWhilePee = am.scnBuyDiaperWhilePee;
        scnFreePeeing = am.scnFreePeeing;
        scnRoomieIntoDiaper = am.scnRoomieIntoDiaper;
        scnPlayWithPee = am.scnPlayWithPee;
        scnRoomieAwakening = am.scnRoomieAwakening;
        scnDescribeLeakyDiapers = am.scnDescribeLeakyDiapers;
        scnDescribeDiaperPee = am.scnDescribeDiaperPee;
        scnPeeOnBed = am.scnPeeOnBed;
        scnDiaperLeakOnBed = am.scnDiaperLeakOnBed;
        scnDiaperPeeOnBed = am.scnDiaperPeeOnBed;
        scnWaterBottlePee = am.scnWaterBottlePee;
        scnHalfMadeIt = am.scnHalfMadeIt;
        scnWetTheBed = am.scnWetTheBed;
        scnPreventWetBed = am.scnPreventWetBed;
        scnMorningPeeOnBed = am.scnMorningPeeOnBed;
    }
}
