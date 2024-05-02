using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour, IDataPersistence
{
    public bool scnDesperatePeeing, scnDiaperPee, scnDiaperLeak, scnCaughtWetPants, scnCaughtWearingDiapers,
                scnCaughtPeeingSelf, scnRoomieIntoOmo, scnPlayWithClothes, scnWorkPee, scnRewetting,
                scnRoomieWorkPee, scnPeeTryWork, scnWorkPeeSoaked, scnShooterWetting, scnShooterHold, 
                scnDrinkWhilePee, scnDiaperWhilePee, scnBuyDiaperWhilePee, scnFreePeeing, scnRoomieIntoDiaper,
                scnPlayWithPee, scnRoomieAwakening, scnDescribeLeakyDiapers, scnDescribeDiaperPee, scnPeeOnBed,
                scnDiaperLeakOnBed, scnDiaperPeeOnBed, scnWaterBottlePee, scnHalfMadeIt, scnWetTheBed,
                scnPreventWetBed, scnMorningPeeOnBed, scnHorrorHalfPee, scnHorrorFullPee;

    //new achievements
    public bool scnOmogeRoomieDiaper, scnOmogeLendADiaper, scnOmogeSecretPee;
    
    //control disabled / enabled
    public Image[] achievementPlates;
    //control descriptions
    public Text[] achievementDescriptions;

    private GameManager gm;

    //public GameObject achievementPlate;

    public void LoadData(PlayerData data) {
        this.scnDiaperPee = data.scnDiaperPee;;
        this.scnDiaperLeak = data.scnDiaperLeak;
        this.scnCaughtWetPants = data.scnCaughtWetPants;
        this.scnCaughtWearingDiapers = data.scnCaughtWearingDiapers;
        this.scnCaughtPeeingSelf = data.scnCaughtPeeingSelf;
        this.scnRoomieIntoOmo = data.scnRoomieIntoOmo;
        this.scnPlayWithClothes = data.scnPlayWithClothes;
        this.scnWorkPee = data.scnWorkPee;
        this.scnRewetting = data.scnRewetting;
        this.scnRoomieWorkPee = data.scnRoomieWorkPee;
        this.scnPeeTryWork = data.scnPeeTryWork;
        this.scnWorkPeeSoaked = data.scnWorkPeeSoaked;
        this.scnShooterWetting = data.scnShooterWetting;
        this.scnShooterHold = data.scnShooterHold;
        this.scnDrinkWhilePee = data.scnDrinkWhilePee;
        this.scnDiaperWhilePee = data.scnDiaperWhilePee;
        this.scnBuyDiaperWhilePee = data.scnBuyDiaperWhilePee;
        this.scnFreePeeing = data.scnFreePeeing;
        this.scnRoomieIntoDiaper = data.scnRoomieIntoDiaper;
        this.scnPlayWithPee = data.scnPlayWithPee;
        this.scnRoomieAwakening = data.scnRoomieAwakening;
        this.scnDescribeLeakyDiapers = data.scnDescribeLeakyDiapers;
        this.scnDescribeDiaperPee = data.scnDescribeDiaperPee;
        this.scnPeeOnBed = data.scnPeeOnBed;
        this.scnDiaperLeakOnBed = data.scnDiaperLeakOnBed;
        this.scnDiaperPeeOnBed = data.scnDiaperPeeOnBed;
        this.scnWaterBottlePee = data.scnWaterBottlePee;
        this.scnHalfMadeIt = data.scnHalfMadeIt;
        this.scnWetTheBed = data.scnWetTheBed;
        this.scnPreventWetBed = data.scnPreventWetBed;
        this.scnMorningPeeOnBed = data.scnMorningPeeOnBed;
        this.scnHorrorHalfPee = data.scnHorrorHalfPee;
        this.scnHorrorFullPee = data.scnHorrorFullPee;
        this.scnOmogeRoomieDiaper = data.scnOmogeRoomieDiaper;
        this.scnOmogeLendADiaper = data.scnOmogeLendADiaper;
        this.scnOmogeSecretPee = data.scnOmogeSecretPee;
    }

    public void SaveData(ref PlayerData data) {
        data.scnDiaperPee = this.scnDiaperPee;
        data.scnDiaperLeak = this.scnDiaperLeak;
        data.scnCaughtWetPants = this.scnCaughtWetPants;
        data.scnCaughtWearingDiapers = this.scnCaughtWearingDiapers;
        data.scnCaughtPeeingSelf = this.scnCaughtPeeingSelf;
        data.scnRoomieIntoOmo = this.scnRoomieIntoOmo;
        data.scnPlayWithClothes = this.scnPlayWithClothes;
        data.scnWorkPee = this.scnWorkPee;
        data.scnRewetting = this.scnRewetting;
        data.scnRoomieWorkPee = this.scnRoomieWorkPee;
        data.scnPeeTryWork = this.scnPeeTryWork;
        data.scnWorkPeeSoaked = this.scnWorkPeeSoaked;
        data.scnShooterWetting = this.scnShooterWetting;
        data.scnShooterHold = this.scnShooterHold;
        data.scnDrinkWhilePee = this.scnDrinkWhilePee;
        data.scnDiaperWhilePee = this.scnDiaperWhilePee;
        data.scnBuyDiaperWhilePee = this.scnBuyDiaperWhilePee;
        data.scnFreePeeing = this.scnFreePeeing;
        data.scnRoomieIntoDiaper = this.scnRoomieIntoDiaper;
        data.scnPlayWithPee = this.scnPlayWithPee;
        data.scnRoomieAwakening = this.scnRoomieAwakening;
        data.scnDescribeLeakyDiapers = this.scnDescribeLeakyDiapers;
        data.scnDescribeDiaperPee = this.scnDescribeDiaperPee;
        data.scnPeeOnBed = this.scnPeeOnBed;
        data.scnDiaperLeakOnBed = this.scnDiaperLeakOnBed;
        data.scnDiaperPeeOnBed = this.scnDiaperPeeOnBed;
        data.scnWaterBottlePee = this.scnWaterBottlePee;
        data.scnHalfMadeIt = this.scnHalfMadeIt;
        data.scnWetTheBed = this.scnWetTheBed;
        data.scnPreventWetBed = this.scnPreventWetBed;
        data.scnMorningPeeOnBed = this.scnMorningPeeOnBed;
        data.scnHorrorHalfPee = this.scnHorrorHalfPee;
        data.scnHorrorFullPee = this.scnHorrorFullPee;
        data.scnOmogeRoomieDiaper = this.scnOmogeRoomieDiaper;
        data.scnOmogeLendADiaper = this.scnOmogeLendADiaper;
        data.scnOmogeSecretPee = this.scnOmogeSecretPee;
    }

    void Start() {
        gm = FindObjectOfType<GameManager>();
        updateAchievementsPanel();
    }

    public void updateAchievementsPanel() {
        //initalize
        gm.omoScenarioUnlocked = 0;
        if(scnDesperatePeeing) {
            achievementPlates[0].color = new Color32(255,217,160,255);
            achievementDescriptions[0].color = new Color32(113,48,0,255);
            achievementDescriptions[0].text = "Held until the very last second until you inevitably pee yourself";
            gm.omoScenarioUnlocked++;
        } else if (!scnDesperatePeeing) {
            achievementPlates[0].color = new Color32(67,24,7,255);
            achievementDescriptions[0].color = new Color32(255,255,255,255);
            achievementDescriptions[0].text = "Unobtained";
        }

        if(scnDiaperPee) {
            achievementPlates[1].color = new Color32(255,217,160,255);
            achievementDescriptions[1].color = new Color32(113,48,0,255);
            achievementDescriptions[1].text = "Worn and used the diaper";
            gm.omoScenarioUnlocked++;
        } else if (!scnDiaperPee) {
            achievementPlates[1].color = new Color32(67,24,7,255);
            achievementDescriptions[1].color = new Color32(255,255,255,255);
            achievementDescriptions[1].text = "Unobtained";
        }

        if(scnDiaperLeak) {
            achievementPlates[2].color = new Color32(255,217,160,255);
            achievementDescriptions[2].color = new Color32(113,48,0,255);
            achievementDescriptions[2].text = "Peed too much into your diapers and spilling all the contents out";
            gm.omoScenarioUnlocked++;
        } else if (!scnDiaperLeak) {
            achievementPlates[2].color = new Color32(67,24,7,255);
            achievementDescriptions[2].color = new Color32(255,255,255,255);
            achievementDescriptions[2].text = "Unobtained";
        }

        if(scnCaughtWetPants) {
            achievementPlates[3].color = new Color32(255,217,160,255);
            achievementDescriptions[3].color = new Color32(113,48,0,255);
            achievementDescriptions[3].text = "Your roommate caught your with soaked pants";
            gm.omoScenarioUnlocked++;
        } else if (!scnCaughtWetPants) {
            achievementPlates[3].color = new Color32(67,24,7,255);
            achievementDescriptions[3].color = new Color32(255,255,255,255);
            achievementDescriptions[3].text = "Unobtained";
        }
        
        if(scnCaughtWearingDiapers) {
            achievementPlates[4].color = new Color32(255,217,160,255);
            achievementDescriptions[4].color = new Color32(113,48,0,255);
            achievementDescriptions[4].text = "Your roommate figured out that you're wearing diapers";
            gm.omoScenarioUnlocked++;
        } else if (!scnCaughtWearingDiapers) {
            achievementPlates[4].color = new Color32(67,24,7,255);
            achievementDescriptions[4].color = new Color32(255,255,255,255);
            achievementDescriptions[4].text = "Unobtained";
        }

        if(scnCaughtPeeingSelf) {
            achievementPlates[5].color = new Color32(255,217,160,255);
            achievementDescriptions[5].color = new Color32(113,48,0,255);
            achievementDescriptions[5].text = "Your roommate saw you peeing yourself";
            gm.omoScenarioUnlocked++;
        } else if (!scnCaughtPeeingSelf) {
            achievementPlates[5].color = new Color32(67,24,7,255);
            achievementDescriptions[5].color = new Color32(255,255,255,255);
            achievementDescriptions[5].text = "Unobtained";
        }

        if(scnRoomieIntoOmo) {
            achievementPlates[6].color = new Color32(255,217,160,255);
            achievementDescriptions[6].color = new Color32(113,48,0,255);
            achievementDescriptions[6].text = "Your roommate confesses his interest in wetting";
            gm.omoScenarioUnlocked++;
        } else if (!scnRoomieIntoOmo) {
            achievementPlates[6].color = new Color32(67,24,7,255);
            achievementDescriptions[6].color = new Color32(255,255,255,255);
            achievementDescriptions[6].text = "Unobtained";
        }

        if(scnPlayWithClothes) {
            achievementPlates[7].color = new Color32(255,217,160,255);
            achievementDescriptions[7].color = new Color32(113,48,0,255);
            achievementDescriptions[7].text = "Changed your clothes mid-pee";
            gm.omoScenarioUnlocked++;
        } else if (!scnPlayWithClothes) {
            achievementPlates[7].color = new Color32(67,24,7,255);
            achievementDescriptions[7].color = new Color32(255,255,255,255);
            achievementDescriptions[7].text = "Unobtained";
        }

        if(scnWorkPee) {
            achievementPlates[8].color = new Color32(255,217,160,255);
            achievementDescriptions[8].color = new Color32(113,48,0,255);
            achievementDescriptions[8].text = "Peed yourself during a work meeting";
            gm.omoScenarioUnlocked++;
        } else if (!scnWorkPee) {
            achievementPlates[8].color = new Color32(67,24,7,255);
            achievementDescriptions[8].color = new Color32(255,255,255,255);
            achievementDescriptions[8].text = "Unobtained";
        }

        if(scnRewetting) {
            achievementPlates[9].color = new Color32(255,217,160,255);
            achievementDescriptions[9].color = new Color32(113,48,0,255);
            achievementDescriptions[9].text = "Re-wet your pants";
            gm.omoScenarioUnlocked++;
        } else if (!scnRewetting) {
            achievementPlates[9].color = new Color32(67,24,7,255);
            achievementDescriptions[9].color = new Color32(255,255,255,255);
            achievementDescriptions[9].text = "Unobtained";
        }

        if(scnRoomieWorkPee) {
            achievementPlates[10].color = new Color32(255,217,160,255);
            achievementDescriptions[10].color = new Color32(113,48,0,255);
            achievementDescriptions[10].text = "Your roommate watched you lose control during a meeting";
            gm.omoScenarioUnlocked++;
        } else if (!scnRoomieWorkPee) {
            achievementPlates[10].color = new Color32(67,24,7,255);
            achievementDescriptions[10].color = new Color32(255,255,255,255);
            achievementDescriptions[10].text = "Unobtained";
        }

        if(scnPeeTryWork) {
            achievementPlates[11].color = new Color32(255,217,160,255);
            achievementDescriptions[11].color = new Color32(113,48,0,255);
            achievementDescriptions[11].text = "Tried working while pissing yourself";
            gm.omoScenarioUnlocked++;
        } else if (!scnPeeTryWork) {
            achievementPlates[11].color = new Color32(67,24,7,255);
            achievementDescriptions[11].color = new Color32(255,255,255,255);
            achievementDescriptions[11].text = "Unobtained";
        }

        if(scnWorkPeeSoaked) {
            achievementPlates[12].color = new Color32(255,217,160,255);
            achievementDescriptions[12].color = new Color32(113,48,0,255);
            achievementDescriptions[12].text = "Started work wet, finished work even wetter";
            gm.omoScenarioUnlocked++;
        } else if (!scnWorkPeeSoaked) {
            achievementPlates[12].color = new Color32(67,24,7,255);
            achievementDescriptions[12].color = new Color32(255,255,255,255);
            achievementDescriptions[12].text = "Unobtained";
        }

        if(scnShooterWetting) {
            achievementPlates[13].color = new Color32(255,217,160,255);
            achievementDescriptions[13].color = new Color32(113,48,0,255);
            achievementDescriptions[13].text = "Pissed yourself during an online game but ended up winning";
            gm.omoScenarioUnlocked++;
        } else if (!scnShooterWetting) {
            achievementPlates[13].color = new Color32(67,24,7,255);
            achievementDescriptions[13].color = new Color32(255,255,255,255);
            achievementDescriptions[13].text = "Unobtained";
        }

        if(scnShooterHold) {
            achievementPlates[14].color = new Color32(255,217,160,255);
            achievementDescriptions[14].color = new Color32(113,48,0,255);
            achievementDescriptions[14].text = "Held your pee during an online game, but ended up losing";
            gm.omoScenarioUnlocked++;
        } else if (!scnShooterHold) {
            achievementPlates[14].color = new Color32(67,24,7,255);
            achievementDescriptions[14].color = new Color32(255,255,255,255);
            achievementDescriptions[14].text = "Unobtained";
        }

        if(scnDrinkWhilePee) {
            achievementPlates[15].color = new Color32(255,217,160,255);
            achievementDescriptions[15].color = new Color32(113,48,0,255);
            achievementDescriptions[15].text = "Enjoy a drink while pissing yourself";
            gm.omoScenarioUnlocked++;
        } else if (!scnDrinkWhilePee) {
            achievementPlates[15].color = new Color32(67,24,7,255);
            achievementDescriptions[15].color = new Color32(255,255,255,255);
            achievementDescriptions[15].text = "Unobtained";
        }

        if(scnDiaperWhilePee) {
            achievementPlates[16].color = new Color32(255,217,160,255);
            achievementDescriptions[16].color = new Color32(113,48,0,255);
            achievementDescriptions[16].text = "Tried sitting on a diaper while peeing yourself";
            gm.omoScenarioUnlocked++;
        } else if (!scnDiaperWhilePee) {
            achievementPlates[16].color = new Color32(67,24,7,255);
            achievementDescriptions[16].color = new Color32(255,255,255,255);
            achievementDescriptions[16].text = "Unobtained";
        }

        if(scnBuyDiaperWhilePee) {
            achievementPlates[17].color = new Color32(255,217,160,255);
            achievementDescriptions[17].color = new Color32(113,48,0,255);
            achievementDescriptions[17].text = "Bought a diaper while pissing yourself";
            gm.omoScenarioUnlocked++;
        } else if (!scnBuyDiaperWhilePee) {
            achievementPlates[17].color = new Color32(67,24,7,255);
            achievementDescriptions[17].color = new Color32(255,255,255,255);
            achievementDescriptions[17].text = "Unobtained";
        }

        if(scnFreePeeing) {
            achievementPlates[18].color = new Color32(255,217,160,255);
            achievementDescriptions[18].color = new Color32(113,48,0,255);
            achievementDescriptions[18].text = "Decided to just let it all out";
            gm.omoScenarioUnlocked++;
        } else if (!scnFreePeeing) {
            achievementPlates[18].color = new Color32(67,24,7,255);
            achievementDescriptions[18].color = new Color32(255,255,255,255);
            achievementDescriptions[18].text = "Unobtained";
        }

        if(scnRoomieIntoDiaper) {
            achievementPlates[19].color = new Color32(255,217,160,255);
            achievementDescriptions[19].color = new Color32(113,48,0,255);
            achievementDescriptions[19].text = "Your roommate likes seeing you in diapers";
            gm.omoScenarioUnlocked++;
        } else if (!scnRoomieIntoDiaper) {
            achievementPlates[19].color = new Color32(67,24,7,255);
            achievementDescriptions[19].color = new Color32(255,255,255,255);
            achievementDescriptions[19].text = "Unobtained";
        }

        if(scnPlayWithPee) {
            achievementPlates[20].color = new Color32(255,217,160,255);
            achievementDescriptions[20].color = new Color32(113,48,0,255);
            achievementDescriptions[20].text = "Played with yourself after wetting";
            gm.omoScenarioUnlocked++;
        } else if (!scnPlayWithPee) {
            achievementPlates[20].color = new Color32(67,24,7,255);
            achievementDescriptions[20].color = new Color32(255,255,255,255);
            achievementDescriptions[20].text = "Unobtained";
        }

        if(scnRoomieAwakening) {
            achievementPlates[21].color = new Color32(255,217,160,255);
            achievementDescriptions[21].color = new Color32(113,48,0,255);
            achievementDescriptions[21].text = "Your roommate wants to watch you pee yourself";
            gm.omoScenarioUnlocked++;
        } else if (!scnRoomieAwakening) {
            achievementPlates[21].color = new Color32(67,24,7,255);
            achievementDescriptions[21].color = new Color32(255,255,255,255);
            achievementDescriptions[21].text = "Unobtained";
        }

        if(scnDescribeLeakyDiapers) {
            achievementPlates[22].color = new Color32(255,217,160,255);
            achievementDescriptions[22].color = new Color32(113,48,0,255);
            achievementDescriptions[22].text = "Described the feeling of leaky diapers to your roommate";
            gm.omoScenarioUnlocked++;
        } else if (!scnDescribeLeakyDiapers) {
            achievementPlates[22].color = new Color32(67,24,7,255);
            achievementDescriptions[22].color = new Color32(255,255,255,255);
            achievementDescriptions[22].text = "Unobtained";
        }

        if(scnDescribeDiaperPee) {
            achievementPlates[23].color = new Color32(255,217,160,255);
            achievementDescriptions[23].color = new Color32(113,48,0,255);
            achievementDescriptions[23].text = "Described the feeling of peeing in your diapers to your roommate";
            gm.omoScenarioUnlocked++;
        } else if (!scnDescribeDiaperPee) {
            achievementPlates[23].color = new Color32(67,24,7,255);
            achievementDescriptions[23].color = new Color32(255,255,255,255);
            achievementDescriptions[23].text = "Unobtained";
        }

        if(scnPeeOnBed) {
            achievementPlates[24].color = new Color32(255,217,160,255);
            achievementDescriptions[24].color = new Color32(113,48,0,255);
            achievementDescriptions[24].text = "Climb on to your bed as pee still flows out of you";
            gm.omoScenarioUnlocked++;
        } else if (!scnPeeOnBed) {
            achievementPlates[24].color = new Color32(67,24,7,255);
            achievementDescriptions[24].color = new Color32(255,255,255,255);
            achievementDescriptions[24].text = "Unobtained";
        }

        if(scnDiaperLeakOnBed) {
            achievementPlates[25].color = new Color32(255,217,160,255);
            achievementDescriptions[25].color = new Color32(113,48,0,255);
            achievementDescriptions[25].text = "Climb on to your bed while leaking through your diapers";
            gm.omoScenarioUnlocked++;
        } else if (!scnDiaperLeakOnBed) {
            achievementPlates[25].color = new Color32(67,24,7,255);
            achievementDescriptions[25].color = new Color32(255,255,255,255);
            achievementDescriptions[25].text = "Unobtained";
        }

        if(scnDiaperPeeOnBed) {
            achievementPlates[26].color = new Color32(255,217,160,255);
            achievementDescriptions[26].color = new Color32(113,48,0,255);
            achievementDescriptions[26].text = "Climb on to your bed while peeing your diapers";
            gm.omoScenarioUnlocked++;
        } else if (!scnDiaperPeeOnBed) {
            achievementPlates[26].color = new Color32(67,24,7,255);
            achievementDescriptions[26].color = new Color32(255,255,255,255);
            achievementDescriptions[26].text = "Unobtained";
        }

        if(scnWaterBottlePee) {
            achievementPlates[27].color = new Color32(255,217,160,255);
            achievementDescriptions[27].color = new Color32(113,48,0,255);
            achievementDescriptions[27].text = "Overfilled a water bottle with your pee";
            gm.omoScenarioUnlocked++;
        } else if (!scnWaterBottlePee) {
            achievementPlates[27].color = new Color32(67,24,7,255);
            achievementDescriptions[27].color = new Color32(255,255,255,255);
            achievementDescriptions[27].text = "Unobtained";
        }

        if(scnHalfMadeIt) {
            achievementPlates[28].color = new Color32(255,217,160,255);
            achievementDescriptions[28].color = new Color32(113,48,0,255);
            achievementDescriptions[28].text = "Left a trail of pee as you tried to go to the bathroom";
            gm.omoScenarioUnlocked++;
        } else if (!scnHalfMadeIt) {
            achievementPlates[28].color = new Color32(67,24,7,255);
            achievementDescriptions[28].color = new Color32(255,255,255,255);
            achievementDescriptions[28].text = "Unobtained";
        }

        if(scnWetTheBed) {
            achievementPlates[29].color = new Color32(255,217,160,255);
            achievementDescriptions[29].color = new Color32(113,48,0,255);
            achievementDescriptions[29].text = "Wet the bed";
            gm.omoScenarioUnlocked++;
        } else if (!scnWetTheBed) {
            achievementPlates[29].color = new Color32(67,24,7,255);
            achievementDescriptions[29].color = new Color32(255,255,255,255);
            achievementDescriptions[29].text = "Unobtained";
        }

        if(scnPreventWetBed) {
            achievementPlates[30].color = new Color32(255,217,160,255);
            achievementDescriptions[30].color = new Color32(113,48,0,255);
            achievementDescriptions[30].text = "Filled your diaper throughout the night";
            gm.omoScenarioUnlocked++;
        } else if (!scnPreventWetBed) {
            achievementPlates[30].color = new Color32(67,24,7,255);
            achievementDescriptions[30].color = new Color32(255,255,255,255);
            achievementDescriptions[30].text = "Unobtained";
        }

        if(scnMorningPeeOnBed) {
            achievementPlates[31].color = new Color32(255,217,160,255);
            achievementDescriptions[31].color = new Color32(113,48,0,255);
            achievementDescriptions[31].text = "Pissed the bed after waking up dry";
            gm.omoScenarioUnlocked++;
        } else if (!scnMorningPeeOnBed) {
            achievementPlates[31].color = new Color32(67,24,7,255);
            achievementDescriptions[31].color = new Color32(255,255,255,255);
            achievementDescriptions[31].text = "Unobtained";
        }

        if(scnHorrorHalfPee) {
            achievementPlates[32].color = new Color32(255,217,160,255);
            achievementDescriptions[32].color = new Color32(113,48,0,255);
            achievementDescriptions[32].text = "Didn't lose control while playing a horror game";
            gm.omoScenarioUnlocked++;
        } else if (!scnHorrorHalfPee) {
            achievementPlates[32].color = new Color32(67,24,7,255);
            achievementDescriptions[32].color = new Color32(255,255,255,255);
            achievementDescriptions[32].text = "Unobtained";
        }

        if(scnHorrorFullPee) {
            achievementPlates[33].color = new Color32(255,217,160,255);
            achievementDescriptions[33].color = new Color32(113,48,0,255);
            achievementDescriptions[33].text = "Lost all bladder control while playing a horror game";
            gm.omoScenarioUnlocked++;
        } else if (!scnHorrorFullPee) {
            achievementPlates[33].color = new Color32(67,24,7,255);
            achievementDescriptions[33].color = new Color32(255,255,255,255);
            achievementDescriptions[33].text = "Unobtained";
        }

        if(scnOmogeRoomieDiaper) {
            achievementPlates[34].color = new Color32(255,217,160,255);
            achievementDescriptions[34].color = new Color32(113,48,0,255);
            achievementDescriptions[34].text = "Put on a show for your diaper benefactor";
            gm.omoScenarioUnlocked++;
        } else if (!scnHorrorFullPee) {
            achievementPlates[34].color = new Color32(67,24,7,255);
            achievementDescriptions[34].color = new Color32(255,255,255,255);
            achievementDescriptions[34].text = "Unobtained";
        }

        if(scnOmogeLendADiaper) {
            achievementPlates[35].color = new Color32(255,217,160,255);
            achievementDescriptions[35].color = new Color32(113,48,0,255);
            achievementDescriptions[35].text = "Your friend peed inside your diaper on an omoge";
            gm.omoScenarioUnlocked++;
        } else if (!scnHorrorFullPee) {
            achievementPlates[35].color = new Color32(67,24,7,255);
            achievementDescriptions[35].color = new Color32(255,255,255,255);
            achievementDescriptions[35].text = "Unobtained";
        }

        if(scnOmogeSecretPee) {
            achievementPlates[36].color = new Color32(255,217,160,255);
            achievementDescriptions[36].color = new Color32(113,48,0,255);
            achievementDescriptions[36].text = "Peed stealthily without getting caught on an omoge";
            gm.omoScenarioUnlocked++;
        } else if (!scnHorrorFullPee) {
            achievementPlates[36].color = new Color32(67,24,7,255);
            achievementDescriptions[36].color = new Color32(255,255,255,255);
            achievementDescriptions[36].text = "Unobtained";
        }
    }


}
