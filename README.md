Cybersecurity Awareness Bot

Project Overview
This is a cybersecurity awareness chatbot developed as part of the PROG6221 Portfolio of Evidence (POE). The bot educates South African users on basic cybersecurity awareness topics such as phishing scams, strong passwords, suspicious links and online privacy.

The project consists of two parts:
- Part 1: Console-based chatbot
- Part 2: WPF GUI-based chatbot with advanced features

Part 1 - Console Application

Features Implemented
- Voice greeting using the user's own recorded audio (greetings.wav)
- Colourful ASCII art logo for "Cyber Bot"
- Numbered menu for easy user interaction
- Input validation for empty or invalid choices
- Predefined responses on key cybersecurity topics
- Slight delays to simulate a conversational feel
- Clean code structure using classes and methods
- Quick quiz to test user knowledge

Technologies Used
- C# (.NET Framework)
- System.Media.SoundPlayer for voice greeting
- Console UI with colours, borders and formatting

How to Run Part 1
1. Open the solution in Visual Studio.
2. Ensure greetings.wav is in the project and set to Copy to Output Directory = Copy if newer.
3. Press F5 to build and run.
4. The bot will play the voice greeting, show the ASCII logo, ask for your name and then display the numbered menu.

Project Structure (Part 1)
- Program.cs – Entry point
- CybersecurityBot.cs – Main bot logic, voice, menu and responses
- User.cs – Simple class with automatic property
- greetings.wav – Recorded voice greeting

Part 2 - WPF GUI Application

Features Implemented
- GUI Interface: Windows Presentation Foundation (WPF) with pink theme, proper spacing and user-friendly design
- Voice Greeting: Plays recorded voice message on startup with matching text displayed
- ASCII Art Header: Visual logo displayed at the top of the GUI window
- Keyword Recognition: Detects cybersecurity topics and provides relevant responses:
  - password – tips for strong passwords and 2FA
  - privacy – advice on protecting personal information
  - scam – how to identify and avoid scams
  - phishing – recognising phishing emails and links
- Random Responses: Multiple predefined responses for each topic, selected randomly
- Memory: Remembers user's name and recalls it during conversation
- Sentiment Detection: Recognises user emotions and responds appropriately:
  - Worried, frustrated, curious, happy, sad
- Conversation Flow: Handles follow-up questions like "tell me more", "another tip", "explain more"
- Yes/No Recognition: Responds appropriately to affirmative or negative answers
- Error Handling: Graceful handling of invalid or unrecognised inputs
- Chat History: Saves all conversations to a History/chat_history.txt file
- Typing Animation: Simulates natural conversation flow
- Auto-Scrolling: Chat window automatically scrolls to show latest messages

Technologies Used (Part 2)
- C# (.NET)
- WPF (Windows Presentation Foundation)
- System.Media.SoundPlayer for voice greeting
- XAML for UI design
- Dictionaries and Lists for response management

How to Run Part 2
1. Open the CybersecurityBot.GUI.csproj in Visual Studio.
2. Ensure greetings.wav is in the project and set to Copy to Output Directory = Copy if newer.
3. Press F5 to build and run.
4. The GUI window will open, play the voice greeting and display matching welcome text.

Project Structure (Part 2)
- MainWindow.xaml – GUI layout and design
- MainWindow.xaml.cs – Main bot logic (sentiment detection, keyword recognition, responses)
- ChatMemory.cs – Stores user name and last topic for follow-up
- User.cs – User class with Name property
- App.xaml / App.xaml.cs – Application entry point
- greetings.wav – Recorded voice greeting
- History/ – Folder where chat logs are saved

Learning Outcomes Achieved

Part 1
- Handling user input and string manipulation
- Using automatic properties
- Building structured console applications with classes and methods
- Enhancing console UI with colours, spacing, borders and delays
- Input validation and graceful error handling

Part 2
- Creating a GUI application using WPF
- Using generic collections (Dictionaries, Lists) to solve programming problems
- Using delegates and events (button clicks, key presses)
- Implementing keyword recognition and random responses
- Adding memory and sentiment detection
- Managing conversation flow with follow-up handling

Part 3 - Full Cybersecurity Bot

New Features:
- Task Assistant with MySQL database
- Cybersecurity Quiz with 12 questions
- Activity Log to track all actions
- NLP Simulation (keyword detection)

Author
Moloko Mothemela  
Course: PROG6221  
Institution: Rosebank International

Workflow Screenshot
![CI Workflow](Screenshot-of-CI-workflow.png)
