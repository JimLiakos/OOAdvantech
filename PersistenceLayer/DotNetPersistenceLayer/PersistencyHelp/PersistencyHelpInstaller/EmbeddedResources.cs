using System;

namespace PersistencyHelpInstaller
{
	using System;
	using System.IO;
	using System.Reflection;

    /// <summary>Utilties to help reading and writing embedded resources.</summary>
    /// <remarks>This is used to access the stylesheets.</remarks>
    /// <MetaDataID>{c24740c8-97d0-41f4-8aa7-4baa51cf55e2}</MetaDataID>
	public class EmbeddedResources
	{

		/// <summary>Writes all the embedded resources with the specified prefix to disk.</summary>
		/// <param name="assembly">The assembly containing the embedded resources.</param>
		/// <param name="prefix">The prefix to search for.</param>
		/// <param name="directory">The directory to write the resources to.</param>
		public static void WriteEmbeddedResources(
			Assembly assembly,
			string prefix,
			string directory)
		{
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			string[] names = assembly.GetManifestResourceNames();

			foreach (string name in names)
			{
				if (name.StartsWith(prefix))
				{
					WriteEmbeddedResource(
						assembly,
						name,
						directory,
						name.Substring(prefix.Length + 1));
				}
			}
		}

		/// <summary>Writes an embedded resource to disk.</summary>
		/// <param name="assembly">The assembly containing the embedded resource.</param>
		/// <param name="name">The name of the embedded resource.</param>
		/// <param name="directory">The directory to write the resource to.</param>
		/// <param name="filename">The filename of the resource on disk.</param>
		public static void WriteEmbeddedResource(
			Assembly assembly,
			string name,
			string directory,
			string filename)
		{
			const int size = 512;
			byte[] buffer = new byte[size];
			int count = 0;

			Stream input = assembly.GetManifestResourceStream(name);
			Stream output = File.Open(Path.Combine(directory, filename), FileMode.Create);

			try
			{
				while ((count = input.Read(buffer, 0, size)) > 0)
				{
					output.Write(buffer, 0, count);
				}
			}
			finally
			{
				output.Close();
			}
		}
	}

}
