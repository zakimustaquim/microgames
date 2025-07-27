# Radial Blur Effect Setup Instructions

This document provides step-by-step instructions to set up the radial blur effect in your Unity project.

## Overview
The radial blur effect starts at the edges of the screen and gradually moves inward as the player moves to the right. The effect intensifies based on how far the player has moved from their starting position.

## Files Created
1. `RadialBlurShader.shader` - Custom shader for the radial blur effect
2. `RadialBlurMaterial.mat` - Material using the custom shader
3. `RadialBlurFeature.cs` - URP Renderer Feature to apply the effect
4. `RadialBlurController.cs` - Controller script to manage blur intensity based on player movement

## Setup Instructions

### Step 1: Configure the Shader Material Reference
1. Select the `RadialBlurMaterial.mat` file in the Project window
2. In the Inspector, change the Shader from "Standard" to "Custom/RadialBlur"
3. The material should now show the radial blur properties

### Step 2: Set up the URP Renderer Feature
1. Navigate to `Assets/Settings/` folder
2. Find your URP Renderer Data asset (usually named something like "UniversalRenderer" or "ForwardRenderer")
3. Select the Renderer Data asset
4. In the Inspector, click "Add Renderer Feature"
5. Select "Radial Blur Feature" from the dropdown
6. In the Radial Blur Feature settings:
   - Set "Render Pass Event" to "Before Rendering Post Processing"
   - Assign the `RadialBlurMaterial` to the "Blur Material" field

### Step 3: Add the Radial Blur Controller to your scene
1. Select your Main Camera in the Hierarchy
2. In the Inspector, click "Add Component"
3. Search for "Radial Blur Controller" and add it
4. Configure the following settings:
   - **Radial Blur Material**: Assign the `RadialBlurMaterial.mat`
   - **Player**: Assign your player GameObject (the one with BoyScript)
   - **Player Camera**: Assign your Main Camera (auto-detected if not set)
   - **Max Blur Strength**: 1.0 (maximum blur intensity)
   - **Blur Radius**: 0.8 (how far from center the blur extends)
   - **Trigger Distance**: 5.0 (distance player must move right before blur starts)
   - **Max Distance**: 20.0 (distance at which blur reaches maximum intensity)
   - **Follow Player**: true (blur center follows player's screen position)
   - **Center X**: 0.5 (used only if Follow Player is false)
   - **Center Y**: 0.5 (used only if Follow Player is false)

### Step 4: Test the Effect
1. Play the scene
2. Move your character to the right using the 'D' key
3. The radial blur effect should gradually increase as you move further right
4. Check the Console for debug messages from the RadialBlurController

## Customization Options

### Adjust Blur Intensity
- Modify `maxBlurStrength` in RadialBlurController to change maximum blur intensity
- Values between 0.5-1.0 usually work best

### Change Blur Center Behavior
- **Follow Player**: When enabled (default), the blur center automatically follows the player's position on screen
- **Manual Center**: When Follow Player is disabled, you can set fixed center coordinates
- Use `SetFollowPlayer(bool)` to toggle this behavior at runtime
- Use `SetBlurCenter(x, y)` to manually override the center (automatically disables Follow Player)

### Modify Trigger Behavior
- `triggerDistance`: How far right the player must move before blur starts
- `maxDistance`: Distance at which blur reaches maximum intensity
- Increase the difference between these values for more gradual blur buildup

### Shader Properties
You can also modify the shader directly for different effects:
- Increase the `samples` variable in the shader for smoother blur (at cost of performance)
- Modify the blur calculation for different blur patterns

## Troubleshooting

### Blur Effect Not Visible
1. Ensure the URP Renderer Feature is properly configured
2. Check that the RadialBlurMaterial is assigned to both the Renderer Feature and the Controller
3. Verify the shader is set to "Custom/RadialBlur" on the material

### Performance Issues
1. Reduce the number of samples in the shader (currently set to 10)
2. Lower the blur radius or strength
3. Consider using the effect only when needed (disable component when not in use)

### Player Not Detected
1. Ensure your player GameObject has the BoyScript component
2. Manually assign the player reference in the RadialBlurController if auto-detection fails

## Advanced Usage

### Dynamic Blur Center
The blur center now automatically follows the player's screen position by default. You can:
- Call `SetFollowPlayer(false)` to use a fixed center
- Call `SetBlurCenter(x, y)` to manually position the center (this also disables follow player)
- Call `SetFollowPlayer(true)` to re-enable player following

### Manual Blur Control
Use `SetBlurStrength(strength)` to manually control blur intensity independent of player movement.

This setup creates a dynamic radial blur effect that enhances the visual experience as the player progresses through your game!
