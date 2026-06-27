using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
namespace CybersecurityBot.GUI
{
    public partial class MainWindow : Window
    {
        private ChatMemory memory = new ChatMemory();
        private Random random = new Random();
        private string botName = "Cyber Bot";
        private bool nameStored = false;
        private string historyFile = "History/chat_history.txt";
        private DatabaseHelper dbHelper = new DatabaseHelper();
        private QuizManager quizManager = new QuizManager();  
        private List<string> activityLog = new List<string>();  

        private Dictionary<string, List<string>> cyberResponses = new Dictionary<string, List<string>>()
        {
            {
                "password", new List<string>()
                {
                    "Use strong passwords with at least 12 characters, mixing uppercase, lowercase, numbers and symbols.",
                    "Never reuse passwords on different websites.",
                    "Enable two-factor authentication (2FA) wherever possible.",
                    "Avoid using personal details like your birthday in passwords."
                }
            },
            {
                "privacy", new List<string>()
                {
                    "Avoid sharing personal information publicly.",
                    "Review app permissions regularly.",
                    "Use privacy settings on social media.",
                    "Be careful what you post online - it stays there forever."
                }
            },
            {
                "scam", new List<string>()
                {
                    "Never trust suspicious emails or calls asking for personal information.",
                    "Scammers often create urgency to trick victims.",
                    "Verify links before clicking them.",
                    "If something sounds too good to be true, it probably is."
                }
            },
            {
                "phishing", new List<string>()
                {
                    "Never click links in emails or SMS that create urgency.",
                    "Check the sender and hover over the link before clicking.",
                    "Report suspicious messages to 0800 171 100.",
                    "Type the website address manually instead of clicking."
                }
            }
        };

        public MainWindow()
        {
            InitializeComponent();
            ChatScrollViewer.ScrollToBottom();

            PlayVoiceGreetingWithText();
        }

        private void PlayVoiceGreetingWithText()
        {
            try
            {
                SoundPlayer player = new SoundPlayer("greetings.wav");
                player.Play();
            }
            catch
            {
                // if voice fails, it should continue.
            }

            AddBotMessage("Hey there. Welcome to the cybersecurity awareness bot.");
            AddBotMessage("I'm your friendly guide here to help you stay safe online.");
            AddBotMessage("With so many phishing scams and dodgy links going around, it's great that you're here.");
            AddBotMessage("What would you like to know about today?");
            AddBotMessage("But first, what is your name?");
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessUserInput();
        }


