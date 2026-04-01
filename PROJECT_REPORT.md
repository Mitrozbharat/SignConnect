# SmartVideoCallApp Project Report

## 1. Title

**SmartVideoCallApp: A Browser-Based Sign-Assisted Video Meeting and Sign Prediction System**

## 2. Abstract

SmartVideoCallApp is a browser-based communication platform designed to support video meetings with sign-assistance features. The system combines live meeting functionality, sign sample capture, hand landmark detection, saved coordinate management, live sign prediction, and optional speech output.  

The project is built using ASP.NET Core MVC, SignalR, EF Core, MySQL, JavaScript, TensorFlow.js, and MediaPipe Hands. Instead of using a full deep-learning sign-language translation model, the system uses a practical custom approach based on hand landmark detection, coordinate normalization, and distance-based sample matching. This makes the system easier to build, test, and expand for custom sign vocabularies.

The project demonstrates how computer vision, realtime communication, and assistive interaction can be combined into a useful foundation for sign-supported communication tools.

## 3. Introduction

Communication accessibility is an important area in modern software systems. Many users benefit from tools that can help convert hand signs into text or speech, especially in online communication environments. Traditional video meeting platforms provide audio and video features, but they usually do not include integrated sign-recognition workflows.

SmartVideoCallApp was developed to address this gap by combining:
- live browser-based meetings
- sign sample training and storage
- live hand-sign prediction
- optional speech output

The system focuses on practical custom sign recognition rather than full natural sign-language translation. This makes it suitable for experiments, prototypes, assistive learning systems, and future expansion.

## 4. Problem Statement

Standard video meeting systems do not provide direct support for:
- capturing custom sign samples
- predicting saved hand signs from a live webcam
- converting recognized signs into readable text
- optionally speaking predicted text

A complete sign-language translation engine is difficult to build because it often requires:
- very large datasets
- sequence modeling
- motion analysis
- facial expression analysis
- body pose analysis

This project solves a more practical and achievable problem:

**How can a browser-based application support custom sign capture, sign prediction, and sign-assisted communication using saved landmark data and realtime camera input?**

## 5. Objectives

The main objectives of this project are:
- to create a browser-based video meeting system
- to support custom hand-sign sample capture
- to store sign coordinate samples in a database
- to predict saved signs using live camera input
- to convert recognized signs into text
- to provide optional speech output
- to build a foundation for future assistive sign-language systems

## 6. Scope of the Project

The scope of this project includes:
- sign-assisted communication support
- one-hand and two-hand sign sample capture
- sign data storage and management
- browser-based live sign prediction
- speech mode support
- video meeting integration through SignalR

The project does not currently include:
- full sentence-level sign-language understanding
- facial expression modeling
- body pose-based meaning extraction
- large-scale deep-learning classifier training
- full temporal motion-sequence sign translation

## 7. Technology Stack

- **Backend:** ASP.NET Core MVC (.NET 8)
- **Frontend:** Razor Views, JavaScript, HTML, CSS
- **Realtime Communication:** SignalR
- **Database:** EF Core with MySQL
- **Hand Detection:** TensorFlow.js + MediaPipe Hands
- **Speech Output:** Browser `speechSynthesis`
- **Meeting Signaling:** WebRTC-related signaling support through SignalR

## 8. System Modules

### 8.1 Home Module
Provides entry navigation into the application.

### 8.2 Meeting Module
Handles live room access, video communication, and meeting-related features.

### 8.3 Train Model Module
Supports:
- camera start
- hand landmark detection
- coordinate preview
- label and description entry
- saving sign samples
- live prediction testing

### 8.4 Sign Coordinates Module
Displays all saved sign samples with search and pagination support.

### 8.5 Backend Controller Module
The controller manages sign sample saving and loading.

### 8.6 SignalR Hub Module
Handles realtime room communication and session events.

## 9. Methodology

The system follows a practical pipeline-based methodology:

1. Capture live video from the browser camera.
2. Detect hand landmarks using TensorFlow.js and MediaPipe Hands.
3. Convert raw landmark output into structured project coordinates.
4. Normalize coordinates to reduce the effect of camera position and scale.
5. Compare current normalized landmarks with saved normalized sign samples.
6. Accept a stable best match as the predicted sign.
7. Display the recognized text and optionally speak it.

This methodology avoids the complexity of large-scale model training while still providing useful custom sign recognition.

## 10. Algorithms Used in the Project

The project currently uses 7 major algorithmic workflows.

### 10.1 Hand Landmark Detection Algorithm

**Used for:** Detecting hand keypoints from live camera input  
**Technology:** TensorFlow.js + MediaPipe Hands

**Why it is used:**
- works directly in the browser
- supports realtime hand landmark detection
- allows one-hand and two-hand tracking

**How it works:**
- the browser reads webcam frames
- the detector estimates hand landmarks
- each hand returns keypoints such as wrist and finger joints

**Benefit:**
- provides the raw hand data required for training and prediction

### 10.2 Coordinate Mapping Algorithm

**Used for:** Converting raw detector output into a storable and comparable format

**How it works:**
- raw landmarks are transformed into structured coordinate objects
- the system stores hand index, hand label, x, y, z values, and point names

**Why it is used:**
- creates a consistent project-specific data format
- simplifies storage and comparison logic

**Benefit:**
- makes sign sample handling cleaner and more reliable

### 10.3 Coordinate Normalization Algorithm

**Used for:** Making coordinates comparable across different positions and scales

**How it works:**
- wrist/origin point is treated as a reference
- all points are shifted relative to that origin
- values are scaled by maximum hand distance

