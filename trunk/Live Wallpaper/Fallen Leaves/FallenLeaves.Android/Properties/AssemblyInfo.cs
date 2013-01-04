using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Android.App;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
#if FREE_VERSION
[assembly: AssemblyTitle("FallenLeavesFree")]
#else
[assembly: AssemblyTitle("FallenLeaves")]
#endif
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("DIVARC GROUP")]
[assembly: AssemblyProduct("FallenLeaves")]
[assembly: AssemblyCopyright("Copyright ©  2012")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("25820818-1d23-4870-9c5d-fb9c34f89395")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// Add some common permissions, these can be removed if not needed
//[assembly: UsesPermission(Android.Manifest.Permission.Internet)]
//[assembly: UsesPermission(Android.Manifest.Permission.WriteExternalStorage)]


[assembly: Application(

#if FREE_VERSION
    Label = "Fallen Leaves Free Live Wallpaper",
#else
    Label = "Fallen Leaves Live Wallpaper",
#endif

#if DEBUG 
    Debuggable = true,
#else
    Debuggable = false,
#endif

    Icon = "@drawable/icon"
)]
