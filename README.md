# iSENS Wireless Vis Website
a unity project that that compiles a webgl website for visualizing wireless signals

This project uses Unity version 2022.3.11f1 and needs the webGL build support. It may also be necessary to use a unity lisence above the default free lisence due to the use of javascript dll's. The unity education lisence was used and worked as of the writing of this file, no other lisences were tested.

## Compiling
Compiling this project involves opening this project in the correct version of unity with webgl build support, clicking 'file' in the unity app and 'build settings'. From there one needs to make sure they are building for webGl and click 'build'. This will build the html and webgl files necessary to host the website. The index.html file is the root target for hosting the built version of the site. You will need to have some variety of webserver to host this. UIUC has cPanel for public hosting. For private testing a node.js server may be helpful.


## Scenes
The general makeup of this project are in three unity scenes. 
### UI Menu
The first scene is the initial UI menu. This menu is where wireless insight files precessed as a CSV (using Saif's python script) may be uploaded. A number of settings may be altered here. The user chooses between a static wireless path visualization, and a dynamic one that "lanuches" packets along the paths. The user also selects a color map at this stage. Theses colormaps are all selected for their superior visualization ability (perceptually uniform etc). The user also selects the interactivity version for the visualization, whether they want to move through a set of fixed camera positions with the space bar use 'wasd' and a mourse to move instead. They then press start and enter into one of two similar scenes depending on what interactivity mode was selected.

### House Scenes
The house scenes feature the wireless visualization making use of the different selections the user made in the UI menu. There are two different versions of this scene depending on which interaction method was chosen. This scene makes use of the house 3d model that the iSENS lab has made use of.


## Scripts
This project features a number of C# and javascript scripts that handle the simulation and web browser interaction. The only scripts detailed in this README are custom scripts for the purpose of this project.

### Options.cs

This is a C# script that handles the UI menu options that the user selects. It is designed to perpetuate the gameobject it is attached to so that is remains present even when the scene changes. It is also responsible for calling the scene change that happens when the user presses start.

### Colormap.cs

This is a C# script that handles the colormaps in the visualizaion. Most of this file is made up of lookup tables for the popular and good colormaps viridis, magma, inferno, and plasma. There is one public member function ```Color GetColor(float val, float minVal, float maxVal, string name)```. It takes in the number to be represented in a color, the minimum and maximum data values, and the name of the colormap to be used. It returns the resulting unity Color class object.

### CSVReader.cs 

This is a C# script that reads in the chosen CSV data file. These files are precessed CSV files from wireless insight simulations using Saif's python script. An exampe of such a file may be found in the Assets\StreamingAssets folder. This file defines the ```Path()``` class used to store information for individual wireless paths. This script is linked to the Options.cs script as it needs to know the name of the data file to read in.

### FileUpload.cs and FileUploader.jslib

This is a C# script and a javascript dll that interface in order to handle the upload of CSV datafiles to the webserver runs the compiled version of this project. The C# file handles the user's interaction with the unity UI button and the jslib file handles the actual upload of the CSV file to the webserver. These scripts call functions from each other to time the different upload stages properly with the UI button click.

### FreeNotifier.cs, FixedNotifier.cs, and TestLib.jslib

These two C# scripts call their respective functions in TestLib in order to create the javascript pop up that informs the user of which controls they are using depending on the interactivity mode they selected.

### CameraMovement.cs

This is a C# script that handles the movement of the camera in the free camera version of the house scene.

### FixedCamera.cs

This is a C# script that handles the change between the static camera postions in the fixed camer version of the house scene.

### PacketVisualizer.cs (*important!*)

This is the primary C# script that handles the actual visualization of the wireless paths. It defines the ```Packet``` class that holds the information for the dynamic visualization where the spheres travel along the wireless paths. This script handles the rendering of the static and dynamic visualization modes, coordinate conversions from the CSV files (which do not agree with Unities coordinate system for the house), and the colorscale that is visible in the scene. If you have questions about this script please contact Alex Shaffer (host of this repository).

