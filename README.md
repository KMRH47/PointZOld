![](Graphic/Logo/pz_banner.png)

# Current Version

| Application | Version      |
| ----------- | ------------ |
| PointZ      | V0.1 (Alpha) |
| PointZerver | V0.1 (Alpha) |

# FAQ

### What is PointZ?

PointZ is an Android/iPhone application built using Xamarin.Forms. The application allows you to remote control any system running PointZerver, as long as they're on the same WiFi network.

### What is PointZerver?

PointZerver is a desktop application built using .NET 5.0. PointZerver, as the name suggests, is the server that interfaces with PointZ, and must be running on a desktop system for PointZ to work.

### Which OS does PointZerver support?

Since PointZerver is built using .NET 5.0, it should work on **Windows**, **iOS** and **Linux**.

### Is PointZ on Google Play and App store?

No. Maybe in the future, but I'd rather upload releases here on GitLab.







# Installation Guide

1. Download PointZerver to the PC you want to remote control
2. Download PointZ to your mobile device
3. Run PointZerver
4. Run PointZ

# How to use

1. When PointZ is running, it'll immediately enter "Discovery mode", listening for any PointZerver applications on the network. 
2. When PointZerver and PointZ runs simultaneously, PointZ will list all available devices.
3. When you tap on the device you want to remote control, a **connect** button appears on the bottom of the screen.

Click **connect** and remote control the desired device!

![](Graphic/Guide/PointZ/Full.png)

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

