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
    }
}