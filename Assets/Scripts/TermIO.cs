using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TermIO sends user input to the terminal to execute and prints its output to the screen

public class TermIO : MonoBehaviour
{
    // The text that is displayed on the screen in-game
    public Text output;

    // maximum lines of text that can fit on the screen
    public int maxLines;

    // determines the frequency of on-screen cursor blinking
    public int blinkDuration;

    // user input in-game
    public InputField input;

    // The Terminal object that produces output given user input
    Terminal terminal;

    string usernameStub = "[tparker] $ ";
    string outputHistory = "";
    string cursor;
    int lineNum = 0;
    
    int frame = 0;
    bool blink = false;

    void Start()
    {
        terminal = new Terminal();
    }

    void Awake()
    {
        input = GetComponent<InputField>();
    }

    void Update()
    {
        input.Select();
        input.ActivateInputField();

        cursorBlink();

        if (Input.GetKeyUp("return"))
        {
            handleReturn();
        }

        output.text = outputHistory + usernameStub + input.text + cursor;
    }


    void cursorBlink()
    {
        frame++;
        if(frame % blinkDuration == 0) blink = !blink;
        if(blink) cursor = " ";
        else cursor = "|";
    }

    void handleReturn()
    {
        lineNum++;
        outputHistory += usernameStub + input.text + "\n";

        if(input.text.Trim() == "clear")
        {
            Clear();
        }
        else{
            executeLine(input.text);
        }

        while (lineNum >= maxLines){
            outputHistory = outputHistory.Substring(outputHistory.IndexOf("\n")+1);
            lineNum--;
        }

        input.text = "";
    }

    void executeLine(string command)
    {
        outputHistory += terminal.executeLine(command, ref lineNum) + "\n";
    }

    void Clear()
    {
        lineNum = 0;
        outputHistory = "";
    }
}
