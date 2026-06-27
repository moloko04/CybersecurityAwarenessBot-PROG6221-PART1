using System;
using System.Collections.Generic;

namespace CybersecurityBot.GUI
{
    internal class QuizManager
    {
        private List<QuizQuestion> questions;
        private int currentQuestionIndex = 0;
        private int score = 0;
        private bool quizActive = false;

        public QuizManager()
        {
            LoadQuestions();
        }

        private void LoadQuestions()
        {
            questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "What should you do if you receive an email asking for your password?",
                    Options = new List<string> { "Reply with your password", "Delete the email", "Report it as phishing", "Ignore it" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Reporting phishing emails helps prevent scams and protects others."
                },
                new QuizQuestion
                {
                    Question = "True or False: Using the same password for multiple accounts is safe.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Using the same password for multiple accounts is dangerous. If one account is hacked, all your accounts are at risk."
                },
                new QuizQuestion
                {
                    Question = "What is a strong password?",
                    Options = new List<string> { "Your birthday", "A mix of letters, numbers and symbols", "Your pet's name", "123456" },
                    CorrectAnswerIndex = 1,
                    Explanation = "A strong password uses a mix of uppercase and lowercase letters, numbers, and special characters."
                },
                new QuizQuestion
                {
                    Question = "True or False: You should click on links in emails from unknown senders.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Never click on links from unknown senders. They could lead to phishing or malware sites."
                },
                new QuizQuestion
                {
                    Question = "What is two-factor authentication (2FA)?",
                    Options = new List<string> { "A password", "An extra layer of security", "A type of virus", "A scam" },
                    CorrectAnswerIndex = 1,
                    Explanation = "2FA adds an extra layer of security by requiring a second form of verification, like a code sent to your phone."
                },
                new QuizQuestion
                {
                    Question = "True or False: It's safe to share your password with a friend.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Never share your passwords with anyone. Keep them private and secure."
                },
                new QuizQuestion
                {
                    Question = "What should you do if you suspect a scam?",
                    Options = new List<string> { "Ignore it", "Report it", "Reply to it", "Forward it to friends" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Always report suspicious activities. This helps authorities track and stop scammers."
                },
                new QuizQuestion
                {
                    Question = "True or False: Public Wi-Fi is always safe to use.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Public Wi-Fi is not always safe. Avoid accessing sensitive information on public networks."
                },
                new QuizQuestion
                {
                    Question = "What is phishing?",
                    Options = new List<string> { "A type of fish", "A scam to steal personal information", "A safe browsing tool", "A social media app" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Phishing is a scam where attackers trick you into giving them your personal information."
                },
                new QuizQuestion
                {
                    Question = "True or False: You should regularly update your passwords.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Regularly updating your passwords helps protect your accounts from being compromised."
                },
                new QuizQuestion
                {
                    Question = "What should you do if you see a suspicious link?",
                    Options = new List<string> { "Click it to see what happens", "Report and delete it", "Forward it to friends", "Ignore it" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Never click suspicious links. Report and delete them immediately."
                },
                new QuizQuestion
                {
                    Question = "True or False: Antivirus software can protect you from all cyber threats.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswerIndex = 1,
                    Explanation = "While antivirus software is helpful, you still need to be careful and follow safe online practices."
                }
            };
        }

        public string StartQuiz()
        {
            currentQuestionIndex = 0;
            score = 0;
            quizActive = true;
            return "Let's start the Cybersecurity Quiz! You'll get 12 questions. Type 'next' for the next question or 'quit' to stop.";
        }

        public string GetCurrentQuestion()
        {
            if (!quizActive || currentQuestionIndex >= questions.Count)
                return null;

            var q = questions[currentQuestionIndex];
            string optionsText = "";
            for (int i = 0; i < q.Options.Count; i++)
            {
                optionsText += $"{i + 1}. {q.Options[i]}\n";
            }
            return $"Question {currentQuestionIndex + 1}/{questions.Count}: {q.Question}\n\n{optionsText}";
        }

        public string SubmitAnswer(string userInput)
        {
            if (!quizActive)
                return "The quiz hasn't started. Type 'start quiz' to begin.";

            if (currentQuestionIndex >= questions.Count)
                return "Quiz already completed! Type 'start quiz' to try again.";

            if (userInput.ToLower() == "quit")
            {
                quizActive = false;
                return $"Quiz ended. Your score was {score}/{questions.Count}. Keep learning to stay safe online!";
            }

            if (userInput.ToLower() == "next")
            {
                currentQuestionIndex++;
                if (currentQuestionIndex >= questions.Count)
                {
                    quizActive = false;
                    return $"Quiz complete! Your final score is {score}/{questions.Count}. " +
                           (score >= 10 ? "Amazing! You're a cybersecurity pro!" :
                            score >= 7 ? "Great job! You're well on your way!" :
                            "Keep learning to stay safe online!");
                }
                return GetCurrentQuestion();
            }

            if (int.TryParse(userInput, out int selectedIndex) && selectedIndex >= 1 && selectedIndex <= questions[currentQuestionIndex].Options.Count)
            {
                var q = questions[currentQuestionIndex];
                int answerIndex = selectedIndex - 1;

                if (answerIndex == q.CorrectAnswerIndex)
                {
                    score++;
                    string result = $"Correct! {q.Explanation}\n\nYour current score: {score}";
                    currentQuestionIndex++;
                    if (currentQuestionIndex >= questions.Count)
                    {
                        quizActive = false;
                        return result + $"\n\nQuiz complete! Final score: {score}/{questions.Count} " +
                               (score >= 10 ? "- You're a cybersecurity pro!" :
                                score >= 7 ? "- Great job!" :
                                "- Keep learning!");
                    }
                    return result + $"\n\nType 'next' for the next question.";
                }
                else
                {
                    string result = $"Incorrect. {q.Explanation}\n\nYour current score: {score}";
                    currentQuestionIndex++;
                    if (currentQuestionIndex >= questions.Count)
                    {
                        quizActive = false;
                        return result + $"\n\nQuiz complete! Final score: {score}/{questions.Count} " +
                               (score >= 10 ? "- You're a cybersecurity pro!" :
                                score >= 7 ? "- Great job!" :
                                "- Keep learning!");
                    }
                    return result + $"\n\nType 'next' for the next question.";
                }
            }
            else
            {
                return "Please type the number of your answer (e.g., 1, 2, 3, or 4). Or type 'next' to skip.";
            }
        }

        public bool IsQuizActive()
        {
            return quizActive;
        }
    }

    internal class QuizQuestion
    {
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public string Explanation { get; set; }
    }
}