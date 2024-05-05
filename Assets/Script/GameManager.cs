using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GameManager : MonoBehaviour, IDataPersistence
{
    private DialogueManager dm;
    private PriceManager pm;
    private AchievementManager am;
    //private PlayerData playData;
    public AudioSource audioSource, bgmAudioSource;
    public AudioClip[] gameAudios, gameBGM;
    public Image holdSprite, handImage, despLevelBG, CGImage, CGBGImage;
    public Sprite[] boxersprites, jeansprites, diapersprites, handsprites, despLevelSprites, bedPeeSprites, bedPeeBoxersSprites,
                    workPeeJeansSprites, workPeeBoxersSprites, workPeeBackgroundSprites;
    public GameObject pissObject;
    public ParticleSystem stream;
    public GameObject showState, showPose, showConfirmUse, showConfirmBuy, showItems, showCleanUp, showDesk, showLetGo, showStore, showGames, showRoommate;
    public GameObject showGameEvent, showGeneralConfirm, showNightTime, showCG, showOptions, showConfirmSave, showSavedText, showAchievementPanel, showCheatPanel;
    public GameObject bedProtectionOpt;
    public Text gameEventSynopsis, gameEventChoiceOne, gameEventChoiceTwo, generalConfirmSynopsis, achievementText, bedProtectText;
    public bool wearBoxers, wearJeans, isHolding, isPeeing, isProtected, isRoommate, isSoaked, letGo, isWatched, roomieWatch, roomieBanned, isWorking;
    public bool firstTimeBuyDiaper, firstTimeUseDiaper, roommateFirstWitnessDiaper, bedProtected, hasBedProtector, isHorrorMax;
    public int desperation, workEventInd;
    public int wetTimes, caughtTimes, omoScenarioUnlocked, money;
    public Text confirmText, confirmTextBuy, wetTimesText, caughtTimesText, omoRankText, desperationLevel, moneyAmountText, buyMoneyAmountText;
    public Text waterPriceText, coffeePriceText, diaperPriceText, shooterPriceText, horrorPriceText, omogePriceText, mattressProtectorPriceText;
    public Text cheatCodeText, cheatActivationText;
    public Button shooterButton, horrorButton, omogeButton, buyOmogeButton;
    public bool boughtShooter, boughtHorror, boughtOmoge, canBuyOmoge, introDone, roommateDelay, omoChooseDiaper;
    public int waterStock, coffeeStock, diaperStock;
    public Text[] itemStockText;
    private string selectedItem, omoRank;
    public int leakState, tempdesperation, tier1desperation, tier2desperation, tier3desperation, tier4desperation;

    // Start is called before the first frame update
    void Start()
    {
        bgmAudioSource.clip = gameBGM[0];
        bgmAudioSource.Play();
        dm = FindObjectOfType<DialogueManager>();
        pm = FindObjectOfType<PriceManager>();
        am = FindObjectOfType<AchievementManager>();
        tier1desperation = 75;
        tier2desperation = 100;
        tier3desperation = 120;
        tier4desperation = 135;
        roommateDelay = false;
        //first
        if(!introDone) {
            dm.StartDialogue("intro");
            introDone = true;
        }

        if(wetTimes >= 3) {
            showLetGo.SetActive(true);
        } else {
            showLetGo.SetActive(false);
        }

        if(canBuyOmoge == true) {
            buyOmogeButton.interactable = true;
        } else {
            buyOmogeButton.interactable = false;
        }

        if(roomieWatch) {
            isRoommate = true;
            showRoommate.SetActive(true);
        }
        
        if(isSoaked) {
            showCleanUp.SetActive(true);
        }

        if(bedProtected && hasBedProtector == false) {
            //this is to automatically update previous saved file that doesn't have this variable
            hasBedProtector = true;
            bedProtectText.text = "Protected";
        } else {
            bedProtectText.text = "Unprotected";
        }

        if(hasBedProtector) {
            bedProtectionOpt.SetActive(true);
        } else {
            bedProtectionOpt.SetActive(false);
        }

        updateDesperation();
        StartCoroutine(checkDelay());
        StartCoroutine(desperationPerSecond());
        //StartCoroutine(bedPeePlay());
    }

    void Update() {
        if(desperation >= tier1desperation && desperation < tier2desperation && leakState == 0) {
            if(!isProtected) {
                if(isSoaked) {
                    dm.sentences.Enqueue("You: (I haven't cleaned up, anyway... A little leak won't hurt.)");
                    dm.sentences.Enqueue("Thinking this, you feel like you might as well have another accident soon. You get more desperate.");
                    desperation += 10;
                } else {
                    //dm.showDialogue("You: Ah.. I think I leaked a little..");
                    dm.sentences.Enqueue("You: Ah.. I think I leaked a little..");
                    if(roomieWatch) {
                        dm.sentences.Enqueue("Roomie: Don't be shy, leak more.");
                        dm.sentences.Enqueue("You: Shit, I said that out loud?");
                        dm.sentences.Enqueue("Your roommate just laughs.");
                    }
                }
            } else {
                if(isSoaked) {
                    //dm.showDialogue("I pee a bit into my already soggy diaper. If I let more out, it might start leaking...");
                    dm.sentences.Enqueue("I pee a bit into my already soggy diaper. If I let more out, it might start leaking...");
                } else {
                    dm.sentences.Enqueue("I pee a bit into my diaper. My crotch gets slightly warmer.");
                }
                desperation += 10;
            }
            //leakState++;
            leakState = 1;
            audioSource.clip = gameAudios[0];
            audioSource.Play();
            //audioSource.PlayOneShot(gameAudios[0]);
            checkState();
            leakPee(); 
            dm.DisplayCurrSentence();
        } else if(desperation >= tier2desperation && desperation < tier3desperation && leakState <= 1) {
            if(!isProtected) {
                if(isSoaked) {
                    dm.sentences.Enqueue("You: (I think I won't be able to hold it for longer...)");
                    desperation += 10;
                } else {
                    dm.sentences.Enqueue("You: Shit, I leaked again...");
                    if(roomieWatch) {
                        dm.sentences.Enqueue("Roomie: What's stopping you from just letting go?");
                        dm.sentences.Enqueue("You: Don't you find the desperation sexy?");
                        dm.sentences.Enqueue("Roomie: ...Okay, you're right. It is sexy.");
                    }
                }
            } else {
                if(isSoaked) {
                    dm.sentences.Enqueue("More pee came out of me and I can feel my diaper leaking.");
                    if(roomieWatch) {
                        dm.sentences.Enqueue("Roomie: Dude, I think your diaper's leaking...");
                        dm.sentences.Enqueue("You: Just as planned.");
                        dm.sentences.Enqueue("Roomie: I see... Wish I can see inside.");
                        dm.sentences.Enqueue("You: You gotta pay premium for that.");
                        dm.sentences.Enqueue("Your roommate snickers.");
                    }
                } else {
                    dm.sentences.Enqueue("Another stream of pee shot out from me, hitting the diaper. It gets warmer.");
                    desperation += 10;
                }
            }
            //leakState++;
            leakState = 2;
            //audioSource.PlayOneShot(gameAudios[1]);
            audioSource.clip = gameAudios[1];
            audioSource.Play();
            checkState();
            leakPee();
            roommateCheck();
            dm.DisplayCurrSentence();
        } else if(desperation >= tier3desperation && desperation < tier4desperation && leakState <= 2) {
            if(!isProtected) {
                if(isSoaked) {
                    dm.sentences.Enqueue("You: (Mmm.. I'll just pee myself again before cleaning up.)");
                    if(roomieWatch) {
                        dm.sentences.Enqueue("Roomie: Doing a rewetting?");
                        dm.sentences.Enqueue("You: Mm, yeah.");
                        dm.sentences.Enqueue("Roomie: Hope you don't mind me watching... and stuff.");
                        dm.sentences.Enqueue("You: It's actually kinda hot having someone watch... and stuff.");
                        dm.sentences.Enqueue("He blushes.");
                        dm.sentences.Enqueue("Roomie: R-right.");
                    }
                } else {
                    dm.sentences.Enqueue("You: This is getting really dangerous, I think it's starting to leak out..");
                    if(roomieWatch) {
                        dm.sentences.Enqueue("Your roommate hears the hiss and immediately looks your way, waiting for the waterfall to start again.");
                    }
                }
            } else {
                if(isSoaked) {
                    dm.sentences.Enqueue("More pee keeps coming out of me, steadily overflowing the diaper. I can feel my pants getting wet as pee starts leaking out from the sides of my diaper.");
                    if(roomieWatch) {
                        dm.sentences.Enqueue("Your roommate keeps his gaze on you, examining the flow of the leaks happening inside carefully, imagining the soggy diaper.");
                    }
                    dm.sentences.Enqueue("SYSTEM: Sorry, the diaper leaking sprites are not yet available and will be updated later in the future.");
                } else {
                    dm.sentences.Enqueue("More pee keeps coming out of me, steadily filling the diaper. The feeling of wet diaper makes it hard for me to stop.");
                    if(roomieWatch) {
                        dm.sentences.Enqueue("Your roommate notices how your pants is slightly stretching bigger. He watches you.");
                    }
                    desperation += 5;
                }
            }
            //leakState++;
            leakState = 3;
            //audioSource.PlayOneShot(gameAudios[2]);
            audioSource.clip = gameAudios[2];
            audioSource.Play();
            checkState();
            leakPee();
            roommateCheck();
            dm.DisplayCurrSentence();
        } else if(desperation >= tier4desperation && leakState <= 3) {
            if(!isProtected) {
                if(isSoaked) {
                    dm.sentences.Enqueue("You: (Ah.. I'm peeing again..)");
                    if(roomieWatch) {
                        dm.sentences.Enqueue("Your roommate silently shoves his hand under his pants.");
                    }
                    if(!am.scnRewetting) {
                        //omoScenarioUnlocked++;
                        am.scnRewetting = true;
                    }
                } else {
                    dm.sentences.Enqueue("You: (Oh fuck! I can't stop it!)");
                    if(roomieWatch) {
                        dm.sentences.Enqueue("Your roommate giddily watches your panicked face as you start losing control of your bladder. He silently shoves his hand under his pants.");
                    }
                    if(!am.scnDesperatePeeing) {
                        //omoScenarioUnlocked++;
                        am.scnDesperatePeeing = true;
                    }
                }
                //wetYourself();
            } else {
                if(isSoaked) {
                    dm.sentences.Enqueue("I start peeing uncontrollably now. The small lake that was sitting below me is now flooding up and releasing its contents into my pants.");
                    dm.sentences.Enqueue("You: (Guess I'm peeing my jeans now...)");
                    if(roomieWatch && !roommateFirstWitnessDiaper) {
                        dm.sentences.Enqueue("Roomie: How does it feel?");
                        dm.sentences.Enqueue("You: It's really warm. Feels like I'm submerged in a sea of my own freshly produced piss. It's pooling up and leaking out, and now I can feel hot pee being poured onto my legs...");
                        dm.sentences.Enqueue("Roomie: ...No need to make it sound so hot.");
                        dm.sentences.Enqueue("He starts rubbing himself.");
                        if(!am.scnDescribeLeakyDiapers) {
                            //omoScenarioUnlocked++;
                            am.scnDescribeLeakyDiapers = true;
                        }
                    }
                    if(!am.scnDiaperLeak) {
                        //omoScenarioUnlocked++;
                        am.scnDiaperLeak = true;
                    }
                    //wetYourself();
                } else {          
                    dm.sentences.Enqueue("Before I knew it, what was once a small stream has now become a river. Feeling protected, I decide to just let it all go. I find my crotch area swimming in a lake of hot piss.");
                    if(roomieWatch && !roommateFirstWitnessDiaper) {
                        dm.sentences.Enqueue("How does it feel?");
                        dm.sentences.Enqueue("You: It's really warm. Feels like I'm submerged in a sea of my own freshly produced piss. I can also clearly feel the way my pee is hitting the diaper below me. It feels really good...");
                        dm.sentences.Enqueue("I touch and rub the diaper beneath my jeans, feeling it steadily expand and rising in temperature.");
                        dm.sentences.Enqueue("Roomie: ...That's not fair.");
                        dm.sentences.Enqueue("He starts rubbing himself.");
                        if(!am.scnDescribeDiaperPee) {
                            //omoScenarioUnlocked++;
                            am.scnDescribeDiaperPee = true;
                        }
                    }
                    if(!am.scnDiaperPee) {
                        //omoScenarioUnlocked++;
                        am.scnDiaperPee = true;
                    }
                }
            }
            //leakState++;
            leakState = 4;
            checkState();
            letGo = true;
            leakPee();
            roommateCheck();
            dm.DisplayCurrSentence();
        }
    }

    public void LoadData(PlayerData data) {
        this.desperation = data.desperation;
        this.leakState = data.leakState;
        this.money = data.money;
        this.isProtected = data.isProtected;
        this.bedProtected = data.bedProtected;
        this.hasBedProtector = data.hasBedProtector;
        this.wearBoxers = data.wearBoxers;
        this.wearJeans = data.wearJeans;
        this.isSoaked = data.isSoaked;
        this.isWatched = data.isWatched;
        this.roomieWatch = data.roomieWatch;
        this.firstTimeBuyDiaper = data.firstTimeBuyDiaper;
        this.firstTimeUseDiaper = data.firstTimeUseDiaper;
        this.roommateFirstWitnessDiaper = data.roommateFirstWitnessDiaper;
        this.wetTimes = data.wetTimes;
        this.caughtTimes = data.caughtTimes;
        this.omoScenarioUnlocked = data.omoScenarioUnlocked;
        this.waterStock = data.waterStock;
        this.coffeeStock = data.coffeeStock;
        this.diaperStock = data.diaperStock;
        this.boughtShooter = data.boughtShooter;
        this.boughtHorror = data.boughtHorror;
        this.boughtOmoge = data.boughtOmoge;
        this.canBuyOmoge = data.canBuyOmoge;
        this.introDone = data.introDone;
    }

    public void SaveData(ref PlayerData data) {
        data.desperation = this.desperation;
        data.leakState = this.leakState;
        data.money = this.money;
        data.isProtected = this.isProtected;
        data.bedProtected = this.bedProtected;
        data.hasBedProtector = this.hasBedProtector;
        data.wearBoxers = this.wearBoxers;
        data.wearJeans = this.wearJeans;
        data.isSoaked = this.isSoaked;
        data.isWatched = this.isWatched;
        data.roomieWatch = this.roomieWatch;
        data.firstTimeBuyDiaper = this.firstTimeBuyDiaper;
        data.firstTimeUseDiaper = this.firstTimeUseDiaper;
        data.roommateFirstWitnessDiaper = this.roommateFirstWitnessDiaper;
        data.wetTimes = this.wetTimes;
        data.caughtTimes = this.caughtTimes;
        data.omoScenarioUnlocked = this.omoScenarioUnlocked;
        data.waterStock = this.waterStock;
        data.coffeeStock = this.coffeeStock;
        data.diaperStock = this.diaperStock;
        data.boughtShooter = this.boughtShooter;
        data.boughtHorror = this.boughtHorror;
        data.boughtOmoge = this.boughtOmoge;
        data.canBuyOmoge = this.canBuyOmoge;
        data.introDone = this.introDone;
    }

    IEnumerator checkDelay() {
        while(true) {
            yield return new WaitForSeconds(30.0f);
            if(!isRoommate && !dm.dialogueBox.activeSelf && !isWorking && 
                !showConfirmBuy.activeSelf &&
                !showConfirmUse.activeSelf &&
                !showGameEvent.activeSelf &&
                !showCG.activeSelf &&
                !showGeneralConfirm.activeSelf &&
                !showOptions.activeSelf &&
                !showCheatPanel.activeSelf &&
                //!showItems.activeSelf &&
                !showDesk.activeSelf &&
                !roommateDelay) {
                //!showStore.activeSelf) {
                roommateCheck();
            } else if(roommateDelay) {
                roommateDelay = false;
            }
        }
    }

    IEnumerator desperationPerSecond() {
        while(true) {
            yield return new WaitForSeconds(1.0f);
            if(!dm.dialogueBox.activeSelf && !isWorking && 
                !showConfirmBuy.activeSelf &&
                !showConfirmUse.activeSelf &&
                !showGameEvent.activeSelf &&
                !showCG.activeSelf &&
                !showGeneralConfirm.activeSelf &&
                //!showItems.activeSelf &&
                !showDesk.activeSelf) {
                //!showStore.activeSelf) {
                desperation++;
                updateDesperation();
            }
        }
    }

    private void updateDesperation() {
        if(desperation <= 49) {
            despLevelBG.sprite = despLevelSprites[0];
            desperationLevel.color = new Color(0.368f,0.133f,0f);
        } else if(desperation >= 50 && desperation <= 74) {
            despLevelBG.sprite = despLevelSprites[1];
            desperationLevel.color = new Color(0.368f,0.133f,0f);
        } else if(desperation >= 75 && desperation <= 99) {
            despLevelBG.sprite = despLevelSprites[2];
            desperationLevel.color = new Color(0.368f,0.133f,0f);
        } else if(desperation >= 100 && desperation <= 119) {
            despLevelBG.sprite = despLevelSprites[3];
            desperationLevel.color = new Color(0.368f,0.133f,0f);
        } else if(desperation >= 120 && desperation <= 135) {
            despLevelBG.sprite = despLevelSprites[4];
            desperationLevel.color = new Color(0.368f,0.133f,0f);
        } else if(desperation >= 135) {
            despLevelBG.sprite = despLevelSprites[5];
            desperationLevel.color = Color.white;
        }
        desperationLevel.text = desperation + "% desperate!";
    }

    public void bedProtectorSetting() {
        if(bedProtected) {
            bedProtected = false;
            dm.sentences.Enqueue("You take off your mattress protector.");
            bedProtectText.text = "Unprotected";
        } else {
            bedProtected = true;
            dm.sentences.Enqueue("You cover your bed with a mattress protector.");
            bedProtectText.text = "Protected";
        }
        dm.DisplayCurrSentence();
    }

    public void cheatPanelTrigger() {
        if(showCheatPanel.activeSelf) {
            showCheatPanel.SetActive(false);
            cheatActivationText.text = "";
        } else {
            showCheatPanel.SetActive(true);
        }
    }

    public void cheatCodeSetting() {
        if(cheatCodeText.text == "peeradise") {
            money += 1000;
            cheatActivationText.text = "Money cheat activated!";
        } else if(cheatCodeText.text == "solopiss") {
            roomieWatch = false;
            isRoommate = false;
            isWatched = false;
            showRoommate.SetActive(false);
            cheatActivationText.text = "Roomie Watch mode disabled!";
        } else if(cheatCodeText.text == "watchmepee") {
            roomieWatch = true;
            roomieBanned = false;
            isRoommate = true;
            isWatched = false;
            showRoommate.SetActive(true);
            cheatActivationText.text = "Roomie Watch mode enabled!";
        } else if(cheatCodeText.text == "banroomie") {
            roomieBanned = true;
            cheatActivationText.text = "Roomie Watch mode banned!";
        } else if(cheatCodeText.text == "unbanroomie") {
            roomieBanned = false;
            cheatActivationText.text = "Roomie Watch ban lifted!";
        } else if(cheatCodeText.text == "amnesiacroomie") {
            //This resets roomie's memories of you wearing diapers & resets amount of time you got caught
            roommateFirstWitnessDiaper = true;
            caughtTimes  = 0;
            cheatActivationText.text = "Roomie has forgotten everything!";
        }
    }

    public void showOptionsPanel() {
        showOptions.SetActive(true);
    }

    public void closeOptionsPanel() {
        showOptions.SetActive(false);
    }

    public void showConfirmSavePanel() {
        showConfirmSave.SetActive(true);
    }

    public void closeConfirmSavePanel() {
        showConfirmSave.SetActive(false);
    }

    public IEnumerator savedText() {
        showSavedText.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        showSavedText.SetActive(false);
        showConfirmSave.SetActive(false);
    }

    public void confirmExitGame() {
        generalConfirmSynopsis.text = "Exit to desktop?";
        showGeneralConfirm.SetActive(true);
    }

    private void updateAchievementStatus() {
        achievementText.text = omoScenarioUnlocked + "/37";
    }

    public void showAchievements() {
        am.updateAchievementsPanel();
        updateAchievementStatus();
        showAchievementPanel.SetActive(true);
    }

    public void closeAchievements() {
        showAchievementPanel.SetActive(false);
    }

    private void roommateCheck() {
        int roommateAppear = Random.Range(0,2);
        //int roommateAppear = 0;
        print("roomCheck = " + roommateAppear);
        if (roommateAppear == 1 && !isRoommate && !isWatched && !roomieWatch) {
            //Everything that happens in this branch is strictly normal route aka roommate walks in on you normally.
            //However, it is possible that your roommate has already seen you wet yourself previously.
            //This is triggered whenever you leak pee / every 5 seconds without dialogue.
            //To trigger isWatched, you need to be:
            // * have peed yourself 3 times
            // a. leaking in your already-soaked pants, with your roommate already witnessing you pee yourself before (in action / aftermath).
            // b. stay in your soaked pants, with your roommate already witnessing you pee yourself before (in action / aftermath).
            showRoommate.SetActive(true);
            dm.sentences.Enqueue("You hear some clicking at the front door and following that, your roommate comes in.");
            isRoommate = true;
            //Check pee conditions
            if(!isPeeing) {
                if(!isProtected) {
                    if(!isSoaked) {
                        switch(leakState) {
                            case 0:
                                if(caughtTimes == 0) {
                                    dm.sentences.Enqueue("Roomie: Hey, I'm just grabbing something, don't mind me.");
                                    dm.sentences.Enqueue("He grabs something from his drawer and leaves quickly.");
                                } else {
                                    //Have been caught before
                                    dm.sentences.Enqueue("Roomie: Oh, you already cleaned up.. I'll just be quick.");
                                    dm.sentences.Enqueue("He grabs something from his drawer and leaves quickly.");
                                }
                                isRoommate = false;
                            break;

                            case 1:
                                if(caughtTimes == 0) {
                                        dm.sentences.Enqueue("Roomie: Hey, I'm just grabbing something, don't mind me.");
                                        dm.sentences.Enqueue("He grabs something from his drawer and leaves quickly.");
                                    } else {
                                        //Have been caught before
                                        dm.sentences.Enqueue("He immediately inspects my pants.");
                                        dm.sentences.Enqueue("Roomie: Oh, you cleaned up.. But are you leaking again?");
                                        dm.sentences.Enqueue("You: Er, just a bit...");
                                        dm.sentences.Enqueue("Roomie: Hmm...");
                                        dm.sentences.Enqueue("He grabs something from his drawer and leaves.");
                                    }
                                    isRoommate = false;
                            break;

                            case 2:
                                if(caughtTimes == 0) {
                                    dm.sentences.Enqueue("Roomie: Yo.");
                                    dm.sentences.Enqueue("You: Yo, where have you been?");
                                    dm.sentences.Enqueue("Roomie: Eh, you know... Places. Anyways, I'm not here for long, just taking some stuff with me.");
                                    dm.sentences.Enqueue("He grabs a water bottle from his desk and heads to the door again.");
                                    dm.sentences.Enqueue("Roomie: Be seeing ya.");
                                    dm.sentences.Enqueue("He leaves and now you're alone in your room again.");
                                    dm.sentences.Enqueue("You: (Good, he didn't notice me leaking at all.)");
                                } else {
                                    //Have been caught before
                                    dm.sentences.Enqueue("He immediately inspects my pants.");
                                    dm.sentences.Enqueue("Roomie: Careful, you might wet yourself again.");
                                    dm.sentences.Enqueue("You: Y-Yeah.");
                                    dm.sentences.Enqueue("Roomie: Hmm...");
                                    dm.sentences.Enqueue("He grabs something from his drawer and leaves.");
                                }
                                isRoommate = false;
                                break;

                            case 3:
                                if(caughtTimes == 0) {
                                    dm.sentences.Enqueue("Your roommate looks at you and greets you.");
                                    dm.sentences.Enqueue("Roomie: Hey there, uh... You all good down there? Did the bathroom break down or something?");
                                    dm.sentences.Enqueue("His gaze is pointed towards your dampened crotch.");
                                    dm.sentences.Enqueue("You: Actually yeah, toilet broke, I called maintenance but they haven't come yet...");
                                    dm.sentences.Enqueue("Roomie: Well shit. Can you hold it? I can ask my friend to lend you the bathroom in their room...");
                                    dm.sentences.Enqueue("You: I think I'll be fine... Probably...");
                                    dm.sentences.Enqueue("Roomie: If you're sure about it.");
                                    dm.sentences.Enqueue("He takes some things from his drawers and heads to the door again.");
                                    dm.sentences.Enqueue("Roomie: Anyways, I'll be out again for now.");
                                    dm.sentences.Enqueue("He leaves and now you're alone in your room again.");
                                    dm.sentences.Enqueue("You: (Well... At least he didn't come in while I'm having an accident or anything...)");
                                    isRoommate = false;
                                } else {
                                    //Have been caught before
                                    dm.sentences.Enqueue("He immediately inspects my pants.");
                                    if(wetTimes >= 3) {
                                        roommateWantsToSee();
                                    } else {
                                        dm.sentences.Enqueue("Roomie: You're leaking quite a lot... You know, you can just go behind a bush or something...");
                                        dm.sentences.Enqueue("You: I-I'll go in a bit...");
                                        dm.sentences.Enqueue("His eyes are still locked on to your wet crotch.");
                                        dm.sentences.Enqueue("You're on the brink of pissing yourself and now you're feeling the pressure of being watched.");
                                        dm.sentences.Enqueue("You suddenly leak another small spurt, visibly wetting your pants more.");
                                        dm.sentences.Enqueue("Your roommate's eyes widen and he immediately turned away.");
                                        dm.sentences.Enqueue("Roomie: Um.. Well, I'm just taking something out and I'll be off.");
                                        dm.sentences.Enqueue("He quickly grabs something from his desk and leaves hurriedly.");
                                        isRoommate = false;
                                    }
                                }
                                break;

                            //case 4-7 is impossible because it would've entered the isPeeing branch.
                            //case 8 is impossible because it would've entered the isSoaked branch.
                        }
                    } else {
                        //isSoaked
                        if(caughtTimes == 0) {
                            dm.sentences.Enqueue("Roomie: ...");
                            dm.sentences.Enqueue("Roomie: Woah, dude.. Did you just... wet yourself?");
                            dm.sentences.Enqueue("You: Uh. Maybe...");
                            dm.sentences.Enqueue("Roomie: What happened?");
                            dm.sentences.Enqueue("You: Well, our toilet's busted and I thought I could just hold it in until it's fixed..");
                            dm.sentences.Enqueue("Roomie: Right...");
                            dm.sentences.Enqueue("For some reason, he keeps staring at my soaked pants and the puddle.");
                            dm.sentences.Enqueue("You: I'll clean it up soon, sorry.");
                            dm.sentences.Enqueue("Roomie: Good luck. That's a... pretty big puddle you got there.");
                            dm.sentences.Enqueue("You: Yeah, I don't know why but I peed so much...");
                            dm.sentences.Enqueue("Roomie: I can tell...");
                            dm.sentences.Enqueue("You: (He's still staring at it. Why does he keep staring? This is too embarassing.)");
                            dm.sentences.Enqueue("You: Anyway, don't you have something to do?");
                            dm.sentences.Enqueue("Roomie: Ah, right. I'll uh.. just grab these and leave. See ya.");
                            dm.sentences.Enqueue("He quickly grabs something from his drawers and left.");
                            dm.sentences.Enqueue("You: (Can't believe he saw me like this... Better get changed soon.)");
                            isRoommate = false;
                            caughtTimes++;
                            if(!am.scnCaughtWetPants) {
                                //omoScenarioUnlocked++;
                                am.scnCaughtWetPants = true;
                            }
                        } else {
                            //Have been caught before
                                dm.sentences.Enqueue("Roomie: You're still wearing those?");
                                dm.sentences.Enqueue("You: Uh, yeah...");
                                if(leakState >= 1) {
                                    //Is re-wetting, but still on the leaking phase
                                    dm.sentences.Enqueue("He notices a newly-made glistening part on your pants.");
                                    if(wetTimes >= 3) {
                                        roommateWantsToSee();
                                    } else {
                                        dm.sentences.Enqueue("Roomie: ...You're leaking again. You still need to go?");
                                        dm.sentences.Enqueue("You: Um.. Yeah...");
                                        dm.sentences.Enqueue("Roomie: Then... I guess it's better if you just pee yourself again before cleaning up.");
                                        dm.sentences.Enqueue("You: R-Right, I was kinda thinking the same thing...");
                                        dm.sentences.Enqueue("Roomie: So... you've been planning to go in your pants again?");
                                        dm.sentences.Enqueue("You: ...Didn't you agree that it's the best choice?");
                                        dm.sentences.Enqueue("Roomie: True... Um, well, I'll be going for now.");
                                        dm.sentences.Enqueue("He quickly leaves, not taking anything with him this time.");
                                        isRoommate = false;
                                    }
                                } else {
                                    dm.sentences.Enqueue("Roomie: Well, alright... I'll just grab this and go.");
                                    dm.sentences.Enqueue("He takes fishes something out of his drawer and left.");
                                    isRoommate = false;
                                }
                        }
                    }
                } else {
                    //Wearing diapers
                    if(leakState <= 3 && roommateFirstWitnessDiaper) {
                        //is still pretty much dry and roommate haven't witnessed you wearing diapers
                        dm.sentences.Enqueue("Your roommate seems to notice the crinkly noises you're making when you move your legs, but doesn't say anything.");
                        dm.sentences.Enqueue("He quickly grabs something from his drawers and left.");
                        isRoommate = false;
                    } else if(leakState <= 3 && !roommateFirstWitnessDiaper) {
                        dm.sentences.Enqueue("Your roommate seems to notice the crinkly noises you're making when you move your legs.");
                        dm.sentences.Enqueue("Roomie: You wearing diapers again?");
                        dm.sentences.Enqueue("You: Yeah... Is the noise that noticable?");
                        dm.sentences.Enqueue("Roomie: Yeah, but me knowing that you wore diapers before probably has something to do with it.");
                        dm.sentences.Enqueue("You: I guess so.");
                        dm.sentences.Enqueue("You suddenly leak a bit more for some reason and your hand reflexively grabbed your groin.");
                        if(wetTimes >= 3) {
                            roommateWantsToSee();
                        } else {
                            dm.sentences.Enqueue("Roomie: I- Uh- I'm gonna go.");
                            dm.sentences.Enqueue("Your roommate probably notices you leaking and immediately leaves. He looks very nervous about something.");
                            isRoommate = false;
                        }
                    }
                    //case 4-7 is impossible because it would've entered the isPeeing branch.
                    //case 8 is impossible because it would've entered the isSoaked branch.
                }
            } else {
                //isPeeing
                //remark leakState >= 4
                if(leakState >= 4 && caughtTimes == 0) {
                    //wetting self and first time being caught
                    if(!isProtected) {
                        dm.sentences.Enqueue("Roomie: Oh... You're...");
                        dm.sentences.Enqueue("You panic, but despite your best efforts, you really can't stop the stream anymore.");
                        dm.sentences.Enqueue("You: Ah, I- Uh, sorry, I can't stop it...");
                        if(!am.scnCaughtPeeingSelf) {
                            //omoScenarioUnlocked++;
                            am.scnCaughtPeeingSelf = true;
                        }
                    } else {
                        dm.sentences.Enqueue("Despite me unleashing a torrent of pee down below, my roommate doesn't notice at all as I'm diapered.");
                    }
                } else if(leakState >= 4 &&caughtTimes >= 1) {
                    //you're peeing yourself and your roommate have seen you wet before.
                    if(!isProtected) {
                        dm.sentences.Enqueue("He sees you peeing yourself again.");
                        dm.sentences.Enqueue("You: (Shit...)");
                        dm.sentences.Enqueue("You think he's going to just take something and leave you be, but he just keeps watching you intensely.");
                    } else {
                        if(roommateFirstWitnessDiaper) {
                            dm.sentences.Enqueue("Despite me unleashing a torrent of pee down below, my roommate doesn't notice at all as I'm diapered.");
                        } else {
                            dm.sentences.Enqueue("He sees my unnaturally bulging pants and probably connected the dots.");
                            dm.sentences.Enqueue("Roomie: Hm..");
                            dm.sentences.Enqueue("He probably knows that I'm wearing diapers again, but he doesn't seem to know that I'm actually peeing right now.");
                        }
                    }
                }
                //if isPeeing then continue from the flood scenarios.
            }
            dm.DisplayCurrSentence();
        }  
    }

    private void roommateWantsToSee() {
        dm.sentences.Enqueue("Roomie: Did you just leak again..? Even after all that pissing?");
        dm.sentences.Enqueue("You: Haha.. I guess, I uh, have more pee to spare...");
        dm.sentences.Enqueue("Roomie: That's wild... You gonna pee yourself again?");
        dm.sentences.Enqueue("You: I mean.. I don't know... Kinda feels like it...");
        dm.sentences.Enqueue("Roomie: ...");
        dm.sentences.Enqueue("He's staring at your crotch again. Is he waiting for you to pee?");
        dm.sentences.Enqueue("You: Um.. Are you gonna leave again?");
        dm.sentences.Enqueue("Roomie: I, uh... What if I stay this time? Would you be okay with that..?");
        dm.sentences.Enqueue("You: I think I should be asking you that... I mean, I haven't cleaned up...");
        dm.sentences.Enqueue("Roomie: I'm good, so...");
        dm.sentences.Enqueue("You: I'm probably gonna pee again in a bit... You cool with that too?");
        dm.sentences.Enqueue("Roomie: ...");
        dm.sentences.Enqueue("Roomie: I'm gonna be honest with you... I kinda wanna see. Sorry if I'm weirding you out...");
        dm.sentences.Enqueue("You are surprised by his confession. So he is into it?");
        dm.sentences.Enqueue("You: Oh- um, okay... It's fine.");
        dm.sentences.Enqueue("You notice your roommate blushing as he sits on his desk. He looks very distracted but he's pretending to play with his phone.");
        isWatched = true;
        if(!am.scnRoomieAwakening) {
            //omoScenarioUnlocked++;
            am.scnRoomieAwakening = true;
        }
    }

    private void roommateLeaves() {
        dm.sentences.Enqueue("Roomie: Wow, uh..");
        dm.sentences.Enqueue("You: I-I can explain!");
        dm.sentences.Enqueue("Roomie: It's alright. Accidents happen...");
        dm.sentences.Enqueue("He watches me for a while, staring at my soaked pants.");
        dm.sentences.Enqueue("Roomie: I'll, uh... leave you to it.");
        isRoommate = false;
    }

    public void changeClothes() {
        if(wearBoxers) {
            if(isPeeing) {
                dm.sentences.Enqueue("As pee is still escaping from under you, you slosh over to the wardrobe and put on a pair of jeans, wetting them in the process.");
                if(isRoommate) {
                    if(isWatched) {
                        dm.sentences.Enqueue("Your roommate watches you intensely, he looks very turned on by your action.");
                    } else if(roomieWatch) {
                        dm.sentences.Enqueue("Roomie: Shit, that's way too hot...");
                        dm.sentences.Enqueue("He starts rubbing himself.");
                    }
                }
                if(!am.scnPlayWithClothes) {
                    //omoScenarioUnlocked++;
                    am.scnPlayWithClothes = true;
                }
            } else {
                dm.sentences.Enqueue("You switch to wearing Jeans.");
            }
            //audioSource.PlayOneShot(gameAudios[7]);
            audioSource.clip = gameAudios[7];
            audioSource.Play();
            wearBoxers = false;
            wearJeans = true;
        } else if(wearJeans && !isProtected) {
            if(isPeeing) {
                dm.sentences.Enqueue("As pee is still escaping from under you, you take off your jeans, revealing your drenched boxers and a more pronounced stream of pee.");
                if(isRoommate) {
                    if(isWatched) {
                        dm.sentences.Enqueue("Your roommate watches you intensely, he looks very turned on by your action.");
                    } else if(roomieWatch) {
                        dm.sentences.Enqueue("Roomie: Shit, that's way too hot...");
                        dm.sentences.Enqueue("He starts rubbing himself.");
                    }
                }
                if(!am.scnPlayWithClothes) {
                    //omoScenarioUnlocked++;
                    am.scnPlayWithClothes = true;
                }
            } else {
                dm.sentences.Enqueue("You switch to wearing Boxers.");
            }
            audioSource.PlayOneShot(gameAudios[7]);
            wearBoxers = true;
            wearJeans = false;
        } else if(isProtected) {
            dm.sentences.Enqueue("You: I'm wearing diapers underneath... I can't wear my boxers.");
        }
        dm.DisplayCurrSentence();
    }

    public void displayDeskOptions() {
        showDesk.SetActive(true);
    }

    public void closeDeskOptions() {
        showDesk.SetActive(false);
    }

    public void workSideHustle() {
        showDesk.SetActive(false);
        if(!isPeeing) {
            isWorking = true;
            switch(workEventInd) {
                case 0:
                    //Just started looking for work
                    dm.StartDialogue("workEvent0");
                    desperation += 30;
                    workEventInd++;
                    isWorking = false;
                    break;
                case 1:
                    //Start working on the project
                    dm.StartDialogue("workEvent1");
                    desperation += 30;
                    workEventInd++;
                    isWorking = false;
                    break;
                case 2:
                    //Done working on the project
                    dm.StartDialogue("workEvent2");
                    break;
                case 3:
                    //Leaking
                    //audioSource.PlayOneShot(gameAudios[0]);
                    audioSource.clip = gameAudios[0];
                    audioSource.Play();
                    if(!isProtected) {
                        if(isSoaked) {
                            dm.sentences.Enqueue("You leak into your already-wet pants. The renewed warmth somehow comforts you.");
                            dm.sentences.Enqueue("You decide to leak more.");
                            dm.dialogueTypeTemp = "workEventPee2";
                            dm.DisplayCurrSentence();
                        } else {
                            dm.StartDialogue("workEventPee2");
                        }
                    } else {
                        dm.StartDialogue("workEventDiaper2");
                    }
                    workEventInd++;
                    break;
                case 4:
                    //More Leaking
                    //audioSource.PlayOneShot(gameAudios[1]);
                    audioSource.clip = gameAudios[1];
                    audioSource.Play();
                    if(!isProtected) {
                        if(isSoaked) {
                            dm.sentences.Enqueue("More pee shoots out of you, warming your groins even more.");
                            dm.sentences.Enqueue("You start leaking more frequently in small spurts to help ease the desperation.");
                            dm.dialogueTypeTemp = "workEventPee3";
                            dm.DisplayCurrSentence();
                        } else {
                            dm.StartDialogue("workEventPee3");
                        }
                        if(roomieWatch) {
                            dm.sentences.Enqueue("You can feel your roommate's gaze from behind you. You figure he probably knows you're leaking.");
                        }
                    } else {
                        dm.StartDialogue("workEventDiaper3");
                    }
                    workEventInd++;
                    break;
                case 5:
                    //Heavy Leaking
                    //audioSource.PlayOneShot(gameAudios[2]);
                    audioSource.clip = gameAudios[2];
                    audioSource.Play();
                    if(!isProtected) {
                        if(isSoaked) {
                            dm.sentences.Enqueue("The small spurts you've been consciously leaking has now become bigger and stronger.");
                            dm.sentences.Enqueue("You feel like you're going to pee yourself in a bit.");
                            dm.sentences.Enqueue("With your right hand on the mouse and your left holding your crotch, you start relaxing your muscles.");
                            dm.dialogueTypeTemp = "workEventPee4";
                            dm.DisplayCurrSentence();
                        } else {
                            dm.StartDialogue("workEventPee4");
                        }
                        if(roomieWatch) {
                            dm.sentences.Enqueue("Your roommate probably heard the loud hiss, you turn around and see his hand covering his swelling groin.");
                            dm.sentences.Enqueue("He's surprised that you're looking at him, and he quickly turns his face away, blushing slightly.");
                        }
                    } else {
                        dm.StartDialogue("workEventDiaper4");
                    }
                    workEventInd++;
                    break;
                case 6:
                    //Wetting
                    //audioSource.PlayOneShot(gameAudios[3]);
                    //audioSource.clip = gameAudios[3];
                    //audioSource.Play();
                    StartCoroutine(workPeeSelf());
                    dm.topDialogue = true;
                    //will be set to false at the end of the CG (ienumerator)
                    if(!isProtected) {
                        //dm.StartDialogue("workEventPee5");
                        if(isSoaked) {
                            dm.sentences.Enqueue("You can feel the pressure of pee shooting out from you into your left hand.");
                            dm.sentences.Enqueue("The dam has broken completely and the loud noise fills your mic.");
                            if(!am.scnWorkPeeSoaked) {
                                //omoScenarioUnlocked++;
                                am.scnWorkPeeSoaked = true;
                            }
                        } else {
                            dm.sentences.Enqueue("The dam has broken completely. The loud noise is very concerning, but you try your hardest to hold it only to fail.");
                        }
                        if(roomieWatch) {
                            dm.sentences.Enqueue("You don't even need to look behind you to know that your roommate is jerking one off from watching you pee yourself on such a hot scenario.");
                            dm.sentences.Enqueue("You are also getting somewhat turned on by the fact that you're secretly pissing yourself in the middle of an online meeting.");
                        }
                        dm.sentences.Enqueue("The noise cancellation seems to also fail you. The client is saying that your voice is unclear due to what seems to be water noise.");
                        dm.sentences.Enqueue("You apologize and make up the excuse that a water pipe broke down, then you mute your mic and pretend that you're trying to fix it.");
                        dm.sentences.Enqueue("After muting, you start peeing even harder.");
                        dm.sentences.Enqueue("You: (Ah.. that feels good..)");
                        if(roomieWatch) {
                            dm.sentences.Enqueue("It seems your roommate also realized you've muted your mic.");
                            dm.sentences.Enqueue("Roomie: I think your water pipe broke down even more...");
                            dm.sentences.Enqueue("You: Mm, well... Need to empty it first before I can fix it...");
                            dm.sentences.Enqueue("You hear your roommate sigh as he starts beating himself faster.");
                            if(!am.scnRoomieWorkPee) {
                                //omoScenarioUnlocked++;
                                am.scnRoomieWorkPee = true;
                            }
                        }
                        dm.DisplayCurrSentence();
                    } else {
                        dm.StartDialogue("workEventDiaper5");
                    }
                    break;
            }
            dm.showContinueWork.SetActive(false);
        } else {
            dm.sentences.Enqueue("You walk over to your desk, pee still escaping from under you.");
            dm.sentences.Enqueue("You wanted to start working while you pee, but the way you're peeing right now just makes you feel like standing up.");
            dm.sentences.Enqueue("You move the chair aside and tried to work standing up for a bit, but you fail to focus as the sensation of pissing just outshines everything.");
            if(roomieWatch) {
                dm.sentences.Enqueue("Your roommate doesn't say anything, but you can tell he's enjoying the view.");
            }
            if(!am.scnPeeTryWork) {
                //omoScenarioUnlocked++;
                am.scnPeeTryWork = true;
            }
            dm.DisplayCurrSentence();
        }
    }

    IEnumerator workPeeSelf() {
        //already in a state of heavy leakage
        var emission = stream.emission;
        var main = stream.main;
        Vector3 workPissPosition = new Vector3(-0.54f, 0.93f, 0f);
        Vector3 workPissScale = new Vector3(0.5f,0.5f,1f);
        pissObject.transform.position = workPissPosition;
        pissObject.transform.localScale = workPissScale;
        main.gravityModifier = 0.15f;
        CGBGImage.sprite = workPeeBackgroundSprites[0];
        if(wearJeans) {
            CGImage.sprite = workPeeJeansSprites[0];
        } else if(wearBoxers) {
            CGImage.sprite = workPeeBoxersSprites[0];
        }
        showCG.SetActive(true);
        pissObject.SetActive(true);
        //yield return new WaitForSeconds(0.2f);
        main.duration = 35;
        emission.rateOverTime = 250.0f;
        audioSource.clip = gameAudios[3];
        audioSource.Play();
        if(wearJeans) {
            CGImage.sprite = workPeeJeansSprites[1];
        } else if(wearBoxers) {
            CGImage.sprite = workPeeBoxersSprites[1];
        }
        yield return new WaitForSeconds(5.0f);
        if(wearJeans) {
            CGImage.sprite = workPeeJeansSprites[2];
        } else if(wearBoxers) {
            CGImage.sprite = workPeeBoxersSprites[2];
        }
        CGBGImage.sprite = workPeeBackgroundSprites[2];
        yield return new WaitForSeconds(5.0f);
        if(wearJeans) {
            CGImage.sprite = workPeeJeansSprites[3];
        } else if(wearBoxers) {
            CGImage.sprite = workPeeBoxersSprites[3];
        }
        CGBGImage.sprite = workPeeBackgroundSprites[3];
        yield return new WaitForSeconds(5.0f);
        if(wearJeans) {
            CGImage.sprite = workPeeJeansSprites[4];
        } else if(wearBoxers) {
            CGImage.sprite = workPeeBoxersSprites[4];
        }
        CGBGImage.sprite = workPeeBackgroundSprites[4];
        yield return new WaitForSeconds(5.0f);
        if(wearJeans) {
            CGImage.sprite = workPeeJeansSprites[5];
        } else if(wearBoxers) {
            CGImage.sprite = workPeeBoxersSprites[5];
        }
        //start 15 seconds
        //yield return new WaitForSeconds(15.0f);
        //replace with moving stream
        yield return new WaitForSeconds(3.0f);
        if(wearJeans) {
            CGImage.sprite = workPeeJeansSprites[6];
        } else if(wearBoxers) {
            CGImage.sprite = workPeeBoxersSprites[6];
        }
        yield return new WaitForSeconds(3.0f);
        if(wearJeans) {
            CGImage.sprite = workPeeJeansSprites[5];
        } else if(wearBoxers) {
            CGImage.sprite = workPeeBoxersSprites[5];
        }
        yield return new WaitForSeconds(3.0f);
        if(wearJeans) {
            CGImage.sprite = workPeeJeansSprites[6];
        } else if(wearBoxers) {
            CGImage.sprite = workPeeBoxersSprites[6];
        }
        yield return new WaitForSeconds(3.0f);
        if(wearJeans) {
            CGImage.sprite = workPeeJeansSprites[5];
        } else if(wearBoxers) {
            CGImage.sprite = workPeeBoxersSprites[5];
        }
        yield return new WaitForSeconds(3.0f);
        if(wearJeans) {
            CGImage.sprite = workPeeJeansSprites[6];
        } else if(wearBoxers) {
            CGImage.sprite = workPeeBoxersSprites[6];
        }
        emission.rateOverTime = 0f;

        showCG.SetActive(false);
        dm.topDialogue = false;
        pissObject.SetActive(false);
        if(roomieWatch) {
            dm.sentences.Enqueue("You finally stop peeing, and your roommate finishes a bit later.");
            dm.sentences.Enqueue("You unmute yourself again to say that the problem is fixed and you continue on with your presentation.");
        } else {
            dm.sentences.Enqueue("After peeing it all out, you unmute yourself again to say that the problem is fixed and you continue on with your presentation.");
        }
        dm.sentences.Enqueue("The client seems to not suspect anything and you earn yourself a paycheck by the end of the meeting.");
        dm.sentences.Enqueue("Received $50!");
        wetYourself();
        dm.DisplayCurrSentence();
        dm.stopPeeAudio = true;
        desperation = 0;
        workEventInd = 0;
        leakState = 0;
        isSoaked = true;
        money += 50;
        isWorking = false;
        audioSource.PlayOneShot(gameAudios[14]);
        if(!am.scnWorkPee) {
            //omoScenarioUnlocked++;
            am.scnWorkPee = true;
        }
        //roommateCheck();
        showCleanUp.SetActive(true);
    }

    public void workEventPee() {
        dm.showWorkWet.SetActive(false);
        if(isSoaked) {
            dm.sentences.Enqueue("You can really care less. You're already soaked with your own piss anyway. Even if you end up peeing yourself again, it's not going do much.");
            dm.sentences.Enqueue("Plus, it's an online meeting, so nobody will be able to notice anything.");            
        } else {
            dm.sentences.Enqueue("You decide that you can manage to hold it in throughout the entirety of the meeting.");
            dm.sentences.Enqueue("...");
        }
        dm.sentences.Enqueue("Halfway through the meeting, you start feeling a light trickle in your groin.");
        dm.dialogueTypeTemp = "workEventPee";
        dm.DisplayCurrSentence();
        workEventInd++;
    }

    public void workEventDry() {
        dm.showWorkWet.SetActive(false);
        dm.sentences.Enqueue("You go outside and find a safe spot to pee. You stay dry during the whole meeting and get your pay.");
        if(leakState <= 2) {
            dm.sentences.Enqueue("Your small leak spots have also dried up by now.");
        } else if(leakState == 3) {
            dm.sentences.Enqueue("Since you leaked quite a lot previously, you also changed your pants into new ones.");
        }
        dm.sentences.Enqueue("Received $50!");
        audioSource.PlayOneShot(gameAudios[14]);
        dm.DisplayCurrSentence();
        desperation = 0;
        leakState = 0;
        workEventInd = 0;
        isWorking = false;
        money += 50;
    }

    public void interactBed() {
        if(!isPeeing) {
            //normal scenario
            generalConfirmSynopsis.text = "Go to sleep and end the day in your current situation?";
            showGeneralConfirm.SetActive(true);
        } else {
            if(!isProtected) {
                if(wetTimes < 3) {
                    dm.sentences.Enqueue("You: (I'm still peeing... I'm not getting near the bed. The dorm manager's going to kill me if he knows I peed on it...)");
                } else {
                    if(!bedProtected) {
                        dm.sentences.Enqueue("You: (I better use a mattress protector if I'm going to pee on the bed.)");
                    } else {
                        dm.sentences.Enqueue("Still peeing all over yourself, you walk over to the bed, leaving trails of pee when you walk.");
                        dm.sentences.Enqueue("You first sit down on the bed, then you climb up and lay down. The sheets are now wet all over with the pee that keeps coming out of you in the process.");
                        dm.sentences.Enqueue("You relish in the sensation, feeling the wetness spread on your sheets, eventually reaching your back and wetting your shirt.");
                        if(roomieWatch) {
                            dm.sentences.Enqueue("Your roommate walks towards your bed and stands in front of it, watching you wet the bed.");
                            dm.sentences.Enqueue("You: Enjoying the view?");
                            dm.sentences.Enqueue("Roomie: Mmhm..");
                        }
                        if(!am.scnPeeOnBed) {
                            //omoScenarioUnlocked++;
                            am.scnPeeOnBed = true;
                        }
                    }
                }
            } else {
                //wearing diapers
                if(!isSoaked) {
                    dm.sentences.Enqueue("Still peeing into your diaper, you walk over to your bed and sit on it.");
                    dm.sentences.Enqueue("The flow of your pee stopped for a bit when you sit down, but you don't find it hard to start it up again.");
                    dm.sentences.Enqueue("You climb up and lay down on your comfy bed, doing all that while peeing.");
                    dm.sentences.Enqueue("Even after relaxing on your bed for a while, your pee is still not stopping and your diaper is getting heavier and warmer each time.");
                    dm.sentences.Enqueue("You pat your pants around, making sure that nothing is leaking before you continue to relax.");
                    if(!am.scnDiaperPeeOnBed) {
                        //omoScenarioUnlocked++;
                        am.scnDiaperPeeOnBed = true;
                    }
                } else {
                    //soggy diapers
                    dm.sentences.Enqueue("You: (My diapers are leaking... I probably shouldn't.)");
                    if(wetTimes >= 3) {
                        if(!bedProtected) {
                            dm.sentences.Enqueue("You: (I better use a mattress protector if I'm going to pee on the bed.)");
                        } else {
                            dm.sentences.Enqueue("You feel your diapers squashing and pouring out more pee whenever you walk. You sit down on your bed, making the diaper burst even more liquid out.");
                            dm.sentences.Enqueue("Your bed and bum immediately gets soaked from all the pee leaking out. You climb to the bed and lay down, splashing pee all over your sheets.");
                            dm.sentences.Enqueue("You proceed to rub and squeeze your diaper, juicing out the pee it managed to absorb before and getting your hand warm and wet.");
                            dm.sentences.Enqueue("You keep peeing and your sheets are now wet with both your previous pee and your current pee.");
                            if(roomieWatch) {
                                dm.sentences.Enqueue("Roomie: ...You're being very naughty right now...");
                                dm.sentences.Enqueue("You: Mmhm, and you like it, don't you?");
                                dm.sentences.Enqueue("Roomie: Yeah, please keep being naughty.");
                                dm.sentences.Enqueue("You snicker.");
                            }
                            if(!am.scnDiaperLeakOnBed) {
                                //omoScenarioUnlocked++;
                                am.scnDiaperLeakOnBed = true;
                            }
                        }
                    }
                }
            }
            dm.DisplayCurrSentence();
        }
    }

    private void goToSleep() {
        if(showCleanUp.activeSelf) {
            dm.sentences.Enqueue("You: Uh.. I should probably clean up first before sleeping. Otherwise the smell would be too much and other people might notice...");
        } else {
            showNightTime.SetActive(true);
            if(desperation <= 75) {
                //Safe / no bedwetting
                dm.sentences.Enqueue("You sleep peacefully into the night.");
                dm.sentences.Enqueue("Morning comes...");
                if(bedProtected) {
                    dm.sentences.Enqueue("You wake up dry, but you feel like you need to go for a morning pee. The toilet is still broken, however.");
                    dm.sentences.Enqueue("But well, your bed is protected and it wouldn't matter even if you went right here, right?");
                    dm.dialogueTypeTemp = "wakeUpDry";
                    gameEventController(5);
                }
            } else {
                if(bedProtected || isProtected) {
                    //Bedwetting
                    dm.sentences.Enqueue("You dream that you're on a journey somewhere. You just arrived at the airport and suddenly the ground sinks beneath you.");
                    dm.sentences.Enqueue("Half of your body sank down to the floor, but it feels like you're bouncing on a pool of warm, gooey substance.");
                    dm.sentences.Enqueue("The feeling is relaxing and you swim towards your destination, feeling your body get wetter and wetter...");
                    dm.sentences.Enqueue("Morning comes...");
                    if(!isProtected) {
                        //bed is protected
                        dm.sentences.Enqueue("You feel something wet beneath you, only to realize you've wet the bed.");
                        dm.sentences.Enqueue("You: Oh...");
                        if(roomieWatch) {
                            dm.sentences.Enqueue("Roomie: Morning. Wet the bed, huh?");
                            dm.sentences.Enqueue("You: Yeah.. I'm using a mattress protector, though.");
                            dm.sentences.Enqueue("Roomie: Look at you being so prepared to pee yourself! How admirable.");
                            dm.sentences.Enqueue("You: Shut up...");
                        }
                        dm.sentences.Enqueue("You take off the sheets for washing and wipe the mattress protector clean before putting on new sheets over it.");
                        dm.sentences.Enqueue("You also clean yourself up in the process.");
                        if(!am.scnWetTheBed) {
                            //omoScenarioUnlocked++;
                            am.scnWetTheBed = true;
                        }
                    } else {
                        //wearing diapers
                        dm.sentences.Enqueue("You feel something wet beneath you, only to realize you've peed in your diapers.");
                        dm.sentences.Enqueue("You: Oh...");
                        dm.sentences.Enqueue("You rub and press on it a little, it's not that warm, but it's definitely swollen and somewhat damp.");
                        if(roomieWatch) {
                            dm.sentences.Enqueue("Roomie: Morning. I see the diaper did its' job.");
                            dm.sentences.Enqueue("You: Yeah... Still feels like I gotta go for a morning pee, though.");
                            dm.sentences.Enqueue("Roomie: Going in your diaper for that too?");
                            dm.sentences.Enqueue("You: Mm, I'll just get out of bed first in case it leaks.");
                            dm.sentences.Enqueue("You get up and do some light stretches before peeing your morning pee. The diaper gets warmer and bigger, but it ends up not leaking.");
                            dm.sentences.Enqueue("Roomie: Hey, it holds.");
                            dm.sentences.Enqueue("You: Yeah but it's getting pretty soggy. I'm gonna change.");
                            dm.sentences.Enqueue("Roomie: Okay.");
                        }
                        dm.sentences.Enqueue("You change out of your diapers and put on a new pair of pants.");
                        if(!am.scnPreventWetBed) {
                            //omoScenarioUnlocked++;
                            am.scnPreventWetBed = true;
                        }
                        if(!firstTimeUseDiaper) {
                            firstTimeUseDiaper = false;
                        }
                    }
                    desperation = 0;
                    leakState = 0;
                } else {
                    //Wake up in the middle of the night to pee
                    dm.sentences.Enqueue("You are woken up by the urgent sense of having to go to the bathroom.");
                    dm.sentences.Enqueue("You quickly get up from bed and make your way towards the bathroom, but once you arrive, you forgot that the toilet is still broken.");
                    dm.sentences.Enqueue("You start leaking.");
                    dm.sentences.Enqueue("You: Wait- Shit..");
                    dm.sentences.Enqueue("You manage to stop the flow before you have a full-blown accident.");
                    if(roomieWatch) {
                        dm.sentences.Enqueue("You hear some rustling and find that your roommate is now awake and looking at you.");
                        dm.sentences.Enqueue("Roomie: You need to go?");
                        dm.sentences.Enqueue("You: Um.. yeah.");
                        dm.sentences.Enqueue("Roomie: I have an empty water bottle if you want.");
                        dm.dialogueTypeTemp = "roomieSleep";
                        gameEventController(4);
                    } else {
                        commonNightPeeRoute();
                    }
                }
            }
        }
        dm.DisplayCurrSentence();
    }

    private void commonNightPeeRoute() {
        dm.sentences.Enqueue("You go out to the hallway of your dorm, planning to go pee in the common area's restroom.");
        dm.sentences.Enqueue("It's a pretty long walk down the hallway and you can feel a trickling sensation running down your thighs.");
        dm.sentences.Enqueue("You are grabbing your crotch and walking carefully since you feel like the smallest amount of pressure can make you burst.");
        dm.sentences.Enqueue("A while later, you finally arrive in front of the restroom, but you've also already leaked quite a lot on the way.");
        dm.sentences.Enqueue("Upon opening the door, you feel relief wash over you and making your bladder muscles unconsciously relax.");
        dm.sentences.Enqueue("You start peeing before you could even get to the urinal. You dash inside and immediately pull out your leaking penis once you get to an urinal.");
        dm.sentences.Enqueue("You finally relax, but despite reaching the toilet in time, you've also basically peed yourself on the way. You can't even hide the big, wet spot on your pants anymore.");
        dm.sentences.Enqueue("After you finish peeing, you walk back to your room, noticing the trail of pee leading straight to the restroom. You can only hope it dries by morning time.");
        dm.sentences.Enqueue("You close the door to your room, clean yourself up, and return to your slumber...");
        dm.sentences.Enqueue("Morning comes...");
        leakState = 0;
        desperation = 0;
        if(!am.scnHalfMadeIt) {
            //omoScenarioUnlocked++;
            am.scnHalfMadeIt = true;
        }
        //dm.DisplayCurrSentence();
    }

    IEnumerator bedPeePlay() {
        //var emission = bedStream.emission;
        //var main = bedStream.main;
        var emission = stream.emission;
        var main = stream.main;
        //dm.topDialogue = true;
        //no dialogues here, so it's fine
        Vector3 bedPissPosition = new Vector3(0.22f, 1.51f, 0f);
        pissObject.transform.position = bedPissPosition;
        Vector3 bedPissScale = new Vector3(0.5f,1f,1f);
        pissObject.transform.localScale = bedPissScale;
        main.gravityModifier = 0.6f;

        CGBGImage.sprite = null;
        if(wearJeans) {
            CGImage.sprite = bedPeeSprites[0];
        } else if(wearBoxers) {
            CGImage.sprite = bedPeeBoxersSprites[0];
        }
        showCG.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        pissObject.SetActive(true);
        main.duration = 3;
        emission.rateOverTime = 100.0f;
        audioSource.clip = gameAudios[0];
        audioSource.Play();
        if(wearBoxers) {
            CGImage.sprite = bedPeeBoxersSprites[1];
        }
        yield return new WaitForSeconds(3.0f);
        main.duration = 3;
        emission.rateOverTime = 200.0f;
        audioSource.clip = gameAudios[1];
        audioSource.Play();
        if(wearJeans) {
            CGImage.sprite = bedPeeSprites[1];
        }
        yield return new WaitForSeconds(3.0f);
        main.duration = 3;
        emission.rateOverTime = 750.0f;
        audioSource.clip = gameAudios[2];
        audioSource.Play();
        if(wearJeans) {
            CGImage.sprite = bedPeeSprites[2];
        } else if(wearBoxers) {
            CGImage.sprite = bedPeeBoxersSprites[2];
        }
        yield return new WaitForSeconds(3.0f);
        main.duration = 35;
        emission.rateOverTime = 1000.0f;
        audioSource.clip = gameAudios[3];
        audioSource.Play();
        if(wearJeans) {
            CGImage.sprite = bedPeeSprites[3];
        } else if(wearBoxers) {
            CGImage.sprite = bedPeeBoxersSprites[4];
        }
        yield return new WaitForSeconds(5.0f);
        if(wearJeans) {
            CGImage.sprite = bedPeeSprites[4];
        } else if(wearBoxers) {
            CGImage.sprite = bedPeeBoxersSprites[5];
        }
        yield return new WaitForSeconds(5.0f);
        if(wearJeans) {
            CGImage.sprite = bedPeeSprites[5];
        } else if(wearBoxers) {
            CGImage.sprite = bedPeeBoxersSprites[6];
        }
        yield return new WaitForSeconds(5.0f);
        if(wearJeans) {
            CGImage.sprite = bedPeeSprites[6];
        } else if(wearBoxers) {
            CGImage.sprite = bedPeeBoxersSprites[7];
        }
        yield return new WaitForSeconds(5.0f);
        if(wearJeans) {
            CGImage.sprite = bedPeeSprites[7];
        } else if(wearBoxers) {
            CGImage.sprite = bedPeeBoxersSprites[8];
        }
        yield return new WaitForSeconds(15.0f);
        emission.rateOverTime = 0f;

        showCG.SetActive(false);
        pissObject.SetActive(false);
        dm.sentences.Enqueue("You: Ah... I'm done.");
        if(roomieWatch) {
            dm.sentences.Enqueue("You notice that your roommate has been watching you all along from the bed next to you.");
            dm.sentences.Enqueue("Roomie: M-Morning...");
            dm.sentences.Enqueue("You: Morning... Uh, sorry for doing this so early in the morning...");
            dm.sentences.Enqueue("Roomie: No no no, it's all good... It's uh... Pretty hot...");
            dm.sentences.Enqueue("He's hiding his lower body in his blanket and you can see one of his hand going under. The blanket slightly moves up and down.");
            dm.sentences.Enqueue("You: Hmm, did I turn you on~?");
            dm.sentences.Enqueue("He blushes and looks away.");
            dm.sentences.Enqueue("Roomie: Shut up, man.");
        }
        dm.sentences.Enqueue("You get up from the bed, dripping some pee to the floor. You clean up and start your day.");
        if(!am.scnMorningPeeOnBed) {
            //omoScenarioUnlocked++;
            am.scnMorningPeeOnBed = true;
        }
        dm.DisplayCurrSentence();
        desperation = 0;
        leakState = 0;
        wetYourself();
    }

    public void generalConfirm() {
        if(generalConfirmSynopsis.text == "Go to sleep and end the day in your current situation?") {
            showGeneralConfirm.SetActive(false);
            goToSleep();
        } else if(generalConfirmSynopsis.text == "Exit to desktop?") {
            Application.Quit();
        }
    }

    public void generalCancel() {
        showGeneralConfirm.SetActive(false);
    }

    private void wetYourself() {
        wetTimes++;
            switch(wetTimes) {
                case 1:
                    dm.sentences.Enqueue("You: (This is so embarassing... I'm already in college and I'm still pissing myself...)");
                    break;
                case 2:
                    dm.sentences.Enqueue("You: (Shit.. That makes it the second time I wet myself today. I can't believe this...)");
                    break;
                case 3:
                    dm.sentences.Enqueue("You: (I keep peeing myself, but... why does it feel so good..? Am I actually into this...?)");
                    dm.sentences.Enqueue("You start slowly rubbing around your pants and your crotch, feeling the wetness in your hand. This is turning you on.");
                    dm.sentences.Enqueue("You: (...I think I'll go to the bathroom...)");
                    dm.sentences.Enqueue("Congratulations, you've wet yourself 3 times! You've unlocked the option to just let go whenever.");
                    showLetGo.SetActive(true);
                    canBuyOmoge = true;
                    break;
            }
            //dm.DisplayCurrSentence();
            dm.wetCountCheck = false;
    }

    public void showGamesPanel() {
        if(boughtShooter) {
            shooterButton.interactable = true;
        } else {
            shooterButton.interactable = false;
        }
        if(boughtHorror) {
            horrorButton.interactable = true;
        } else {
            horrorButton.interactable = false;
        }
        if(boughtOmoge) {
            omogeButton.interactable = true;
        } else {
            omogeButton.interactable = false;
        }
        showGames.SetActive(true);
    }

    public void closeGamesPanel() {
        showGames.SetActive(false);
    }

    public void playGame(string gameType) {
        closeGamesPanel();
        closeDeskOptions();
        //use tempdesperation to store desperate values (so it wont interfere with events)
        tempdesperation = desperation;
        desperation = 0;
        //value will be returned when player manages to pick all the right decisions
        if(gameType == "shooter") {
            dm.sentences.Enqueue("You boot up the shooter game and start playing.");
            if(roomieWatch) {
                dm.sentences.Enqueue("Your roommate seems interested in the game you're playing and rolls his chair over to watch you.");
            }
            dm.sentences.Enqueue("You've played a couple of multiplayer shooter games in the past as well, so you're pretty used to it.");
            dm.sentences.Enqueue("Being a multiplayer game, you hop into voice chat to communicate with your teammates with more ease.");
            dm.sentences.Enqueue("You've been playing for quite a long time now, and your desperation only keeps growing.");
            dm.sentences.Enqueue("You are now trying to flank an enemy support. They're alone and it should be an easy kill for you. Do you go immediately for the kill or do you wait a little longer?");
            dm.dialogueTypeTemp = "gameDecision1";
            //show choice between two of them
            gameEventController(1);
        } else if(gameType == "horror") {
            dm.sentences.Enqueue("You boot up the horror game and start playing.");
            dm.sentences.Enqueue("The game initially advises you to turn off the lights and play at night for the best experience...");
            dm.sentences.Enqueue("Should you follow through with the advice?");
            dm.dialogueTypeTemp = "horrorDecision1";
            //show choice between two of them
            gameEventController(6);
        } else if(gameType == "omoge") {
            //dm.sentences.Enqueue("SYSTEM: NOT YET IMPLEMENTED");
            dm.sentences.Enqueue("Since discovering this fetish of yours, you've been wanting to play a game about it for quite a while now.");
            dm.sentences.Enqueue("You boot up the game giddily. The title screen fades in, showing the title of 'Busy Bee' proudly.");
            if(roomieWatch) {
                dm.sentences.Enqueue("Your roommate seems interested in the game you're playing and rolls his chair over to watch you.");
            }
            dm.sentences.Enqueue("The game is one where you play the daily life of a male character. You must eat, drink, go to classes, study, build relationships, and all that.");
            dm.sentences.Enqueue("In such a free world, you realize you can just drink a lot of water to make the character wet himself.");
            dm.sentences.Enqueue("Well, should you play around with this freedom and make your character pee himself everywhere?");
            dm.dialogueTypeTemp = "omogeDecision1";
            gameEventController(9);
        }
        dm.DisplayCurrSentence();
    }

    private void gameEventController(int gameEventInd) {
        switch(gameEventInd) {
            case 1:
            gameEventSynopsis.text = "The support is right there and you're perfectly hidden as of now. But do you go immediately for the kill or do you wait a little longer to make sure they're truly alone before acting?";
            gameEventChoiceOne.text = "Go directly for the kill";
            gameEventChoiceTwo.text = "Wait to make sure";
            break;

            case 2:
            gameEventSynopsis.text = "Should you mute the guy with the slightly noisy mic?";
            gameEventChoiceOne.text = "Mute him";
            gameEventChoiceTwo.text = "Let him be";
            break;

            case 3:
            gameEventSynopsis.text = "Should you pee a little to relieve some pressure from your bladder?";
            gameEventChoiceOne.text = "Just a little bit...";
            gameEventChoiceTwo.text = "No, I can do this, maybe...";
            break;

            case 4:
            gameEventSynopsis.text = "Pee on the empty water bottle?";
            gameEventChoiceOne.text = "No other choice";
            gameEventChoiceTwo.text = "I'll find another way";
            break;

            case 5:
            gameEventSynopsis.text = "Go right here on the bed?";
            gameEventChoiceOne.text = "Mm.. I'll just go here (CG Sequence)";
            gameEventChoiceTwo.text = "I'll just go to the common restroom.";
            break;

            case 6:
            gameEventSynopsis.text = "Wait until nighttime and play with lights off?";
            gameEventChoiceOne.text = "Yeah, I think it will be better that way";
            gameEventChoiceTwo.text = "I think I'll pass...";
            break;

            case 7:
            gameEventSynopsis.text = "Look behind you?";
            gameEventChoiceOne.text = "Just a quick look...";
            gameEventChoiceTwo.text = "No... I'll just move forward...";
            break;

            case 8:
            gameEventSynopsis.text = "Fearlessly walk and talk to the lone, singing girl?";
            gameEventChoiceOne.text = "Onwards! No fear!";
            gameEventChoiceTwo.text = "Zoom-in on the girl to check";
            break;

            case 9:
            gameEventSynopsis.text = "Play around and try to pee yourself everywhere?";
            gameEventChoiceOne.text = "Paint the town gold!";
            gameEventChoiceTwo.text = "I'll just continue plot first";
            break;

            case 10:
            gameEventSynopsis.text = "Wear diapers or just keep going in your pants?";
            gameEventChoiceOne.text = "Contain the pee";
            gameEventChoiceTwo.text = "Free the pee";
            break;

            case 11:
            gameEventSynopsis.text = "Research the camp grounds for scary stories?";
            gameEventChoiceOne.text = "Browsing time!";
            gameEventChoiceTwo.text = "Just do generic ones...";
            break;
        }
    }

    public void gameWetting() {
        dm.gameWettingBool = true;
        dm.sentences.Enqueue("You: Ah- fuck...");
        dm.sentences.Enqueue("You can feel yourself peeing, and HARD. Your focus faltered and the enemy managed to kill you as a result.");
        if(roomieWatch) {
            dm.sentences.Enqueue("Sure enough, you roommate immediately switches his focus to watch you burst.");
        }
        if(!isProtected) {
            dm.sentences.Enqueue("You just stayed silent, listening to the sounds of your pee as you wait to respawn. You tried cutting off the stream or even slowing it down a bit, but to no avail. You've lost all control of your bladder.");
            dm.sentences.Enqueue("You completely forgot that you're on open mic until someone in your team asks if everything's alright. They can probably hear you peeing.");
            dm.sentences.Enqueue("You immediately mute your mic, slightly panicking. You try to get back to your team on time.");
            if(roomieWatch) {
                dm.sentences.Enqueue("Roomie: They heard it..?");
                dm.sentences.Enqueue("You: ...");
                dm.sentences.Enqueue("The silence probably gave it away, or maybe it's your reddened face. Anyway, it doesn't take long until you can hear a familiar beating sound.");
            }
            dm.sentences.Enqueue("After being freed from the torment of your bladder, you can now play even better than before and your team ended up winning.");
            dm.sentences.Enqueue("You: (...A battle lost for the war won, I guess.)");
            if(roomieWatch) {
                dm.sentences.Enqueue("You: Maybe I should start charging you for the show.");
                dm.sentences.Enqueue("Roomie: Right, and what will you do if I don't pay? Stop pissing yourself?");
                dm.sentences.Enqueue("You: ...");
                dm.sentences.Enqueue("You: ...Smart bastard.");
            }
        } else {
            //diapered
            if(!isSoaked) {
                dm.sentences.Enqueue("You are thankful that you've decided to wear diapers before playing, since you can relax and not worry about any mess being created.");
                dm.sentences.Enqueue("The sensation of wetting your diapers has a calming effect and you effectively rejoin the team after spawning.");
                if(roomieWatch) {
                    if(wearJeans) {
                        dm.sentences.Enqueue("You can tell that nothing is really showing on your jeans, your roommate only knows you're peeing due to your usage of pretty language before and your visibly relaxed body.");
                    } else {
                        dm.sentences.Enqueue("Your roommate is seemingly giving his full attention to the way your diapers slowly change colors from the bottom up.");
                    }
                }
                dm.sentences.Enqueue("It feels like wetting while sitting down diapered makes you pee way longer than usual, as the stream keeps going even after you win the last fight.");
                dm.sentences.Enqueue("The stream is also smaller, though, so it makes sense. It's also probably due to your current sitting position.");
                dm.sentences.Enqueue("You lift yourself up from the chair a little to pee better. Once you do, it feels like a flood suddenly came out of you.");
                dm.sentences.Enqueue("You thought you've already peed quite a lot into your diaper since you could already feel the wetness reaching your bum, and yet it turns out that you still have a lot more pee stored inside you.");
                dm.sentences.Enqueue("Your already somewhat heavy diaper just keeps getting more and more heavy and you can feel leaks of urine coming out from the sides.");
                if(roomieWatch) {
                    dm.sentences.Enqueue("You can tell that your roommate is as surprised as you are. Usually a single diaper is capable of holding all of your pee, after all.");
                    dm.sentences.Enqueue("You quickly and carefully waddle to the bathroom to prevent any mess from being formed in the room, leaving your roommate stunned.");
                } else {
                    dm.sentences.Enqueue("Genuinely surprised by the amount of pee coming out of you, you quickly and carefully waddle to the bathroom to prevent any mess from being formed in the room.");
                }
                dm.sentences.Enqueue("Spills were undoubtedly made along the way due to the excessive peeing that you're doing. The stream only tapers off around 5 seconds after you park yourself in the bathroom.");
                if(wearJeans) {
                    dm.sentences.Enqueue("You also had to loosen your jeans as to not squeeze the loaded diaper too much.");
                    dm.sentences.Enqueue("Despite already being careful enough when taking off your jeans, waterfalls of pee still manage leak out. There's probably a sea of piss just sitting there on your diaper, unabsorbed. You can feel it.");
                    dm.sentences.Enqueue("After successfully freeing yourself from your jeans, you carefully untape your diapers. Of course, more pee spills out anyway.");
                } else {
                    dm.sentences.Enqueue("You could feel the splashes of pee under you whenever you take a step. It's a very telling sign that a sea of piss has formed under you, unabsorbed.");
                    dm.sentences.Enqueue("You carefully untape your diapers, but no matter how careful you were, more pee still manage to spill out.");
                }
                dm.sentences.Enqueue("It is thoroughly soaked and there really was a sea of piss inside. You tip the diaper to empty it on the bathroom floor before doing the usual clean up routine.");
                if(roomieWatch) {
                    dm.sentences.Enqueue("Roomie: Did your... bladder evolve or something? That was... Wow. You peed like crazy...");
                    dm.sentences.Enqueue("You: I didn't expect it either. Maybe it's because I keep drinking while playing...");
                    dm.sentences.Enqueue("Roomie: Good. You should stay hydrated.");
                    dm.sentences.Enqueue("You: ...I think I was over-hydrated. I'll drink less next time.");
                    dm.sentences.Enqueue("Roomie: ...Despite my disappointment, it's probably healthier that way so I can't argue. Your pee was... way too clear this time, as if you just peed water out.");
                    dm.sentences.Enqueue("Roomie: Still smells like your usual pee, though.");
                    dm.sentences.Enqueue("You: Fuck, shut up.");
                    dm.sentences.Enqueue("He snickers.");
                }
            } else {
                //diaper is SOAKED and he peein
                dm.sentences.Enqueue("Despite wearing diapers, it's already soaked and there's no way it can absorb another full bladder of yours. You can feel it getting even wetter before it gives up taking in your pee.");
                if(wearJeans) {
                    dm.sentences.Enqueue("Your jeans doesn't act much as a secondary protective layer as pee easily penetrates the fabric and leaks out to the floor.");
                } else {
                    dm.sentences.Enqueue("Pee keeps spilling from the sides of your diaper, creating unsteady waterfalls as it forms a growing puddle onto the floor below.");
                }
                dm.sentences.Enqueue("You just stayed silent, listening to the sounds of your pee as you wait to respawn. You tried cutting off the stream or even slowing it down a bit, but to no avail. You've lost all control of your bladder.");
                dm.sentences.Enqueue("You completely forgot that you're on open mic until someone in your team asks if everything's alright. They can probably hear you peeing.");
                dm.sentences.Enqueue("You immediately mute your mic, slightly panicking. You try to get back to your team on time.");
                if(roomieWatch) {
                    dm.sentences.Enqueue("Roomie: They heard it..?");
                    dm.sentences.Enqueue("You: ...");
                    dm.sentences.Enqueue("The silence probably gave it away, or maybe it's your reddened face. Anyway, it doesn't take long until you can hear a familiar beating sound.");
                }
                dm.sentences.Enqueue("After being freed from the torment of your bladder, you can now play even better than before and your team ended up winning.");
                dm.sentences.Enqueue("It feels like wetting while sitting down diapered makes you pee way longer than usual, as the stream keeps going even after you win the last fight.");
                dm.sentences.Enqueue("The stream is also smaller, though, so it makes sense. It's also probably due to your current sitting position.");
                dm.sentences.Enqueue("You lift yourself up from the chair a little to pee better. Once you do, it feels like a flood suddenly came out of you.");
                dm.sentences.Enqueue("At this point, it feels like you're just pissing directly on the floor.");
                if(roomieWatch) {
                    dm.sentences.Enqueue("Your roommate's pants inevitably gets splashed by pee and he's even stepping on the warm puddle spreading under.");
                    dm.sentences.Enqueue("You feel yourself blushing when you see your roommate using his feet to play around with it.");
                }
                dm.sentences.Enqueue("You feel like there's no point in saving it now, so you straighten yourself up and just let everything go right there and then.");
                if(wearJeans) {
                    dm.sentences.Enqueue("The tightness of your jeans are making pee seemingly burst out from the sides, heavily pouring down your inner thighs and noisily streaming down from the hems of your pants.");
                    dm.sentences.Enqueue("Once you're finally done pissing, a huge puddle has pooled under you and your jeans are drenched in warm urine.");
                } else {
                    dm.sentences.Enqueue("The weight of your diaper keeps growing. You thought you've put the diaper on quite snugly, but the waistbands are now starting to slide down.");
                    dm.sentences.Enqueue("Once you're finally done pissing, a huge puddle has pooled under you and you have to grab on your waistbands to stop it from being dragged down even further.");
                }
                if(roomieWatch) {
                    dm.sentences.Enqueue("Roomie: Wow... That was... a lot.");
                    dm.sentences.Enqueue("You: Definitely more than normal... and stop splashing my pee around.");
                    dm.sentences.Enqueue("Roomie: Mmm, it feels good, though.");
                    dm.sentences.Enqueue("Your roommate is still diligently beating his meat, so you give up on telling him off and decide to give him a little special service.");
                }
                dm.sentences.Enqueue("You can feel that your groin is basically drowning in pee, so you open the sides of your diaper to release the trapped, unabsorbed pee to the floor.");
                dm.sentences.Enqueue("You also press and squeeze on the diaper a couple of times to really let everything out before cleaning up to make it easier for you later on.");
                if(roomieWatch) {
                    dm.sentences.Enqueue("You purposefully poured your pee over your friend's foot. The whole act turns him on so much he immediately came.");
                    dm.sentences.Enqueue("Roomie: Haa... Shit... for free?");
                    dm.sentences.Enqueue("You: No. Treat me to some food later.");
                    dm.sentences.Enqueue("Roomie: Your wish is my command.");
                }
                dm.sentences.Enqueue("After you think it's 'dry' enough, you waddle to the bathroom and do the usual clean up routine.");
            }
        }
        dm.stopPeeAudio = true;
        desperation = 0;
        leakState = 0;
        isSoaked = true;
        if(!am.scnShooterWetting) {
            //omoScenarioUnlocked++;
            am.scnShooterWetting = true;
        }
        wetYourself();
        showCleanUp.SetActive(true);
        dm.dialogueTypeTemp = "";
    }

    public void gameFirstOption() {
        showGameEvent.SetActive(false);
        if(dm.dialogueTypeTemp == "gameDecision1") {
            //Correct option
            dm.sentences.Enqueue("You go directly for the kill and you manage to safely get back to your teammates.");
            if(roomieWatch) {
                dm.sentences.Enqueue("Roomie: Nice one.");
                dm.sentences.Enqueue("You: Too easy.");
            }
            gameCommonPath(1);
        } else if(dm.dialogueTypeTemp == "gameDecision2") {
            //Wrong option
            dm.sentences.Enqueue("You decide to mute him and continue playing round two.");
            dm.sentences.Enqueue("Your team fought head on with the enemy team and won, but you died in the fight itself.");
            dm.sentences.Enqueue("After respawning, you immediately start heading back to your team.");
            dm.sentences.Enqueue("However, it turns out someone from the enemy team is spawn-camping you.");
            //Leak -> controlled in DM
            if(leakState < 3) {
                dm.sentences.Enqueue("You feel yourself leak from the surprise attack, but you barely pay it attention since you're focused on killing them.");
                dm.sentences.Enqueue("You somehow manage to win the duel and return to your team safely.");
                if(roomieWatch) {
                    if(!isProtected) {
                        dm.sentences.Enqueue("You can feel that your roommate's gaze has moved from the screen to your crotch... He probably noticed the leaking.");
                        dm.sentences.Enqueue("He watched for a bit, but since you don't look like you're going to lose control anytime soon, he continues watching your gameplay instead.");
                    } else {
                        dm.sentences.Enqueue("Your roommate doesn't notice at all since you're diapered up. The idea of secretly losing control is turning you on.");
                    }
                }
                gameCommonPath(2);
            } else {
                //full on wetting
                gameWetting();
            }
        } else if(dm.dialogueTypeTemp == "gameDecision3") {
            //Wrong option
            dm.sentences.Enqueue("You decide to let go for a bit. Small leaks of pee are successfully shot out of you, relieving some bladder pressure.");
            if(roomieWatch) {
                if(!isProtected) {
                    dm.sentences.Enqueue("Your roommate alternates between watching your expression and your pants, fully invested.");
                } else {
                    dm.sentences.Enqueue("Your roommate is kept oblivious to the fact.");
                }
            }
            //Leak -> controlled in DM
            if(leakState < 3) {
                dm.sentences.Enqueue("However, you are in the middle of a major teamfight. The winners of this fight will gain a great amount of advantage for the next one.");
                dm.sentences.Enqueue("You: Focus their supports--");
                //Leak -> controlled in DM
                //Leak event also controlled in DM because it will mess up the leakstate otherwise (based on real experience)
            } else {
                //full on wetting
                gameWetting();
            }
        } else if(dm.dialogueTypeTemp == "roomieSleep") {
            //Pee in bottle
            dm.sentences.Enqueue("You: Alright...");
            dm.sentences.Enqueue("He then grabs an empty water bottle from his desk and holds it under you.");
            dm.sentences.Enqueue("You find it weird as to why he's holding it for you, but your desperation makes it hard for you to think too much about it. You fit your penis to the hole and start peeing.");
            dm.sentences.Enqueue("Your roommate is just watching you pee and filling the bottle up. However, from the rate you're peeing, it seems like the bottle won't hold everything.");
            dm.sentences.Enqueue("You try to stop peeing once the bottle is almost full, but you can't even slow down the stream.");
            dm.sentences.Enqueue("You: H-Hold on, I can't stop it... It's going to flood out-");
            dm.sentences.Enqueue("Just as you're saying that, your pee reaches the top and starts flooding to the sides of the bottle, pouring down onto your roommate's hand.");
            dm.sentences.Enqueue("Your roomate didn't flinch at all when the pee streams down his arm, almost as if he was expecting it.");
            dm.sentences.Enqueue("You: Uh... Sorry, I really still can't stop it at all... Wait, don't tell me you're enjoying this...");
            dm.sentences.Enqueue("Roomie: Haha, sorry. I kinda knew it's going to flood with how much you usually pee... And you made me dream about- Uh, anyway. I woke up horny and you gave me a nice opportunity.");
            dm.sentences.Enqueue("You: I should've known... Oh well.");
            dm.sentences.Enqueue("Your tip is still submerged inside your own piss in the bottle, and even now you're still shooting more liquid in it. The sensation is pretty unique.");
            dm.sentences.Enqueue("After you're finally done peeing, you take your tip out of the bottle, dripping some more pee to the floor.");
            dm.sentences.Enqueue("Roomie: Look, I caused this so I'll clean up the mess. You should just wipe yourself clean and change.");
            dm.sentences.Enqueue("You see that your roommate is very hard right now and probably needs to take care of that too anyways, so you accept the proposal.");
            dm.sentences.Enqueue("You clean yourself up and go to bed, leaving the puddle of piss for your roommate to handle.");
            dm.sentences.Enqueue("Morning comes...");
            dm.dialogueTypeTemp = "";
            desperation = 0;
            leakState = 0;
            if(!am.scnWaterBottlePee) {
                //omoScenarioUnlocked++;
                am.scnWaterBottlePee = true;
            }
            dm.stopPeeAudio = true;
        } else if(dm.dialogueTypeTemp == "wakeUpDry") {
            //Trigger CG
            dm.dialogueTypeTemp = "";
            StartCoroutine(bedPeePlay());
        } else if(dm.dialogueTypeTemp == "horrorDecision1") {
            //Decide to play with maximum scare effects
            bgmAudioSource.clip = gameBGM[1];
            bgmAudioSource.Play();
            isHorrorMax = true;
            dm.sentences.Enqueue("Once night comes, you dim the lights and boot the game up again. Eeriely enough, now the game is not showing advice again, as if it knows you've already followed it.");
            if(roomieWatch) {
                dm.sentences.Enqueue("Your roommate seems interested in the game you're playing and rolls his chair over to watch you.");
                dm.sentences.Enqueue("You feel somewhat reassured from the presence of your roommate, but it doesn't really make the game any less scarier.");
            }
            dm.sentences.Enqueue("The game starts off with the player being in an abandoned cabin somewhere on an unnamed forest, waking up without any memory whatsoever.");
            dm.sentences.Enqueue("The atmosphere inside the game is already very creepy and the silent night only adds to the effect.");
            dm.sentences.Enqueue("You carefully try to walk around the cabin. You feel like your heart isn't ready yet for what you'll encounter in the vast forest. However...");
            dm.sentences.Enqueue("A sound was heard behind you out of nowhere and your headphones are making it sound very realistic.");
            dm.sentences.Enqueue("You feel yourself leaking with fear.");
            dm.sentences.Enqueue("You: Ah... damnit..");
            if(!isProtected) {
                dm.sentences.Enqueue("You leaked quite a bit and by the time you manage to stop the flow, you can already see a decent-sized wet spot on your pants.");
                if(roomieWatch) {
                    dm.sentences.Enqueue("Of course, your roommate is not wearing headphones and can't hear the game sounds. The only thing he can hear is the peeing noise you're making.");
                    dm.sentences.Enqueue("Roomie: Hasn't even been 5 minutes into the game and you're already wetting yourself?");
                    dm.sentences.Enqueue("You feel wronged. This guy can't even hear the game and dares to make such comment...");
                    dm.sentences.Enqueue("You: I peed for 3 seconds and you're already hard. Have some shame.");
                    dm.sentences.Enqueue("Your roommate immediately shuts up. Victory is yours.");
                }
            } else {
                //diapered
                dm.sentences.Enqueue("You leaked quite a bit and by the time you manage to stop the flow. You can feel your diapers getting wet, but the damage is still fairly small.");
                if(roomieWatch) {
                    dm.sentences.Enqueue("Roomie: Did you leak?");
                    dm.sentences.Enqueue("You: Use your imagination.");
                    dm.sentences.Enqueue("He gets hard.");
                }
            }
            gameCommonPath(3);
        } else if(dm.dialogueTypeTemp == "horrorDecision2") {
            dm.sentences.Enqueue("You turn to look behind you and the scenery turns into that of an abandoned school.");
            dm.sentences.Enqueue("You start walking towards the school slowly while looking around the new area.");
            dm.sentences.Enqueue("Suddenly, you see something move past you from the corner of your eye.");
            dm.sentences.Enqueue("You try looking for it and found a humanoid looking creature smiling wide at you while crawling inside the school.");
            if(isHorrorMax) {
                //pee self
                dm.sentences.Enqueue("The creature looked oddly disfigured all over and its smile haunts you.");
                dm.sentences.Enqueue("Your heart is beating fast and you don't really feel like going inside the school since that creature just went in. You decide to just walk around for a bit to gather confidence.");
                dm.sentences.Enqueue("However, after a while of walking, you see said creature crawling out of the school again and it's now coming after you. You try to run away, but it's way faster than your speed.");
                dm.sentences.Enqueue("Once it gets to you, your whole screen is filled with a zoom in of the creature's disfigured face saying 'Come in' before crawling back inside the school...");
                dm.sentences.Enqueue("You feel like your heart almost stopped from the sudden jumpscare and you immediately pause the game.");
                if(roomieWatch) {
                    dm.sentences.Enqueue("From the corner of your eyes, you can see that your roommate also jumped in surprise.");
                }
                if(!isProtected) {
                    dm.sentences.Enqueue("You don't even realize you're peeing yourself before resting your hands on your inner thighs, in which you feel the warm flow of urine.");
                    dm.sentences.Enqueue("You grab your crotch in attempt to stop the stream and it luckily works. Your pants are already very wet now, though.");
                    if(roomieWatch) {
                        dm.sentences.Enqueue("This time, your roommate doesn't comment on anything and only stares at your glistening wet pants.");
                        dm.sentences.Enqueue("He quietly starts stroking his dick. You're surprised he can still keep his libido after getting scared by a jumpscare.");
                    }
                } else {
                    //diapered
                    dm.sentences.Enqueue("You don't even realize you're peeing yourself until you feel your bum continuously getting wetter.");
                    dm.sentences.Enqueue("You grab your crotch in attempt to stop the stream and it luckily works. Your diapers seem to be half full now, though.");
                    if(roomieWatch) {
                        if(wearJeans) {
                            dm.sentences.Enqueue("Your roommate sees you grabbing your crotch and you can tell his imagination is running wild again.");
                        } else {
                            dm.sentences.Enqueue("Your roommate sees your diapers visibly expanding as you were grabbing it and you can tell his imagination is running wild again.");
                        }
                        dm.sentences.Enqueue("He quietly starts stroking his dick. You're surprised he can still keep his libido after getting scared by a jumpscare.");
                    }
                }
                dm.sentences.Enqueue("After regaining your cool, you unpause and head into the school.");
            } else {
                //not pee self
                dm.sentences.Enqueue("You find it scary but not think much of it since it's a horror game after all.");
                dm.sentences.Enqueue("Thinking that you've explored the front of the school enough, you head into the school.");
            }
            gameCommonPath(4);
        } else if(dm.dialogueTypeTemp == "horrorDecision3") {
            //Nothing will happen, right? Well, not the worst.
            dm.sentences.Enqueue("You walk towards the girl and start talking to her. Turns out it was all good! She tells you some plot points and answers some questions you have.");
            dm.sentences.Enqueue("Afterwards, she asks you to take her to the graveyard and starts following you around.");
            dm.sentences.Enqueue("You guide her to the school's graveyard, which you are also unsure why it's there in the first place. But the school seems to be deteriorating.");
            dm.sentences.Enqueue("You feel that something is not right, and when you turn around, you find that the girl is now deformed and her face looks like that of the Screamer.");
            dm.sentences.Enqueue("You then turn your camera again to find that the rest of the students and teachers all have their faces deformed in such way.");
            dm.sentences.Enqueue("The eerie music returns and you are now being chased by the girl with a high-pitched scream. You start running away in panic.");
            if(isHorrorMax) {
                //completely pee yourself
                dm.sentences.Enqueue("Your heart feels like it's going jump out of you from how hard it's beating right now.");
                if(!isProtected) {
                    dm.sentences.Enqueue("You can't think about anything else but run away. You somewhat notice that your pants are getting warmer by the second, but you're too scared to mind it.");
                } else {
                    //diapered
                    dm.sentences.Enqueue("You can't think about anything else but run away. You somewhat notice that your diapers are expanding and getting warmer by the second, but you're too scared to mind it.");
                }
            } else {
                dm.sentences.Enqueue("You feel a pretty big leak but you still manage to hold it in while attempting to run away from the girl.");
            }
            dm.sentences.Enqueue("You keep running towards the school's graveyard because that was where the girl said she wanted to go, but you're not sure if she'll be satisfied or not.");
            dm.sentences.Enqueue("When you reach the graveyard, it has turned into a maze. You remember a certain pattern hinted in the game before and figures it's probably for this.");
            dm.sentences.Enqueue("You try your best advancing through the maze, sometimes taking wrong turns and getting damaged by the ghost.");
            dm.sentences.Enqueue("After great effort, you finally reach the end of the maze, in which you see a familiar cabin from the beginning. You enter and the ending sequence of the game plays out.");
            if(isHorrorMax) {
                dm.sentences.Enqueue("Seeing that you've finally made it to the end, you can finally pay more attention to the situation under you.");
                dm.sentences.Enqueue("You've thoroughly wet yourself now and your bladders are emptied.");
                if(wetTimes >= 3) {
                    if(!isProtected) {
                        dm.sentences.Enqueue("You've just had a genuine fear wetting and this is turning you on. You start rubbing yourself and playing with the puddle of piss sitting under you.");
                    } else {
                        //diapered
                        dm.sentences.Enqueue("You've just had a genuine fear wetting and this is turning you on. You start rubbing yourself and playing with your filled diapers.");
                    }
                    if(roomieWatch) {
                        dm.sentences.Enqueue("Your roommate watches you intensely while still doing his thing. He deliberately controls himself so the both of you can come together. You find it pretty cute.");
                    }
                } else {
                    dm.sentences.Enqueue("You: No way... I wet myself from playing a game...");
                }
                dm.sentences.Enqueue("You watch the ending of the game soaked in your own piss. After it ends, you finally turn off the game and start the clean up phase.");
                wetYourself();
                dm.dialogueTypeTemp = "";
                desperation = 0;
                leakState = 0;
                isSoaked = true;
                showCleanUp.SetActive(true);
                isHorrorMax = false;
                if(!am.scnHorrorFullPee) {
                    //omoScenarioUnlocked++;
                    am.scnHorrorFullPee = true;
                }
            } else {
                dm.sentences.Enqueue("You watch the ending of the game without fully wetting yourself, but there's a big wet spot on your pants and you still need to go pretty badly.");
                dm.sentences.Enqueue("After you reach the credits, you finally turn off the game.");
                dm.dialogueTypeTemp = "";
                desperation = tier3desperation;
                leakState = 3;
                isSoaked = false;
                isHorrorMax = false;
                if(!am.scnHorrorHalfPee) {
                    //omoScenarioUnlocked++;
                    am.scnHorrorHalfPee = true;
                }
            }
        } else if(dm.dialogueTypeTemp == "omogeDecision1") {
            dm.sentences.Enqueue("You make your character drink a ton of water and walk around school desperate until he inevitably wet himself in front of everyone.");
            dm.sentences.Enqueue("Then you make him drink more water before exploring the town. You get to watch him get more and more desperate while you learn about the town's layout.");
            dm.sentences.Enqueue("Your character ends up pissing himself again inside a supermarket.");
            dm.sentences.Enqueue("After that, it's the neighboring town, then the residential area, then the park...");
            dm.sentences.Enqueue("Seeing the character pee themselves over and over again somewhat turns you on and you start feeling like pissing yourself as well...");
            dm.sentences.Enqueue("You let out a short leak, enough to warm your groin.");
            dm.sentences.Enqueue("You: Mm..");
            if(roomieWatch) {
                dm.sentences.Enqueue("You can tell that the way you're enjoying yourself right now is turning your roommate on. He grabs his water bottle and takes a good drink from it.");
            }
            dm.sentences.Enqueue("Thinking that you've had enough fun making the guy piss all over the place, you navigate back to your home.");
            dm.sentences.Enqueue("After reading all the dialogue you had with your mom and brother for witnessing your 'accident', you change into something dry and end the day.");
            gameCommonPath(5);
        } else if(dm.dialogueTypeTemp == "omogeDecision2") {
            //Diapers
            omoChooseDiaper = true;
            dm.sentences.Enqueue("Arriving back home, you immediately put the diapers on before going to ending the day. Your character seems to feel weird wearing them, but can't do anything about it.");
            dm.sentences.Enqueue("Upon waking up... Let's just say the diapers was nicely used. You change diapers and keep using it throughout the day, not even bothering to use toilets anymore.");
            if(!isProtected) {
                if(diaperStock > 0) {
                    dm.sentences.Enqueue("Watching your character pee in his diaper so many times makes you want to wear one too in real life. You give in to your desires and happily put one on.");
                    omogeDiaperRoute();
                } else {
                    dm.sentences.Enqueue("Watching your character pee in his diaper so many times makes you want to wear one too in real life. Sadly, you don't have any at the moment.");
                    if(money >= pm.diaperPrice && !roomieWatch) {
                        dm.sentences.Enqueue("You do have the money for it, though, so you end up making a quick trip to a nearby store to buy some.");
                        money -= pm.diaperPrice;
                        diaperStock += 8;
                        audioSource.PlayOneShot(gameAudios[13]);
                        dm.sentences.Enqueue("After that, you immediately put one on and continue playing the game.");
                        omogeDiaperRoute();
                    } else {
                        dm.sentences.Enqueue("You don't even have enough money to buy some right now.");
                        if(roomieWatch) {
                            dm.sentences.Enqueue("Roomie: What are you looking for?");
                            dm.sentences.Enqueue("You: A diaper.");
                            dm.sentences.Enqueue("Your roommate has been watching your playthrough all along and he knows you well enough to know exactly what you mean.");
                            dm.sentences.Enqueue("He opens his wardrobe and picks out a tape diaper before handing it to you.");
                            dm.sentences.Enqueue("You: Huh, what, you have diapers? I thought you're more of an observer.");
                            if(!roommateFirstWitnessDiaper) {
                                dm.sentences.Enqueue("Roomie: Sh-Shut up, I was just curious... You're the one making it look so damn pleasurable all the time, anyways.");
                            } else {
                                    dm.sentences.Enqueue("Roomie: Sh-Shut up, I was just curious... and didn't really wanna make a mess all over the place like you did.");
                            }
                            dm.sentences.Enqueue("You laugh a little and gleefully accept the diaper, putting it on immediately.");
                            dm.sentences.Enqueue("Your roommate is flustered at your sudden action of removing your pants and equipping the diaper in your chair.");
                            dm.sentences.Enqueue("Roomie: W-Wait, you're changing here?!");
                            dm.sentences.Enqueue("Despite trying to look away, you can see that your roommate can't keep his eyes off you in the end.");
                            dm.sentences.Enqueue("You: Special service for being so kind to give me something to pee on~");
                            dm.sentences.Enqueue("Your words left him speechless and his face all red. After ensuring a snug fit, you turn to look at him again.");
                            dm.sentences.Enqueue("You: I'm gonna continue playing now. Feel free to keep watching.");
                            dm.sentences.Enqueue("Your roommate nods, still slightly too embarrassed to say anything.");
                            dm.sentences.Enqueue("You happily return to the game...");
                            if(!am.scnOmogeRoomieDiaper) {
                                am.scnOmogeRoomieDiaper = true;
                            }
                            omogeDiaperRoute();
                        } else {
                            dm.sentences.Enqueue("You: ...Oh well, nothing I can do about it. I'll just pee my pants like usual.");
                            dm.sentences.Enqueue("When your character is attending class and ends up peeing his diapers...");
                            dm.sentences.Enqueue("You pee a little, too.");
                            dm.sentences.Enqueue("When your character is hanging out with his friends and secretly fills his diapers with pee mid-conversation...");
                            dm.sentences.Enqueue("You can't help but also release some pee into your pants.");
                            dm.sentences.Enqueue("When your character just changed diapers and immediately wets his new one...");
                            dm.sentences.Enqueue("You proceed to open the floodgates a little more...");
                            dm.sentences.Enqueue("...Or at least, 'a little more' was the plan, until you find that pee is still streaming out even after you try holding it in.");
                            dm.sentences.Enqueue("You start feeling the wetness in your groin spread wider and wider, seeping to your bum and thighs. It doesn't take long until warm urine starts dripping noisily to the floor below.");
                            dm.sentences.Enqueue("Looking down, you can see your puddle of piss overflowing the chair, glistening as it moves steadily.");
                            dm.sentences.Enqueue("You start rubbing down your pants slowly, letting the flowing pee wet your hand.");
                            if(wearBoxers) {
                                dm.sentences.Enqueue("The material of your boxers is showing the route of your pee very nicely. A vein of pee is noticably shooting upwards, pooling into one area before it branches down into smaller veins of pee.");
                                dm.sentences.Enqueue("The thin fabric also gives you a nice view of the stream you're making.");
                            } else if(wearJeans) {
                                dm.sentences.Enqueue("Your jeans make for a good pee absorbent. You watch as pee trails down your calves, tracing a dark line on your jeans. Once it reaches the hems, the unabsorbed pee escapes, adding to the noise.");
                                dm.sentences.Enqueue("You lift yourself off the chair a little, freeing the trapped liquid that has been pooling under your bum resulting in a multiple waterfalls forming under you.");
                            }
                            dm.sentences.Enqueue("After peeing it all out, you play with yourself for a bit before heading to the bathroom to clean up.");
                        }
                    }
                }
            } else {
                dm.sentences.Enqueue("You're currently wearing diapers in real life as well, so now you sync with the main character.");
                omogeDiaperRoute();
            }
            gameCommonPath(6);
        } else if(dm.dialogueTypeTemp == "omogeDecision3") {
            //Research == scary points+++
            dm.sentences.Enqueue("You decide to do an in-depth research so you can scare the fuck out of all your friends during horror night.");
            dm.sentences.Enqueue("...The camp grounds used to be a graveyard...");
            dm.sentences.Enqueue(".....A lot of people testified that they can hear uncanny voices calling out to them in the middle of the night....");
            dm.sentences.Enqueue("........The campfire might suddenly die down in an instant by itself. When it happens, it means you need to hide in your tents and never go back out before morning time........");
            dm.sentences.Enqueue("Good job, now you've scared yourself so badly that you're uncontrollably peeing yourself while researching all of this.");
            dm.sentences.Enqueue("You: (Well, this is good. That means my friends will probably also wet themselves when they hear my bone-chilling stories...)");
            dm.sentences.Enqueue("That night, you couldn't sleep at all. In the end, you decide to swallow your pride and ask your brother if you can sleep in his room. Having someone else is reassuring, after all.");
            dm.sentences.Enqueue("Your brother teases you about it, but scoots away to accomodate you anyway. You're grateful that you have such an understanding brother.");
            dm.sentences.Enqueue("The following day, you head to the camp grounds and meet your friends there.");
            dm.sentences.Enqueue("After setting everything up, you guys start having fun. Numerous board games are played, cuts of meat and vegetables are barbecued, and topics of conversation are discussed.");
            dm.sentences.Enqueue("Time flies and night comes. Everyone is sitting around the campfire, grilling marshmallows. It is time for horror night.");
            dm.sentences.Enqueue("As fate has it, you get to be the last person to tell your story. The mood has already been set from the previous tales and you can tell that some of your friends are scared.");
            dm.sentences.Enqueue("To be honest, since you've meticulously researched the camp grounds before, you can tell where the previous stories possibly gained their roots. Knowing them, it scares you even more.");
            if(omoChooseDiaper) {
                dm.sentences.Enqueue("Your heart is racing and your diaper is damp from all the short uncontrollable fear peeing you did listening to the stories.");
            } else {
                dm.sentences.Enqueue("Your heart is racing and your pants are damp from all the short uncontrollable fear peeing you did listening to the stories.");
            }
            dm.sentences.Enqueue("You start telling your story, all based on true stories and testimonies from a lot of people. Somehow, you can feel the air getting colder around you.");
            dm.sentences.Enqueue("All of your friends seem to be on edge as well. You can tell that your story has very effectively scared them.");
            dm.sentences.Enqueue("However, what scares everyone the most is the fact that the campfire suddenly dies on its own a few seconds after you finished your story.");
            dm.sentences.Enqueue("Nobody screams. The fear and shock has rendered everyone speechless. Thinking that you should take responsibility for telling such a scary story, you take the lead...");
            dm.sentences.Enqueue("You: I think... we should return to our tents...");
            dm.sentences.Enqueue("Hearing this, everyone immediately scurries off to their tents. You included.");
            if(omoChooseDiaper) {
                dm.sentences.Enqueue("Your legs are shaking hard from fear and they feel like jelly. The unexpected act of the fire dying down made you flood hard into your diapers.");
                dm.sentences.Enqueue("You're still releasing torrents of urine even while running inside your tent and closing the tent. The warm diaper hugging you is very comforting right now.");
            } else {
                dm.sentences.Enqueue("Your legs are shaking hard from fear and they feel like jelly. The unexpected act of the fire dying down made you flood hard into your pants.");
                dm.sentences.Enqueue("You're still releasing torrents of urine even while running inside your tent and closing the tent. You know you're making a mess everywhere, but you'll take care of it once you calm down.");
            }
            dm.sentences.Enqueue("At least the story says that you'll be safe if you stay in your tents...");
            dm.sentences.Enqueue("You look at your friend beside you. It is very noticable how he's trembling in fear.");
            dm.sentences.Enqueue("He seems to look conflicted about something while grabbing at his expanded pants.");
            dm.sentences.Enqueue("Friend: G-Guess there's no use in hiding it, but I'm wearing diapers... I really need to change into fresh ones after this though... If you don't mind...");
            if(omoChooseDiaper) {
                dm.sentences.Enqueue("You: Same... I also need to change in a bit.");
                dm.sentences.Enqueue("Friend: Woah, so we're all wearing diapers? Sweet!");
                dm.sentences.Enqueue("You: There's not much choice with this stupid virus going around anyway.");
            } else {
                dm.sentences.Enqueue("Friend: You know, you could try wearing them too...");
                dm.sentences.Enqueue("He looks at you, then to the very visible puddles of piss leading straight to you.");
                dm.sentences.Enqueue("You: I'm gonna clean it later!");
                dm.sentences.Enqueue("Friend: You better.");
            }
            dm.sentences.Enqueue("You both clean up the evidence of your accidents and sleep.");
            gameCommonPath(7);
        }

        dm.DisplayCurrSentence();
    }

    private void omogeDiaperRoute() {
        isProtected = true;
        diaperStock--;
        if(roomieWatch) {
            dm.sentences.Enqueue("All of a sudden, your roommate gets up from his chair and walks out of the room.");
            dm.sentences.Enqueue("Roomie: Sorry, can you pause for a bit? I'll be back in like 10 mins.");
            dm.sentences.Enqueue("You: Oh, sure...?");
            dm.sentences.Enqueue("His actions leave you confused, but you pause the game anyway and wait for him to return.");
            dm.sentences.Enqueue("Around 10 minutes later, he returns and sits himself next to you.");
            dm.sentences.Enqueue("Roomie: Thanks for waiting... Really appreciate it. You can continue now.");
            dm.sentences.Enqueue("You: What did you even do?");
            dm.sentences.Enqueue("Roomie: Mm... You'll find out later.");
            dm.sentences.Enqueue("You eye him suspiciously, but since it doesn't seem like he's going to budge, you give up on it and continue with your game.");
        }
        dm.sentences.Enqueue("When your character is attending class and ends up peeing his diapers...");
        dm.sentences.Enqueue("You pee a little, too.");
        dm.sentences.Enqueue("When your character is hanging out with his friends and secretly fills his diapers with pee mid-conversation...");
        dm.sentences.Enqueue("You also release some into yours.");
        dm.sentences.Enqueue("When your character just changed diapers and immediately wets his new one...");
        dm.sentences.Enqueue("You also wet yours a little more...");
        dm.sentences.Enqueue("...Or at least, 'a little more' was the plan, until you find that pee is still streaming out even after you try holding it in.");
        dm.sentences.Enqueue("Giving up, you opt to lay back and start feeling your expanding diaper instead. Slowly rubbing your diapered groin, you can feel it growing hotter and hotter with pee.");
        dm.sentences.Enqueue("Bit by bit, you relax your bladder muscles even more, rapidly filling the diaper and swelling it to its maximum capacity.");
        dm.sentences.Enqueue("You enjoy the feeling of your own hot urine climbing up to your skin and clinging to it.");
        if(!firstTimeUseDiaper) {
            dm.sentences.Enqueue("Somehow, your body always become still whenever you pee in your diapers. It's as if you want to just focus on the feeling of it.");
        }
        dm.sentences.Enqueue("Once you're done, you wiggle your hips slightly, making the pee stored below slosh around.");
        if(!firstTimeUseDiaper) {
            dm.sentences.Enqueue("Knowing full well this diaper can't hold more than one pee session, you carefully stand up, feeling the heavy diaper in your crotch.");
        } else {
            dm.sentences.Enqueue("Despite this being your first diapered experience, you feel like it won't be able to hold any more. You carefully stand up, feeling the heavy diaper in your crotch.");
            firstTimeUseDiaper = false;
        }
        if(roomieWatch) {
            dm.sentences.Enqueue("Your roommate has been suspiciously quiet. Usually he'd be jerking off by now, but he's just sitting still.");
            dm.sentences.Enqueue("You: You good?");
            dm.sentences.Enqueue("Roomie: ...Huh? Y-Yeah.");
            dm.sentences.Enqueue("He sounds nervous, so you inspect him even more to find out what he's hiding. It's not really noticable at first, but you feel like your roommate's pants look more swollen than usual.");
            if(am.scnOmogeRoomieDiaper) {
                dm.sentences.Enqueue("Well, you do know that your roommate actually has some diapers stocked up. Could it be...?");
                dm.sentences.Enqueue("You: Are you wearing diapers?");
                dm.sentences.Enqueue("Roomie: Wha- How did you know?");
                dm.sentences.Enqueue("You: Just thinking that your pants look fuller than usual. Also from the unusual behavior you're showing.");
                dm.sentences.Enqueue("Roomie: I see... Well, alright. You caught me.");
                dm.sentences.Enqueue("You: Are you peeing?");
                dm.sentences.Enqueue("Roomie: ...Mm. Almost done.");
                dm.sentences.Enqueue("You: So, you wear your diapers often?");
                dm.sentences.Enqueue("Roomie: Uh, only when I feel like it...");
            } else {
                dm.sentences.Enqueue("You figure it must be just your imagination, though. Other than that, you find nothing odd about him.");
                dm.sentences.Enqueue("You: If you say so... Well, I'm gonna change my diapers now.");
            }
        }
        dm.sentences.Enqueue("You slightly waddle to the bathroom and change into fresh diapers after cleaning yourself up.");
    }

    public void gameSecondOption() {
        showGameEvent.SetActive(false);
        if(dm.dialogueTypeTemp == "gameDecision1") {
            //Wrong option
            dm.sentences.Enqueue("You only waited for a bit, but with that time, the enemy team has now moved near your target. There's no way you can kill your target without them retaliating and killing you.");
            dm.sentences.Enqueue("You attempt to pull out from the flank mission, but the enemies have now noticed your presence.");
            dm.sentences.Enqueue("You try your best to stay alive and head back to your teammates, but it's really not the easiest task in the world.");
            dm.sentences.Enqueue("You: (They're actually chasing after me... Fuck!)");
            dm.sentences.Enqueue("You feel your heartbeat racing as you're running away from the enemy, trying your best to survive.");
            //Leak -> controlled in DM
            if(leakState < 3) {
                dm.sentences.Enqueue("Your crotch gets slightly warmer and you realize you just leaked, but you pay it no mind as you've successfully returned to your team.");
                if(roomieWatch && !isProtected) {
                    dm.sentences.Enqueue("Your roommate silently stares at the small wet spot on your pants.");
                }
                gameCommonPath(1);
            } else {
                //full on wetting
                gameWetting();
            }
        } else if(dm.dialogueTypeTemp == "gameDecision2") {
            //Correct option
            dm.sentences.Enqueue("You decide to just leave it be. Who knows if he suddenly says something important later?");
            dm.sentences.Enqueue("Your team fought head on with the enemy team and won, but you died in the fight itself.");
            dm.sentences.Enqueue("After respawning, you hear the noisy-mic guy talk, warning you about a spawncamp.");
            dm.sentences.Enqueue("You look around the spawn area and spots the enemy that guy warned you about, you manage to kill them pretty easily thanks to being prepared.");
            dm.sentences.Enqueue("After that, you rejoin your team safely.");
            gameCommonPath(2);
        } else if(dm.dialogueTypeTemp == "gameDecision3") {
            //Correct option
            dm.sentences.Enqueue("You decide to try and hold it in until the end of the game. You somehow manage to not leak, but at the cost of your focus.");
            dm.sentences.Enqueue("Your team ends up losing the game, but hey, you haven't leaked even once! That should be an achievement in itself!");
            if(!am.scnShooterHold) {
                //omoScenarioUnlocked++;
                am.scnShooterHold = true;
            }
            dm.dialogueTypeTemp = "";
            desperation = tempdesperation;
        } else if(dm.dialogueTypeTemp == "roomieSleep") {
            //Common Route
            dm.sentences.Enqueue("You: I think I'm good. I'll find some other way.");
            dm.sentences.Enqueue("Your roommate looks somewhat disappointed.");
            dm.sentences.Enqueue("Roomie: Suit yourself. I'll go back to sleep then.");
            dm.sentences.Enqueue("He walks back to his bed and tucks himself to sleep.");
            commonNightPeeRoute();
            dm.dialogueTypeTemp = "";
        } else if(dm.dialogueTypeTemp == "wakeUpDry") {
            dm.sentences.Enqueue("You go to the common restroom to relieve yourself.");
            dm.sentences.Enqueue("After you're done, you return to your room.");
            dm.dialogueTypeTemp = "";
            desperation = 0;
            leakState = 0;
        } else if(dm.dialogueTypeTemp == "horrorDecision1") {
            //Decide to play with minimal effect
            bgmAudioSource.clip = gameBGM[1];
            bgmAudioSource.Play();
            dm.sentences.Enqueue("You decide to just play it right now, too scared to play the game at night.");
            if(roomieWatch) {
                dm.sentences.Enqueue("Your roommate seems interested in the game you're playing and rolls his chair over to watch you.");
            }
            dm.sentences.Enqueue("The game starts off with the player being in an abandoned cabin somewhere on an unnamed forest, waking up without any memory whatsoever.");
            dm.sentences.Enqueue("The atmosphere inside the game is already very creepy, but you're glad the sun is still shining outside in real life.");
            dm.sentences.Enqueue("You carefully try to walk around the cabin. You feel like your heart isn't ready yet for what you'll encounter in the vast forest. However...");
            dm.sentences.Enqueue("A sound was heard behind you out of nowhere and your headphones are making it sound very realistic.");
            dm.sentences.Enqueue("You feel yourself leaking slightly.");
            if(!isProtected) {
                dm.sentences.Enqueue("It was just a spurt of pee and it doesn't do much damage to your pants.");
            } else {
                dm.sentences.Enqueue("It was just a spurt of pee and it doesn't wet your diapers that much.");
            }
            if(roomieWatch) {
                dm.sentences.Enqueue("Your roommate doesn't seem to notice either. He can't hear the voice, anyway.");
            }
            gameCommonPath(3);
        } else if(dm.dialogueTypeTemp == "horrorDecision2") {
            dm.sentences.Enqueue("You decide to walk briskly forward to avoid looking back.");
            dm.sentences.Enqueue("However, no matter how much you walk, the scenery in front of you doesn't seem to change.");
            dm.sentences.Enqueue("You: (Is it a loop?)");
            dm.sentences.Enqueue("Just as you're thinking that, the forest turns dark all of a sudden. Then suddenly, a disfigured face smiles at you, filling the screen.");
            dm.sentences.Enqueue("You jump and a warm sensation fills your crotch.");
            if(roomieWatch) {
                dm.sentences.Enqueue("From the corner of your eyes, you can see that your roommate also jumped in surprise.");
            }
            if(isHorrorMax) {
                if(!isProtected) {
                    dm.sentences.Enqueue("You don't realize it immediately, but when your hand falls to your thighs and feels something wet, that's when it hits you.");
                } else { 
                    dm.sentences.Enqueue("You don't realize it immediately, but when you feel the soft padding under you get wetter and warmer, that's when it hits you.");
                }
                dm.sentences.Enqueue("Realizing you're peeing yourself, you grab at your crotch in attempt to stop the flow. Luckily, it works.");
                dm.sentences.Enqueue("You: (No way... Almost wet myself completely just from that...)");
                if(roomieWatch) {
                    if(!isProtected) {
                        dm.sentences.Enqueue("This time, your roommate doesn't comment on anything and only stares at your glistening wet pants.");
                    } else {
                        if(wearJeans) {
                            dm.sentences.Enqueue("Your roommate sees you grabbing your crotch and you can tell his imagination is running wild again.");
                        } else {
                            dm.sentences.Enqueue("Your roommate sees your diapers visibly expanding as you were grabbing it and you can tell his imagination is running wild again.");
                        }
                    }
                    dm.sentences.Enqueue("He quietly starts stroking his dick. You're surprised he can still keep his libido after getting scared by a jumpscare.");
                }
            } else {
                dm.sentences.Enqueue("You leaked a little, but it's nothing big. You can still do this.");
            }
            dm.sentences.Enqueue("The creature is telling you to turn around, so you do and the scenery changes into that of an abandoned school.");
            dm.sentences.Enqueue("You explore the new area for a bit, then you head inside the school building.");
            gameCommonPath(4);
        } else if(dm.dialogueTypeTemp == "horrorDecision3") {
            //You try to be careful, but it backfires
            dm.sentences.Enqueue("You try to get a good look on the girl with the zoom-in feature, but after a while of inspecting her, she stops singing and turns to look at you.");
            dm.sentences.Enqueue("Then her face slowly deforms alongside with her body. You stop the camera mode in fear, and turns around to leave only to find the deformed students and teachers staring at you.");
            if(isHorrorMax) {
                if(!isProtected) {
                    dm.sentences.Enqueue("You just start pissing yourself at the sight, urine rapidly gushing out of you and soaking your pants.");
                    dm.sentences.Enqueue("Too scared to continue, you pause the game. You look down to watch yourself pee while trying to calm down.");
                    dm.sentences.Enqueue("You try to hold it, but your muscles just won't let you anymore, so you opt to just relax and let it flow.");
                } else {
                    //diapered
                    dm.sentences.Enqueue("You just start pissing yourself at the sight, urine rapidly gushing out of you and soaking your diapers.");
                    dm.sentences.Enqueue("Too scared to continue, you pause the game. Your senses focus to the sensation of losing control.");
                    dm.sentences.Enqueue("You see no point in trying to stop the flow. The feeling of your diaper expanding and hugging you warmly is very comforting.");
                }
            } else {
                dm.sentences.Enqueue("You realize that you've started pissing yourself. You pause the game and quickly grabs your crotch to try and hold it in.");
                if(!isProtected) {
                    dm.sentences.Enqueue("Pee gushes over your hand but you find no success in stopping it whatsoever.");
                    dm.sentences.Enqueue("The flow only stopped when you've already let out about 80% of your bladder and your pants are already fully soaked. You sigh and decide to just let it all out.");
                } else {
                    dm.sentences.Enqueue("Your diapers only continue expanding and it feels impossible to stop the stream now.");
                    dm.sentences.Enqueue("The flow only stopped when you've already let out about 80% of your bladder and you feel like your lower regions are already fully submerged in pee. You see no point in holding and decide to just let it all out.");
                }
            }
            dm.sentences.Enqueue("Still peeing yourself, you decide that this distracted you enough from the fear and you continue playing the game.");
            dm.sentences.Enqueue("The girl is now chasing you and you just run aimlessly until you notice that the school's graveyard has been turned into a maze.");
            dm.sentences.Enqueue("You remember a pattern mentioned somewhere previously and advance through it using the knowledge.");
            dm.sentences.Enqueue("After great effort, you finally reach the end of the maze, in which you see a familiar cabin from the beginning. You enter and the ending sequence of the game plays out as your stream finally tapers off.");
            dm.sentences.Enqueue("After you reach the credits, you finally turn off the game.");
            if(wetTimes >= 3) {
                if(!isProtected) {
                    dm.sentences.Enqueue("You've just had a genuine fear wetting and this is turning you on. You start rubbing yourself and playing with the puddle of piss sitting under you.");
                } else {
                    //diapered
                    dm.sentences.Enqueue("You've just had a genuine fear wetting and this is turning you on. You start rubbing yourself and playing with your filled diapers.");
                }
                if(roomieWatch) {
                    dm.sentences.Enqueue("Your roommate watches you intensely while still doing his thing. He deliberately controls himself so the both of you can come together. You find it pretty cute.");
                }
            } else {
                dm.sentences.Enqueue("You: Sigh... I wet myself from playing a game...");
            }
            wetYourself();
            dm.dialogueTypeTemp = "";
            desperation = 0;
            leakState = 0;
            isSoaked = true;
            showCleanUp.SetActive(true);
            isHorrorMax = false;
            if(!am.scnHorrorFullPee) {
                //omoScenarioUnlocked++;
                am.scnHorrorFullPee = true;
            }
        } else if(dm.dialogueTypeTemp == "omogeDecision1") {
            dm.sentences.Enqueue("You decide to go home and just continue on with the plot.");
            dm.sentences.Enqueue("You had to end the day to continue on with the plot, so you did just that.");
            gameCommonPath(5);
        } else if(dm.dialogueTypeTemp == "omogeDecision2") {
            //Undiapered
            omoChooseDiaper = false;
            dm.sentences.Enqueue("Ignoring the doctor's warnings, you let your character keep wearing his briefs instead of diapers. Your character seems happy about it, confident about not needing it.");
            dm.sentences.Enqueue("Of course, your character wakes up to a wet bed the next morning. You change his underwear to a new pair of white briefs and head to school as usual.");
            dm.sentences.Enqueue("The incontinence keeps getting worse each day. Eventually, it comes to a point where you don't even feel the need to make the character drink much to make them have an accident anymore.");
            dm.sentences.Enqueue("The game has also fully stopped giving you any sort of warning before the peeing occurs, giving a nice, unpredictability element in the game.");
            dm.sentences.Enqueue("You start focusing more on what the game has to offer: building relationships, working part-time jobs, story events...");
            dm.sentences.Enqueue("...And of course, sometimes your character just pees himself in the middle of them. Those times are always very pleasantly surprising and you tend to also let yourself leak everytime he wets.");
            dm.sentences.Enqueue("During a guys' night out, he unknowingly starts peeing after witnessing his friend drunkenly wetting himself.");
            dm.sentences.Enqueue("As if triggering a chain reaction, you leak a good amount before stopping yourself.");
            dm.sentences.Enqueue("The next day, he unexpectedly wets himself after being asked a question and answering correctly.");
            dm.sentences.Enqueue("You imagine him nervously answering the question, accidentally relaxing his bladders after feeling relief from being right.");
            dm.sentences.Enqueue("Putting yourself in his shoes, you relax for a while, enlarging the wet spot on your pants even more.");
            dm.sentences.Enqueue("The next wetting occurrence happens right after he wakes up. As usual, he wets the bed and you must change him into new undergarments before dressing him up for school.");
            dm.sentences.Enqueue("However, his pee starts trickling out the moment he puts on his uniform, soaking it thoroughly.");
            dm.sentences.Enqueue("You were really caught off guard since the timing was too good. You decide to just slowly let everything go as you carefully read the way his accident is described, imagining it for yourself.");
            dm.sentences.Enqueue("You start feeling the wetness in your groin spread wider and wider, seeping to your bum and thighs. It doesn't take long until warm urine starts dripping noisily to the floor below.");
            dm.sentences.Enqueue("Looking down, you can see your puddle of piss overflowing the chair, glistening as it moves steadily.");
            dm.sentences.Enqueue("You start rubbing down your pants slowly, letting the flowing pee wet your hand.");
            if(wearBoxers) {
                dm.sentences.Enqueue("The material of your boxers is showing the route of your pee very nicely. A vein of pee is noticably shooting upwards, pooling into one area before it branches down into smaller veins of pee.");
                dm.sentences.Enqueue("The thin fabric also gives you a nice view of the stream you're making.");
            } else if(wearJeans) {
                dm.sentences.Enqueue("Your jeans make for a good pee absorbent. You watch as pee trails down your calves, tracing a dark line on your jeans. Once it reaches the hems, the unabsorbed pee escapes, adding to the noise.");
                dm.sentences.Enqueue("You lift yourself off the chair a little, freeing the trapped liquid that has been pooling under your bum resulting in a multiple waterfalls forming under you.");
            }
            dm.sentences.Enqueue("After peeing it all out, you play with yourself for a bit before heading to the bathroom to clean up.");
            gameCommonPath(6);
            //Scrapped dialogue
            /*dm.sentences.Enqueue("Everything is going smoothly until your character suddenly pees himself without prior warning by the end of the class.");
            dm.sentences.Enqueue("As a player, you are surprised because there are usually some sort of bladder dialogue that prepares you before completely losing control.");
            dm.sentences.Enqueue("It turns out to be a game event and you are introduced to another student who helps you get to the nurse's office stealthily.");
            dm.sentences.Enqueue("Seeing the condition of your pants, the nurse gives you a diaper to change into. She even helps you put it on, successfully embarrassing your character.");
            dm.sentences.Enqueue("Due to everyone pissing themselves nowadays, the nurse's office is all out of clean pants. Thankfully, the student who helped you earlier lends you a clean pair.");
            dm.sentences.Enqueue("Later on, you find out that he's also dealing with incontinence and therefore brings spare pants with him everywhere just to be safe.");*/
            
        } else if(dm.dialogueTypeTemp == "omogeDecision3") {
            //Generic == normal scary points
            dm.sentences.Enqueue("You decide to just come up with some generic-ass horror stories that you've heard before. You can only hope your friends haven't.");
            dm.sentences.Enqueue("The following day, you head to the camp grounds and meet your friends there.");
            dm.sentences.Enqueue("After setting everything up, you guys start having fun. Numerous board games are played, cuts of meat and vegetables are barbecued, and topics of conversation are discussed.");
            dm.sentences.Enqueue("Time flies and night comes. Everyone is sitting around the campfire, grilling marshmallows. It is time for horror night.");
            dm.sentences.Enqueue("As fate has it, you get to be the last person to tell your story. The mood has already been set from the previous tales and you can tell that some of your friends are scared.");
            dm.sentences.Enqueue("You tell a story about a weeping ghost in the forest who lures people out using their wails, only to trap them in the forest forever.");
            dm.sentences.Enqueue("It's a pretty basic story and most of the scare factor comes from the mood and atmosphere. The night is silent and cold, with occasional crow caws heard from the distance.");
            dm.sentences.Enqueue("After talking for a bit more to ease up the mood, you head back in to your tent to sleep while some of your friends are still outside and doing their thing.");
            dm.sentences.Enqueue("...");
            dm.sentences.Enqueue("Somewhere around 4 AM...");
            if(omoChooseDiaper) {
                dm.sentences.Enqueue("You are awoken by something, but not quite sure what. Moving your butt around, you can feel that you've filled your diaper.");
                dm.sentences.Enqueue("You decide to change into new ones since you're awake, careful not to wake your tentmate.");
                dm.sentences.Enqueue("While changing, you pick up some sounds from outside of the tent. You wonder if one of your friends is up and about.");
            } else {
                dm.sentences.Enqueue("You are awoken by something, but not quite sure what. You can feel that you somewhat need to go, though, and you don't want to go in your sleeping bag.");
                dm.sentences.Enqueue("You crawl out and your ears pick some noises up from outside of the tent. You wonder if one of your friends is up and about.");
            }
            dm.sentences.Enqueue("Peeking outside, you can indeed see your friend sitting on a log around the campfire. You decide to approach him.");
            dm.sentences.Enqueue("You: Can't sleep?");
            dm.sentences.Enqueue("Your voice startled him, but he scoots away to give you sitting space beside him after recognizing you.");
            dm.sentences.Enqueue("Friend: Something woke me up... It's hard for me to fall back asleep after waking up.");
            dm.sentences.Enqueue("You: Yeah, I also don't know what woke me up. Usually I just sleep all the way to morning...");
            dm.sentences.Enqueue("Friend: Weird. Maybe it's because we're just not used to the sleeping conditions or something.");
            dm.sentences.Enqueue("You: Could be...");
            dm.sentences.Enqueue("Both of you talk about miscellaneous things for a while.");
            dm.sentences.Enqueue("Friend: You probably already know this, but I usually wear diapers. I think I packed too little for this trip, though...");
            dm.sentences.Enqueue("You: How much you have left?");
            dm.sentences.Enqueue("Friend: ...Two.");
            dm.sentences.Enqueue("You: Dude... We still have the whole Saturday and half Sunday...");
            dm.sentences.Enqueue("Friend: Exactly. I'm actually not wearing any right now to minimize my diaper usage...");
            dm.sentences.Enqueue("You: Wow, and how are you holding up?");
            dm.sentences.Enqueue("Friend: I was actually planning to pee somewhere in the woods but...");
            dm.sentences.Enqueue("Friend: I heard some noises... So I backed out...");
            dm.sentences.Enqueue("You: Did my story scare you?");
            dm.sentences.Enqueue("Friend: What if it has some truth to it? We both woke up out of nowhere and there are sounds coming from the woods.");
            dm.sentences.Enqueue("You: Well, when you put it like that...");
            dm.sentences.Enqueue("Friend: And if I pee around the campfire, I fear it's gonna smell up the place and stuff...");
            if(omoChooseDiaper) {
                dm.sentences.Enqueue("You: Wanna go in mine?");
                dm.sentences.Enqueue("Friend: You mean you have a spare?");
                dm.sentences.Enqueue("You: No, but I'm wearing one right now. They're also the thick ones, so I should still be able to use it after you used it.");
                dm.sentences.Enqueue("Friend: R-really?");
                dm.sentences.Enqueue("I pull my pants down and loosen the tapes on my diaper, widening the waistband enough for someone to pee in.");
                dm.sentences.Enqueue("You: Just go. It's fine.");
                dm.sentences.Enqueue("He looks somewhat embarrassed, but ends up giving in to his urgent need. He took his dick out and slides it down the waistband opening.");
                dm.sentences.Enqueue("It doesn't take long for him to start peeing. The familiar wet and warm sensation rapidly grows under me.");
                dm.sentences.Enqueue("The strong stream goes for a while before it finally slows down.");
                dm.sentences.Enqueue("Once he's done, he takes it out and I re-tape my diaper to a snug fit.");
                dm.sentences.Enqueue("Friend: Sorry about that... And thanks.");
                dm.sentences.Enqueue("You: No worries. When you gotta go, you gotta go.");
                if(!am.scnOmogeLendADiaper) {
                    am.scnOmogeLendADiaper = true;
                }
            } else {
                //wearing pants
                dm.sentences.Enqueue("You: It will be fine as long as we dillute it with enough water.");
                dm.sentences.Enqueue("Friend: Oh, right... You probably know all the tricks to secretly wetting yourself, huh?");
                dm.sentences.Enqueue("You: Yeah. I have plenty of tricks in my book, so I can help you.");
                dm.sentences.Enqueue("Friend: Alright, I'll believe you.");
                dm.sentences.Enqueue("He pulls out his dick and aims it to the ground below. It doesn't take long for him to start peeing.");
                dm.sentences.Enqueue("Watching his stream, I silently start peeing myself as well. If one inspects me carefully, they'll be able to see that the log under me is suspiciously darker in color than the rest.");
                dm.sentences.Enqueue("I don't need to be too careful with the noises since his pee will mask mine for now.");
                dm.sentences.Enqueue("I slow down once I see his stream getting weaker. I still have quite a lot of pee left even after he's done with his, so I decide to talk a bit more as a distraction.");
                dm.sentences.Enqueue("You: Are you done or are you holding back?");
                dm.sentences.Enqueue("Friend: Why would I hold back? I peed everything out.");
                dm.sentences.Enqueue("You steadily pee, controlling the power to prevent making any splashing sounds on the ground.");
                dm.sentences.Enqueue("You: Well, it just seemed like it wasn't a lot.");
                dm.sentences.Enqueue("Friend: That's not a lot to you? How much do you usually pee?");
                dm.sentences.Enqueue("You: I can go on for a minute or more with a full bladder.");
                dm.sentences.Enqueue("Friend: Oh, alright, I get that. I think I just didn't drink that much today due to my diaper stock problems...");
                dm.sentences.Enqueue("You're finally almost done peeing, so there's no need for distractions anymore.");
                dm.sentences.Enqueue("You: Makes sense. Alright then, let's take some water from the river to dillute the puddle.");
                dm.sentences.Enqueue("Friend: Alright, I'll just grab the bucket we usually use to wash up.");
                dm.sentences.Enqueue("Your friend goes in his tent and takes a bucket out just as you finish peeing. Both of you then proceed to 'wash' the logs and the ground with water.");
                dm.sentences.Enqueue("It seems he really doesn't notice that you peed yourself. You've successfully wet yourself secretly yet again.");
                if(!am.scnOmogeSecretPee) {
                    am.scnOmogeSecretPee = true;
                }
            }
            dm.sentences.Enqueue("Both of you talk for a bit more before going back to your tents to try and sleep a bit more before sunrise.");
            gameCommonPath(7);
        }

        dm.DisplayCurrSentence();
    }

    private void gameCommonPath(int gameInd) {
        switch(gameInd) {
            case 1:
                dm.sentences.Enqueue("Your team won the fight for that round, and you are now preparing to go to the second round.");
                dm.sentences.Enqueue("One of your teammates have their mic open and it sounds pretty noisy there, though it's not that badly distracting.");
                dm.sentences.Enqueue("This teammate haven't really said anything in the first round, but should you just mute them?");
                dm.dialogueTypeTemp = "gameDecision2";
                //show option
                gameEventController(2);
            break;
            
            case 2:
                dm.sentences.Enqueue("The second fight was a tight game and the enemy team ended up winning that one. You are now prepping for the third round.");
                dm.sentences.Enqueue("You feel your bladder screaming for relief as you haven't done so in a long while. You start shifting in your seat to hold it in.");
                dm.sentences.Enqueue("It is now the final and deciding round on whether you win or lose this game.");
                dm.sentences.Enqueue("You try to play as usual, but your full bladder is making it hard for your to completely focus on the game.");
                dm.sentences.Enqueue("Maybe you should just pee a bit to make it better?");
                dm.dialogueTypeTemp = "gameDecision3";
                //show option
                gameEventController(3);
            break;

            case 3:
                dm.sentences.Enqueue("You look around but see nothing that could've caused such a sound in the cabin, but you don't want to take any chances.");
                dm.sentences.Enqueue("You figure that you've done enough exploring in the cabin anyway, so you head out to the forest.");
                dm.sentences.Enqueue("There are a couple wild animals in the forest, but something seems rather off with them.");
                dm.sentences.Enqueue("The rabbits jump side to side while staring directly at you, the birds are really big, and the ants are banding together to make some sort of symbol.");
                dm.sentences.Enqueue("Now that you inspect it further, the ants are forming an arrow pointing towards you... or is it pointing behind you?");
                dm.dialogueTypeTemp = "horrorDecision2";
                gameEventController(7);
            break;

            case 4:
                dm.sentences.Enqueue("The school is dark and everything looks old. You explore the rooms and find clues and items to progress the story.");
                if(isHorrorMax) {
                    dm.sentences.Enqueue("There are a few jumpscares here and there that made you leak a couple times, but at least you didn't lose all control like before.");
                }
                dm.sentences.Enqueue("You finally solved a key puzzle and the game becomes eerily silent, following that is a haunting singing voice coming from somewhere.");
                dm.sentences.Enqueue("It's clear that the game wants you to explore and find out where the voice is coming from, so you do.");
                dm.sentences.Enqueue("Upon exiting the room you're in, the music starts again but this time it's a cheerful one. The school is suddenly restored to its pristine condition and it has even turned into morning time now with students and teachers walking about.");
                dm.sentences.Enqueue("You feel a sense of relief now that it doesn't look as scary anymore, but there's still the singing voice.");
                dm.sentences.Enqueue("After exploring for a bit, you finally find a singular girl singing alone in a classroom. Now, everything is bright and cheery now but do you trust it and fearlessly walk up to the girl?");
                dm.dialogueTypeTemp = "horrorDecision3";
                gameEventController(8);
            break;

            case 5:
                dm.sentences.Enqueue("The next day, you attend school as usual. This time, an event triggered and your character ends up hanging out with a friend.");
                dm.sentences.Enqueue("He invites you to have a sleepover at his place and your character happily accepts, thinking that it will be fun.");
                dm.sentences.Enqueue("It indeed turned out to be fun. However, something embarrassing happened on the morning after the sleepover.");
                dm.sentences.Enqueue("You unexpectedly wet the bed. Your friend only laughed and didn't make a big deal out of it, so you thought it's all good in the end.");
                dm.sentences.Enqueue("Turns out his mom snitched and told yours. Your mom immediately sends you to a doctor because wetting the bed at your age is not exactly common.");
                dm.sentences.Enqueue("You are then informed that a lot of boys have been having incontinence issues recently. It seems like a strange virus has struck the entire city and it's getting worse.");
                dm.sentences.Enqueue("The doctor then prescribes you diapers because no cure have been found just yet. Apparently, you just have to keep having accidents until then.");
                dm.sentences.Enqueue("Despite your reluctance, you still ended up buying some diapers on the way home.");
                dm.sentences.Enqueue("As a player, you can decide whether to wear the diapers or not. So, what's your verdict?");
                dm.dialogueTypeTemp = "omogeDecision2";
                gameEventController(10);
            break;

            case 6:
                dm.sentences.Enqueue("You continue playing the game afterwards, making sure to drink a lot of water in the process.");
                dm.sentences.Enqueue("You: (Time to immerse myself in some roleplay!)");
                dm.sentences.Enqueue("(You will now describe things as if you are the game character himself.)");
                dm.sentences.Enqueue("The next major event is a camping trip with you and all of your friends. Your incontinence is really bad now, and you're peeing yourself way more frequently.");
                dm.sentences.Enqueue("This problem should also be spreading throughout the city, but you don't know how bad the others have it. You've never really witnessed your friends peeing themselves either.");
                dm.sentences.Enqueue("The logical conclusion is that they're probably wearing diapers.");
                if(omoChooseDiaper) {
                    dm.sentences.Enqueue("You've also been properly wearing diapers everywhere now. Heck, you never even touch your normal underwear anymore.");
                    dm.sentences.Enqueue("It's not regular diapers anymore, though. You've started wearing thick diapers that can be used multiple times over without leaking.");
                    dm.sentences.Enqueue("Sure, that makes it super visible through your pants, but everyone's wearing diapers nowadays, anyway... So, who cares?");
                } else {
                    dm.sentences.Enqueue("Your friends have witnessed you wetting yourself on multiple occassions, though. It can't be helped since you've decided to not wear diapers.");
                    dm.sentences.Enqueue("Since it's happening a lot more now, you've started only wearing dark-colored pants in attempt to hide the evidence of your... wet adventures.");
                    dm.sentences.Enqueue("In fact, you've become so good at hiding your accidents that you get away with it most of the time now.");
                    dm.sentences.Enqueue("You've learned that if you deliberately let your pee out slowly it won't make any audible hissing sounds, so whenever you're indoors, you'll quietly let out a controlled pee.");
                    dm.sentences.Enqueue("Meanwhile, if you're going to be in a place with a lot of noise, you'll just let it all out while staying hidden or low-key.");
                }
                dm.sentences.Enqueue("Anyway, there's definitely going to be a horror night involved on the camping trip and you're feeling a little mischievous...");
                dm.sentences.Enqueue("Maybe you can do some research about the camping grounds before hand and see if there's any spooky horror stories about it. That way, it's going to be realistic and super scary.");
                dm.dialogueTypeTemp = "omogeDecision3";
                gameEventController(11);
            break;

            case 7:
                dm.sentences.Enqueue("...");
                dm.sentences.Enqueue("The game is now over.");
                if(isProtected) {
                    dm.sentences.Enqueue("Your diapers are filled nicely to the brim again. You drank a lot and peed a lot following the plot of the game. It was a nice experience.");
                    dm.sentences.Enqueue("You spread your legs and start playing with the gel-like feeling of trapped urine in your diapers.");
                    if(am.scnOmogeLendADiaper) {
                        dm.sentences.Enqueue("You: (That scene where the friend peed in the main character's diaper is pretty hot...)");
                        if(roomieWatch) {
                            dm.sentences.Enqueue("You glance at the roommate behind you, planning to entice him into doing the same.");
                            dm.sentences.Enqueue("SYSTEM: This scenario is on progress.");
                        }
                    }
                } else {
                    dm.sentences.Enqueue("Your pants are soaked from all the peeing you did following the plot of the game.");
                    dm.sentences.Enqueue("You also drank everytime you peed, so your bladder was constantly getting refilled, making you pee a lot more than usual.");
                    dm.sentences.Enqueue("You spread your legs and start playing with the aftermath.");
                    if(am.scnOmogeSecretPee) {
                        dm.sentences.Enqueue("You: (That scene where the main character peed himself in secret is pretty hot... Maybe I can try it someday.)");
                        dm.sentences.Enqueue("You: (I'll need some dark colored pants for that, though.)");
                        dm.sentences.Enqueue("SYSTEM: Later on, you'll be able to buy more clothes to wet in, but this function is not yet implemented.");
                    }
                }
                wetYourself();
                dm.dialogueTypeTemp = "";
                desperation = 0;
                leakState = 0;
                isSoaked = true;
                showCleanUp.SetActive(true);
                omoChooseDiaper = false;
            break;
        }
    }

    public void checkState() {
        pissObject.SetActive(true);
        if(wearBoxers == true && !isProtected) {
            if(isSoaked) {
                holdSprite.sprite = boxersprites[8];
            } else {
                holdSprite.sprite = boxersprites[leakState];
            }
        } else if(wearJeans == true && !isProtected) {
            if(isSoaked) {
                holdSprite.sprite = jeansprites[8];
            } else {
                holdSprite.sprite = jeansprites[leakState];
            }
        } else if(isProtected) {
            if(isSoaked) {
                holdSprite.sprite = diapersprites[8];
            } else {
                holdSprite.sprite = diapersprites[leakState];
            }
        }

        showState.SetActive(true);
        showPose.SetActive(true);
    }

    public void closeState() {
        pissObject.SetActive(false);
        showState.SetActive(false);
        showPose.SetActive(false);
    }

    public void holdIt() {
        if(isHolding) {
            handImage.sprite = handsprites[0];
            isHolding = false;
        } else {
            handImage.sprite = handsprites[1];
            isHolding = true;
        }
    }

    public void cleanUp() {
        if(!isPeeing) {
            if(isProtected && leakState < 3) {
                dm.StartDialogue("diapercleanup");
                isProtected = false;
                isSoaked = false;
                leakState = 0;
                showCleanUp.SetActive(false);
            } else if(wearBoxers && leakState < 3) {
                dm.StartDialogue("boxercleanup");
                isSoaked = false;
                leakState = 0;
                showCleanUp.SetActive(false);
            } else if(wearJeans && leakState < 3) {
                dm.StartDialogue("jeanscleanup");
                isSoaked = false;
                leakState = 0;
                showCleanUp.SetActive(false);
            } else if(leakState >= 3) {
                dm.sentences.Enqueue("You: (I feel like I'm going to have an accident soon. I'll clean up after that happens.)");
                dm.DisplayCurrSentence();
            }
        } else {
            dm.sentences.Enqueue("You: (I'm still peeing myself... Might as well wait until I'm done.)");
            dm.DisplayCurrSentence();
        }
    }

    public void checkItem() {
        updateItemStock();
        updateStatus();
        showItems.SetActive(true);
    }

    private void updateItemStock() {
        itemStockText[0].text = "x" + waterStock;
        itemStockText[1].text = "x" + coffeeStock;
        itemStockText[2].text = "x" + diaperStock;
    }

    private void omoRankChecker() {
        if(omoScenarioUnlocked <= 4) {
            omoRank = "OmoBaby";
        } else if(omoScenarioUnlocked >= 5 && omoScenarioUnlocked <= 9) {
            omoRank = "OmoStarter";
        } else if(omoScenarioUnlocked >= 10 && omoScenarioUnlocked <= 14) {
            omoRank = "OmoHobbyist";
        } else if(omoScenarioUnlocked >= 15 && omoScenarioUnlocked <= 19) {
            omoRank = "OmoPrince";
        } else if(omoScenarioUnlocked >= 20 && omoScenarioUnlocked <= 29) {
            omoRank = "OmoKing";
        } else if(omoScenarioUnlocked >= 30) {
            omoRank = "OmoLegend";
        }
    }

    private void updateStatus() {
        wetTimesText.text = wetTimes + " times";
        caughtTimesText.text = caughtTimes + " times";
        omoRankChecker();
        omoRankText.text = omoRank;
        moneyAmountText.text = "$ " + money;
    }

    public void closeItem() {
        showItems.SetActive(false);
    }

    public void openStore() {
        updateMoney();
        waterPriceText.text = "$" + pm.waterPrice;
        coffeePriceText.text = "$" + pm.coffeePrice;
        diaperPriceText.text = "$" + pm.diaperPrice;
        shooterPriceText.text = "$" + pm.shooterPrice;
        horrorPriceText.text = "$" + pm.horrorPrice;
        mattressProtectorPriceText.text = "$" + pm.mattressProtectorPrice;
        if(canBuyOmoge) {
            buyOmogeButton.interactable = true;
            omogePriceText.text = "$" + pm.omogePrice;
        } else {
            omogePriceText.text = "N/A";
        }
        showDesk.SetActive(false);
        showStore.SetActive(true);
    }

    public void closeStore() {
        showStore.SetActive(false);
    }

    public void showConfirmationUse(string itemName) {
        selectedItem = itemName;
        confirmText.text = "Use " + itemName + "?";
        showConfirmUse.SetActive(true); 
    }

    public void showConfirmationBuy(string itemNameBuy) {
        selectedItem = itemNameBuy;
        confirmTextBuy.text = "Buy " + itemNameBuy + "?";
        showConfirmBuy.SetActive(true); 
    }

    public void confirmUseItem() {
        showConfirmUse.SetActive(false);
        switch(selectedItem) {
            case "water":
                //audioSource.PlayOneShot(gameAudios[4]);
                if(waterStock >= 1) { 
                    if(isPeeing) {
                        dm.sentences.Enqueue("You decide to refill your bladder as it's being drained out.");
                        if(roomieWatch) {
                            dm.sentences.Enqueue("Roomie: Drinking more liquid while peeing yourself...");
                            dm.sentences.Enqueue("It looks as if you're immediately peeing out the water you just drank, and the idea turns him on.");
                        }
                        if(!am.scnDrinkWhilePee) {
                            //omoScenarioUnlocked++;
                            am.scnDrinkWhilePee = true;
                        }
                    }
                    audioSource.clip = gameAudios[4];
                    audioSource.Play();
                    dm.sentences.Enqueue("You drink the water.");
                    waterStock--;
                    desperation += 10;
                } else {
                    dm.sentences.Enqueue("You: I don't have any on me, maybe I should buy one.");
                }
                break;
            case "coffee":
                //audioSource.PlayOneShot(gameAudios[5]);
                if(coffeeStock >= 1) { 
                    if(isPeeing) {
                        dm.sentences.Enqueue("You decide to refill your bladder as it's being drained out.");
                        if(roomieWatch) {
                            dm.sentences.Enqueue("Roomie: Drinking more liquid while peeing yourself...");
                            dm.sentences.Enqueue("Coffee being a diuretic makes him think that you're trying to pee even more, and the idea turns him on.");
                        }
                        if(!am.scnDrinkWhilePee) {
                            //omoScenarioUnlocked++;
                            am.scnDrinkWhilePee = true;
                        }
                    }
                    audioSource.clip = gameAudios[5];
                    audioSource.Play();
                    dm.sentences.Enqueue("You drink the coffee.");
                    coffeeStock--;
                    desperation += 20;
                } else {
                    dm.sentences.Enqueue("You: I don't have any on me, maybe I should buy one.");
                }
                break;
            case "diaper":
                if(diaperStock >= 1) { 
                    if(!isProtected) {
                        if(isPeeing) {
                            dm.sentences.Enqueue("...");
                            dm.sentences.Enqueue("While pissing, you grab a diaper to pee in as to minimize the mess on the floor.");
                            dm.sentences.Enqueue("You don't feel the need to properly wear it, so you just sit yourself on top of it fully-clothed and hold up the sides.");
                            dm.sentences.Enqueue("The diaper quickly absorbs the pee that's coming straight out of you.");
                            if(roomieWatch) {
                                dm.sentences.Enqueue("Your roommate watches as you sit atop of your diapers, straddling it as you pee.");
                                dm.sentences.Enqueue("Roomie: Does that even do anything?");
                                dm.sentences.Enqueue("You: Well, it absorbs some of the pee...");
                                dm.sentences.Enqueue("Roomie: ...You're just doing this for fun, aren't you?");
                                dm.sentences.Enqueue("You: You're so smart.");
                                dm.sentences.Enqueue("He doesn't say more, but you can tell he thinks it's pretty hot, too.");
                            }
                            diaperStock--;
                            if(!am.scnDiaperWhilePee) {
                                //omoScenarioUnlocked++;
                                am.scnDiaperWhilePee = true;
                            }
                        } else {
                            if(!isSoaked) {
                                if(leakState > 0 && leakState <= 3) {
                                    dm.sentences.Enqueue("You: I think I should wear this before I start peeing myself even more...");
                                }
                                if(firstTimeUseDiaper) {
                                    //dm.StartDialogue("firstTimeUseDiaper");
                                    dm.sentences.Enqueue("You: (I can't believe I'm really doing this... A college student having to wear diapers.. Really?)");
                                    dm.sentences.Enqueue("You go to the bathroom and put on the diaper. You wear jeans over it to hide the fact.");
                                    dm.sentences.Enqueue("You: (Now I'm making crinkly plastic sounds whenever I walk... Let's just hope nobody notices...)");
                                    firstTimeUseDiaper = false;
                                } else {
                                    dm.sentences.Enqueue("You go to the bathroom and put on a diaper. You put jeans over it to hide it.");
                                }
                                wearJeans = true;
                                wearBoxers = false;
                                isProtected = true;
                                diaperStock--;
                                //audioSource.PlayOneShot(gameAudios[6]);
                                audioSource.clip = gameAudios[6];
                                audioSource.Play();
                            } else {
                                if(isProtected) {
                                    dm.sentences.Enqueue("You: (I should probably switch out of these diapers, but it's better to clean up everything too.)");
                                } else {
                                    dm.sentences.Enqueue("You: (Is there even a point? My pants are already wet and there's giant puddle on the floor... I don't think I need a diaper now.)");
                                }
                            }
                        }
                    } else {
                        dm.sentences.Enqueue("You: (I'm already wearing a diaper. I should just wear it until I pee everything out.)");
                    }
                } else {
                    dm.sentences.Enqueue("You: I don't have any on me, maybe I should buy one.");
                }
                break;
        }
        dm.DisplayCurrSentence();
        updateItemStock();
        updateStatus();
    }

    public void rejectUseItem() {
        selectedItem = "";
        showConfirmUse.SetActive(false);
    }

    public void rejectBuyItem() {
        selectedItem = "";
        showConfirmBuy.SetActive(false);
    }

    public void confirmBuyItem() {
        showConfirmBuy.SetActive(false);
        switch(selectedItem) {
            case "water":
                if(money >= pm.waterPrice) {
                    dm.sentences.Enqueue("You buy a pack of 5 bottled water.");
                    waterStock += 5;
                    money -= pm.waterPrice;
                    audioSource.PlayOneShot(gameAudios[13]);
                } else {
                    dm.sentences.Enqueue("You don't have enough money.");
                }
                break;
            case "coffee":
                if(money >= pm.coffeePrice) {
                    dm.sentences.Enqueue("You buy a pack of 5 canned coffee.");
                    coffeeStock += 5;
                    money -= pm.coffeePrice;
                    audioSource.PlayOneShot(gameAudios[13]);
                } else {
                    dm.sentences.Enqueue("You don't have enough money.");
                }
                break;
            case "diaper":
                if(money >= pm.diaperPrice) {
                    if(isPeeing) {
                        dm.sentences.Enqueue("With pee still rushing out from you noisily, you head to your computer to buy some diapers.");
                        if(roomieWatch) {
                            dm.sentences.Enqueue("Your roommate's gaze follows your every move, seemingly turned on by the way you're pissing all over the place.");
                            dm.sentences.Enqueue("Roomie: You're buying... diapers?");
                            dm.sentences.Enqueue("You: With the way I'm pissing myself right now, I should probably prepare some protection, yeah?");
                            dm.sentences.Enqueue("Roomie: (Implying he's going to pee himself even more in the future...)");
                            dm.sentences.Enqueue("Your roommate rubs himself more.");
                        }
                        if(!am.scnBuyDiaperWhilePee) {
                            //omoScenarioUnlocked++;
                            am.scnBuyDiaperWhilePee = true;
                        }
                    }
                    if(firstTimeBuyDiaper) {
                        if(!isPeeing) {
                            dm.StartDialogue("firstTimeBuyDiaper");
                        } else {
                            dm.sentences.Enqueue("You: (..Hate to admit it, but I might actually need this.. based on my current experience...)");
                            dm.sentences.Enqueue("You wonder if these diapers would be able to absorb all of your pee, as you're peeing quite a lot right now.");
                        }
                        firstTimeBuyDiaper = false;
                    } else {
                        dm.sentences.Enqueue("You buy a pack of adult diapers. It contains 8 diapers.");
                    }
                    diaperStock += 8;
                    money -= pm.diaperPrice;
                    audioSource.PlayOneShot(gameAudios[13]);
                } else {
                    dm.sentences.Enqueue("You don't have enough money.");
                }
                break;
            case "multiplayer shooter game":
                if(!boughtShooter) {
                    if(money >= pm.shooterPrice) {
                        dm.sentences.Enqueue("You: (Multiplayer shooter game huh... Looks interesting.)");
                        money -= pm.shooterPrice;
                        boughtShooter = true;
                        audioSource.PlayOneShot(gameAudios[13]);
                    } else {
                        dm.sentences.Enqueue("You don't have enough money.");
                    }
                } else {
                    dm.sentences.Enqueue("You: (I already have these)");
                }
                break;
            case "horror game":
                if(!boughtHorror) {
                    if(money >= pm.horrorPrice) {
                        dm.sentences.Enqueue("You: (It's a highly-rated horror game... Looks interesting.)");
                        money -= pm.horrorPrice;
                        boughtHorror = true;
                        audioSource.PlayOneShot(gameAudios[13]);
                    } else {
                        dm.sentences.Enqueue("You don't have enough money.");
                    }
                } else {
                    dm.sentences.Enqueue("You: (I already have these)");
                }
                break;
            case "omo game":
                if(!boughtOmoge) {
                    if(money >= pm.omogePrice) {
                        dm.sentences.Enqueue("After discovering this fetish of yours, you actively look for games about wetting and found one that you like.");
                        dm.sentences.Enqueue("This game looks interesting... And it's targeted to people who like it.");
                        money -= pm.omogePrice;
                        boughtOmoge = true;
                        audioSource.PlayOneShot(gameAudios[13]);
                    } else {
                        dm.sentences.Enqueue("You don't have enough money.");
                    }
                } else {
                    dm.sentences.Enqueue("You: (I already have these)");
                }
                break;
            case "mattress protector":
                if(!bedProtected) {
                    if(money >= pm.mattressProtectorPrice) {
                        if(wetTimes >= 3) {
                            dm.sentences.Enqueue("You buy the mattress protector and use it on your bed.");
                            dm.sentences.Enqueue("You: (Now I can just wet the bed whenever I feel like it.)");
                            bedProtected = true;
                            hasBedProtector = true;
                            bedProtectText.text = "Protected";
                            bedProtectionOpt.SetActive(true);
                            money -= pm.mattressProtectorPrice;
                            audioSource.PlayOneShot(gameAudios[13]);
                        } else {
                            dm.sentences.Enqueue("You: (...I don't think I need this.)");
                        }
                    } else {
                        dm.sentences.Enqueue("You don't have enough money.");
                    }
                } else {
                    dm.sentences.Enqueue("You: (I already have these)");
                }
                break;
        }
        dm.DisplayCurrSentence();
        updateMoney();
    }

    private void updateMoney() {
        buyMoneyAmountText.text = "$ " + money;
    }

    public void chooseToLetGo() {
        if(!isPeeing) {
            letGo = true;
            leakState++;
            leakPee();
            if(!am.scnFreePeeing) {
                //omoScenarioUnlocked++;
                am.scnFreePeeing = true;
            }
        }
    }

    private void unlockRoomieWatch() {
        dm.sentences.Enqueue("Your roommate shivers. It seems he's done too. He blushes hard afterwards.");
        dm.sentences.Enqueue("Roomie: Fuck, sorry.. I couldn't help it. That was too hot... Something might be wrong with me.");
        dm.sentences.Enqueue("You: You're not alone. I also think it is...");
        dm.sentences.Enqueue("Roomie: Yeah... You really awakened something within me. I never would've thought someone pissing themselves would be hot.");
        dm.sentences.Enqueue("You: You should try pissing yourself. I think you'll like it.");
        dm.sentences.Enqueue("Roomie: For real? Wait... were you just pissing yourself for fun?");
        dm.sentences.Enqueue("You: Not really. Well, it started off as a genuine accident... But I started enjoying it too much.");
        dm.sentences.Enqueue("Roomie: Haha... I guess mine also started by accidentally walking in on you pissing yourself.");
        dm.sentences.Enqueue("You: Yeah, you were so obvious, though.");
        dm.sentences.Enqueue("Roomie: Huh, was I?");
        dm.sentences.Enqueue("You: You couldn't keep your eyes off my pants. You were so into it.");
        dm.sentences.Enqueue("Roomie: Damnit, and here I was hoping you didn't notice.");
        dm.sentences.Enqueue("You: That's one of the reasons why I wasn't really rushing to clean up.");
        dm.sentences.Enqueue("Roomie: So you can catch me staring at you again when I return?");
        dm.sentences.Enqueue("You: Haha, maybe.");
        if(roomieBanned == false) {
            dm.sentences.Enqueue("Roomie: Well, anyways, I'm gonna stay here now. Just let me know if you need help cleaning up.");
            dm.sentences.Enqueue("You: I think I'll be fine. Thanks, though.");
            dm.sentences.Enqueue("Your roommate will now stay permanently in the room.");
            roomieWatch = true;
            isWatched = false;
            if(!am.scnRoomieIntoOmo) {
                //omoScenarioUnlocked++;
                am.scnRoomieIntoOmo = true;
            }
        } else {
            dm.sentences.Enqueue("SYSTEM: Roomie Watch mode is banned. Your roommate will not stay in the room.");
            isWatched = false;
            
        }
        //END OF ROOMIE DISCOVERY PHASE
    }
    
    public void leakPee() {
        checkState();
        //if(!isProtected) {
        if(letGo) {
            StartCoroutine(floodPants());
        } else {
            StartCoroutine(leakPants());
            if(isWatched && leakState == 3) {
                dm.sentences.Enqueue("You wanted to just let everything go, but with how your roommate is watching you right now, you got nervous and stopped your pee.");
                dm.sentences.Enqueue("You try to relax your muscles but it's not doing much. I guess you need more liquid in you.");
                dm.DisplayCurrSentence();
            }
        }
    }

    IEnumerator leakPants() {
        print("leak pants");
        var emission = stream.emission;
        var main = stream.main;
        if(!isProtected) {
            //Vector3 normalPissPosition = new Vector3(0.22f, 4.232f, 0f);
            Vector3 normalPissPosition = new Vector3(0f, 4.3f, 0f);
            pissObject.transform.position = normalPissPosition;
            Vector3 normalPissScale = new Vector3(1f,1f,1f);
            pissObject.transform.localScale = normalPissScale;
            main.gravityModifier = 1.5f;
        }
            switch(leakState) {
                case 1:
                if(!isProtected) { 
                main.duration = 2;
                emission.rateOverTime = 2.0f;
                }
                //audioSource.PlayOneShot(gameAudios[0]);
                audioSource.clip = gameAudios[0];
                audioSource.Play();
                yield return new WaitForSeconds(3.0f);
                emission.rateOverTime = 0f;
                break;

                case 2:
                //audioSource.PlayOneShot(gameAudios[1]);
                audioSource.clip = gameAudios[1];
                audioSource.Play();
                if(!isProtected) {
                    main.duration = 5;
                    emission.rateOverTime = 250.0f;
                }
                yield return new WaitForSeconds(2.0f);
                emission.rateOverTime = 0f;
                break;

                case 3:
                //audioSource.PlayOneShot(gameAudios[2]);
                audioSource.clip = gameAudios[2];
                audioSource.Play();
                if(!isProtected) {
                    main.duration = 5;
                    emission.rateOverTime = 20.0f;
                }
                yield return new WaitForSeconds(3.0f);
                emission.rateOverTime = 0f;
                break;

                case 4:
                if(!isProtected) {
                    main.duration = 40;
                    emission.rateOverTime = 1000.0f;
                }
                //allows more audio to be played over it
                audioSource.PlayOneShot(gameAudios[3]);
                //audioSource.clip = gameAudios[3];
                //audioSource.Play();
                break;
            }
            if(isSoaked) {
                if(wearBoxers) {
                    holdSprite.sprite = boxersprites[8];
                } else if(wearJeans && !isProtected) {
                    holdSprite.sprite = jeansprites[8];
                } else if(isProtected) {
                    holdSprite.sprite = diapersprites[8];
                }
            } else {
                if(wearBoxers) {
                    holdSprite.sprite = boxersprites[leakState];
                } else if (wearJeans && !isProtected) {
                    holdSprite.sprite = jeansprites[leakState];
                } else if(isProtected) {
                    holdSprite.sprite = diapersprites[leakState];
                }
            }
    }

    IEnumerator floodPants() {
        print("flood pants");
        isPeeing = true;
        var emission = stream.emission;
        var main = stream.main;
        if(!isProtected) {
            //Vector3 normalPissPosition = new Vector3(0.22f, 4.232f, 0f);
            Vector3 normalPissPosition = new Vector3(0f, 4.3f, 0f);
            pissObject.transform.position = normalPissPosition;
            Vector3 normalPissScale = new Vector3(0.6f,1f,1f);
            pissObject.transform.localScale = normalPissScale;
            main.gravityModifier = 1.5f;
        }
        for(int i=leakState; i<=7; i++) {
            switch(leakState) {
                case 1:
                if(!isProtected) {
                    main.duration = 2;
                    emission.rateOverTime = 2.0f;
                }
                //audioSource.PlayOneShot(gameAudios[0]);
                audioSource.clip = gameAudios[0];
                audioSource.Play();
                if(!isRoommate) {
                    roommateCheck();
                } else if(!isWatched && !roomieWatch) {
                    dm.sentences.Enqueue("Your roommate walks in and starts rummaging through his desk.");
                    dm.sentences.Enqueue("You pee a little.");
                    dm.DisplayCurrSentence();
                }
                yield return new WaitForSeconds(3.0f);
                break;

                case 2:
                //audioSource.PlayOneShot(gameAudios[1]);
                audioSource.clip = gameAudios[1];
                audioSource.Play();
                if(!isProtected) {
                    main.duration = 5;
                    emission.rateOverTime = 5.0f;
                }
                if(!isRoommate) {
                    roommateCheck();
                } else if(!isWatched && !roomieWatch) {
                    dm.sentences.Enqueue("Your roommate seems oblivious about what you're currently doing.");
                    dm.sentences.Enqueue("You pee a bit more.");
                    dm.DisplayCurrSentence();
                }
                yield return new WaitForSeconds(2.0f);
                break;

                case 3:
                //audioSource.PlayOneShot(gameAudios[2]);
                audioSource.clip = gameAudios[2];
                audioSource.Play();
                if(!isProtected) {
                    main.duration = 5;
                    emission.rateOverTime = 250.0f;
                }
                if(!isRoommate) {
                    roommateCheck();
                } 
                if(isRoommate && !isWatched && !roomieWatch) {
                    if(!isProtected) {
                        dm.sentences.Enqueue("Your roommate seems to have heard the hiss. He looks at you.");
                        dm.sentences.Enqueue("Roomie: Huh? Did you just...");
                    } else {
                        dm.sentences.Enqueue("Your diaper is surpressing the sound of you hissing. Your roommate doesn't notice.");
                    }
                    dm.DisplayCurrSentence();
                } else if(roomieWatch) {
                    if(!isProtected) {
                        dm.sentences.Enqueue("Your roommate seems to have heard the hiss. He looks at you.");
                        dm.sentences.Enqueue("He focuses at the wet spot on your crotch, waiting for it to leak even more.");
                    } else {
                        dm.sentences.Enqueue("Your diaper is surpressing the sound of you hissing. Your roommate doesn't notice.");
                    }
                    dm.DisplayCurrSentence();
                }
                break;

                case 4:
                if(!isProtected) {
                    main.duration = 40;
                    emission.rateOverTime = 1000.0f;
                }
                audioSource.PlayOneShot(gameAudios[3]);
                if(!isRoommate) {
                    roommateCheck();
                }
                //audioSource.clip = gameAudios[3];
                //audioSource.Play();

                if(isProtected) {
                    dm.sentences.Enqueue("You start peeing freely into your diaper. There's no sound, but you can feel your diaper expanding.");
                    dm.DisplayCurrSentence();
                } else {
                    if(isRoommate && !isWatched && !roomieWatch) {
                        dm.sentences.Enqueue("Your roommate is right there, staring at you pissing yourself with full force.");
                        dm.sentences.Enqueue("You can feel yourself blushing.");
                        dm.DisplayCurrSentence();
                    } else if(isWatched) {
                        dm.sentences.Enqueue("Hearing the noise, your roommate immediately shifts his focus to the stream coming out from you.");
                        if(wearBoxers) {
                            dm.sentences.Enqueue("Your boxers are soaked and can't absorb much more pee, therefore it somewhat looks like you're peeing directly to the floor.");
                        } else if(wearJeans) {
                            dm.sentences.Enqueue("Your jeans are soaked and can't absorb much more pee, therefore the overflowing liquid escapes from every possible seam.");
                        }
                        dm.sentences.Enqueue("You can see that your roommate is blushing as he's relishing in the sight. He seems to be really enjoying this.");
                        dm.DisplayCurrSentence();
                    } else if(roomieWatch && !roommateFirstWitnessDiaper) {
                        dm.sentences.Enqueue("You relax your muscles, letting yourself go. Your pee flows out of you, wetting your pants with warm heat and making them cling to your skin.");
                        dm.sentences.Enqueue("Your roommate is watching closely, his bulge growing.");
                        dm.DisplayCurrSentence();
                    }
                }
                break;

                case 6:
                if(isProtected) {
                    dm.sentences.Enqueue("You: (I'm peeing so much... Hope it doesn't leak...)");
                    if(isRoommate) {
                        if(roommateFirstWitnessDiaper) {
                            dm.sentences.Enqueue("Your roommate seems to have noticed how your jeans are bulging due to the diaper inside.");
                            dm.sentences.Enqueue("Roomie: Huh? Are you...");
                        } else {
                            dm.sentences.Enqueue("Roomie: You're peeing into your diaper again?");
                            dm.sentences.Enqueue("You: ...You caught me.");
                            dm.sentences.Enqueue("He doesn't say more and keeps staring at it.");
                        }
                    }
                    /*if(roomieWatch) {
                        dm.sentences.Enqueue("Roomie: How does it feel?");
                        dm.sentences.Enqueue("You: It's really warm. Feels like I'm submerged in a sea of my own freshly produced piss. I can also clearly feel the way my pee is hitting the diaper below me. It feels really good...");
                        dm.sentences.Enqueue("Roomie: ...No need to make it sound so hot.");
                        dm.sentences.Enqueue("He rubs himself more.");
                    }*/
                } else {
                    dm.sentences.Enqueue("You: Ah... I'm peeing so much...");
                    if(isRoommate && !isWatched && !roomieWatch) {
                        dm.sentences.Enqueue("Your roommate seems to be just watching you.");
                    } else if(isWatched) {
                        dm.sentences.Enqueue("You can see that your roommate is starting to get excited below the belt. You don't blame him, you like it too.");
                    } else if(roomieWatch) {
                        dm.sentences.Enqueue("Roomie: Enjoying yourself?");
                        dm.sentences.Enqueue("You: Mmm.. Yeah... It feels so good. I just keep peeing...");
                        dm.sentences.Enqueue("He strokes himself even faster. My words seem to be turning him on even more.");
                    }
                }
                dm.DisplayCurrSentence();
                break;
            }
            print(i);
            leakState++;
            if(!isSoaked) {
                if(wearBoxers) {
                    holdSprite.sprite = boxersprites[leakState];
                } else if(wearJeans && !isProtected) {
                    holdSprite.sprite = jeansprites[leakState];
                } else if(wearJeans && isProtected) {
                    holdSprite.sprite = diapersprites[leakState];
                }
            }
            if(leakState >= 4) {
                yield return new WaitForSeconds(3.0f);
            }
        }
        yield return new WaitForSeconds(10.0f);
        if(isProtected) {
            dm.sentences.Enqueue("The diaper continues expanding and my bum starts feeling wet.");
            if(isRoommate) {
                if(roommateFirstWitnessDiaper) {
                    dm.sentences.Enqueue("Roomie: Dude... Are you wearing diapers?");
                    dm.sentences.Enqueue("I can feel myself blushing from the embarassment.");
                    dm.sentences.Enqueue("You: Yeah... I mean, it's better than going on the floor... The toilet's broken...");
                    if(!roomieWatch) {
                        dm.sentences.Enqueue("He doesn't say anything afterwards and just keeps watching it expand.");
                    } else {
                        dm.sentences.Enqueue("Roomie: I don't mind you going on the floor, but I guess I'm okay with this too...");
                    }
                    roommateFirstWitnessDiaper = false;
                    if(!am.scnCaughtWearingDiapers) {
                        //omoScenarioUnlocked++;
                        am.scnCaughtWearingDiapers = true;
                    }
                } else {
                    dm.sentences.Enqueue("Roomie: Peeing your diapers again?");
                    dm.sentences.Enqueue("You: Y-Yeah.");
                    if(roomieWatch) {
                        dm.sentences.Enqueue("Roomie: You know... I already know you're wearing diapers so what is there to hide?");
                        dm.sentences.Enqueue("You: Just say that you want to see me peeing my diapers.");
                        dm.sentences.Enqueue("Roomie: Am I that easy to figure out?");
                        dm.sentences.Enqueue("You: With you drooling all over the place, there's not even a mystery to begin with.");
                        dm.sentences.Enqueue("Roomie: Whatever, dude. So... You gonna take off your jeans or what?");
                        dm.sentences.Enqueue("You: Wait, it's kinda embarassing so I'll do it the next time I put one on...");
                        dm.sentences.Enqueue("Roomie: Alright, make sure to refill your bladders well.");
                        dm.sentences.Enqueue("You: You're so fucking horny.");
                        dm.sentences.Enqueue("Roomie: I won't even refute that...");
                        dm.sentences.Enqueue("You: ...");
                        dm.sentences.Enqueue("SYSTEM: Sorry, this feature is not yet available and will be updated later in the future.");
                        //You've unlocked the ability to just wear your diapers.
                    }
                    dm.sentences.Enqueue("He doesn't say anything afterwards and just keeps watching it expand.");
                }
            }
        } else {
            dm.sentences.Enqueue("You: It's still coming out..");
            if(isRoommate && !isWatched && !roomieWatch) {
                if(caughtTimes == 0) {
                    dm.sentences.Enqueue("Roomie: ...Damn, you're really still going...");
                } else {
                    dm.sentences.Enqueue("Roomie: You always pee so much everytime...");
                }
                if(wearBoxers) {
                    dm.sentences.Enqueue("He seems to be entranced by the sight of you peeing yourself. His eyes are glued to your lower body and the stream coming out of it.");
                } else if(wearJeans && !isProtected) {
                    dm.sentences.Enqueue("He seems to be entranced by the sight of you peeing yourself. His eyes are glued to your lower body, watching as you continue soaking your jeans.");
                }
            } else if(isWatched) {
                dm.sentences.Enqueue("Your words only further excite him. He grabbed his bulging crotch and starts rubbing it slowly through his pants.");
            } else if(roomieWatch) {
                dm.sentences.Enqueue("I start groaning a bit as I wet, fully immersing myself in the pleasure. My breath also getting heavier. My hands also start playing around with the pee I'm letting out.");
                dm.sentences.Enqueue("Roomie: ...");
                dm.sentences.Enqueue("I can hear my roommate beating himself even faster now.");
            }
        }
        dm.DisplayCurrSentence();
        yield return new WaitForSeconds(15.0f);
        if(isProtected) {
            dm.sentences.Enqueue("After what feels like an eternity of peeing, I peek through the bands of my diaper. The diaper couldn't absorb everything resulting in a small lake of pee inside. I should probably change.");
            if(isRoommate) {
                dm.sentences.Enqueue("Roomie: You're done..?");
                if(!roomieWatch) {
                    dm.sentences.Enqueue("The question caught me off guard. Maybe he can tell from the way I start checking inside.");
                    dm.sentences.Enqueue("You: Uh... Yeah.");
                    dm.sentences.Enqueue("Roomie: You peed for so long... Did the diaper absorb all of that?");
                    dm.sentences.Enqueue("You: No.. I peed too much, I think... Some of it is just there.");
                    dm.sentences.Enqueue("Roomie: Yeah, that's not really surprising...");
                    dm.sentences.Enqueue("I start walking towards the bathroom, causing the pee to slosh around noisily along the way. He keeps watching as I do so.");
                    dm.sentences.Enqueue("You: I'm gonna clean up.");
                    dm.sentences.Enqueue("He seems to snap out of it after I say that, remembering what he returned to the room for. He grabs some things from his drawer and walks out.");
                    dm.sentences.Enqueue("Roomie: Okay, I'm gonna head out again.");
                    dm.sentences.Enqueue("After he leaves, I am left alone with my soggy diapers.");
                    dm.sentences.Enqueue("You: (Can't believe I just did that... That was embarassing...)");
                    isRoommate = false;
                    //showRoommate.SetActive(false);
                    //this is remarked to let dialogue finish before roommate leaves
                    caughtTimes++;
                } else {
                    dm.sentences.Enqueue("You: Mm, yeah...");
                    dm.sentences.Enqueue("Roomie: Diaper reveal?");
                    dm.sentences.Enqueue("You: Fine.");
                    dm.sentences.Enqueue("I take off my jeans, revealing a heavy, swollen diaper inside. I grab and rub at it to feel the heat of all the pee stored inside.");
                    dm.sentences.Enqueue("My roommate seems to inspect every inch of the diaper carefully.");
                    dm.sentences.Enqueue("Roomie: Can I come over and touch?");
                    dm.sentences.Enqueue("You: Go ahead.");
                    dm.sentences.Enqueue("He stands up from his seat. The bulge of his pants tells me he's still erect. He walks over to me while still slowly rubbing himself.");
                    dm.sentences.Enqueue("Roomie: ...So this is the smell of your pee.");
                    dm.sentences.Enqueue("His comment makes me slightly embarassed.");
                    dm.sentences.Enqueue("You: Shut up.");
                    dm.sentences.Enqueue("He snickers at my reaction and starts rubbing his hands on the surface of my diaper, feeling every part of it.");
                    dm.sentences.Enqueue("Roomie: It gets hotter the lower I touch...");
                    dm.sentences.Enqueue("You: That's where most of the pee is. Also, uh, there's still some pee below left unabsorbed...");
                    dm.sentences.Enqueue("Hearing that, he continues touching and feeling my diaper, sometimes pressing on it to make the residual pee inside slosh.");
                    dm.sentences.Enqueue("While doing so, he keeps satisfying himself down there as well. A while later, he finally finishes.");
                    dm.sentences.Enqueue("Roomie: Thanks for the meal...");
                    if(!am.scnRoomieIntoDiaper) {
                        //omoScenarioUnlocked++;
                        am.scnRoomieIntoDiaper = true;
                    }
                }
            }
        } else {
            dm.sentences.Enqueue("You: I'm finally done... Sigh...");
            if(isRoommate && !isWatched && !roomieWatch) {
                if(caughtTimes == 0) {
                    if(wearJeans) {
                        dm.sentences.Enqueue("Roomie: Damn... Your jeans are drenched, man. How were you able to pee that much?");
                    } else if(wearBoxers) {
                        dm.sentences.Enqueue("Roomie: That's crazy... How much did you drink, dude..?");
                    }
                    dm.sentences.Enqueue("You: I don't know what to tell you... I never expected to pee so much...");
                    dm.sentences.Enqueue("Roomie: ...");
                } else {
                    dm.sentences.Enqueue("Roomie: ...Hmm...");
                }
                dm.sentences.Enqueue("You: Sorry, I uh.. I'll clean this up real quick.");
                dm.sentences.Enqueue("Roomie: ...Don't worry about it. It happens.");
                dm.sentences.Enqueue("You notice that he's keeping his eyes fixed at the puddle under you.");
                dm.sentences.Enqueue("You: ...");
                dm.sentences.Enqueue("You wait for a bit and he's still staring. Could it be that he's into this..?");
                dm.sentences.Enqueue("He finally notices you staring at him. He blushes and quickly grabs something from his drawer and heads for the door.");
                dm.sentences.Enqueue("Roomie: A-Anyways, I'm leaving again. Good luck with the cleaning.");
                if(wearBoxers) {
                    dm.sentences.Enqueue("You are left alone with your puddle of piss and soaked boxers.");
                } else if(wearJeans) {
                    dm.sentences.Enqueue("You are left alone with your puddle of piss and soaked jeans.");
                }
                dm.sentences.Enqueue("You: (Can't believe he caught me peeing myself... This is too embarassing...)");
                isRoommate = false;
                //showRoommate.SetActive(false);
                caughtTimes++;
            } else if (isWatched) {
                unlockRoomieWatch();
            } else if(roomieWatch) {
                dm.sentences.Enqueue("You look down to check the damage. Your pants are pretty much soaked and there's a huge puddle under you.");
                dm.sentences.Enqueue("You slowly start touching your pants, rubbing it to feel the warmth on your hands, squeezing it to wring out the pee it has absorbed and washing your hands with it.");
                dm.sentences.Enqueue("Seeing that your roommate is still watching you silently while pleasuring himself, you decide to just do the same as you're very much turned on right now.");
                dm.sentences.Enqueue("You start rubbing your groin, sometimes squeezing it to warm your hand with more pee. After you get excited enough, you shove one hand inside to stroke it directly.");
                dm.sentences.Enqueue("After playing around for a while, you finally finish. Looking at your roommate, it seems he did, too.");
                dm.sentences.Enqueue("Roomie: You looked like you were enjoying yourself so much there... Playing around with your piss like that.");
                dm.sentences.Enqueue("You: I really did... and I know you did, too.");
                dm.sentences.Enqueue("Roomie: Mm, thanks for the show.");
                if(!am.scnPlayWithPee) {
                    //omoScenarioUnlocked++;
                    am.scnPlayWithPee = true;
                }
            }
        }
        dm.DisplayCurrSentence();
        isPeeing = false;
        showCleanUp.SetActive(true);
        desperation = 0;
        leakState = 0;
        isSoaked = true;
        emission.rateOverTime = 0f;
        letGo = false;
        wetYourself();
    }


}
