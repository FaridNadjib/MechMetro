Welcome to Beautify for Universal Rendering Pipeline!

Requirements:
- Unity 2021.3.16 or later
- URP (Universal RP package, install it from Package Manager)

Please check the Documentation folder for detailed setup instructions and more details.
To use Beautify, add Beautify override to a Post Processing Volume and customize!
Setup video: https://youtu.be/6fpeiysj6KM


License
-------
Beautify is licensed under the Unity Asset Store EULA or Kronnect EULA (depending where you purchased the asset).
Beautify also includes two optional custom ports of tonemap operators (ACES Fitted and AGX) which are licensed under the MIT license:
https://opensource.org/license/mit
Check the BeautifyACESFitted.hlsl and BeautifyAGX.hlsl files respectively. You can clear them if you don't use these tonemappers.


Change Log
----------

Version 24.1
- Depth of field: added "Affects Bloom" option. If enabled, bloom will affect the output of depth of field effect.

Version 24.0
- New effect: film grain
- Outline: added "Outer Only" option for per-object-id technique
- [Fix] Bloom tint can now be used without enabling "Customize" option
- [Fix] The command "Clear Effects" now resets saturate setting

Version 23.0
- Important change: Stripping options have been moved to the Beautify Render Feature inspector improving robustness and preventing conflicts with multiple volume configurations
For details, check https://kronnect.com/guides/build-tips-beautify-urp/ 

Version 22.4
- Eye Adaptation: added metering mode with depth & mask options
- [Fix] Fixed DoF autofocus issue when using multiple cameras

Version 22.2.1
- Added DOTs Instancing support to custom prepasses

Version 22.2
- Outline: added new technique option based on per-object-id prepass
- DoF: added "Resolution Invariant" option

Version 22.1
- Outline: added saturation change detection for improved results
- Optimization Options: added "Ignore Depth Texture". Beautify won't request a depth texture but some effects or options that requires depth data will be unavailable.
- Keywords optimization: outline and edge antialiasing are now mutually exclusive resulting in less variants

Version 22.0
- Added AGX tonemapper
- Depth of field: added max CoC radius setting

Version 21.3
- Improved VR state detection

Version 21.2.1
- [Fix] Fixed an issue when using a combination of multiple URP renderers and stacked cameras

Version 21.2
- Bloom & Anamorphic Flares: new options to include or exclude specific layers
- Outline with exclusion layers now uses depth clipping
- Outline: added new option "Use Optimized Shader"

Version 21.1
- Outline: added CutOff option
- [Fix] Fixed an import issue on Unity 2023.2

Version 21.0
- Outline: added Downscale Blur option which can be disabled to increase outline quality/sharpness
- Outline: added Min Depth Threshold option

Version 20.0
- Added support for Unity 2023.3 Render Graph
- Outline: added "Layer Mask" option (let you add outline over specific opaque objects)

Version 12.0
- Minimum Unity version required is now Unity 2021.3.16
- LUT Browser: added option to specify a custom volume profile
- Automatic stripping of unused effects is now enabled by default

Version 11.5
- Beautify no longer applies default effects if there's no volume in the scene. Instead, a "Quick Settings" button has been added to Beautify inspector so the default settings can be quickly applied if desired.

Version 11.4
- Bloom: added individual tint color options per each layer in best quality mode for improved artistic control when bloom compose option is enabled

Version 11.3
- "Direct Write To Camera" option on Unity 2022.3 now uses URP swap buffers

Version 11.2
- Added "Max Brightness" option to anamorphic flares

Version 11.1
- Added new Compare Mode styles
- Added "Clear Color Buffer" option to render feature. This option is a workaround for a bug in OpenXR and occlusion mesh implementation which causes light leaking when bloom is enabled.

Version 11.0
- Added "Max Visible Depth" option to Night Vision effect
- Improved compatibility with Unity 2023

Version 10.9
- Sun Flares: added "Depth Occlusion Mode". None = no depth buffer checking. Simple = sample depth buffer at Sun position. Smooth = sample 4 positions around Sun position and interpolate value across frames.

Version 10.8
- Added "Conservative" threshold option to bloom and anamorphic flares

Version 10.7.1.1
- [Fix] Fixed bloom / anamorphic flares exclusion layers depth buffer issue

Version 10.7.1
- [Fix] Fixed artifacts on very bright pixels and using dithering
- [Fix] Fixed "Direct Write To Camera" not working in Unity 2021.3.18

Version 10.7
- Depth of Field: new "min/max" distance and "fallback" options
- Anamorphic Flares: spread range adjusted based on screen aspect ratio
- Added "Thermal Vision" distortion amount parameter
- Vignette: added Fit Mode to Circular shape option
- Pixelate: added instructions to inspector

Version 10.6
- Added "Thermal Vision" effect (under Artistic Choices section)
- Added "Keep Source On Top" option to Final Blur effect (useful to create blur edge effect)
- Editor: stripped features now display a label instead of just being hidden

Version 10.5.1
- [Fix] Fixed Unity PPS chromatic aberration being stripped

Version 10.5
- Chromatic Aberration: added "Separate Pass" option
- Bloom & anamorphic exclude layer options now support transparent objects

Version 10.4
- Anamorphic Flares: added Exclusion Mask options
- Bloom & Anamorphic Flares: removed upper cap to threshold parameters
- ACES: added "Max Input Brightness" option to avoid artifacts due to NaN or out of range pixel values
- Added option to demo scene DoF to toggle the effect
- [Fix] Fixed Bloom customize option issue in VR when using multiple cameras

Version 10.3
- Added "Downscale Mode" option
- [Fix] Fixed LUT effect being stripped from build

Version 10.2
- Edge Antialias: added "Max Spread" option