        private void UserInput_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                ProcessUserInput();
            }
        }

        private string DetectSentiment(string message)
        {
            string lower = message.ToLower();

            if (lower.Contains("worried") || lower.Contains("scared") || lower.Contains("anxious") ||
                lower.Contains("nervous") || lower.Contains("afraid"))
                return "worried";

            if (lower.Contains("frustrated") || lower.Contains("annoyed") || lower.Contains("angry") ||
                lower.Contains("upset"))
                return "frustrated";

            if (lower.Contains("curious") || lower.Contains("interested") || lower.Contains("tell me") ||
                lower.Contains("learn"))
                return "curious";

            if (lower.Contains("happy") || lower.Contains("great") || lower.Contains("good") ||
                lower.Contains("thanks"))
                return "happy";

            return "neutral";
        }

        private async void ProcessUserInput()
        {
            string message = UserInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(message))
            {
                if (!nameStored)
                {
                    AddBotMessage("Please enter a valid name.");
                }
                else
                {
                    AddBotMessage("I didn't quite understand that. Could you rephrase?");
                }
                UserInput.Clear();
                return;
            }

            AddUserMessage(message);
            SaveMessage("USER", message);
            UserInput.Clear();

            string lower = message.ToLower();

            // if name is not stored yet, it will store it.
            if (!nameStored)
            {
                if (string.IsNullOrEmpty(message) || message.Length < 2)
                {
                    memory.UserMemory["name"] = "Chomi";
                    AddBotMessage("That name is too short, I'll call you Chomi instead!");
                }
                else
                {
                    memory.UserMemory["name"] = message;
                    AddBotMessage($"Nice to meet you, {message}! Ready to learn how to stay safe online.");
                }
                nameStored = true;
                AddBotMessage("You can ask me about: passwords, privacy, scams, or phishing.");
                return;
            }

            await TypingAnimation();

            if (lower.Contains("what is my name"))
            {
                AddBotMessage($"Your name is {memory.UserMemory["name"]}.");
                return;
            }

            if (lower.Contains("your name") || lower.Contains("who are you"))
            {
                AddBotMessage($"My name is {botName}! I'm your Cybersecurity Awareness Assistant.");
                return;
            }

            // yes//no recognition.
            if (lower == "yes" || lower == "yeah" || lower == "sure" || lower == "ok" || lower == "okay")
            {
                AddBotMessage("Great! Let's get started. What would you like to learn about?");
                AddBotMessage("You can ask me about: passwords, privacy, scams or phishing.");
                return;
            }
            else if (lower == "no" || lower == "nope" || lower == "not really" || lower == "nah")
            {
                AddBotMessage("That's okay. Take your time. I'm here whenever you're ready to learn.");
                AddBotMessage("Just ask me about passwords, privacy, scams or phishing whenever you like.");
                return;
            }

            // this will be the task commands.
            if (lower.Contains("add task") || lower.Contains("new task") ||
                lower.Contains("show my tasks") || lower.Contains("view tasks") ||
                lower.Contains("complete task") || lower.Contains("delete task") ||
                lower.Contains("remind me in"))
            {
                HandleTaskCommand(message);
                return;
            }

            // this will be the quiz commands.
            // when the user wants to start the quiz.
            if (lower.Contains("start quiz") || lower.Contains("play quiz"))
            {
                AddBotMessage(quizManager.StartQuiz());
                AddBotMessage(quizManager.GetCurrentQuestion());
                activityLog.Add("Quiz started");  
                return;
            }

            // when the user wants to quit the quiz.
            if (quizManager.IsQuizActive() && lower.Contains("quit"))
            {
                AddBotMessage(quizManager.SubmitAnswer("quit"));
                activityLog.Add("Quiz quit early");  
                return;
            }

            // shows if the quiz is active then it will process answers.
            if (quizManager.IsQuizActive())
            {
                string result = quizManager.SubmitAnswer(message);
                AddBotMessage(result);

                // it will check if quiz just completed.
                if (result.Contains("complete") || result.Contains("final score"))
                {
                    activityLog.Add("Quiz completed");  
                }
                return;
            }

            // this is the activity log.
            if (lower.Contains("show activity log") || lower.Contains("what have you done for me") ||
                lower.Contains("show log") || lower.Contains("activity log"))
            {
                if (activityLog.Count == 0)
                {
                    AddBotMessage("No activity yet. Start a quiz, add a task, or ask me something!");
                    return;
                }

                string logMessage = "Here's a summary of recent actions:\n\n";
                int startIndex = Math.Max(0, activityLog.Count - 10);
                for (int i = startIndex; i < activityLog.Count; i++)
                {
                    logMessage += $"{i + 1}. {activityLog[i]}\n";
                }
                AddBotMessage(logMessage);
                return;
            }

            // this is the follow up handling.
            if (lower.Contains("tell me more") || lower.Contains("another tip") ||
                lower.Contains("explain more") || lower.Contains("more tips"))
            {
                if (!string.IsNullOrEmpty(memory.LastTopic) && cyberResponses.ContainsKey(memory.LastTopic))
                {
                    var responses = cyberResponses[memory.LastTopic];
                    string randomResponse = responses[random.Next(responses.Count)];
                    AddBotMessage(randomResponse);
                }
                else
                {
                    AddBotMessage("Please ask about a topic first.");
                }
                return;
            }

            // this is the sentiment detection.
            string sentiment = DetectSentiment(message);

            if (sentiment == "worried")
            {
                AddBotMessage("It's completely understandable to feel that way. Let me share some tips to help you stay safe.");
                return;
            }
            else if (sentiment == "frustrated")
            {
                AddBotMessage("I understand it can be frustrating. Let me help make this easier for you.");
                return;
            }
            else if (sentiment == "curious")
            {
                AddBotMessage("That's great that you're curious! Learning about cybersecurity is important.");
                return;
            }
            else if (sentiment == "happy")
            {
                AddBotMessage("I'm glad you're feeling happy! Keep that positive energy.");
                return;
            }
            else if (sentiment == "sad")
            {
                AddBotMessage("I'm here for you. Let's talk about staying safe online.");
                return;
            }

            // this is the keyword detection.
            bool found = false;
            foreach (var keyword in cyberResponses.Keys)
            {
                if (lower.Contains(keyword))
                {
                    memory.LastTopic = keyword;
                    var responses = cyberResponses[keyword];
                    string randomResponse = responses[random.Next(responses.Count)];
                    AddBotMessage(randomResponse);
                    AddBotMessage("Say 'tell me more' for another tip on this topic.");
                    found = true;
                    break;
                }
            }

            // the default response of the bot.
            if (!found)
            {
                AddBotMessage("I didn't quite understand that. Could you rephrase? Try asking about passwords, privacy, scams, or phishing.");
            }
        }

        private void HandleTaskCommand(string message)
        {
            string lower = message.ToLower();

            // Add task: "add task - review privacy settings".
            if (lower.Contains("add task") || lower.Contains("new task"))
            {
                string taskTitle = message.Substring(message.IndexOf("-") + 1).Trim();
                if (string.IsNullOrEmpty(taskTitle))
                {
                    AddBotMessage("Please specify a task. Example: 'add task - Enable 2FA'");
                    return;
                }

                dbHelper.AddTask(taskTitle, "", null);
                AddBotMessage($"Task added: '{taskTitle}'. Would you like to set a reminder? (yes/no)");
                activityLog.Add($"Task added: {taskTitle}");  
                return;
            }

            // View tasks: "show my tasks".
            if (lower.Contains("show my tasks") || lower.Contains("view tasks") || lower.Contains("list tasks"))
            {
                var tasks = dbHelper.GetTasks();
                if (tasks.Count == 0)
                {
                    AddBotMessage("You have no tasks. Add one with 'add task - your task'.");
                    return;
                }

                string taskList = "Here are your tasks:\n";
                foreach (var task in tasks)
                {
                    string status = task.IsCompleted ? "Completed" : "Pending";
                    taskList += $"- {task.Title} [{status}]\n";
                }
                AddBotMessage(taskList);
                activityLog.Add("Viewed task list"); 
                return;
            }

            // Complete task: "complete task 1".
            if (lower.Contains("complete task"))
            {
                string[] parts = message.Split(' ');
                if (parts.Length >= 3 && int.TryParse(parts[2], out int taskId))
                {
                    dbHelper.CompleteTask(taskId);
                    AddBotMessage($"Task {taskId} marked as completed!");
                    activityLog.Add($"Task {taskId} completed"); 
                }
                else
                {
                    AddBotMessage("Please specify the task ID. Example: 'complete task 1'");
                }
                return;
            }

            // Delete task: "delete task 1".
            if (lower.Contains("delete task"))
            {
                string[] parts = message.Split(' ');
                if (parts.Length >= 3 && int.TryParse(parts[2], out int taskId))
                {
                    dbHelper.DeleteTask(taskId);
                    AddBotMessage($"Task {taskId} deleted.");
                    activityLog.Add($"Task {taskId} deleted"); 
                }
                else
                {
                    AddBotMessage("Please specify the task ID. Example: 'delete task 1'");
                }
                return;
            }

            // Set reminder: "remind me in 3 days".
            if (lower.Contains("remind me in") || lower.Contains("set reminder"))
            {
                AddBotMessage("Reminder set! I'll remind you when it's time.");
                activityLog.Add("Reminder set");  
                return;
            }

            AddBotMessage("I didn't quite understand. Try 'add task - your task' or 'show my tasks'.");
        }


        private async System.Threading.Tasks.Task TypingAnimation()
        {
            Border typingBorder = new Border()
            {
                Background = Brushes.Gray,
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(10),
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Left
            };

            TextBlock typingText = new TextBlock()
            {
                Text = $"{botName} is typing...",
                Foreground = Brushes.White
            };

            typingBorder.Child = typingText;
            ChatPanel.Children.Add(typingBorder);

            await System.Threading.Tasks.Task.Delay(1200);

            ChatPanel.Children.Remove(typingBorder);
        }

        private void AddBotMessage(string message)
        {
            StackPanel stack = new StackPanel();

            TextBlock time = new TextBlock()
            {
                Text = DateTime.Now.ToString("HH:mm"),
                Foreground = Brushes.Coral,
                FontSize = 11,
                FontWeight = FontWeights.Bold
            };

            Border border = new Border()
            {
                Background = new SolidColorBrush(Color.FromRgb(255, 20, 147)),
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(12),
                Margin = new Thickness(5),
                MaxWidth = 420,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            TextBlock text = new TextBlock()
            {
                Text = $"{botName}: {message}",
                Foreground = Brushes.White,
                FontSize = 16,
                TextWrapping = TextWrapping.Wrap
            };

            border.Child = text;
            stack.Children.Add(time);
            stack.Children.Add(border);
            ChatPanel.Children.Add(stack);
            SaveMessage("BOT", message);

            ChatScrollViewer.ScrollToBottom();
        }

        private void AddUserMessage(string message) 
        {
            StackPanel stack = new StackPanel()
            {
                HorizontalAlignment = HorizontalAlignment.Right
            };

            TextBlock time = new TextBlock()
            {
                Text = DateTime.Now.ToString("HH:mm"),
                Foreground = Brushes.Coral,
                FontSize = 11,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            Border border = new Border()
            {
                Background = new SolidColorBrush(Color.FromRgb(255, 182, 193)),
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(12),
                Margin = new Thickness(5),
                MaxWidth = 420,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            TextBlock text = new TextBlock()
            {
                Text = $"You: {message}",
                Foreground = new SolidColorBrush(Color.FromRgb(80, 40, 60)),
                FontSize = 16,
                TextWrapping = TextWrapping.Wrap
            };

            border.Child = text;
            stack.Children.Add(time);
            stack.Children.Add(border);
            ChatPanel.Children.Add(stack);

            ChatScrollViewer.ScrollToBottom();
        }

        private void SaveMessage(string sender, string message)
        {
            Directory.CreateDirectory("History");
            string line = $"{DateTime.Now} [{sender}] {message}";
            File.AppendAllText(historyFile, line + Environment.NewLine);
        }
    }
}
