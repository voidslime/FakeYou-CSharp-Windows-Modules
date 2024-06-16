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

This module console applications gets all models from the api.fakeyou.com/tts/list and saves each model_token and title to a text file.

### FakeYou Model Blacklist

This is an empty module which houses an export of the streamerbot actions that I used to call the programs.

## Getting Started

### Dependencies

Visual Studios 2022 will attempt to install and configure itself when running the solution file for the first time.

### Installing

* Pull and build the full project.

## Help

Any advise for common problems or issues.
```
command to run if program contains helper info
```

## Authors

Primarily me, Voidslime
The base files and projects come from [FakeYou Wrapper CSharp by jgric2](https://github.com/jgric2/FakeYou-Wrapper-CSharp)