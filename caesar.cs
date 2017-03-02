using System;
using System.Collections.Generic;
using System.Text;

    public static class Caesar
    {
        public static String encode(String plainText, int shift)
        {
            //encodes with caesar cipher
            List<object> retList = establishVariables();
            String alphabet = Convert.ToString(retList[0]);
            Dictionary<char, int> characterToInteger = retList[1] as Dictionary<char, int>;
            Dictionary<int, char> integerToCharacter = retList[2] as Dictionary<int, char>;
            // performs the shift and generates cipher text
            var finalTextList = new List<char>();
            foreach (char character in plainText)
            {
                if (alphabet.Contains(character.ToString()))
                {
                    int current = characterToInteger[character];
                    int finalInteger = (current + shift) % 26; // perform shift
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

        public static String decode(String cipher)
        {
            //decodes with the Caesar cipher
            List<object> retList = establishVariables();
            cipher = cipher.ToLower();
            String alphabet = Convert.ToString(retList[0]);
            Dictionary<char, int> characterToInteger = retList[1] as Dictionary<char, int>;
            Dictionary<int, char> integerToCharacter = retList[2] as Dictionary<int, char>;

            char[] cipherText = cipher.ToCharArray(); //so we have a permanent copy
            char[] guessText = cipher.ToCharArray(); //this will be manipulated
            int iter = 0;
            bool found = false; // when to break out of the while loop
            while (iter <= 25)
            {
                // constructs the shift "guess"
                var textList = new List<char>();
                foreach (char elem in guessText)
                {
                    if (alphabet.Contains(elem.ToString()))
                    {
                        int val = characterToInteger[elem];
                        val = (val + iter) % 26; //performs shift
                        textList.Add(integerToCharacter[val]);
                    }
                    else
                    {
                        textList.Add(elem);
                    }
                }

                String result = MakeString(textList);
                string[] strRes = result.Split(' ');

                // checks to see if NetSpell is working correctly
                NetSpell.SpellChecker.Dictionary.WordDictionary oDict = new NetSpell.SpellChecker.Dictionary.WordDictionary();
                String path = "..//..//en-US.dic";
                oDict.DictionaryFile = path;
                oDict.Initialize();
                NetSpell.SpellChecker.Spelling oSpell = new NetSpell.SpellChecker.Spelling();
                oSpell.Dictionary = oDict;

                // checks to see if the guess is proper English by at least 50%
                double englishThreshold = 0.5;
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
                        int shiftValue = 26 - iter;
                        found = true;
                    }
                }
                if (found == true)
                {
                    return result;
                }
                //iterates the while
                iter++;
            }

            return "I'm sorry, indecipherable with a shift cipher.";
        }


        private static List<object> establishVariables()
        {
            //established the hash maps that contain integer <=> character mappings for the alphabet
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
    }

