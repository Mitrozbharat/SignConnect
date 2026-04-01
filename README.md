# SmartVideoCallApp

SmartVideoCallApp is a browser-based sign-assisted video meeting and training system built with ASP.NET Core MVC, SignalR, EF Core, MySQL, TensorFlow.js, and MediaPipe Hands.

The project combines:
- live video meeting features
- sign-sample capture and storage
- live sign prediction from webcam input
- optional speech output for recognized text

This project is designed as a practical custom sign-recognition platform. It is not a full sign-language translation engine, but it provides a strong working foundation for sign training, recognition, and communication support.

## Project Objectives

The main goals of this project are:
- support live browser-based video meetings
- help users capture and store custom hand-sign samples
- recognize saved signs from a live camera feed
- convert recognized signs into text
- optionally speak recognized output
- provide a base system for future sign-language assistive tools

## Main Features

- Room-based live video meeting flow
- SignalR-based realtime communication
- Training page for sign capture and storage
- Sign prediction page inside the training workflow
- One-hand and two-hand sign support
- Saved sign data management page
- Recognized text output from predicted signs
- Speech mode ON/OFF toggle
- Repeat speech cooldown for the same sign
- No-hand reset behavior to clear stale recognized text

## Technology Stack

- Backend: ASP.NET Core MVC (.NET 8)
- Frontend: Razor Views, JavaScript, HTML, CSS
- Realtime Communication: SignalR
- Database: EF Core with MySQL
- Sign Detection: TensorFlow.js + MediaPipe Hands
- Voice Output: Browser `speechSynthesis`
- Video/Meeting Signaling: WebRTC signaling flow with SignalR support

## System Modules

### 1. Home Module
Provides the entry point and navigation into the system.

### 2. Meeting Module
Supports room join, live video/audio communication, chat, and local sign-assist behavior.

### 3. Train Model Module
Allows the user to:
- start the camera
- detect hand landmarks
- preview coordinates
- enter sign label and description
- save sign samples
- test live prediction

### 4. Sign Coordinates Module
Displays saved training samples in a searchable and paginated table.

### 5. Backend Controller Module
The `CallController` handles sign sample saving and loading for prediction and management pages.

### 6. SignalR Hub Module
The `CallHub` supports realtime room communication and meeting events.

## Core Algorithms Used In This Project

This project currently uses 7 key algorithmic workflows:

1. Hand landmark detection
2. Coordinate mapping
3. Coordinate normalization
4. Distance-based sample matching
5. Two-hand alignment and swapped-hand comparison
6. Stable prediction timing
7. Speech cooldown and speech-mode control

## Algorithm Details

### 1. Hand Landmark Detection
The project uses TensorFlow.js with MediaPipe Hands to estimate hand landmarks from live camera video.

Why it is used:
- browser-based processing
- no separate ML server required
- supports fast landmark detection
- supports multi-hand detection

How it helps:
- provides the raw landmark points needed for training and prediction

### 2. Coordinate Mapping
Raw detector output is converted into a structured coordinate object that includes:
- hand index
- hand label
- point coordinates
- point names
- 2D and optional 3D-like values

Why it is used:
- makes data easier to store
- creates a consistent format for comparison

### 3. Coordinate Normalization
The saved and live coordinates are normalized relative to the wrist/origin point and scaled by maximum point distance.

Why it is used:
- reduces the effect of hand position, screen position, and size differences
- improves comparison reliability

How it helps:
- the system compares hand shape more than raw location

### 4. Distance-Based Sample Matching
The project compares normalized live coordinates against normalized saved samples and calculates the average point distance.

Why it is used:
- simple and effective for custom sign datasets
- avoids needing a fully trained classifier

How it helps:
- saved signs become immediately usable for prediction

### 5. Two-Hand Alignment And Swapped-Hand Comparison
Two-hand prediction can fail if the detected hand order changes. The project solves this by:
- normalizing each hand separately
- comparing only samples with the same hand count
- matching left hand to left hand and right hand to right hand when labels are available
- checking swapped order when needed

Why it is used:
- improves two-hand sign prediction
- fixes order mismatch problems

### 6. Stable Prediction Timing
The system does not accept a sign immediately. A label must remain matched for a minimum stability period before being accepted.

Why it is used:
- reduces flicker
- avoids unstable prediction output

Important values:
- prediction poll interval
- prediction stability delay

