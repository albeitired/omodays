using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    //private GameManager gm;
    //private AchievementManager am;
    public int desperation, leakState, money;
    public bool isProtected, bedProtected, hasBedProtector, wearBoxers, wearJeans, isSoaked, isWatched, roomieWatch;
    public bool firstTimeBuyDiaper, firstTimeUseDiaper, roommateFirstWitnessDiaper;
    public int wetTimes, caughtTimes, omoScenarioUnlocked;
    public int waterStock, coffeeStock, diaperStock;
    public bool boughtShooter, boughtHorror, boughtOmoge, canBuyOmoge, introDone;
    public bool scnDesperatePeeing, scnDiaperPee, scnDiaperLeak, scnCaughtWetPants, scnCaughtWearingDiapers,
                scnCaughtPeeingSelf, scnRoomieIntoOmo, scnPlayWithClothes, scnWorkPee, scnRewetting,
                scnRoomieWorkPee, scnPeeTryWork, scnWorkPeeSoaked, scnShooterWetting, scnShooterHold, 
                scnDrinkWhilePee, scnDiaperWhilePee, scnBuyDiaperWhilePee, scnFreePeeing, scnRoomieIntoDiaper,
                scnPlayWithPee, scnRoomieAwakening, scnDescribeLeakyDiapers, scnDescribeDiaperPee, scnPeeOnBed,
                scnDiaperLeakOnBed, scnDiaperPeeOnBed, scnWaterBottlePee, scnHalfMadeIt, scnWetTheBed,
                scnPreventWetBed, scnMorningPeeOnBed, scnHorrorHalfPee, scnHorrorFullPee,
                scnOmogeRoomieDiaper, scnOmogeLendADiaper, scnOmogeSecretPee, scnOmogeRunningPee;
    public bool interactionDiaperPee;

    public PlayerData() {
        this.desperation = 0;
        this.leakState = 0;
        this.money = 0;
        this.isProtected = false;
        this.bedProtected = false;
        this.hasBedProtector = false;
        this.wearBoxers = false;
        this.wearJeans = true;
        this.isSoaked = false;
        this.isWatched = false;
        this.roomieWatch = false;
        this.firstTimeBuyDiaper = true;
        this.firstTimeUseDiaper = true;
        this.roommateFirstWitnessDiaper = true;
        this.wetTimes = 0;
        this.caughtTimes = 0;
        this.omoScenarioUnlocked = 0;
        this.waterStock = 0;
        this.coffeeStock = 0;
        this.diaperStock = 0;
        this.boughtShooter = false;
        this.boughtHorror = false;
        this.boughtOmoge = false;
        this.canBuyOmoge = false;
        this.introDone = false;

        this.scnDesperatePeeing = false;
        this.scnDiaperPee = false;
        this.scnDiaperLeak = false;
        this.scnCaughtWetPants = false;
        this.scnCaughtWearingDiapers = false;
        this.scnCaughtPeeingSelf = false;
        this.scnRoomieIntoOmo = false;
        this.scnPlayWithClothes = false;
        this.scnWorkPee = false;
        this.scnRewetting = false;
        this.scnRoomieWorkPee = false;
        this.scnPeeTryWork = false;
        this.scnWorkPeeSoaked = false;
        this.scnShooterWetting = false;
        this.scnShooterHold = false;
        this.scnDrinkWhilePee = false;
        this.scnDiaperWhilePee = false;
        this.scnBuyDiaperWhilePee = false;
        this.scnFreePeeing = false;
        this.scnRoomieIntoDiaper = false;
        this.scnPlayWithPee = false;
        this.scnRoomieAwakening = false;
        this.scnDescribeLeakyDiapers = false;
        this.scnDescribeDiaperPee = false;
        this.scnPeeOnBed = false;
        this.scnDiaperLeakOnBed = false;
        this.scnDiaperPeeOnBed = false;
        this.scnWaterBottlePee = false;
        this.scnHalfMadeIt = false;
        this.scnWetTheBed = false;
        this.scnPreventWetBed = false;
        this.scnMorningPeeOnBed = false;
        this.scnHorrorHalfPee = false;
        this.scnHorrorFullPee = false;
        this.scnOmogeRoomieDiaper = false;
        this.scnOmogeLendADiaper = false;
        this.scnOmogeSecretPee = false;
        this.scnOmogeRunningPee = false;
        this.interactionDiaperPee = false;
    }

    /*public PlayerData(GameManager gm, AchievementManager am) {
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
    }*/
}
