#region Copyright

//----------------------------------------------------------------------
// Gold Parser engine.
// See more details on http://www.devincook.com/goldparser/
// 
// Original code is written in VB by Devin Cook (GOLDParser@DevinCook.com)
//
// This translation is done by Vladimir Morozov (vmoroz@hotmail.com)
// 
// The translation is based on the other engine translations:
// Delphi engine by Alexandre Rai (riccio@gmx.at)
// C# engine by Marcus Klimstra (klimstra@home.nl)
//----------------------------------------------------------------------

#endregion

#region Using directives

using System;
using System.IO;
using System.Resources;
using System.Globalization;
using System.Reflection;

#endregion

namespace GoldParser
{
    /// <summary>
    /// Custom resource class. Usage:
    /// string s = Res.GetString(Res.MyIdenfitier);
    /// </summary>
    /// <MetaDataID>{682f9c4c-2995-473d-95cd-73a3693c5d42}</MetaDataID>
	internal sealed class SR
	{
		static SR ms_loader = new SR();
		
		private ResourceManager m_resources;

		private SR()
		{
#if DeviceDotNet
            m_resources = new ResourceManager("GoldParser", this.GetType().GetTypeInfo().Assembly);
#else
            m_resources = new ResourceManager("GoldParser", this.GetType().Module.Assembly);
#endif
        }

		/* These function can be useful in other applications.
		 		
		public static string GetString(string name, params object[] args)
		{
			// null CultureInfo: let ResouceManager determine the culture
			return GetString(null, name, args);
		}

		public static string GetString(CultureInfo culture, string name, params object[] args)
		{
			string res = ms_loader.m_resources.GetString(name, culture);

			if (args != null && args.Length > 0)
			{
				return String.Format(culture, res, args);
			}
			else
			{
				return res;
			}
		}
*/
		public static string GetString(string name)
		{
			return GetString(null, name);
		}

		public static string GetString(CultureInfo culture, string name)
		{
			return ms_loader.m_resources.GetString(name, culture);
		}

		// Code below is automatically generated by GenResNm.exe.
		// Do not modify it manually.
#region Resource String Names

		internal const string Grammar_WrongFileHeader = "Grammar_WrongFileHeader";

		internal const string Grammar_InvalidRecordType = "Grammar_InvalidRecordType";

		internal const string Grammar_NoEntry = "Grammar_NoEntry";

		internal const string Grammar_EmptyEntryExpected = "Grammar_EmptyEntryExpected";

		internal const string Grammar_StringEntryExpected = "Grammar_StringEntryExpected";

		internal const string Grammar_IntegerEntryExpected = "Grammar_IntegerEntryExpected";

		internal const string Grammar_ByteEntryExpected = "Grammar_ByteEntryExpected";

		internal const string Grammar_BooleanEntryExpected = "Grammar_BooleanEntryExpected";

		internal const string Grammar_InvalidRecordHeader = "Grammar_InvalidRecordHeader";

#endregion
	}
}

