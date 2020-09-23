# CONNECT
I don't have a name to use, so I'm using this. :)

Basically this project, instantiate a TCP communication between two or more devices. One of these will be the Server (a normal pc that can run the program) and the others the clients (for this project an Android phone). Clients send commands or messages to the server, which will use to control a slave device connected to it (in this case Arduino with a serial communication).


### CONNECT Host:
As the name says, these are the file used for the program of the Server side using .NET Core.

### CONNECT Client:
This is the Android application created using Xamarin for the client side.

### CONNECT Arduino:
Contains the C++ program used by Arduino for the read of the commands sent with serial communication.

## Why this project ?
I have to much free time, also I wanted to learn more about C#, networking, Arduino and Android. All started because I didn't bought a WiFi shield for Arduino (I have an RGB strip inside the case of my PC), so I decided to find an alternative solution (I could buy a new one now, but is less fun). Coming soon I will try to use USB OTG so I will be able to communicate with Arduino directly using a smartphone.

## Should I use it ?
No, I mean, I created a public repositories with a MIT license (so you can), but this project has no documentation (good luck figuring out my code). There are a lot of bugs or things to do in a better way, I will try to improve the code. However, if you want to suggest something, do not esitate.
