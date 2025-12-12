# Robot Configuration Guide

## Overview

Position Converter now uses a configuration file (`RobotConfigurations.json`) to define robot DH parameters instead of hard-coding them. This allows users to easily add support for additional robot models without modifying the source code.

## Configuration File Location

The `RobotConfigurations.json` file must be located in the same directory as the Position Converter executable (`PositionConverter.exe`).

## Configuration File Format

The configuration file uses JSON format with the following structure:

```json
{
  "robots": [
    {
      "name": "Robot Display Name",
      "matchPattern": "Primary pattern to match",
      "specificPattern": "Specific variant pattern",
      "excludePattern": "Pattern to exclude (optional)",
      "dhParameters": {
        "j1LinkA": 320.0,
        "j2LinkA": 870.0,
        "j3LinkA": 225.0,
        "j4LinkD": 1015.0,
        "facePlateThickness": 175.0
      }
    }
  ]
}
```

### Field Descriptions

- **name**: A descriptive name for the robot configuration (used for documentation)
- **matchPattern**: Primary string pattern to match against the robot type from the backup file
- **specificPattern**: Additional pattern for specific variants (optional)
- **excludePattern**: Pattern that, if present, will exclude this configuration (optional)
- **dhParameters**: The Denavit-Hartenberg parameters for this robot
  - **j1LinkA**: Link length A for joint 1 (mm)
  - **j2LinkA**: Link length A for joint 2 (mm)
  - **j3LinkA**: Link length A for joint 3 (mm)
  - **j4LinkD**: Link distance D for joint 4 (mm)
  - **facePlateThickness**: Thickness of the face plate (mm)

## How Pattern Matching Works

When Position Converter reads a robot type from a Fanuc backup file, it searches the configuration file for a matching robot entry using the following logic:

1. The robot type string must contain the `matchPattern`
2. If `specificPattern` is defined, the robot type must also contain it
3. If `excludePattern` is defined, the robot type must NOT contain it
4. Configurations are evaluated from most specific to least specific (based on pattern length)

### Examples

**Example 1: Basic Match**
```json
{
  "name": "ARC Mate 120iC",
  "matchPattern": "ARC Mate 120iC",
  "dhParameters": { ... }
}
```
This matches any robot type containing "ARC Mate 120iC"

**Example 2: Specific Variant with Exclusion**
```json
{
  "name": "R-1000iA/80F",
  "matchPattern": "R-1000",
  "specificPattern": "80F",
  "excludePattern": "IF",
  "dhParameters": { ... }
}
```
This matches robot types containing both "R-1000" AND "80F" but NOT "IF"
- Matches: "R-1000iA/80F"
- Does NOT match: "R-1000iA/80F-IF" (contains "IF")
- Does NOT match: "R-1000iA/100F" (doesn't contain "80F")

**Example 3: Variant with Insulation**
```json
{
  "name": "R-1000iA/80F-IF",
  "matchPattern": "R-1000",
  "specificPattern": "80F-IF",
  "dhParameters": { ... }
}
```
This matches robot types containing both "R-1000" AND "80F-IF"
- Matches: "R-1000iA/80F-IF"

## Adding a New Robot Model

To add support for a new robot model:

1. **Obtain DH Parameters**: Get the Denavit-Hartenberg parameters for your robot model
   - You can find these in Fanuc documentation, robot manuals, or by measuring the robot
   - You need: j1LinkA, j2LinkA, j3LinkA, j4LinkD, and facePlateThickness (all in millimeters)

2. **Edit RobotConfigurations.json**: Open the file in a text editor

3. **Add New Entry**: Add a new robot configuration entry to the "robots" array
   ```json
   {
     "name": "R-2000iC/270F",
     "matchPattern": "R-2000iC",
     "specificPattern": "270F",
     "excludePattern": "IF",
     "dhParameters": {
       "j1LinkA": 312.0,
       "j2LinkA": 1075.0,
       "j3LinkA": 225.0,
       "j4LinkD": 1280.0,
       "facePlateThickness": 240.0
     }
   }
   ```

4. **Save the File**: Save the changes (ensure valid JSON format)

5. **Test**: Load a backup file for that robot model and verify the conversions work correctly

## Validating Your Configuration

Ensure your JSON file is valid:
- All string values must be in double quotes
- Numbers should NOT have quotes
- Don't forget commas between array elements
- Make sure all brackets and braces are properly closed

You can use an online JSON validator (like jsonlint.com) to check your file before using it.

## Troubleshooting

### "Robot configuration file not found"
- Ensure `RobotConfigurations.json` is in the same directory as `PositionConverter.exe`
- Check the file name is exactly `RobotConfigurations.json` (case-sensitive on some systems)

### "Unsupported Fanuc Robot Type"
- The robot type from your backup doesn't match any configuration
- Check the robot type string in your backup's `version.dg` file
- Add a new configuration entry that matches this robot type
- The error message will include the robot type string it's trying to match

### "Failed to load robot configuration"
- The JSON file may have syntax errors
- Validate your JSON using an online validator
- Check for missing commas, quotes, or brackets
- Ensure all numeric values are valid numbers

### "Robot configuration file is empty or invalid"
- The JSON file exists but couldn't be parsed
- Verify the JSON structure matches the format shown above
- Ensure the root object has a "robots" array

## Backup and Safety

Before editing the configuration file:
1. Make a backup copy of `RobotConfigurations.json`
2. Test your changes with a known robot backup file
3. Keep the original configuration file as a reference

## Technical Details

- The configuration file is loaded once when the application first needs it (lazy loading)
- The configuration is cached in memory for performance
- To reload after changes, restart the application
- Pattern matching is case-sensitive
- Configurations are matched from most specific to least specific

## Example: Full Configuration Entry

Here's a complete example with all optional fields:

```json
{
  "name": "R-2000iB/210F with Insulation",
  "matchPattern": "R-2000iB",
  "specificPattern": "210F-IF",
  "dhParameters": {
    "j1LinkA": 312.0,
    "j2LinkA": 1075.0,
    "j3LinkA": 225.0,
    "j4LinkD": 1280.0,
    "facePlateThickness": 258.0
  }
}
```

## Getting Help

If you encounter issues:
1. Check this README for troubleshooting steps
2. Validate your JSON file format
3. Verify the DH parameters are correct for your robot model
4. Contact dwagner@synapticrobotics.com with:
   - The robot type from your backup file
   - The DH parameters you're trying to use
   - Any error messages you receive
