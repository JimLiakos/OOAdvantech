using System.Reflection;
using System.Runtime.CompilerServices;

//
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//
[assembly: AssemblyTitle("")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("")]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]		

// 
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:


#if Net4
[assembly: AssemblyVersion("4.0.0.0")]
[assembly: AssemblyFileVersion("4.0.0.0")]
#else
[assembly: AssemblyVersion("1.0.2.0")]
[assembly: AssemblyFileVersion("1.0.2.0")]
#endif

//
// In order to sign your assembly you must specify a key to use. Refer to the 
// Microsoft .NET Framework documentation for more information on assembly signing.
//
// Use the attributes below to control which key is used for signing. 
//
// Notes: 
//   (*) If no key is specified, the assembly is not signed.
//   (*) KeyName refers to a key that has been installed in the Crypto Service
//       Provider (CSP) on your machine. KeyFile refers to a file which contains
//       a key.
//   (*) If the KeyFile and the KeyName values are both specified, the 
//       following processing occurs:
//       (1) If the KeyName can be found in the CSP, that key is used.
//       (2) If the KeyName does not exist and the KeyFile does exist, the key 
//           in the KeyFile is installed into the CSP and used.
//   (*) In order to create a KeyFile, you can use the sn.exe (Strong Name) utility.
//       When specifying the KeyFile, the location of the KeyFile should be
//       relative to the project output directory which is
//       %Project Directory%\obj\<configuration>. For example, if your KeyFile is
//       located in the project directory, you would specify the AssemblyKeyFile 
//       attribute as [assembly: AssemblyKeyFile("..\\..\\mykey.snk")]
//   (*) Delay Signing is an advanced option - see the Microsoft .NET Framework
//       documentation for more information on this.
// 
[assembly: AssemblyDelaySign(false)]
//[assembly: AssemblyKeyFile("")]
//[assembly: AssemblyKeyFile("..\\..\\StrongName.snk")]
[assembly: AssemblyKeyName("")]
[assembly: OOAdvantech.MetaDataRepository.BuildAssemblyMetadata]
[assembly: ModulePublisher.AddGAC]
#if DEBUG
//[assembly: ModulePublisher.Encrypt] 
#else
[assembly: ModulePublisher.Encrypt]
#endif



#if Net4
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Linq, PublicKey=0024000004800000940000000602000000240000525341310004000001000100370b04adef82ed771e4d38c14432deb0e10e380522ef755a55e494d9098e145e44801cd6d7bd36a0cc7b1bf4b86fd0eba2cdd904be6837277e51b7517e3db65d011ccbd0bf08966a406e394fad25e1be1402120479b6e54a9718613803c4e04099414115ec1f6b8e1a5f20a38714350b49c3fa410444dbc59083dd8f559ee1d8")]// 002400000480000094000000060200000024000052534131000400000100010055D2D257286A75F264669621BA4A5F8B6D9903F303A18A29EB373F40E4A124E3C4BBDB93CD535BD304389008117DD194B18E3343E908AA9E1B8047327C2E11153C2EBB6122E48A6721B257D296F2DAC0B94FADDCD1B6B8CA5F9213605065F2EF7BC472E6620663F1E74A07AB5B78BA5573A9D55380A9D54DF243F7DCB3E88ACE
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("PersistenceLayerRunTime, PublicKey=00240000048000009400000006020000002400005253413100040000010001004505377dd713410720c9993d4b5e3d17b5bb0b51c15593eb0c228da366f5f2fcf439f4f3200907ef50259882ea3cb4169fc55e56df1030fd7ad8f45b9225b3b496169c5aec3a99a20bf80d459d7dc09daeec38f4dce98334486f2397ceca9988c0f09d9665b45874262b8f433c47672eb771af1212e3bb7afa1da2ad7b49f9a9")]
#else

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Linq, PublicKey=0024000004800000940000000602000000240000525341310004000001000100370b04adef82ed771e4d38c14432deb0e10e380522ef755a55e494d9098e145e44801cd6d7bd36a0cc7b1bf4b86fd0eba2cdd904be6837277e51b7517e3db65d011ccbd0bf08966a406e394fad25e1be1402120479b6e54a9718613803c4e04099414115ec1f6b8e1a5f20a38714350b49c3fa410444dbc59083dd8f559ee1d8")]// 002400000480000094000000060200000024000052534131000400000100010055D2D257286A75F264669621BA4A5F8B6D9903F303A18A29EB373F40E4A124E3C4BBDB93CD535BD304389008117DD194B18E3343E908AA9E1B8047327C2E11153C2EBB6122E48A6721B257D296F2DAC0B94FADDCD1B6B8CA5F9213605065F2EF7BC472E6620663F1E74A07AB5B78BA5573A9D55380A9D54DF243F7DCB3E88ACE
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("PersistenceLayerRunTime, PublicKey=00240000048000009400000006020000002400005253413100040000010001004505377dd713410720c9993d4b5e3d17b5bb0b51c15593eb0c228da366f5f2fcf439f4f3200907ef50259882ea3cb4169fc55e56df1030fd7ad8f45b9225b3b496169c5aec3a99a20bf80d459d7dc09daeec38f4dce98334486f2397ceca9988c0f09d9665b45874262b8f433c47672eb771af1212e3bb7afa1da2ad7b49f9a9")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Linq, PublicKey=002400000480000094000000060200000024000052534131000400000100010055D2D257286A75F264669621BA4A5F8B6D9903F303A18A29EB373F40E4A124E3C4BBDB93CD535BD304389008117DD194B18E3343E908AA9E1B8047327C2E11153C2EBB6122E48A6721B257D296F2DAC0B94FADDCD1B6B8CA5F9213605065F2EF7BC472E6620663F1E74A07AB5B78BA5573A9D55380A9D54DF243F7DCB3E88ACE")]// 002400000480000094000000060200000024000052534131000400000100010055D2D257286A75F264669621BA4A5F8B6D9903F303A18A29EB373F40E4A124E3C4BBDB93CD535BD304389008117DD194B18E3343E908AA9E1B8047327C2E11153C2EBB6122E48A6721B257D296F2DAC0B94FADDCD1B6B8CA5F9213605065F2EF7BC472E6620663F1E74A07AB5B78BA5573A9D55380A9D54DF243F7DCB3E88ACE
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("PersistenceLayerRunTime, PublicKey=00240000048000009400000006020000002400005253413100040000010001004965CD71296DFA0C243BBF28794838C379942339B5C6F3C06537B7DA801907B2850082871AF91D70E19DAE680F90D72F8994E83EBDC7F7A67C9D53AE1CA4D6EDCE5E53663EF9C423AC68D163CA2BF8FD2CD4C8158C4C876DB8E8B5C5462B558EE96247174C1C70B81155D9A4E862F5E92358DED99CF60865D20FA9742EB7B4D7")]

#endif

//00240000048000009400000006020000002400005253413100040000010001000FE26277A329637E06B744A64398D6C0C8CD92530AD76269C889DD5F53FB8AD4BB3442F0B6C068BA07C33DC4A2EE13599C4299FC66EFD74B1B1F76CEB0D153E5FC3E08616EFCE2AA554C5DFE103D369B1AA5180DE862E1F678B08018315E36C92727F8BEA0E94760D8071D66718CF7DEE003A4434FA5BFBBF74225FF0C0BC6CB



