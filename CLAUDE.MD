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
- Manages Denavit-Hartenberg (DH) parameters
- Handles user frames (UFRAME) and user tools (UTOOL)
- Provides robot-specific kinematic data

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
1. Add DH parameters in `FanucRobot.cs`
2. Verify joint limits for the specific model
3. Test with actual backup files from that robot model
4. Update supported models list in documentation

### When Modifying Conversions
- Test accuracy against Fanuc controller conversions
- Verify all configuration types still work
- Check boundary conditions (singularities, joint limits)
- Ensure error handling provides clear user feedback

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
