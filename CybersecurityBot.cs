using System;
using System.Media;
using System.Threading;

namespace CybersecurityAwarenessBot
{
    public class CybersecurityBot
    {
        private User currentUser = new User();
        private SoundPlayer voicePlayer;   

        public CybersecurityBot()
        {
            try
            {
                voicePlayer = new SoundPlayer("greetings.wav");
            }
            catch
            {
                Console.WriteLine("Warning: greetings.wav file not found.");
            }
        }


        public void Start()
        {
            DisplayHeader();
            PlayVoiceGreeting();
            GreetUser();
            RunChatbot();
        }



        private void DisplayHeader()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine(@"
             ██████╗██╗   ██╗██████╗ ███████╗██████╗     ██████╗  ██████╗ ████████╗
    ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗    ██╔══██╗██╔═══██╗╚══██╔══╝
    ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝    ██████╔╝██║   ██║   ██║   
    ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗    ██╔══██╗██║   ██║   ██║   
    ╚██████╗   ██║   ██████╔╝███████╗██║  ██║    ██████╔╝╚██████╔╝   ██║   
     ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝    ╚═════╝  ╚═════╝    ╚═╝   
                                                                           
                               C Y B E R B O T
            ");

            Console.WriteLine(@"
          ┌───                                      ───┐
                     CYBERSECURITY AWARENESS BOT   
                       Protecting You OnlinE
          └───                                      ───┘
    ");

            Console.WriteLine("==================================================");
            Console.ResetColor();
            Console.WriteLine();
        }

        private void PlayVoiceGreeting()
        {
            if (voicePlayer != null)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" Playing greeting voice message...");
                    Console.ResetColor();

                    voicePlayer.Play();
                    Thread.Sleep(4500);        
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not play voice: {ex.Message}");
                }
            }
        }

        



