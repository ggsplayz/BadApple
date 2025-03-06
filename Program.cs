/*

  CREDITS TO gh/viliusi FOR THE ORIGINAL PROJECT (https://github.com/viliusi/Bad-Apple-CSharp/)
  CREDITS TO gh/Chion82 FOR THE ORIGINAL CONVERSION TOOL (https://github.com/Chion82/ASCII_bad_apple)
  CREDITS TO yt/Raveyboi FOR THE HI-FI AUDIO (https://youtu.be/QOeShlDRSas?si=1HyQt2jEOSc0L3Kr)
  CREDITS TO yt/avrilloosing (aka YoshiFan) FOR THE 60FPS VIDEO (https://youtu.be/ThHvx5a9IYA?si=rBtA6xr4MJEjBdAv)

  Changes made on this version: (06/03/2025)

    - Replaced some characters from the frames file for a cleaner view.

    - Set console sizes (tested on Windows) which were missing on the original version.

    - Added audio playback using NAudio + Tried my best to sync music and visual

    - Speed changed to match the original video (60fps version)

 */

using NAudio.Wave;
using System.Diagnostics;
public class Program
{
    static void Main()
    {
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        String debug = Directory.GetCurrentDirectory();
        String allFrames = debug.Remove(debug.Length - 17) + "/test60.txt";

        String soundtrack = debug.Remove(debug.Length - 17) + "/soundtrack.mp3"; // Set audio as well

        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Title = "(New) Bad Apple C#";

        // Set the console sizes so no flashy imagery takes place

        Console.SetWindowSize(151, 45);

        string line;

        // THIS IS NOT PERFECT AND MAY DESYNC FROM THE VIDEO

        using var audioFile = new AudioFileReader(soundtrack);
        using var outputDevice = new WaveOutEvent();
        outputDevice.Init(audioFile);
        outputDevice.Play();

        Thread.Sleep(100); // Wait for the audio
        Stopwatch stopwatch = Stopwatch.StartNew(); // Starts at the same time as the audio

        // Thread musicThread = new Thread(() => PlayMusic(soundtrack));
        // musicThread.IsBackground = true;
        // musicThread.Start();

        try
        {
            StreamReader sr = new StreamReader(allFrames);
            line = sr.ReadLine();
            bool split;
            List<string> currentFrame = new List<string>();

            int frameCount = 0; // Know the frame to sync
            double frameDuration = 1000.0 / 60.0; // Aprox. 60fps (some frames are lost during conversion)

            // musicThread.Start(); // This ended up not working :(

            while (line != null)
            {
                switch (line)
                {
                    case var containsSplit when line.Contains("SPLIT"):
                        split = true;

                        // This kinda resembles analog tv haha

                        line = sr.ReadLine();
                        line = sr.ReadLine();

                        break;
                    default:
                        split = false;
                        currentFrame.Add(line);
                        line = sr.ReadLine();
                        break;
                }

                switch (split)
                {
                    case true:
                        currentFrame.ForEach(i => Console.Write("{0}\n", i));
                        currentFrame.Clear();

                        // Original code used this - not needed anymore as now it is real time

                        // Thread.Sleep(50);


                        Console.SetCursorPosition(0, 0);

                        // My best attempt for a proper audio sync

                        double targetTime = frameCount * frameDuration;
                        double elapsedTime = stopwatch.Elapsed.TotalMilliseconds;
                        double sleepTime = targetTime - elapsedTime;

                        if (sleepTime > 0)
                            Thread.Sleep((int)sleepTime);

                        frameCount++; 

                        break;
                    case false:
                        break;
                    default:
                        break;
                }
            }
        }
        finally
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n                   \"Hope you enjoyed the work, while I myself didn't do too much of the work, I'm still proud of what I achieved :)\"\n                   - viliusi\n\n                   Support both projects!\n                   - ggsplayz\n\n                   https://github.com/viliusi/Bad-Apple-CSharp/\n                   https://github.com/ggsplayz/BadApple");
            Console.ReadKey();
        }
    }

    /*
     * This is not needed anymore
     * 
    static void PlayMusic(string path)
    {
        using var audioFile = new AudioFileReader(path);
        using var outputDevice = new WaveOutEvent();
        outputDevice.Init(audioFile);
        outputDevice.Play();

        // Wait for the playback to end, just in case
        while (outputDevice.PlaybackState == PlaybackState.Playing)
        {
            Thread.Sleep(500);
        }
    }

    */

    // Left just in case - not needed right now
    static void PreciseSleep(double milliseconds)
    {
        int intPart = (int)milliseconds; // Int
        double fracPart = milliseconds - intPart; // Decimals

        Thread.Sleep(intPart);

        if (fracPart > 0)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.Elapsed.TotalMilliseconds < fracPart) { }
        }
    }


}
