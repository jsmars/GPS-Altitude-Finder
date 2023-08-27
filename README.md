# GPS-Altitude-Finder
GPS Altitude Finder for DJI FPV and similar files

# TLDR
Searches text files. Compares min/max altitude and multiplies it by 10 to give a decent altitude value per file for DJI FPV GPS data.

![GPS_Altitude_Finder_0qap0YAUpd](https://github.com/jsmars/GPS-Altitude-Finder/assets/5105918/812cbbd5-1d10-47de-bd73-40cd4f8565ff)

# Instructions
This application was mainly made to search through a folder of GPS files created for DJI FPV and calculating the max altitude of the clips, but could in theory be used for other stuff as well.It will search though all files and show you the ranges of altitude values in the files.If I've understood things correctly, the altitude data in the DJI FPV is based on barometer data and might not be completely accurate, and also it will show the value divided by 10, and this is compensated for. In some cases the altitude is displayed as a negative value as well. The displayed height is the difference between the min and max altitude values multiplied by 10, so if you start or land in the video, and the barometer is working correctly, it should be fairly accurate, but check manually if you are unsure.

# Usage
1. Use the source, or download the build: https://github.com/jsmars/GPS-Altitude-Finder/releases
2. Press browse and choose directory
3. Press the search button

Hope this is helpful to somebody!
