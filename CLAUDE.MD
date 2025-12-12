# Position Converter - AI Assistant Context

## Project Overview

Position Converter is a freeware Windows desktop application for converting Fanuc TP (Teach Pendant) program points between two coordinate representations:
- **XYZWPR** (Cartesian: X, Y, Z positions with W, P, R rotations)
- **Joint** (Joint angles: J1-J6)

The application parses full Fanuc MD: backup files, including all `.ls` files and configuration data for robot arm types, user tools, and user frames to perform accurate conversions.

## Technology Stack

- **Language**: C# (.NET Framework 4.6.1)
- **UI Framework**: Windows Forms
- **Target Platform**: Windows desktop (WinExe)
- **Key Dependencies**:
  - MathNet.Numerics 4.5.1 (for mathematical computations)
  - System.Windows.Forms
  - System.Drawing

## Project Structure

```
PositionConverter/
├── Forms/                          # Windows Forms UI
│   ├── PositionConverterForm.cs    # Main application form
│   ├── AboutForm.cs                # About dialog
│   ├── RobotInfoForm.cs            # Robot information display
│   ├── RobotDHParamsForm.cs        # Denavit-Hartenberg parameters display
│   └── BoundaryCheckerForm.cs      # Boundary checking functionality
├── FanucRobot.cs                   # Robot model and configuration
├── FanucMath.cs                    # Kinematic calculations and conversions
├── FanucPoint.cs                   # Point data structure and handling
├── FanucProgram.cs                 # TP program parsing and manipulation
├── RobotConfiguration.cs           # Robot config loader and management
├── RobotConfigurations.json        # Robot DH parameters database (user-editable)
├── RobotConfiguration.README.md    # Guide for adding new robot models
├── Program.cs                      # Application entry point
├── Icon/                           # Application icons
├── Images/                         # Robot reference images
└── Instructions/                   # User documentation (PDF)
```

## Core Functionality

### Main Features
1. **Bidirectional Conversion**: Convert between XYZWPR ↔ Joint representations
2. **Batch Processing**: Process entire TP programs at once
3. **Visual Feedback**:
   - Green points: XYZWPR representation
   - Blue points: Joint representation
   - Red points: Failed conversion (with error tooltip)
4. **CSV Export**: Export program points to CSV format
5. **Backup Parsing**: Full MD: backup file parsing for complete robot context

### Supported Robot Models
- R-1000iA/80F, /100F (with/without insulation plate)
- R-2000iA/165F, /200F (with/without insulation plate)
- R-2000iB/125L, /165F, /210F (with/without insulation plate)
- R-2000iC/125L, /165F, /210F (with/without insulation plate)
- Arc Mate 120iC / M20iA
- M710iC series: /12L, /20L, /20M, /45M, /50, /70

## Known Limitations

1. **Accuracy**: Minor differences (hundredths to thousandths of mm) compared to Fanuc controller conversions
2. **Configuration Support**: Only NUT (Non-flip, Upright, Top) and FUT (Flip, Upright, Top) configurations supported
   - Not yet supported: NDT, FDT, NDB, FDB
3. **Multi-Group**: Partial support - only first group gets converted
4. **Not Supported**:
   - Remote TCP (Tool Center Point)
   - Extended Axes
5. **Untested Models**: Several M710iC variants listed as untested

## Key Classes and Their Responsibilities

### FanucRobot
- Stores robot model information
- Loads Denavit-Hartenberg (DH) parameters from configuration file
- Handles user frames (UFRAME) and user tools (UTOOL)
- Provides robot-specific kinematic data

### RobotConfiguration
- Loads and manages robot configurations from JSON file
- Provides pattern-matching logic to identify robot models
- Supports user-editable robot definitions without code changes
- Thread-safe singleton pattern for configuration loading

### FanucMath
- Forward kinematics (Joint → XYZWPR)
- Inverse kinematics (XYZWPR → Joint)
- Matrix transformations
- Angular conversions and calculations
- Uses MathNet.Numerics for linear algebra operations

### FanucPoint
- Represents a single point in either XYZWPR or Joint format
- Stores configuration flags (flip, up, turn)
- Handles point validation and conversion status

