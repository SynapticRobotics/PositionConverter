Intro:


I've created a freeware tool I would like to share with the community. I've named it Position Converter. Position Converter’s purpose is to convert Fanuc TP program points from XYZWPR representation to Joint representation and vice versa. The program does this by opening a full MD: backup and parsing all .ls files contained along with a number of various files for the robot’s arm type, user tools, and user frames in order to do the conversion properly.


Installation:


Download and extract the attached .zip file into a folder somewhere.
Make sure your computer has .Net 4.6.1 Framework installed.


Program Tips:

    Hovering over a point will display its contents in a tooltip.
    Hovering over a program will display its comment in a tooltip.
    Points are color coded by their point representation. Green for XYZWPR, and blue for Joint.
    If a conversion fails, the point will be colored red, and the tooltip will be updated on why the conversion failed.
    You can view the parsed in robot type, user frames and robot model under the view tab.


Known Issues:

    The math will have slight differences from a conversion done on the Fanuc controller vs a conversion done by this program. Typically the differences are in the order of hundredths to thousandths of a millimeter (far below the repeatability of a typical arm).
    Only NUT or FUT point configurations are supported. I plan on adding support for NDT, FDT, NDB, and FDB at a later date.
    Only the following arms are supported, more to be added in the future:


        R-1000iA/80F with and without insulation plate.
        R-1000iA/100F with and without insulation plate.
        R-2000iA/165F,/200F with and without insulation plate.
        R-2000iB/125L,/165F,/210F with and without insulation plate.
        R-2000iC/125L,/165F,/210F with and without insulation plate.
        Arc Mate 120iC or M20iA
        M710iC/12L
        M710iC/20L - Untested
        M710iC/20M - Untested
        M710iC/45M - Untested
        M710iC/50 - Untested
        M710iC/70 - Untested
    Multiple groups are now kinda supported, but only the first group will get converted.
    Remote TCP is not supported.
    Extended Axes are not supported.


Help Me Out:


This release is pretty much alpha level. As such there will be bugs and crashes. Please let me know if this program works out for you, or if you would like to have a particular arm added. If the program craps out for any reason, please dwagner@synapticrobotics.com the backup so I can fix the issue. I plan on supporting this for a while.


Revision History:

V1.1.1.0 - Initial Release

V1.1.1.1 - Added R2000iA/165 and 200F variants

V1.1.1.2 - Added R2000iB/125L and R2000iC/125L variants

V1.1.1.3 - Added .csv output for programs when 'saving as'. Outputs a .csv of the program points.
