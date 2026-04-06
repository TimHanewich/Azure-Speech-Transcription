using System;

namespace AzureSpeechTranscription
{
    public class Spoken
    {
        public int? Speaker { get; set; } //speaker ID identifier (i.e. 1, 2) if speaker dichtarization was turned on
        public string Display { get; set; }
        public float Confidence { get; set; }
        public uint OffsetMilliseconds {get; set;} //the time at which it was spoken, in milliseconds (like a timestamp)

        public Spoken()
        {
            Speaker = null;
            Display = "";
            Confidence = 0.0f;
            OffsetMilliseconds = 0;
        }

        //The time at which it was spoken, in seconds (like a timestamp)
        public float OffsetSeconds
        {
            get
            {
                return OffsetMilliseconds / 1000.0f;
            }
        }

        public string FriendlyOffset
        {
            get
            {
                // Convert the float to a TimeSpan object
                TimeSpan t = TimeSpan.FromSeconds(OffsetSeconds);
                
                List<string> parts = new List<string>();

                // Check each unit and add to the list if it's greater than zero
                if (t.Hours > 0)
                    parts.Add($"{t.Hours} {(t.Hours == 1 ? "hour" : "hours")}");

                if (t.Minutes > 0)
                    parts.Add($"{t.Minutes} {(t.Minutes == 1 ? "minute" : "minutes")}");

                // We usually show seconds even if they are 0, unless there are hours/mins
                // But based on your requirements, we'll add them if they exist or if the string is empty
                if (t.Seconds > 0 || parts.Count == 0)
                    parts.Add($"{t.Seconds} {(t.Seconds == 1 ? "second" : "seconds")}");

                return string.Join(" ", parts);
            }
        }
    }
}