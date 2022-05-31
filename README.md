<h1 align="center">
  :video_game: Oops! All Bards :video_game:
</h1>

Code base for a game featuring a heterogeneous AI system comprised of ABL and CiF, built with Unity.

## Contributors ##

1. [Kyle Mitchell](https://github.com/kdavidmitchell)
2. [Camden Pettijohn](https://github.com/Camden-png)
3. [Joshua McCoy](https://github.com/dr-jam)

## About ##

<p align="center">
    <img src="" alt="Main Menu render"
    height="75%" width="75%">
</p>

### Project Overview ###

Broadly, this repository is a code base for an ongoing research project concerned with game agent believability, artificial intelligence for game NPCs, and leveraging heterogeneous AI systems to open new game design spaces. Within the ```abl``` folder of this repository, you will find source code for a Java proxy server that runs a game agent using [ABL](http://www.cs.cmu.edu/~michaelm/publications/AI-IE2002.pdf). Within the ```oops-all-bards``` folder, you will find source code for a game demo built with Unity that is intended to be run alongside the Java proxy server to showcase the game agent.

### The _Oops! All Bards_ Game Demo
Central to our current work is a testbed virtual world to test new ideas. In our case, this takes the form of _Oops! All Bards_. _Oops! All Bards_ is a 3D roleplaying game in which the player can inhabit the persona of a bard living in a medieval fantasy world currently under siege by a mysterious and maleficent entity. We drew much inspiration from classic tabletop RPGs, in which it is common that the player gathers a party of NPCs to join them as they progress through the game. 

## Motivation ##

We intend our system in its current state to be used to improve the believability of moments between player-driven events and NPC reaction. Thus, it made sense to showcase how an NPC can react and behave in different scenarios using our combined system. For the purposes of the demo shown here, we crafted an NPC called Quinton: a semi-retired, grizzled bard who manages entertainment at a tavern called The Thirsty Whale. Quinton is an NPC who can be recruited to the player’s party and will aid the player in one of the scenarios shown in the demo.

## Installation ##

You can get a copy of this project up and running in the following ways,
depending on how you wish to use it:

### Game Only ###

If you only wish to play the demo, please download/clone this repository
and find the executable file in the ```Build``` folder within the ```oops-all-bards``` directory. There will be executables for Windows and MacOS.

### Build Project from Source ###

1. If you wish to build this project from source, it is recommended you have
[Unity 2021.2.9f1]([https://unity3d.com/unity/whats-new/2021.2.9]) installed on
your machine.

1. Clone this repository:
```shell
git clone https://github.com/singlab/oops-all-bards.git
```
After cloning/downloading this repository, open a new project in
Unity with the ```oops-all-bards``` directory as the root project folder.
