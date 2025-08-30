using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spectre.Console;

namespace AzureSpeechTranscription
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Go();
        }

        public static void Go()
        {

            //Ask what they want to do?
            SelectionPrompt<string> WhatToDo = new SelectionPrompt<string>();
            WhatToDo.Title("What do you want to do?");
            WhatToDo.AddChoice("Convert Transcription to line-by-line script.");
            string ToDo = AnsiConsole.Prompt(WhatToDo);

            if (ToDo == "Convert Transcription to line-by-line script.")
            {
                //Ask for path to transcription.json
                string path = AnsiConsole.Ask<string>("What is the path to your transcription JSON file?");

                //Validate that it is real
                if (System.IO.File.Exists(path) == false)
                {
                    Console.WriteLine("File doesn't exist!");
                    return;
                }

                //What file to save it in?
                string outputPath = AnsiConsole.Ask<string>("What file do you want to output the results in?");
                if (System.IO.File.Exists(outputPath) == false)
                {
                    Console.WriteLine("Path '" + outputPath + "' is not a valid file.");
                }

                //Unpack it
                string content = System.IO.File.ReadAllText(path);
                JObject jo = JObject.Parse(content);

                //Get recognized phrases
                JToken? recognizedPhrasesToken = jo.SelectToken("recognizedPhrases");
                if (recognizedPhrasesToken == null)
                {
                    Console.WriteLine("Unable to find recognizedPhrases!");
                    return;
                }
                JArray recognizedPhrases = (JArray)recognizedPhrasesToken;

                //Loop through all
                List<Spoken> ToReturn = new List<Spoken>();
                foreach (JObject spokenJSON in recognizedPhrases)
                {
                    Spoken ThisOne = new Spoken();

                    //Get speaker
                    JProperty? prop_speaker = spokenJSON.Property("speaker");
                    if (prop_speaker != null)
                    {
                        ThisOne.Speaker = Convert.ToInt32(prop_speaker.Value.ToString());
                    }

                    //get display
                    JToken? display = spokenJSON.SelectToken("nBest[0].display");
                    if (display != null)
                    {
                        ThisOne.Display = display.ToString();
                    }

                    //Get confidence
                    JToken? confidence = spokenJSON.SelectToken("nBest[0].confidence");
                    if (confidence != null)
                    {
                        ThisOne.Confidence = Convert.ToSingle(confidence.ToString());
                    }

                    ToReturn.Add(ThisOne);
                }

                //Write into the file now
                string ToWrite = "";
                foreach (Spoken s in ToReturn)
                {

                    //Determine line
                    string LineToWrite = "";
                    if (s.Speaker != null)
                    {
                        LineToWrite = "Speaker " + s.Speaker.ToString() + " @ " + s.Confidence.ToString("#0%") + " confidence: " + s.Display;
                    }
                    else
                    {
                        LineToWrite = s.Confidence.ToString("#0%") + " confidence: " + s.Display;
                    }


                    //Append the line
                    ToWrite = ToWrite + LineToWrite + "\n\n";
                }
                if (ToWrite.Length > 0)
                {
                    ToWrite = ToWrite.Substring(0, ToWrite.Length - 2); // trim off last 2 new lines
                }

                //Write it!
                System.IO.File.WriteAllText(outputPath, ToWrite);
            }


            

        }

    }
}