### FanucProgram
- Parses Fanuc .ls (List) files
- Extracts points from TP programs
- Manages program metadata and comments

## Build Information

- **Solution**: PositionConverter.sln
- **Target Framework**: .NET Framework 4.6.1
- **Output Type**: Windows Executable (WinExe)
- **Platform**: AnyCPU
- **Code Analysis**: Enabled in Debug builds

### Build Configurations
- **Debug**: Full symbols, no optimization, code analysis enabled
- **Release**: Optimized, PDB only

## Development Guidelines

### When Working with Kinematics
- All angle calculations should account for Fanuc's specific coordinate systems
- DH parameters are robot-model specific - verify against the correct model
- Configuration flags (NUT/FUT) significantly affect inverse kinematics solutions

### When Adding Robot Support
**DH parameters are now configured via JSON file - no code changes needed!**

1. Edit `RobotConfigurations.json` and add a new robot entry with DH parameters
2. Define pattern matching rules (matchPattern, specificPattern, excludePattern)
3. Verify joint limits for the specific model (if needed, update backup parsing)
4. Test with actual backup files from that robot model
5. See `RobotConfiguration.README.md` for detailed instructions

### When Modifying Conversions
- Test accuracy against Fanuc controller conversions
- Verify all configuration types still work
- Check boundary conditions (singularities, joint limits)
- Ensure error handling provides clear user feedback

## Robot Configuration System

### Configuration File (RobotConfigurations.json)
- **Location**: Same directory as executable
- **Format**: JSON with robot definitions and DH parameters
- **Purpose**: Allows users to add robot support without modifying code
- **Pattern Matching**: Uses matchPattern, specificPattern, and excludePattern for robot identification
- **Thread-Safe**: Configuration loaded once and cached in memory
- **Extensible**: Users can add custom robot models by editing the JSON file

### How to Add a New Robot Model
1. Open `RobotConfigurations.json` in a text editor
2. Add a new entry with the robot's name, pattern matching rules, and DH parameters
3. Save the file and restart the application
4. See `RobotConfiguration.README.md` for detailed instructions and examples

### Configuration Classes
- **RobotConfigurationLoader**: Static loader with singleton pattern, pattern matching logic
- **RobotConfig**: Represents a single robot configuration entry
- **DHParameters**: Contains j1LinkA, j2LinkA, j3LinkA, j4LinkD, facePlateThickness

## File Formats

### Input Files (.ls)
- Fanuc TP program files from MD: backup
- Plain text format with Fanuc-specific syntax
- Contains position registers, points, and program logic

### Configuration Files (from MD: backup)
- Robot model specifications
- User frame definitions (UFRAME)
- User tool definitions (UTOOL)
- System variables

### Output Files (.csv)
- Comma-separated values
- Contains program points with all coordinates
- Useful for external analysis or import into other tools

## Testing Considerations

- Test with real Fanuc backup files from various robot models
- Verify conversion accuracy within acceptable tolerances
- Test edge cases: near singularities, joint limits, workspace boundaries
- Validate all supported robot models
- Test both conversion directions (XYZWPR → Joint and Joint → XYZWPR)

## Common Issues and Debugging

- **Red Points**: Conversion failed - check tooltip for specific error
- **Accuracy Differences**: Expected behavior, typically within repeatability limits
- **Unsupported Configuration**: Verify point uses NUT or FUT configuration
- **Parse Errors**: Ensure MD: backup is complete and uncorrupted

## Version History

- V1.1.1.0 - Initial Release
- V1.1.1.1 - Added R2000iA/165 and 200F variants
- V1.1.1.2 - Added R2000iB/125L and R2000iC/125L variants
- V1.1.1.3 - Added CSV output for programs

## Contact

Project maintained by Synaptic Robotics
Email: dwagner@synapticrobotics.com

## Notes for AI Assistants

- This is a production tool used by robotics professionals
- Accuracy and reliability are critical - test all mathematical changes thoroughly
- The codebase uses established robotics conventions (DH parameters, rotation matrices)
- When suggesting changes, consider the impact on existing robot model support
- Performance is important for processing large programs with many points
