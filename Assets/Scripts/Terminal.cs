using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;

// This file implements how the terminal reads and executes lines of input
// The reading of input from the user is handled by TermIO.cs

enum TokenID
{
    COMMAND,      // one of the defined shell commands
    INTEGER,      // numeric type
    TEXT,         // anything of the form "text here" or 'text here'
    END,          // there are no more arguements to read
    INVALID       // syntactically invalid phrase
}

class Token
{
    public Token(){
        id = TokenID.INVALID;
        value = -1;
        text = "default";
    }
    public Token(TokenID i, int v, string t){
        id = i;
        value = v;
        text = t;
    }

    public TokenID id;
    public int value;
    public string text;
};

public enum ParameterType
{
    INTEGER,      // int
    TEXT,        // string
    INVALID     // Something wrong!
}

// Used to Pass Parameters to Funtions called in the command line
public class Parameter
{
    public ParameterType type;
    public int value;
    public string text;
}

public class Terminal
{
    Commands commandRunner;

    List<string> args;
    List<Token> tokenList;
    Token token;
    int currentArg = 0;

    string returnText;
    int returnLine;

    public Terminal()
    {
        commandRunner = new Commands();
    }

    public string executeLine(string input, ref int lineNum)
    {
        currentArg = 0;
        lineNum += 1;
        
        args = split(input);
        if(args.Count == 0) return "";

        Token lexResult = Lexer();
        if(lexResult.id == TokenID.INVALID) return "Error: " + lexResult.text;
 
        string output = Parser(tokenList);
        return output;
    }


    private List<string> split(string input)
    {   
        List<string> splitList = new List<string>();
        // regular expression for splitting arguments correctly
        Regex rgx = new Regex(@"(\b\w+\b)|(""[^""\\]*(?:\\.[^""\\]*)*"")|(""[^""\\]*(?:\\.[^""\\]*)*)");
        foreach (Match match in rgx.Matches(input)) 
        {
            splitList.Add(match.Value);
        }
        return splitList;
    }

    private Token Lexer()
    {
        token = new Token();
        tokenList = new List<Token>();

        while(token.id != TokenID.END){
            token = getNextToken();

            if(token.id == TokenID.INVALID)
            {
                return token;
            }
            else
            {
                tokenList.Add(token);
            }
            currentArg++;
        }

        return new Token(TokenID.END, 0, "Lexer completed with no issues.");
    }

    private Token getNextToken()
    {
        if(currentArg >= args.Count) 
        {
            return new Token(TokenID.END, 0,"End of arguments reached.");
        }

        string arg = args[currentArg];
        // TEXT
        if(arg[0] == '\"')
        {
            return textArg(arg);
        }
        // INTEGER
        else if(char.IsDigit(arg[0]))
        {
            return integerArg(arg);
        }
        // COMMAND
        else{
            return commandArg(arg);
        }
    }

    private Token textArg(string arg)
    {
        if(arg.Length == 1) return new Token(TokenID.INVALID, -1, "Closing \" missing!");
        
        if(arg[arg.Length-1] == '\"')
        {
            return new Token(TokenID.TEXT, 0, arg.Substring(1,arg.Length-2));
        }
        else
        {
            return new Token(TokenID.INVALID, -1, "Closing \"  missing!");
        }
    }

    private Token integerArg(string arg)
    {
        int value;
        bool goodInteger = int.TryParse(arg, out value);
        if(!goodInteger)
        {
            return new Token(TokenID.INVALID, -1, "Invalid Number");
        }
        else
        {
            return new Token(TokenID.INTEGER, value, "Integer");
        }
    }

    private Token commandArg(string arg)
    {
        return new Token(TokenID.COMMAND, 0, arg);
    }

    private string Parser(List<Token> tokens)
    {
        if(tokens[0].id != TokenID.COMMAND)
        {
            return "Error: Invalid command name."; 
        }
        else
        {
            string command = tokens[0].text;
            List<Parameter> args = getParamsFromTokenList(tokens);
            if(commandRunner.Run(command, args))
            {
                return commandRunner.GetOutput().Trim();
            }
            else
            {
                return "Error: Command \'"+ tokens[0].text + "\' not found.";                 
            }
        }
    }

    private List<Parameter> getParamsFromTokenList(List<Token> tokens)
    { 
        List<Parameter> args = new List<Parameter>();

        for(int i = 1; i < tokens.Count - 1; i++)
        {
            Parameter p = new Parameter();
            p.value = tokens[i].value;
            p.text = tokens[i].text;
            if(tokens[i].id == TokenID.INTEGER) p.type = ParameterType.INTEGER;
            else if(tokens[i].id == TokenID.TEXT || tokens[i].id == TokenID.COMMAND) p.type = ParameterType.TEXT;
            else p.type = ParameterType.INVALID;
            args.Add(p);
        }
        return args;
    }
}
