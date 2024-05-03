using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Dialogue dialogue;
    private GameManager gm;
    public Text dialogueText, topDialogueText;
    public string dialogueTypeTemp;
    public bool wetCountCheck, isTalking, stopPeeAudio, gameWettingBool, topDialogue;
    public GameObject dialogueBox, topDialogueBox, showContinueWork, showWorkWet, showGameOptions;
    public Queue<string> sentences;
    private float timestamp, timeBetweenDialogues;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        dialogue = FindObjectOfType<Dialogue>();
        gm = FindObjectOfType<GameManager>();
        timeBetweenDialogues = 0.3333f;
    }

    void Update() {
         if(Time.time >= timestamp && Input.GetKey("space")) {
            DisplayNextSentence();
            timestamp = Time.time + timeBetweenDialogues;
         }
    }

    public void StartDialogue (string dialogueType) {
        //sentences.Clear();
        dialogueTypeTemp = dialogueType;
        switch(dialogueType) {
            case "intro":
                foreach (string sentence in dialogue.introSentences) {
                    sentences.Enqueue(sentence);
                }
                break;
            case "boxercleanup":
                foreach (string sentence in dialogue.boxercleanUpSentences) {
                    sentences.Enqueue(sentence);
                }
                break;
            case "jeanscleanup":
                foreach (string sentence in dialogue.jeanscleanUpSentences) {
                    sentences.Enqueue(sentence);
                }
                break;
            case "diapercleanup":
                foreach (string sentence in dialogue.diapercleanUpSentences) {
                    sentences.Enqueue(sentence);
                }
                break;
            case "workEvent0":
                foreach (string sentence in dialogue.workEvent0Sentences) {
                    sentences.Enqueue(sentence);
                }
                break;
            case "workEvent1":
                foreach (string sentence in dialogue.workEvent1Sentences) {
                    sentences.Enqueue(sentence);
                }
                break;
            case "workEvent2":
                foreach (string sentence in dialogue.workEvent2Sentences) {
                    sentences.Enqueue(sentence);
                }
                break;
            case "workEventPee":
                foreach (string sentence in dialogue.workEventPeeSentences) {
                    sentences.Enqueue(sentence);
                }
                break;
            case "workEventPee2":
                foreach (string sentence in dialogue.workEventPee2Sentences) {
                    sentences.Enqueue(sentence);
                }
                break;
            case "workEventPee3":
                foreach (string sentence in dialogue.workEventPee3Sentences) {
                    sentences.Enqueue(sentence);
                }
                break;
            case "workEventPee4":
                foreach (string sentence in dialogue.workEventPee4Sentences) {
                    sentences.Enqueue(sentence);
                }
                break;
            case "workEventPee5":
                foreach (string sentence in dialogue.workEventPee5Sentences) {
                    sentences.Enqueue(sentence);
                    wetCountCheck = true;
                }
                break;
            case "workEventDiaper2":
                foreach (string sentence in dialogue.workEventDiaper2Sentences) {
                    sentences.Enqueue(sentence);
                }
                break;
            case "workEventDiaper3":
                foreach (string sentence in dialogue.workEventDiaper3Sentences) {
                    sentences.Enqueue(sentence);
                }
                break;
            case "workEventDiaper4":
                foreach (string sentence in dialogue.workEventDiaper4Sentences) {
                    sentences.Enqueue(sentence);
                }
                break;
            case "workEventDiaper5":
                foreach (string sentence in dialogue.workEventDiaper5Sentences) {
                    sentences.Enqueue(sentence);
                    wetCountCheck = true;
                }
                break;
            case "firstTimeUseDiaper":
                foreach (string sentence in dialogue.firstUseDiaperSentences) {
                    sentences.Enqueue(sentence);
                }
                break;
            case "firstTimeBuyDiaper":
                foreach (string sentence in dialogue.firstBuyDiaperSentences) {
                    sentences.Enqueue(sentence);
                }
                break;
        }

        //if(!wetCountCheck) {
            //DisplayNextSentence();
        DisplayCurrSentence();
        //}
    }

    public void DisplayCurrSentence() {
        //prevent this from showing more dialogue before the previous dialogue ends...
        //if(!isTalking) {
        if(!dialogueBox.activeSelf) {
            string sentence = sentences.Dequeue();
            //string sentence = sentences.Peek();
            showDialogue(sentence);
            print(sentence);
            checkTriggers(sentence);
        }
    }

    private void checkTriggers(string sentence) {
        if(sentence == "After you reach the credits, you finally turn off the game." ||
            sentence == "You both clean up the evidence of your accidents and sleep." ||
            sentence == "Both of you talk for a bit more before going back to your tents to try and sleep a bit more before sunrise.") {
            gm.bgmAudioSource.clip = gm.gameBGM[0];
            gm.bgmAudioSource.Play();
        }

        if(sentence == "Time flies and night comes. Everyone is sitting around the campfire, grilling marshmallows. It is time for horror night.") {
            gm.bgmAudioSource.clip = gm.gameBGM[1];
            gm.bgmAudioSource.Play();
        }

        if(sentence == "It doesn't take long for him to start peeing. The familiar wet and warm sensation rapidly grows under me.") {
             gm.audioSource.PlayOneShot(gm.gameAudios[15]);
        }

        if(sentence == "You feel yourself leaking slightly.") {
            gm.audioSource.PlayOneShot(gm.gameAudios[0]);
        }

        if(sentence == "You jump and a warm sensation fills your crotch.") {
            if(gm.isHorrorMax) {
                gm.audioSource.clip = gm.gameAudios[3];
                gm.audioSource.Play();
            } else {
                gm.audioSource.PlayOneShot(gm.gameAudios[1]);
            }
        }

        if(sentence == "You let out a short leak, enough to warm your groin.") {
            gm.audioSource.PlayOneShot(gm.gameAudios[1]);
        }

        if(sentence == "You start leaking." ||
            sentence == "You feel yourself leaking with fear." ||
            sentence == "You feel a pretty big leak but you still manage to hold it in while attempting to run away from the girl." ||
            sentence == "You pee a little, too." ||
            sentence == "You also release some into yours." ||
            sentence == "As if triggering a chain reaction, you leak a good amount before stopping yourself." ||
            sentence == "Putting yourself in his shoes, you relax for a while, enlarging the wet spot on your pants even more.") {
            gm.audioSource.PlayOneShot(gm.gameAudios[2]);
        }

        if(sentence == "Upon opening the door, you feel relief wash over you and making your bladder muscles unconsciously relax." ||
            sentence == "Once it gets to you, your whole screen is filled with a zoom in of the creature's disfigured face saying 'Come in'..." ||
            sentence == "However, after a while of walking, you see said creature crawling out of the school again and it's now coming after you. You try to run away, but it's way faster than your speed." ||
            sentence == "Your heart feels like it's going jump out of you from how hard it's beating right now." ||
            sentence == "You just start pissing yourself at the sight, urine rapidly gushing out of you and soaking your pants." ||
            sentence == "You realize that you've started pissing yourself. You pause the game and quickly grabs your crotch to try and hold it in." ||
            sentence == "You also wet yours a little more..." ||
            sentence == "You were really caught off guard since the timing was too good. You decide to just slowly let everything go as you carefully read the way his accident is described, imagining it for yourself." ||
            sentence == "Watching his stream, I silently start peeing myself as well. If one inspects me carefully, they'll be able to see that the log under me is suspiciously darker in color than the rest.") {
            gm.audioSource.clip = gm.gameAudios[3];
            gm.audioSource.Play();
        }

        if(sentence == "After you finish peeing, you walk back to your room, noticing the trail of pee leading straight to the restroom. You can only hope it dries by morning time." ||
            sentence == "You grab your crotch in surprise in attempt to stop the stream and it works." ||
            sentence == "You grab your crotch in attempt to stop the stream and it luckily works. Your pants are already very wet now, though." ||
            sentence == "You: (No way... Almost wet myself completely just from that...)" ||
            sentence == "After you reach the credits, you finally turn off the game." ||
            sentence == "You watch the ending of the game soaked in your own piss. After it ends, you finally turn off the game and start the clean up phase." ||
            sentence == "Once you're done, you wiggle your hips slightly, making the pee stored below slosh around." ||
            sentence == "After peeing it all out, you play with yourself for a bit before heading to the bathroom to clean up." ||
            sentence == "Your friend goes in his tent and takes a bucket out just as you finish peeing. Both of you then proceed to 'wash' the logs and the ground with water.") {
            gm.audioSource.Stop();
        }
        
        if(sentence =="You find it weird as to why he's holding it for you, but your desperation makes it hard for you to think too much about it. You fit your penis to the hole and start peeing.") {
            gm.audioSource.clip = gm.gameAudios[11];
            gm.audioSource.Play();
        }
        
        if(sentence == "Morning comes...") {
            gm.showNightTime.SetActive(false);
        }

        if(sentence == "When your character is attending class and ends up peeing his diapers..." && !gm.isProtected) {
            gm.audioSource.clip = gm.gameAudios[6];
            gm.audioSource.Play();
        }
        
        if(sentence == "You've played a couple of multiplayer shooter games in the past as well, so you're pretty used to it.") {
            gm.audioSource.clip = gm.gameAudios[8];
            gm.audioSource.Play();
        }

        if(sentence == "You carefully try to walk around the cabin. You feel like your heart isn't ready yet for what you'll encounter in the vast forest. However...") {
            gm.audioSource.clip = gm.gameAudios[12];
            gm.audioSource.Play();
        }

        if(sentence == "You go directly for the kill and you manage to safely get back to your teammates." ||
            sentence == "You somehow manage to win the duel and return to your team safely." || 
            sentence == "You look around the spawn area and spots the enemy that guy warned you about, you manage to kill them pretty easily thanks to being prepared.") {
            gm.audioSource.PlayOneShot(gm.gameAudios[10]);
        }

        if(sentence == "You try your best to stay alive and head back to your teammates, but it's really not the easiest task in the world.") {
            gm.audioSource.PlayOneShot(gm.gameAudios[9]);
        }

        if(sentence == "You feel your heartbeat racing as you're running away from the enemy, trying your best to survive." ||
            sentence == "However, it turns out someone from the enemy team is spawn-camping you." ||
            sentence == "You decide to let go for a bit. Small leaks of pee are successfully shot out of you, relieving some bladder pressure." ||
            sentence == "You: Focus their supports--") {
            //Leak
            print("in check trigger");
            if(gm.leakState == 0) {
                gm.audioSource.PlayOneShot(gm.gameAudios[0]);
                gm.leakState++;
            } else if(gm.leakState == 1) {
                gm.audioSource.PlayOneShot(gm.gameAudios[1]);
                gm.leakState++;
            } else if(gm.leakState == 2) {
                gm.audioSource.PlayOneShot(gm.gameAudios[2]);
                gm.leakState++;
            } else if(gm.leakState == 3) {
                gm.audioSource.clip = gm.gameAudios[3];
                gm.audioSource.Play();
                gm.leakState++;
            }

            if(sentence == "You: Focus their supports--") {
                if(gm.leakState <= 3) {
                    sentences.Enqueue("You feel yourself leaking, but you still got it under control.");
                    sentences.Enqueue("Somehow, you manage to not completely lose control over your bladder, but it still costed you your focus and you end up losing the game.");
                    sentences.Enqueue("Afterwards, you shut off the game and stand up to check the damage. You leaked quite a bit and it's showing on your pants.");
                    if(gm.roomieWatch) {
                        sentences.Enqueue("Roomie: ...Why even try holding it in?");
                        sentences.Enqueue("You: Just wanted to try a holding challenge for once.");
                        sentences.Enqueue("Roomie: So, was succeeding the holding challenge worth losing the game?");
                        sentences.Enqueue("You: You're so disappointed I didn't fail.");
                        sentences.Enqueue("Roomie: I really am. Heartbroken, even.");
                        sentences.Enqueue("You: Better luck next time.");
                    }
                    dialogueTypeTemp = "";
                    if(gm.leakState == 1) {
                        gm.desperation = gm.tier1desperation;
                    } else if(gm.leakState == 2) {
                        gm.desperation = gm.tier2desperation;
                    } else if(gm.leakState == 3) {
                        gm.desperation = gm.tier3desperation;
                    }
                } else {
                    //full on wetting
                    gm.gameWetting();
                }
            }

            if(gameWettingBool) {
                gm.audioSource.clip = gm.gameAudios[3];
                gm.audioSource.Play();
                gameWettingBool = false;
            }
        } 
    }

    public void DisplayNextSentence() {
        //only by player click can we close the dialogue
        if (sentences.Count == 0) {
            isTalking = false;
            closeDialogue();
            return;
        } else {
            isTalking = true;
            //DisplayCurrSentence();
            string sentence = sentences.Dequeue();
            //string sentence = sentences.Peek();
            showDialogue(sentence);
            print(sentence);
            checkTriggers(sentence);
        }
    }

    public void showDialogue(string dialogueMessage) {
        if(!topDialogue) {
            dialogueText.text = dialogueMessage;
            dialogueBox.SetActive(true);
            topDialogueBox.SetActive(false);
        } else {
            topDialogueText.text = dialogueMessage;
            topDialogueBox.SetActive(true);
            dialogueBox.SetActive(false);
        }
    }


    private void closeDialogue() {
        if(!topDialogue) {
            dialogueBox.SetActive(false);
        } else {
            topDialogueBox.SetActive(false);
        }

        if(!gm.isRoommate) {
            gm.showRoommate.SetActive(false);
            gm.roommateDelay  = true;
        }

        if(dialogueTypeTemp == "workEvent2") {
            showWorkWet.SetActive(true);
            dialogueTypeTemp = "";
        }
        if(dialogueTypeTemp == "workEventPee" || 
            dialogueTypeTemp == "workEventPee2" || 
            dialogueTypeTemp == "workEventPee3" || 
            dialogueTypeTemp == "workEventPee4" ||
            dialogueTypeTemp == "workEventDiaper2" || 
            dialogueTypeTemp == "workEventDiaper3" || 
            dialogueTypeTemp == "workEventDiaper4") {
                showContinueWork.SetActive(true);
                dialogueTypeTemp = "";
        }
        if(stopPeeAudio == true) {
            gm.audioSource.Stop();
            stopPeeAudio = false;
        }
        if(dialogueTypeTemp == "gameDecision1" || 
            dialogueTypeTemp == "gameDecision2" ||
            dialogueTypeTemp == "gameDecision3" ||
            dialogueTypeTemp == "horrorDecision1" ||
            dialogueTypeTemp == "horrorDecision2" ||
            dialogueTypeTemp == "horrorDecision3" ||
            dialogueTypeTemp == "omogeDecision1" ||
            dialogueTypeTemp == "omogeDecision2" ||
            dialogueTypeTemp == "omogeDecision3" ||
            dialogueTypeTemp == "roomieSleep" ||
            dialogueTypeTemp == "wakeUpDry") {
            showGameOptions.SetActive(true);
        }
    }

}
