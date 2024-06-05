# FakeYou C# Windows Modules

Barebones modularized FakeYou API calls in C#.

## Description

These modules have been developed for use with Streamer.bot

### FakeYou Post TTS Request

This module is a background windows application that requests, downloads, and plays, a tts message.

#### Environment Variables

this module expects strings "MESSAGE" and "MODELTOKEN" to be passed as environment variables.

#### POST

It sends a post request to the api.fakeyou.com/tts/inference endpoint.

#### GET Status

It then repeatedly gets status updates from the api.fakeyou.com/tts/job endpoint.

#### GET TTS Byte Stream

Once the status updates to completed, it downloads the bytestream from the provided cdn link, and writes it to a .wav file.

#### Play the TTS

Lastly it uses NAudio to identify the desired output device and plays the .wav file on that output. (I have it set to a virtual audio cable. Delete the WaveOut.DeviceCount for loop to use the default output device)

### FakeYou Model Get

This module console applications gets all models from the api.fakeyou.com/tts/list and saves some data to a text file.

## Getting Started

### Dependencies

* Describe any prerequisites, libraries, OS version, etc., needed before installing program.
* ex. Windows 10

### Installing

* How/where to download your program
* Any modifications needed to be made to files/folders

### Executing program

* How to run the program
* Step-by-step bullets
```
code blocks for commands
```

## Help

Any advise for common problems or issues.
```
command to run if program contains helper info
```

## Authors

Contributors names and contact info

ex. Dominique Pizzie  
ex. [@DomPizzie](https://twitter.com/dompizzie)

## Version History

* 0.2
    * Various bug fixes and optimizations
    * See [commit change]() or See [release history]()
* 0.1
    * Initial Release

## License

This project is licensed under the [NAME HERE] License - see the LICENSE.md file for details

## Acknowledgments

Inspiration, code snippets, etc.
* [awesome-readme](https://github.com/matiassingers/awesome-readme)
* [PurpleBooth](https://gist.github.com/PurpleBooth/109311bb0361f32d87a2)
* [dbader](https://github.com/dbader/readme-template)
* [zenorocha](https://gist.github.com/zenorocha/4526327)
* [fvcproductions](https://gist.github.com/fvcproductions/1bfc2d4aecb01a834b46)