### 7. Speech Cooldown And Speech Mode Control
The system includes:
- speech mode ON/OFF toggle
- cooldown before repeating the same sign again
- speech cancellation when speech mode is off

Why it is used:
- reduces repeated speech spam
- gives users better speaking control

## How The Current Recognition Pipeline Works

Camera Input  
-> Hand Landmark Detection  
-> Coordinate Mapping  
-> Coordinate Normalization  
-> Distance Comparison  
-> Two-Hand Alignment Check  
-> Stable Prediction Check  
-> Recognized Label  
-> Optional Speech Output

## Data Flow

### Training Flow
1. User opens the training page.
2. User starts the camera.
3. System detects one or two hands.
4. Landmarks are converted into coordinate arrays.
5. User enters a label and description.
6. The sample is saved to the database.
7. The saved sample becomes available for prediction.

### Prediction Flow
1. User opens the predict tab.
2. System loads saved sign samples.
3. Live hand landmarks are detected.
4. Coordinates are normalized.
5. Current input is compared against saved samples.
6. A stable best match becomes the recognized sign.
7. If speech mode is on, recognized text can be spoken.

### Meeting Flow
1. User joins a room.
2. Local sign-related features run in the browser.
3. Sign results can be converted into text for meeting use.
4. Realtime room messaging is handled through SignalR.

## Database Design

The project includes entities such as:
- `UserAccount`
- `Organization`
- `MeetingRoom`
- `ChatMessage`
- `ActivityLog`
- `SignCoordinate`

The main sign-recognition dataset entity is `SignCoordinate`.

Important fields:
- `Id`
- `Label`
- `Description`
- `TimeId`
- `CoordinatesJson`
- `CreatedAtUtc`

`CoordinatesJson` stores one-hand or two-hand landmark samples used by the prediction logic.

## How This Project Was Developed

This project combines built-in library capabilities with custom application logic.

Library-provided capabilities:
- hand landmark detection
- realtime web communication
- browser speech support

Custom logic developed in this project:
- sign sample save/load flow
- structured coordinate mapping
- normalization logic
- sample-matching logic
- two-hand order correction logic
- stable prediction control
- repeated speech delay
- speech mode toggle behavior

This means the most important application behavior was designed and implemented specifically for this project, not just imported from a library.

## Why This Approach Was Chosen

This approach was chosen because it is:
- practical for a custom project
- easier to implement with limited training data
- faster to test and improve
- browser-friendly
- suitable for rapid sign-sample experimentation

Instead of training a full large-scale deep-learning sign-language model, this project uses saved landmark samples and matching logic. That makes it a strong prototype and a usable assistive system.

## Strengths

- runs in the browser
- no external ML server required
- supports one-hand and two-hand signs
- easy to add new saved sign samples
- integrates sign recognition with meetings
- provides optional speech output
- supports quick experimentation

## Current Limitations

- not a full sign-language translation engine
- accuracy depends on sample quality
- dynamic motion-based signs are harder than static signs
- face and body pose are not yet included
- this is sample matching, not full sequence-model recognition

## Future Improvements

- collect more sign samples per label
- add threshold tuning per sign
- support motion and sequence-based sign recognition
- include pose and face landmarks
- train a classifier on a larger dataset
- improve meeting-side sentence generation
- add instructor/admin dataset tools

## How To Run The Project

### Basic Setup
1. Clone or open the project.
2. Configure database settings in `appsettings.json`.
3. Apply migrations or ensure the MySQL database is available.
4. Build and run the ASP.NET Core project.

### Main Pages
- Meeting page: `/Call/Index`
- Training and prediction page: `/Call/TrainModel`
- Saved sign data page: `/Call/SignCoordinates`

## Documentation Notes

Detailed internal project documentation is also available in:

- [SmartVideoCallApp_2026-03-31_Documentation.txt](/c:/Users/bhara/Desktop/Sing%20Conn/SmartVideoCallApp/SmartVideoCallApp_2026-03-31_Documentation.txt)

## Final Summary

SmartVideoCallApp is a practical sign-assisted communication system that combines:
- live meetings
- sign training
- sign prediction
- optional speech output

Its current intelligence is built on:
- hand landmark detection
- coordinate normalization
- sample matching
- two-hand comparison logic
- stability and speech-control logic

This makes it a useful foundation for future sign-recognition and assistive communication development.
