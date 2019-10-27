using System;
using System.IO;
using System.Text;
using System.Net;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace WebCrawler
{
    class MainProgram
    {
        static void Main(string[] args)
        {
            PageConnection pageConnection = new PageConnection();
            //Method to get name of file and webpage name from user
            string[] connectionInfo = pageConnection.NameFile();
            //Connect to listed webpage and scan to text file
            pageConnection.makeConnection(connectionInfo);
        }

    }

    //Class to get name and page to scan
    class PageConnection
    {
        public string[] NameFile()
        {
            string fileName = "";
            string webpageName = "";
            string[] connectionInfo = new string[2];

            //Waits for user to give the text file name
            while (fileName=="") {
                Console.WriteLine("Type the name of the txt file you would like to create.");
                fileName = Console.ReadLine() + ".txt";
                if (File.Exists(fileName)){
                Console.WriteLine("File of that name already exists, try another.");
                    fileName = "";
                }
            }
            //Waits for user to give the webpage name they want to pull data from
            while (webpageName == "")
            {
                Console.WriteLine("Type the name of the webpage you would like to pull data from.");
                webpageName = Console.ReadLine();
                if (webpageName.StartsWith("www"))
                    webpageName = "http://" + webpageName;
                if(!webpageName.StartsWith("http://www."))
                    webpageName = "http://www." + webpageName;
            }

            //Assigns to String array
            connectionInfo[0] = fileName;
            connectionInfo[1] = webpageName;

            return connectionInfo;
        }

        public void makeConnection(string[] connectionInfo)
        {
            string filePath = "";
            try {
                //Get path to location where text file will be created
                filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                //Create empty file with title given by user 
                filePath = Path.Combine(filePath, connectionInfo[0]);
                StreamWriter outputFile = new StreamWriter(filePath);
                //Write raw data to text file
                outputFile.WriteLine(dataStream(connectionInfo[1]));
                outputFile.Close();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
            //Call method to sort common words and save to text file
            wordCounter(connectionInfo[0], File.ReadAllText(filePath));
        }
        private string dataStream(string address)
        {
            string webpageData;
            var client = new WebClient();
            webpageData = client.DownloadString(address);

            return webpageData;
        }

        //Method counts and ranks words in each text file
        public void wordCounter(string fileName, string webpageData)
        {
            //Get path to location where text file will be created
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //Create empty file with title given by user + sorted
            string newFile = "WordCount." + fileName;
            StreamWriter outputFile2 = new StreamWriter(Path.Combine(filePath, newFile));

            Dictionary<string, int> words = new Dictionary<string, int>();
            String[] wordArray = webpageData.Split(" ");  //create list of words based on spaces between. Future parser could improve on this looking for other delimiting characters.
            for (int i=0;i<wordArray.Length;i++) //cycle through words adding them to dictionary
            {
                //Call data parser method, to be built at later time
                //Parse out anything not full of letters


                wordArray[i] = wordArray[i].Trim();
                //if word is longer than 15 letters
                if (wordArray[i].Length > 15)
                    continue;
                //if word is "" or has a number, ignore
                if (wordArray[i].Any(char.IsDigit) || wordArray[i].Equals(""))
                    continue;

                if (!words.ContainsKey(wordArray[i]))
                    words.Add(wordArray[i], 1);
                else {
                    words.TryGetValue(wordArray[i], out var value);
                    words[wordArray[i]] = value++; //increment value in dictionary
                }
            }

            //Write data to text file
            IDictionaryEnumerator wordsEnum = words.GetEnumerator();
            while (wordsEnum.MoveNext()) {
                outputFile2.WriteLine(wordsEnum.Value + ":".PadRight(5) + wordsEnum.Key);
            }
            outputFile2.Close();
        }

        /**
        *  Build method here, call it data cleaner method, takes all webpage data and cleans it before feeding into dictionary
        */

    }
}
