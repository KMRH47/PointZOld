![](Graphic/Logo/pz_banner.png)

# Current Version

**BE AWARE THAT THIS APPLICATION IS CURRENTLY IN ITS EARLY STAGES AND THUS FUNCTIONALITY IS VERY LIMITED.**

| Application | Version      |
| ----------- | ------------ |
| PointZ      | V0.1 (Alpha) |
| PointZerver | V0.1 (Alpha) |

# FAQ

### What is PointZ?

PointZ is an Android/iPhone application built using Xamarin.Forms. The application allows you to emulate mouse and keyboard on any system running PointZerver, as long as they're on the same WiFi network. 

### What is PointZerver?

PointZerver is a desktop application built using .NET 5.0. PointZerver, as the name suggests, is the server that interfaces with PointZ, and must be running on a compatible system for PointZ to work.

### What operating system is compatible with PointZerver ?

Since PointZerver is built using .NET 5.0, it works on **Windows**, **macOS** and **Linux**.

### Is PointZ on Google Play and App store?

No. Maybe in the future, but I'd rather upload releases here on GitLab.

### Is  PointZ free?

PointZ is free and open-source.

# Installation Guide

1. Download PointZerver to the PC you want to remote control
2. Download PointZ to your mobile device
3. Run PointZerver
4. Run PointZ

# Get started

1. When PointZ is running, it'll immediately enter "Discovery mode", listening for any PointZerver applications on the network. 
2. When PointZerver and PointZ runs simultaneously, PointZ will list all available devices.
3. When you tap on the device you want to remote control, a **connect** button appears on the bottom of the screen.

Click **connect** and remote control the desired device!

![](Graphic/Guide/PointZ/Full.png)

# How to use

| Action                | Equivalent                     | Sequence              | Condition                                                    | Time frame |
| --------------------- | ------------------------------ | --------------------- | ------------------------------------------------------------ | ---------- |
| Tap                   | Primary Mouse Button Click     | Down, up              | Time after putting finger down and releasing it is within the set time frame. | 150ms      |
| Tap and hold          | Primary Mouse Button Hold      | Down, up, down (hold) | Time between a tap and putting the finger down again is within the set time frame. | 250ms      |
| Double tap            | Primary Mouse Button Click x 2 | Down, up, Down, up    | Two taps in a row. Time between the single taps must match the set time frame. | 250ms      |
| Multi-tap (2 fingers) | Secondary Mouse Button Click   | Down (2 fingers)      | Time after putting fingers down and releasing them is within the set time frame. | 150ms      |
| Multi-tap (3 fingers) | Middle-Mouse Button Click      | Down (3 fingers)      | Time after putting fingers down and releasing them is within the set time frame. | 150ms      |

# Download

### PointZ

https://drive.google.com/drive/folders/1Wb_Bz7FsOtTk9ZraFRQ9Ve7e_yYNw8HR

### PointZerver

https://drive.google.com/drive/folders/1E1ca5ZQqQigOoMo5ikrgok3YpFGECbMH

# Troubleshooting

### PointZ isn't discovering my PC

**First of all, ensure that the mobile device running PointZ and the PC running PointZerver, are on the same WiFi-network.**

PointZerver constantly broadcasts UDP packets for the clients running PointZ to pick up. These broadcast messages might be blocked by a firewall on the network.

Below is a table of all ports, protocols and roles used by PointZ and PointZerver.

## Ports used

| Application | Port     | Protocol | Role             |
| ----------- | ----- | -------- | ---------------- |
| PointZ      | 45454 | UDP      | Command Sender   |
| PointZ      | 45455 | UDP      | Listener         |
| PointZerver | 45454 | UDP      | Command Receiver |
| PointZerver | 45455 | UDP      | Broadcaster      |