**Why it is used:**
- the same sign can appear at different positions in the camera frame
- users can stand at different distances from the camera

**Benefit:**
- improves matching quality by comparing relative hand shape

### 10.4 Distance-Based Sample Matching Algorithm

**Used for:** Comparing the live sign against saved samples

**How it works:**
- normalized live points are compared with normalized saved points
- average point-to-point distance is calculated
- the best match with the smallest distance is selected
- a threshold is used to reject weak matches

**Why it is used:**
- simple and effective for custom sign datasets
- does not require full classifier training

**Benefit:**
- saved signs can be recognized immediately after capture

### 10.5 Two-Hand Alignment and Swapped-Hand Comparison Algorithm

**Used for:** Improving two-hand sign prediction

**Problem solved:**
- two-hand signs may fail if hand order changes between saved and live frames

**How it works:**
- each hand is normalized separately
- comparison only happens when hand counts match
- if left and right hand labels are available, the system matches left-to-left and right-to-right
- if hand order is uncertain, the system also checks swapped order
- the lower distance is chosen

**Why it is used:**
- two-hand signs are more complex than one-hand signs
- hand ordering can change between frames

**Benefit:**
- improves prediction reliability for two-hand signs

### 10.6 Stable Prediction Timing Algorithm

**Used for:** Reducing unstable or flickering predictions

**How it works:**
- a matched sign becomes pending first
- the same label must remain matched for a fixed stability time
- only then is it accepted as a final recognized sign

**Why it is used:**
- live predictions can flicker from frame to frame

**Benefit:**
- gives more stable and trustworthy output

### 10.7 Speech Cooldown and Speech Mode Control Algorithm

**Used for:** Managing repeated speech output

**How it works:**
- users can turn speech mode on or off
- the system delays repeated speaking of the same recognized sign
- speech is cancelled when speech mode is turned off

**Why it is used:**
- avoids audio repetition
- gives users better control

**Benefit:**
- improves usability and listening comfort

## 11. How the Developer Built the Algorithmic Logic

This project uses both third-party technology and custom logic written specifically for the application.

### Library-Based Parts
- hand landmark detection
- browser speech synthesis
- realtime communication support

### Custom Logic Developed in This Project
- sign coordinate storage design
- coordinate formatting structure
- coordinate normalization logic
- distance-comparison logic
- two-hand alignment logic
- stable prediction workflow
- no-hand reset workflow
- repeated speech delay
- speech mode toggle

This means the application behavior was not only imported from libraries. The most important recognition and usability logic was designed and implemented as part of this project.

## 12. System Workflow

### 12.1 Training Workflow
1. User opens the training page.
2. User starts the camera.
3. System detects one or two hands.
4. Coordinates are generated.
5. User enters sign label and description.
6. Coordinates are saved in the database.
7. Saved data becomes available for prediction.

### 12.2 Prediction Workflow
1. User opens the predict tab.
2. System loads saved sign samples.
3. Live hand landmarks are detected.
4. Coordinates are normalized.
5. The system compares live coordinates with saved samples.
6. A stable best match is selected.
7. Recognized text is shown.
8. If speech mode is enabled, the text can be spoken.

### 12.3 Meeting Workflow
1. User joins a video room.
2. Browser-based realtime communication starts.
3. Sign-related local logic can operate in the meeting workflow.
4. Final sign-related text can support communication.

## 13. Database Design

The important sign-recognition entity is:

### SignCoordinate
Fields:
- `Id`
- `Label`
- `Description`
- `TimeId`
- `CoordinatesJson`
- `CreatedAtUtc`

`CoordinatesJson` stores one-hand or two-hand landmark sample data.

Other application entities include:
- `UserAccount`
- `Organization`
- `MeetingRoom`
- `ChatMessage`
- `ActivityLog`

## 14. Advantages of the System

- browser-based and easy to access
- no separate ML server required
- supports both sign training and sign prediction
- supports one-hand and two-hand signs
- practical for custom sign vocabularies
- integrates with meeting workflows
- includes optional speech output
- can be improved incrementally

## 15. Limitations of the System

- not a full sign-language translation engine
- mainly frame-based rather than sequence-based
- dynamic signs are harder than static signs
- accuracy depends on saved sample quality
- face and body pose are not yet included
- does not yet perform deep semantic sentence understanding

## 16. Applications of the Project

This project can be useful for:
- assistive communication tools
- educational sign-learning systems
- custom sign vocabulary experiments
- accessibility-focused meeting platforms
- student research and prototype development

## 17. Future Enhancements

- collect multiple samples per sign label
- add threshold tuning for each sign
- support motion and temporal sequence recognition
- include face and body landmarks
- train classifier models on larger datasets
- add instructor/admin training datasets
- improve automatic sentence generation from predictions

## 18. Conclusion

SmartVideoCallApp is a useful and practical sign-assisted communication project that combines:
- realtime video meeting functionality
- sign sample capture and storage
- custom sign prediction
- optional speech output

The system uses a strong engineering approach based on:
- hand landmark detection
- coordinate normalization
- distance-based matching
- two-hand comparison logic
- stability control
- speech-control logic

Although it is not yet a full sign-language translation engine, it is an effective and expandable foundation for future assistive communication and sign-recognition systems.

## 19. References

- ASP.NET Core MVC documentation
- SignalR documentation
- Entity Framework Core documentation
- MySQL documentation
- TensorFlow.js documentation
- MediaPipe Hands documentation
- Browser Web Speech API documentation
