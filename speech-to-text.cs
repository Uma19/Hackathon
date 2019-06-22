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
using MailComponent;
using AzureMail;

namespace HomeAutomation
{
    class Program
    {
        static void Main(string[] args)
        {
            ContinuousRecognitionWithFileAsync().Wait();
            Console.WriteLine("Please press Enter to continue.");
            Console.ReadLine();
           
        }

        public static async Task ContinuousRecognitionWithFileAsync()
        {
            // <recognitionContinuousWithFile>
            // Creates an instance of a speech config with specified subscription key and service region.
            // Replace with your own subscription key and service region (e.g., "westus").
            var config = SpeechConfig.FromSubscription("f6bc37e263114ef7a232d0c7a3ecd4eb", "westus");

            var stopRecognition = new TaskCompletionSource<int>();
            // List<string> contents = new List<string>();
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
                            AzureMailProgram.SendMail(ResultAuthTicket);
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


        static async Task RecognizeIntentAsync()
        {
            // Creates an instance of a speech config with specified subscription key
            // and service region. Note that in contrast to other services supported by
            // the Cognitive Services Speech SDK, the Language Understanding service
            // requires a specific subscription key from https://www.luis.ai/.
            // The Language Understanding service calls the required key 'endpoint key'.
            // Once you've obtained it, replace with below with your own Language Understanding subscription key
            // and service region (e.g., "westus").
            // The default language is "en-us".
            var config = SpeechConfig.FromSubscription("38f56c8ed26f4cd18ff492217ce8347a", "westus");

            // Creates an intent recognizer using microphone as audio input.
            using (var recognizer = new IntentRecognizer(config))
            {
                // Creates a Language Understanding model using the app id, and adds specific intents from your model
                var model = LanguageUnderstandingModel.FromAppId("23ffd55a-0ed4-4377-9494-176c11682ed8");
               /* recognizer.AddIntent(model, "YourLanguageUnderstandingIntentName1", "id1");
                recognizer.AddIntent(model, "YourLanguageUnderstandingIntentName2", "id2");
                recognizer.AddIntent(model, "YourLanguageUnderstandingIntentName3", "any-IntentId-here");*/
                recognizer.AddIntent(model, "HomeAutomation.TurnOff", "off");
                recognizer.AddIntent(model, "HomeAutomation.TurnOn", "on");

                // Starts recognizing.
                Console.WriteLine("Say something...");

                // Starts intent recognition, and returns after a single utterance is recognized. The end of a
                // single utterance is determined by listening for silence at the end or until a maximum of 15
                // seconds of audio is processed.  The task returns the recognition text as result. 
                // Note: Since RecognizeOnceAsync() returns only a single utterance, it is suitable only for single
                // shot recognition like command or query. 
                // For long-running multi-utterance recognition, use StartContinuousRecognitionAsync() instead.
                var result = await recognizer.RecognizeOnceAsync().ConfigureAwait(false);

                // Checks result.
                if (result.Reason == ResultReason.RecognizedIntent)
                {
                    Console.WriteLine($"RECOGNIZED: Text={result.Text}");
                    Console.WriteLine($"    Intent Id: {result.IntentId}.");
                    Console.WriteLine($"    Language Understanding JSON: {result.Properties.GetProperty(PropertyId.LanguageUnderstandingServiceResponse_JsonResult)}.");
                }
                else if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    Console.WriteLine($"RECOGNIZED: Text={result.Text}");
                    Console.WriteLine($"    Intent not recognized.");
                }
                else if (result.Reason == ResultReason.NoMatch)
                {
                    Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                }
                else if (result.Reason == ResultReason.Canceled)
                {
                    var cancellation = CancellationDetails.FromResult(result);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                        Console.WriteLine($"CANCELED: Did you update the subscription info?");
                    }
                }
            }
        }
    }
}