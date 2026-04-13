Cybersecurity Awareness Bot  Part 1

Project Overview
This is a console based chatbot developed as Part 1 of the PROG6221 Portfolio of Evidence (POE). 
The bot educates South African users on basic cybersecurity awareness topics such as phishing scams, strong passwords and suspicious links in a friendly way.

These are the features implemented
- Voice greeting using the user's own recorded audio (greetings.wav)
- Colourful ASCII art logo for "Cyber Bot"
- Numbered menu for easy user interaction
- Input validation for empty or invalid choices
- Predefined responses on key cybersecurity topics
- Slight delays to simulate a conversational feel
- Clean code structure using classes and methods

The Technologies Used
- C# (.NET Framework)
- System.Media.SoundPlayer for voice greeting
- Console UI with colours, borders and formatting

How to Run the Application
1. Open the solution in Visual Studio.
2. Ensure greetings.wav is in the project and set to Copy to Output Directory = Copy always.
3. Press F5 to build and run.
4. The bot will play the voice greeting, show the ASCII logo, ask for your name and then display the numbered menu.

Project Structure
- Program.cs – Entry point
- CybersecurityBot.cs – Main bot logic, voice, menu and responses
- User.cs – Simple class with automatic property
- greetings.wav – Recorded voice greeting (user's own voice)


The project builds and runs successfully locally in Visual Studio.

Learning Outcomes 
- Handling user input and string manipulation
- Using automatic properties
- Building structured console applications with classes and methods
- Enhancing console UI with colours, spacing, borders and delays
- Input validation and graceful error handling

Workflow Screenshot
![Workflow](Screenshot(10).png)


Author:Moloko Mothemela 
Course:PROG6221 
Part:1 Basic Chatbot Interaction with Voice Greeting & ASCII Display
