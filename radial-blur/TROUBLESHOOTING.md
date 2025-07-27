# Radial Blur Troubleshooting Guide

## Issue: White Ball Effect

If you're seeing a white ball instead of the expected radial blur effect, here are the steps to diagnose and fix the issue:

### Step 1: Enable Debug Mode
1. Select your GameObject with the `RadialBlurController` component
2. In the Inspector, check the "Debug Mode" checkbox
3. Set "Debug Blur Strength" to 0.5
4. Play the scene - you should see a consistent blur effect

### Step 2: Verify Setup Checklist

#### Material Setup:
- [ ] Material shader is set to "Custom/RadialBlur"
- [ ] Material has the correct property values (check in Inspector)
- [ ] _BlurStrength should be > 0 when debug mode is enabled

#### URP Renderer Feature:
- [ ] RadialBlurFeature is added to your URP Renderer Data
- [ ] The feature has the RadialBlurMaterial assigned
- [ ] Render Pass Event is set to "Before Rendering Post Processing"

#### Component Setup:
- [ ] RadialBlurController is on your camera or in the scene
- [ ] Radial Blur Material field is assigned
- [ ] Player field is assigned (or auto-detected)
- [ ] Player Camera field is assigned (or auto-detected)

### Step 3: Check Console Logs
Look for these log messages:
- "Found player: [PlayerName]"
- "Using main camera: [CameraName]"
- "Initialized radial blur material properties"
- "Debug mode: blur strength set to X.XX"

### Step 4: Common Issues and Solutions

#### White Ball Issue:
- **Cause**: Shader is sampling incorrectly or material properties aren't set
- **Solution**: 
  1. Verify the shader compiled without errors
  2. Check that _MainTex is being passed correctly by the renderer feature
  3. Ensure blur strength > 0

#### No Effect Visible:
- **Cause**: Renderer feature not properly configured
- **Solution**:
  1. Check URP Asset has the correct Renderer Data
  2. Verify Renderer Feature is enabled and configured
  3. Make sure material is assigned to the feature

#### Effect Too Subtle:
- **Cause**: Blur strength or radius too low
- **Solution**:
  1. Increase "Debug Blur Strength" to 0.8-1.0
  2. Increase "Blur Radius" in the controller
  3. Adjust shader sample count if needed

### Step 5: Manual Testing
If debug mode works but the normal mode doesn't:
1. Disable debug mode
2. Move your player character to the right beyond the trigger distance
3. Check console for movement logs
4. Verify blur strength is being calculated correctly

### Advanced Debugging
If the issue persists, check:
1. **Shader Compilation**: Look for shader errors in Console
2. **Render Pipeline**: Ensure you're using URP (not Built-in or HDRP)
3. **Camera Stack**: If using camera stacking, ensure the effect is on the base camera
4. **Post-Processing**: Disable other post-processing effects temporarily to isolate issues

### Quick Fix Checklist
1. Set Debug Mode = true, Debug Blur Strength = 0.5
2. Verify shader is "Custom/RadialBlur"
3. Check URP Renderer Feature setup
4. Look at console logs for errors

If none of these steps resolve the issue, there may be a compatibility issue with your specific Unity version or URP setup.
