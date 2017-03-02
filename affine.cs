using System;
using System.Collections.Generic;
using System.Text;

public static class Affine
{
    public static String encode(int a, int b, String plainText)
	{
        List<object> retList = establishVariables();
        String alphabet = Convert.ToString(retList[0]);
        Dictionary<char, int> characterToInteger = retList[1] as Dictionary<char, int>;
        Dictionary<int, char> integerToCharacter = retList[2] as Dictionary<int, char>;

        // generates cipher text
        var finalTextList = new List<char>();
        foreach (char character in plainText)
        {
            if (alphabet.Contains(character.ToString()))
            {
                int current = characterToInteger[character];
                int finalInteger = ((current * a) + b) % 26; //performs affine calculation
                char resultingCharacter = integerToCharacter[finalInteger];
                finalTextList.Add(resultingCharacter);
            }
            else
            {
                finalTextList.Add(character);
            }
        }
        String result = MakeString(finalTextList);
        return result;
	}

    public static String decode(String cipherText)
    {
        List<object> retList = establishVariables();
        cipherText = cipherText.ToLower();
        String alphabet = Convert.ToString(retList[0]);
        Dictionary<char, int> characterToInteger = retList[1] as Dictionary<char, int>;
        Dictionary<int, char> integerToCharacter = retList[2] as Dictionary<int, char>;

        for (int i = 1; i <= 26; i++)
        {
            //Iterate through all possible "a" values
            int inverse = calculateInverse(i);
            if (inverse == 0 || i == 13)
            {
                // This value of a has no inverse, skip
            }
            else
            {
                for (int b = 0; b <= 26; b++)
                {
                    //iterate through all possible "b" values
                    var cipherTextGuess = new List<char>();
                    foreach (char letter in cipherText)
                    {
                        if (alphabet.Contains(letter.ToString()))
                        {
                            int value = characterToInteger[letter];
                            int affineValue = (inverse * (value - b)) % 26; //perform affine calculation
                            if (affineValue < 0)
                            {
                                affineValue = affineValue + 26;
                            }
                            char charValue = integerToCharacter[affineValue];
                            cipherTextGuess.Add(charValue);
                        }
                        else
                        {
                            cipherTextGuess.Add(letter);
                        }
                    }
                    String guess = MakeString(cipherTextGuess);
                    string[] strRes = guess.Split(' ');

                    NetSpell.SpellChecker.Dictionary.WordDictionary oDict = new NetSpell.SpellChecker.Dictionary.WordDictionary();
                    String path = "..//..//en-US.dic";
                    oDict.DictionaryFile = path;
                    oDict.Initialize();
                    NetSpell.SpellChecker.Spelling oSpell = new NetSpell.SpellChecker.Spelling();
                    oSpell.Dictionary = oDict;

                    // checks to see if the guess is proper English by at least 60%
                    double englishThreshold = 0.6;
                    int sizeOfGuess = strRes.Length;
                    int properEnglishCount = 0;

                    foreach (String current in strRes)
                    {
                        if (oSpell.TestWord(current))
                        {
                            properEnglishCount++;
                        }
                        double thresholdCheck = (double)properEnglishCount / (double)sizeOfGuess;
                        if (thresholdCheck >= englishThreshold)
                        {
                            //if threshold met, return guess
                            return guess;
                        }
                    }
                }
            }
        }
        return "I'm sorry, indecipherable cipher.";
    }


    private static List<object> establishVariables()
    {
        // estabishes the hash maps that provide integer <=> character mappings
        String alphabet = "abcdefghijklmnopqrstuvwxyz";
        char[] alphaList = alphabet.ToCharArray();
        var characterToInteger = new Dictionary<char, int>();
        var integerToCharacter = new Dictionary<int, char>();
        var returnList = new List<object>();

        // generates the number->letter and letter->number hash maps
        int accum = 0;
        foreach (char elem in alphaList)
        {
            characterToInteger.Add(elem, accum);
            integerToCharacter.Add(accum, elem);
            accum++;
        }
        returnList.Add(alphabet);
        returnList.Add(characterToInteger);
        returnList.Add(integerToCharacter);
        return returnList;
    }

    private static String MakeString(List<char> finalTextList)
    {
        // converts the array to a string
        StringBuilder build = new StringBuilder();
        foreach (char elem in finalTextList)
        {
            build.Append(elem);
        }
        String cipherText = build.ToString();
        return cipherText;
    }

    private static int calculateInverse(int a)
    {
        //calculates the modular inverse of "a"
        for (int x = 1; x <= 26; x++)
        {
            if (a % 2 == 0)
            {
                return 0;
            }
            int attempt = (a * x) % 26;
            if (attempt == 1)
            {
                return x;
            }
        }
        return 0;
    }
}
