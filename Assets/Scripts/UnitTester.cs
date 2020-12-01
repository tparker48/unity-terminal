using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class UnitTester : MonoBehaviour
{
    public bool Enabled;

    Terminal term;
    int currentLineNo = 15;

    string testResults = "";
    int numCorrect = 0;
    int numTests = 0;
    //float percentPassed = 0.0f;

    void Start()
    {
        if(Enabled)
        {
            runTests();
            Debug.Log(testResults);
        }
        else
        {
            Debug.Log("Unit Tests Disabled!");
        }
    }

    void runTests()
    {
        test_0();
        test_1();
        test_2();
        test_3();
        test_4();
        test_5();
        test_6();
        test_7();
        test_8();
        test_9();
        test_10();
    }

    void testAssert(string output, string expected, string testName)
    {
        if(output == expected)
        {
            Debug.Log(testName + ": SUCCESS");
            numCorrect++;
        }
        else{
            Debug.Log(testName + ": FAIL" + "\n" + testName + " Expected Output: " + expected + "\n" + testName + "   Actual Output: " + output);
        }
        numTests++;
    }

    void test_0()
    {
        term = new Terminal();
        string input = "Test 123";
        string expected = "Command 123";
        string output = term.executeLine(input, ref currentLineNo);
        testAssert(output, expected, "test_0");
    }
    void test_1()
    {
        term = new Terminal();
        string input = "123 123 test123";
        string expected = "123 123 Command";
        string output = term.executeLine(input, ref currentLineNo);
        testAssert(output, expected, "test_1");
    }
    void test_2()
    {
        term = new Terminal();
        string input = "Testing String: \"value_here\"";
        string expected = "Command Command value_here";
        string output = term.executeLine(input, ref currentLineNo);
        testAssert(output, expected, "test_2");
    }
    void test_3()
    {
        term = new Terminal();
        string input = "Double  Space";
        string expected = "Command Command";
        string output = term.executeLine(input, ref currentLineNo);
        testAssert(output, expected, "test_3");
    }
    void test_4()
    {
        term = new Terminal();
        string input = "L5\"\"";
        string expected = "Command";
        string output = term.executeLine(input, ref currentLineNo);
        testAssert(output, expected, "test_4");
    }
    void test_5()
    {
        term = new Terminal();
        string input = "123f";
        string expected = "Error: Invalid Number";
        string output = term.executeLine(input, ref currentLineNo);
        testAssert(output, expected, "test_5");
    }
    void test_6()
    {
        term = new Terminal();
        string input = "\"";
        string expected = "Error: Closing \" missing!";
        string output = term.executeLine(input, ref currentLineNo);
        testAssert(output, expected, "test_6");
    }
    void test_7()
    {
        term = new Terminal();
        string input = "\"\'\"";
        string expected = "\'";
        string output = term.executeLine(input, ref currentLineNo);
        testAssert(output, expected, "test_7");
    }
    void test_8()
    {
        term = new Terminal();
        string input = "this 1 \"is a\" simple test!";
        string expected = "Command 1 is a Command Command";
        string output = term.executeLine(input, ref currentLineNo);
        testAssert(output, expected, "test_8");
    }
    void test_9()
    {
        term = new Terminal();
        string input = "\"\"\"";
        string expected = "Error: Closing \" missing!";
        string output = term.executeLine(input, ref currentLineNo);
        testAssert(output, expected, "test_9");
    }
    void test_10()
    {
        term = new Terminal();
        string input = "\"Should not work";
        string expected = "Error: Closing \"  missing!";
        string output = term.executeLine(input, ref currentLineNo);
        testAssert(output, expected, "test_10");
    }
}