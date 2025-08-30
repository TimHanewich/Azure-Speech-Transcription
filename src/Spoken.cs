using System;

namespace AzureSpeechTranscription
{
    public class Spoken
    {
        public int? Speaker { get; set; } //speaker ID identifier (i.e. 1, 2) if speaker dichtarization was turned on
        public string Display { get; set; }
        public float Confidence { get; set; }

        public Spoken()
        {
            Speaker = null;
            Display = "";
            Confidence = 0.0f;
        }
    }
}