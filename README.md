# Team Mulan
Repository for Collaboration with Virtual Agents using NLP and CV in VR

### Owners
* Andrew Miller
* Pranav Vaidik Dhulipala
* Abishalini Sivaraman
* Swarnabha Roy

### Tools to install 
* Unity Version 2018.4.11
* SteamVR Plugin version 2.3.2
* HTC VIVE VR System/Valve Index
* Unity ML Agents version 0.10
* TensorFlow 1.7.1
* Anaconda 3 or above
* Python 3.xx
* Watson API
* OpenCV 3
* [Steam VR Plug-in](https://steamcommunity.com/app/250820/discussions/7/2605804632880587168/)

### Instructions
1) Clone this project repository, checkout the master branch, and open the project with Unity 2018.4.11 LTS.
2) To install and use ML Agents, install Python with additional dependencies.

    Use the following coomand in Anaconda promt:  
```sh
    pip3 install mlagents==0.10
```

3) Navigate to the project folder through Unity Hub and open the project.
4) Open Command Prompt and navigate to the Project Scripts folder using the following command:
```sh
    cd Assets/Scripts
```
4) Run the ```detect_colors.py``` script using the command.
```sh
   python detect_colors.py
```
5) Wait for the instructions on terminal screen to press play button.

6) Click play scene and click on the game view that is showing from **display 2**. Once the image flickering in game view stops, move to game view display 1. The game is now ready to play using the VR headset, controls to which are in the next section.

### Controls
Action | Controller Binding
------------ | -------------
Activate Watson for Speech recognition | Left Hand Trigger press
Navigation through pointing | Right hand Touchpad press
Ray Casting | Right Hand Trigger Press

### Project Video
[![Project Video](https://github.tamu.edu/VIST-477-VIZA-677-CSCE-446-CSCE-650/team-mulan/blob/master/Swarnabha/MULAN.png?raw=true)](https://drive.google.com/file/d/1hC-oJd6HDmf6Ho6iFczW3uwS-PGSuFrH/view?usp=sharing)