Version 10.1
- Added "Motion Restore Speed". Improved accuracy of motion sensibility.
- Edge Antialias: added "Depth Attenuation". Reduces antialias effect on distance

Version 10.0
- Added "Edge Antialiasing" option
- Frame: added "Cinematic Bands" style
- Bloom: added "Bloom Spread" option
- Bloom: added "Quicker Blur" option
- Bloom: uncapped "Depth Attenuation" limit
- Anamorphic Flares: added "Quicker Blur" option
- Anamorphic Flares: uncapped "Depth Attenuation" limit
- Outline: added "Outline Depth Fade" option (requires "Outline Customize" to be enabled)
- Chromatic Aberration: added "Hue Shift" parameter
- Chromatic Aberration: added CHROMATIC_ABERRATION_ALT shader option (see documentation)
- Depth of field: improved foreground blur effect
- Depth of field: improved bokeh effect in Single Pass Instanced mode
- Added "Camera Layer Mask" to the render feature. This option let you specify which cameras can render Beautify effects
- Volume inspector GUI performance optimizations
- [Fix] Fixed bloom & anamorphic flares not showing in secondary camera on VR setups
- [Fix] Fixes for Unity 2022.2 beta

Version 9.0.1
- Direct Write to Camera option works again (requires Unity 2021.3.3 or later)

Version 9.0
- Added "Ignore Post Processing Option" in Beautify Render Feature so no need to enable Post Processing option in cameras
- Added "Flip Vertically" option to compensate vertical flip in 2D renderer with camera stacking
- [Fix] Sun flares now use the direction set by the assigned Sun transform and not the main directional light
- [Fix] Fixed flipped input image with 2D renderer and camera stacking

Version 8.9
- Added new options to compare mode
- Added LUT 3D texture support and option to import CUBE LUT format

Version 8.8.1
- Change: adjusted opacity of vignette mask plus vignette color alpha now controls overall opacity as well

Version 8.8
- Depth of field: added real camera settings

Version 8.7
- Depth of field: added "Transparent Alpha Test Support" options
- Added "Render Pass Event" option to the Beautify Render Feature inspector

Version 8.6.3
- Final Blur now applies after depth of field
- Added "Double Sided" option to transparent depth of field option
- [Fix] Fixed inspector issue which hides chromatic aberration section when lens dirt feature is stripped

Version 8.6.2
- [Fix] BeautifySettings gameobject is no longer created if Beautify is not being used in the scene when camera post processing is enabled

Version 8.6.1
- [Fix] Fixes to Sun Flares effect in VR

Version 8.6
- Added Frame Pack browser

Version 8.5.1
- LUT browser UI improvements

Version 8.5
- Depth of Field: added Composition option for bokeh
- Added Depth of Field demo scene

Version 8.4
- Added LUT Browser (access it from the Windows menu)

Version 8.3.1
- Beautify cached profiles get updated now automatically when loading new scenes

Version 8.3
- Added "Blur Mask" option to Final Blur effect
- [Fix] Fixed some issues with Unity 2021.2 beta

Version 8.2
- Added new Outline options
- Version number upped to 8.2 to sync with built-in version

Version 2.0
- Added Chromatic Aberration effect
- [Fix] Fixed blink method issue when changing scenes

Version 1.7.3 18/Mar/2021
- DoF: added blur spread option to foreground blur
- [Fix] Fixed depth of field CoC radius calculation issue when using multiple cameras

Version 1.7.2 25/Feb/2021
- [Fix] Fixed Single Pass Stereo/MultiView issues due to Blit bug on XR
- [Fix] Fixed transparent support for Depth of Field not rendering in Editor

Version 1.7.1 8/Feb/2021
- [Fix] Fixed depth of field issue on Android with Unity 2020.2

Version 1.7 24/Jan/2021
- Added support for orthographic camera

Version 1.6
- Added "Vignetting Blink Style" option
- Added "Vignetting Center" option
- Added "Bloom Near Attenuation" option
- Added "Anamorphic Flares Near Attenuation" option
- Added new debug layers to Debug View

Version 1.5
- Added Depth Of Field Transparent Support option

Version 1.4
- Added Sun Flares "Occlusion Layer Mask" option
- Added Sun Flares "Attenuation Speed" (works with Occlusion Layer Mask option)
- [Fix] Fixed an issue that could produce Beautify to use a disabled camera when computing Sun Flares effect

Version 1.3.1 15/NOV/2020
- Improved compatibility with URP 10.1
- [Fix] Fixed an issue that prevents correct shader keyword stripping (ie. cloud build)

Version 1.3 18/OCT/2020
- Added "Bloom Exclusion Mask" option
- Added new demo scene "LUT Blending"
- [Fix] Fixed regression which disabled sharpen in build

Version 1.2.2 23/SEP/2020
- Optimized scriptable render pass initialization

Version 1.2.1 31/AGO/2020
- Support for LUT textures of 256x8 size
- [Fix] Fixed DoF material memory leak

Version 1.2 24/JUL/2020
- Added bloom color tint option under "Customize Bloom" section
- [Fix] Inspector fixes

Version 1.1 28/MAY/2020
- Added "Downscaling" option to Optimizations section

Version 1.0.2 19/MAY/2020
- Added Depth of Field "Distance Shift" parameter

Version 1.0.1 1/MAY/2020
- [Fix] Fixed max clamp values for some sharpen parameters

Version 1.0 April/2020
- Tested on Windows, Mac, Android.
- Added VR Single Pass Stereo support (tested with Oculus Quest)
- Added Beautify and Unity Post Processing build optimization options
- Added Best Performance Mode
- Added Final Blur effect
- Added White Balance color grading option
- Added Night Vision effect
- Added "Direct Write To Camera" option in Performance section


