using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Intent;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Collections.Specialized;  
using AzureMail;
using TextSummary;

namespace SpeechToText
{
    class Program
    {
        static void Main(string[] args)
        {
            // ContinuousRecognitionWithFileAsync().Wait();
            // Console.WriteLine("Please press Enter to continue.");
            // Console.ReadLine();

            TextSummaryConvertor convertor = new TextSummaryConvertor();
            convertor.processFile();
        }

        public static async Task ContinuousRecognitionWithFileAsync()
        {
            // <recognitionContinuousWithFile>
            // Creates an instance of a speech config with specified subscription key and service region.
            // Replace with your own subscription key and service region (e.g., "westus").
            var config = SpeechConfig.FromSubscription("f6bc37e263114ef7a232d0c7a3ecd4eb", "westus");

            var stopRecognition = new TaskCompletionSource<int>();
            StringBuilder contents = new StringBuilder();
            
            // Creates a speech recognizer using file as audio input.
            // Replace with your own audio file name.
            using (var audioInput = AudioConfig.FromWavFileInput(@"Recording2.wav"))
            //using (var audioInput = AudioConfig.FromDefaultMicrophoneInput())
            {
                
                using (var recognizer = new SpeechRecognizer(config, audioInput))
                {
                    Console.WriteLine("recognizer "+recognizer);
                    // Subscribes to events.
                    recognizer.Recognizing += (s, e) =>
                    {
                        Console.WriteLine($"RECOGNIZING Voice: Text={e.Result.Text}");
                    };

                    recognizer.Recognized += (s, e) =>
                    {
                        if (e.Result.Reason == ResultReason.RecognizedSpeech)
                        {
                            Console.WriteLine($"RECOGNIZED voice: Text={e.Result.Text}");
                            Console.WriteLine(File.Exists("@Voice-Recognized.txt"));
                            contents.Append(e.Result.Text);                            
                        }
                        else if (e.Result.Reason == ResultReason.NoMatch)
                        {
                            Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                        }
                    };

                    recognizer.Canceled += (s, e) =>
                    {
                        Console.WriteLine($"CANCELED: Reason={e.Reason}");

                        if (e.Reason == CancellationReason.Error)
                        {
                            Console.WriteLine($"CANCELED: ErrorCode={e.ErrorCode}");
                            Console.WriteLine($"CANCELED: ErrorDetails={e.ErrorDetails}");
                            Console.WriteLine($"CANCELED: Did you update the subscription info?");
                        }

                        stopRecognition.TrySetResult(0);
                    };

                    recognizer.SessionStarted += (s, e) =>
                    {
                        Console.WriteLine("\n    Session started event.");
                    };

                    recognizer.SessionStopped += (s, e) =>
                    {
                        using (WebClient wc = new WebClient())
                        {
                            NameValueCollection values = new NameValueCollection();
                            values.Add("text", contents.ToString());
                            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                            byte[] result = wc.UploadValues("http://esummarizer.com/main/getsummary", "POST", values);
                            string ResultAuthTicket = Encoding.UTF8.GetString(result);
                            File.WriteAllText("C:\\Users\\PiyuNir\\hackerton-project\\Voice-Recognized.txt", ResultAuthTicket);
                            MailComponent.SendMail(ResultAuthTicket);
                        }
                        Console.WriteLine("\n    Session stopped event.");
                        Console.WriteLine("\nStop recognition.");
                        stopRecognition.TrySetResult(0);
                    };

                    // Starts continuous recognition. Uses StopContinuousRecognitionAsync() to stop recognition.
                    await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

                    // Waits for completion.
                    // Use Task.WaitAny to keep the task rooted.
                    Task.WaitAny(new[] { stopRecognition.Task });

                    // Stops recognition.
                    await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
                }
            }
            // </recognitionContinuousWithFile>
        }
    }
}