# TheLighthouse

This desktop application is meant to be used with a Raspberry Pi to display on a remote computer a window with a color depending on the level of the board pin 16 of the Raspberry Pi.

The idea is to use this window color as a discrete signal when someone (your boss?) enters the room.

### Colors

- **Green**: The API sent 0 because the voltage at the board pin 16 is low
- **Red**: The API sent 1 because the voltage at the board pin 16 is high
- **Black**: The API sent something but it couldn't be recognised
- **Grey/transparent**: Nothing was received from the API in the last 1 second

## Installing / Getting started

### Computer application

- Just build and launch the visual studio solution *TheLighthouse.sln*
- There is only one project *TheLighthouse.csproj*. It's hard to get lost lol.
- The URI of the Raspberry Pi asked on the first window is its IP address preceeded by the protocol, suchh as : "http://xxx.xxx.xxx.xxx"
- You can also publish the application to have a portable version
  - In the solution explorer, right click on the project **TheLightHouse**
  - Click pusblish
  - ... follow the flow...
  - Click the .exe that has been created: this will install the app on your computer

### Raspberry Pi installation

- Use dataplicity and modify the code of the website to make the API answer *{"Button": 0}* or *{"Button": 1}* depending on the state of the board pin 16: https://docs.dataplicity.com/docs/control-gpios-using-rest-api
  - You can use a simple sensor and some hardware to send 5V to the pin 16 when there is someone, and 0V when there is no one for instance...
  - Or you can modify the code of the API and the desktop application to be more creative
- Modify /etc/rc.local to export the python script location and launch the app at boot time (don't forget to add *&* at the nd of the line responsible to launch the dataplicity app, otherwise you would block the Raspberry Pi during its boot sequence...)

## Features

- The window is set to TopMost: the winndow will always be on top of the other windows
- If a color is present is has a meaning: the app is constantly checking for new information from the Raspberry Pi. If nothing is received it becomes grey/transparent
- The Uri Uri of the Raspberry Pi is saved from a start to another

## Contributing

If you'd like to contribute, please fork the repository and use a feature branch. Pull requests are warmly welcome.

If you'd like to modify the API you can modify the function *private void UpdateGuiWithNewResult(string result)*: https://github.com/nicovv44/TheLighthouse/blob/05a2df3255cd8e1d0021cb726619e4715afe4ade/RpiRequestor/Forms/FormLightHouse.cs#L69

## Links

- Project homepage: https://github.com/nicovv44/TheLighthouse
- Issue tracker: https://github.com/nicovv44/TheLighthouse/issues
  - In case of sensitive bugs like security vulnerabilities, please contact nicolas.nv.verhelst@gmail.com directly instead of using issue tracker. We value your effort to improve the security and privacy of this project!

# Licenseing

The code in this project is licensed under MIT license.

> Copyright 2020 Nicolas VERHELST
> 
> Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
> 
> The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
> 
> THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
