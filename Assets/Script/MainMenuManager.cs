using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class MainMenuManager : MonoBehaviour
{
    public GameObject creditsScreen, confirmNewGame, confirmNewGameOptions, continueButton;
    private SceneLoader sl;
    private string saveFilePath;
    private PlayerData playerData;
    private FileDataHandler dataHandler;

    void Start() {
        sl = FindObjectOfType<SceneLoader>();
        saveFilePath = Path.Combine(Application.persistentDataPath, "data.omo");
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, "data.omo");
        if(File.Exists(saveFilePath)) {
            continueButton.SetActive(true);
        } else {
            continueButton.SetActive(false);
        }
    }

    public void showhideCredits() {
        if(creditsScreen.activeSelf) {
            creditsScreen.SetActive(false);
        } else {
            creditsScreen.SetActive(true);
        }
    }

    public void newGameConfirm() {
        if(File.Exists(saveFilePath)) {
            if(confirmNewGame.activeSelf) {
                confirmNewGame.SetActive(false);
            } else {
                confirmNewGame.SetActive(true);
            }
        } else {
            //No Saved Game Yet
            sl.startGame();
        }
    }

    public void newGameOptions() {
        confirmNewGame.SetActive(false);
        if(confirmNewGameOptions.activeSelf) {
            confirmNewGameOptions.SetActive(false);
        } else {
            confirmNewGameOptions.SetActive(true);
        }
    }

    public void confirmFullReset() {
        fullReset();
        sl.startGame();
    }
 
    private void fullReset()
    {
        try
        {
            File.Delete(saveFilePath);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public void achievementKeep() {
        //load player data
        this.playerData = dataHandler.Load();

        //delete file
        fullReset();

        //init empty with achievements
        initReset();

        //re-save
        dataHandler.Save(this.playerData);

        //load game
        sl.startGame();
    }

    private void initReset() {
        this.playerData.desperation = 0;
        this.playerData.leakState = 0;
        this.playerData.money = 0;
        this.playerData.isProtected = false;
        this.playerData.bedProtected = false;
        this.playerData.hasBedProtector = false;
        this.playerData.wearBoxers = false;
        this.playerData.wearJeans = true;
        this.playerData.isSoaked = false;
        this.playerData.isWatched = false;
        this.playerData.roomieWatch = false;
        this.playerData.firstTimeBuyDiaper = true;
        this.playerData.firstTimeUseDiaper = true;
        this.playerData.roommateFirstWitnessDiaper = true;
        this.playerData.wetTimes = 0;
        this.playerData.caughtTimes = 0;
        this.playerData.omoScenarioUnlocked = 0;
        this.playerData.waterStock = 0;
        this.playerData.coffeeStock = 0;
        this.playerData.diaperStock = 0;
        this.playerData.boughtShooter = false;
        this.playerData.boughtHorror = false;
        this.playerData.boughtOmoge = false;
        this.playerData.canBuyOmoge = false;
        this.playerData.introDone = false;
    }

}
