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
1) Clone this project repository.
2) Create a new Unity 3D application using Unity 2019.1.11f1.
3) Close the project and delete the "Assets" and "Project Settings" folders.
4) Copy the cloned "Assets" and "Project Settings" to the new Unity project folder.
5) Reopen the new Unity project and let Unity recompile the project.
6) To install and use ML Agents, install Python 3 with additional dependencies.

    Use the following coomand in Anaconda promt:  
```sh
    pip3 install mlagents==0.10
```

7) Navigate to the project folder through Unity Hub and open the project.
8) Open Windows Command Prompt and navigate to the Project Scripts folder using the following command:
```sh
    cd Assets/Scripts
```
9) Run the ```detect_colors.py``` script using the command.
```sh
   python detect_colors.py
```
10) Wait for the instructions on terminal screen to press play button.

11) Click play scene and click on the game view that is showing from **display 2**. Once the image flickering in game view stops, move to game view display 1. The game is now ready to play using the VR headset, controls to which are in the next section.

### Controls
Action | Controller Binding
------------ | -------------
Activate Watson for Speech recognition | Left Hand Trigger Hold
Navigation through pointing | Right hand Touchpad press
Ray Casting | Right Hand Trigger Hold

### Known Bugs
There should be two cameras active in a project window: display 1 and display 7. At the start of each room, display 7 must be active (clicked upon / highlighted) for about 3 second until the robot's camera stops moving. Once the comera stops moving, make display 1 active. This must be done each time the user starts a new room.

### Asset References
1) Mixamo Y-Bot for Robot Animation - https://www.mixamo.com/#/?page=2&type=Character
2) Portal Buttons - open source cad models
3) Basement and Sewage Modular Kit - https://assetstore.unity.com/packages/3d/environments/urban/basement-and-sewerage-modular-location-121248
4) Clipboard - https://assetstore.unity.com/packages/3d/props/clipboard-137662
5) Tim's Assets - https://assetstore.unity.com/packages/3d/props/interior/tim-s-horror-assets-the-bloody-door-70847
6) Teleportation Pad - open source cad model
7) Basic Metal Tecture Pack - open source textures

### Project Video
[![Project Video](https://github.tamu.edu/VIST-477-VIZA-677-CSCE-446-CSCE-650/team-mulan/blob/master/Swarnabha/MULAN.png?raw=true)](https://drive.google.com/file/d/1hC-oJd6HDmf6Ho6iFczW3uwS-PGSuFrH/view?usp=sharing)
