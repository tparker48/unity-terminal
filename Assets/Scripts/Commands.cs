using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is where custom commands can be defined!
public class Commands
{
    private string output = "";


    // The in-game terminal will call this function repeatedly
    // FOR CUSTOM COMMANDS: You need to add a line here that will
    //                      call your function, follow the example of help()
    public bool Run(string functionName, List<Parameter> parameters)
    {
        if(functionName == "help")      {this.help();               return true;}
        if(functionName == "testArgs")  {this.testArgs(parameters); return true;}
        return false;
    }

    // returns the output of the last command called
    public string GetOutput()
    {
        string ret = output;
        output = "";
        return ret;
    }

    // Commands write to the terminal via the output string
    // whatever is in the output string at the end of your function will be printed
    // on the terminal
    private void help()
    {
        output =  "HELP============================\n";
        output += "This is normaly where I would provide some help!\n";
        output += "================================";
    }

    // Functions take parameters as a List of type Parameter
    // Parameters have a type (INTEGER or TEXT), Acces integers with .value and text with .text
    // Example Usage:
    private void testArgs(List<Parameter> Parameters)
    {
        int numParams = Parameters.Count;
        output += "\'testArgs\' recieved " + numParams + " parameters!\n";
        for(int i = 0; i < numParams; i++)
        {
            output += "ARG " + i + " Type: " + Parameters[i].type + "\n";
            if(Parameters[i].type == ParameterType.INTEGER) output += "ARG " + i + " Numeric Value: " + Parameters[i].value + "\n";
            else if(Parameters[i].type == ParameterType.TEXT) output += "ARG " + i + " Text Value: " + Parameters[i].text + "\n";     
        }
    }
}
