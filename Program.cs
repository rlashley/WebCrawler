using System;
using System.IO;
using System.Text;
using System.Net;

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
                Console.WriteLine("Type the name of the txt file you would like to create./n");
                fileName = Console.ReadLine();
                if (File.Exists(fileName)){
                Console.WriteLine("File of that name already exists, try another./n");
                    fileName = "";
                }
            }
            //Waits for user to give the webpage name they want to pull data from
            while (webpageName == "")
            {
                Console.WriteLine("Type the name of the webpage you would like to pull data from./n");
                webpageName = Console.ReadLine();

            }

            //Assigns to String array
            connectionInfo[0] = fileName;
            connectionInfo[1] = webpageName;

            return connectionInfo;
        }

        public void makeConnection(string[] connectionInfo)
        {
            try {
                //Get path to location where text file will be created
                string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                //Create empty file with title given by user 
                StreamWriter outputFile = new StreamWriter(Path.Combine(filePath, connectionInfo[0]));
                //Write to text file
                outputFile.WriteLine(dataStream(connectionInfo[1]));
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
        }
        private string dataStream(string address)
        {
            string webpageData;
            var client = new WebClient();
            webpageData = client.DownloadString(address);

            return webpageData;
        }


    }




}
