OpenTrack Head Tracking using Windows Phone
===================


Based on the Android Implementation:

https://github.com/opentrack/opentrack/wiki/Smartphone-Headtracking

This uses OpenTrack UDP instead of FreePie UDP

----------


Requirements
-------------
- Windows Phone 8.1 with **Gyroscope**
- Way to mount the phone on your head

Setup
-------------

###Install OpenTrack 

1. Start OpenTrack app
2. Set Tracker to `UDP sender` and set Port to 4242 (or any other port). Remember you may need to open this port up in your firewall.
3. Set Protocol to `freetrack 2.0`
4. Configure `Center` shortcut in `Keys` option
5. Configure Mappings as outlined below (based on Android version)
6. Click `Start`		
		
Yaw mapping

![yawmapping](https://cloud.githubusercontent.com/assets/4406961/5425422/47b2572a-82e4-11e4-8094-dc33017594ae.PNG)
		
Pitch mapping

![pitchmapping](https://cloud.githubusercontent.com/assets/4406961/5425424/58dee9f0-82e4-11e4-8be9-455f10effd60.PNG)

Roll Mapping

![rollmapping](https://cloud.githubusercontent.com/assets/4406961/5425429/6f837da6-82e4-11e4-850c-5c5cd37f34e7.PNG)

Remapping Options

![remapoptions](https://cloud.githubusercontent.com/assets/4406961/5425431/924254ac-82e4-11e4-8d26-99fb5a19c0cb.PNG)


###Install Windows Phone App

1. Download Windows Phone App and install:
  - [Stable Version](http://www.windowsphone.com/en-us/store/app/opentrack-head-tracking/1c604f32-6d68-40ef-aa44-3163e30f547f)
		
2. Start the App and enter your machine Ip Adress, the OpenTrack UDP port and Refresh Rate
3. Click `Start` to start broadcasting messages to OpenTrack

![opentrack headtracking](https://cloud.githubusercontent.com/assets/4406961/5425611/09179c42-82ed-11e4-8867-0d440c6d5d85.png)


###Mounting

Now its time to mount the phone on your head.  The guide on original Android post works well. 
