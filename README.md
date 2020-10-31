# TheLighthouse

## Idea

This desktop application is meant to be used with a Raspberry Pi to display on a remote computer a window with a color depending on the level of the board pin 16 of the Raspberry Pi.

The idea is to use this winndow color as a signal when someone enters the room.

## Colors

- Green: The API sent 0 because the voltage at the board pin 16 is low
- Red: The API sent 1 because the voltage at the board pin 16 is high
- Black: The API sent something but it couldn't be recognised
- Grey/transparent: Nothing was received from the API in the last 1 second

## Installation 

### Computer application

- Just build and launch the visual studio solution
- There is only one project... It's hard to get lost lol.
- The Uri of the Raspberry Pi asked in the first winndow is its IP adress preceeded by the protocol, suchh as : "http://xxx.xxx.xxx.xxx"
- You can also publish the application to have a portable version

### Raspberry Pi installation

- Use dataplicity to make it anser {"Button": 0} or {"Button": 1} depending on the state of board pin 16: https://docs.dataplicity.com/docs/control-gpios-using-rest-api
- Modify /etc/rc.local to export the python script location and launch the app at boot time (don't forget & at the nd of the line launching the app otherwise you would block the Raspberry Pi during its boot)

## Features

- The window is set to TopMost: the winndow will always be on top of the other windows
- If a color is present is has a meaning: the app is constantly checking for new information from the Raspberry Pi. If nothing is received it becomes grey/transparent
- The Uri Uri of the Raspberry Pi is saved from a start to another
