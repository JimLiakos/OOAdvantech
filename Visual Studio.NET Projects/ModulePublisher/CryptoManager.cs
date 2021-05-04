using System;
using System.Security.Cryptography;
using System.IO;
namespace CryptoSafea
{
	public enum CryptoEventType
	{
		Message,
		FileProgress
	}

	public class CryptoEventArgs: EventArgs
	{
		CryptoEventType type;
		string message;
		int fileLength;
		int filePosition;

		public CryptoEventArgs(string message)
		{
			type = CryptoEventType.Message;
			this.message = message;
		}
		public CryptoEventArgs(string fileName, int fileLength, int filePosition)
		{
			type = CryptoEventType.FileProgress;
			this.fileLength = fileLength;
			this.filePosition = filePosition;
			message = fileName;
		}
		public CryptoEventType Type
		{
			get {return type;}
		}
		public string Message
		{
			get {return message;}
		}
		public string FileName
		{
			get {return message;}
		}
		public int FileLength
		{
			get {return fileLength;}
		}
		public int FilePosition
		{
			get {return filePosition;}
		}
	}
	public class CryptoAssembly
	{
		static CryptoManager  CryptoManager =new CryptoManager(); 
		public static void EncryptAssembly(string fileName,string password)
		{
			if(!System.IO.File.Exists(fileName))
				throw new System.Exception("Assembly file '"+fileName+"' does not exist");
			byte [] cryptoKey  = null;
			byte []cryptoIV = null;
			CryptoManager.getKeysFromPassword(password,out cryptoKey, out cryptoIV);
			CryptoManager.EncryptData(fileName,fileName+".enc",cryptoKey,cryptoIV);
			System.IO.File.Copy(fileName+".enc",fileName,true);
			System.IO.File.Delete(fileName+".enc");	
		}
		public static System.Reflection.Assembly Load(string fileName,string password)
		{
			if(!System.IO.File.Exists(fileName))
				throw new System.Exception("Assembly file '"+fileName+"' does not exist");
			byte [] cryptoKey  = null;
			byte []cryptoIV = null;
			CryptoManager.getKeysFromPassword(password,out cryptoKey, out cryptoIV);
			System.IO.MemoryStream memoryStream=new MemoryStream();

			CryptoManager.DecryptData(fileName,memoryStream,cryptoKey,cryptoIV);
			System.IO.FileInfo fileInfo  = new System.IO.FileInfo(fileName);
			string DebugInfo=fileInfo.FullName;
			DebugInfo=DebugInfo.Replace(fileInfo.Extension,"");
			DebugInfo+=".pdb";
			System.IO.FileInfo DebugfileInfo = new System.IO.FileInfo(DebugInfo);
			if(DebugfileInfo.Exists)
			{
				System.IO.FileStream DebugInfoStream=DebugfileInfo.OpenRead();
				
				byte[] buffer=new byte[DebugInfoStream.Length];
				DebugInfoStream.Read(buffer,0,(int)DebugInfoStream.Length);
				DebugInfoStream.Close();
				return System.AppDomain.CurrentDomain.Load(memoryStream.GetBuffer(),buffer);
			}

			return System.Reflection.Assembly.Load(memoryStream.GetBuffer());

		}
	}
	public delegate void cryptoEventHandler(object sender, CryptoEventArgs e);
	/// <summary>
	/// 
	/// </summary>
	public class CryptoManager
	{
		byte [] testHeader = null;//used to verify if decryption succeeded 

		string testHeaderString = null;
		
		public CryptoManager()
		{
			testHeader = System.Text.Encoding.ASCII.GetBytes("testing header");
			testHeaderString = BitConverter.ToString(testHeader);
		}


		public event cryptoEventHandler progressMessage = null;
		private void fireMessage(string msg)
		{
			if (progressMessage != null)
				progressMessage(this, new CryptoEventArgs(msg));
		}
		private void fireMessage(string fileName, int fileLength, int filePosition)
		{
			if (progressMessage != null)
				progressMessage(this, new CryptoEventArgs(fileName, fileLength,filePosition));
		}

		#region key encrypt
		public void getKeysFromPassword(string pass, out byte[] rijnKey, out byte[] rijnIV)
		{
			byte[] salt = System.Text.Encoding.ASCII.GetBytes("System.Text.Encoding.ASCII.GetBytes");
			PasswordDeriveBytes pb = new PasswordDeriveBytes(pass,salt);
			rijnKey = pb.GetBytes(32);
			rijnIV = pb.GetBytes(16);
		}

		
		#endregion
		#region Rijndael encrypt
		const int bufLen = 4096;
		public  void EncryptData(String inName, String outName, byte[] rijnKey, byte[] rijnIV)
		{    
			FileStream fin = null;
			FileStream fout = null;
			CryptoStream encStream = null;

			try
			{
				//Create the file streams to handle the input and output files.
				fin = new FileStream(inName, FileMode.Open, FileAccess.Read);
				fout = new FileStream(outName, FileMode.Create, FileAccess.Write);
       
				//Create variables to help with read and write.
				byte[] bin = new byte[bufLen]; //This is intermediate storage for the encryption.
				long rdlen = 0;              //This is the total number of bytes written.
				long totlen = fin.Length;    //This is the total length of the input file.
				int len;                     //This is the number of bytes to be written at a time.
 
				RijndaelManaged rijn = new RijndaelManaged();
				//SymmetricAlgorithm rijn = SymmetricAlgorithm.Create(); //Creates the default implementation, which is RijndaelManaged.         
				encStream = new CryptoStream(fout, rijn.CreateEncryptor(rijnKey, rijnIV), CryptoStreamMode.Write);
                
				fireMessage("Rijndael encrypting...");

				//zakoduj testowy fragment
				encStream.Write(testHeader,0,testHeader.Length);

				//Read from the input file, then encrypt and write to the output file.
				while(true)
				{
					len = fin.Read(bin, 0, bufLen);
					if (len == 0)
						break;
					encStream.Write(bin, 0, len);
					rdlen += len;
					fireMessage(inName,(int)totlen,(int)rdlen);
				}
				fireMessage(string.Format("{0} - {1} bytes processed", inName ,rdlen));
			}
			finally
			{
				if (encStream != null)
					encStream.Close();  
				if (fout != null)
					fout.Close();
				if (fin != null)
					fin.Close();                   
			}
		}

		#endregion
		#region Rijndael decrypt
		public bool DecryptData(String inName, String outName, byte[] rijnKey, byte[] rijnIV)
		{    
			//Create the file streams to handle the input and output files.
			FileStream fin = null;
			FileStream fout = null;
			CryptoStream decStream = null;
			
			try
			{
				fin = new FileStream(inName, FileMode.Open, FileAccess.Read);
       
				//Create variables to help with read and write.
				byte[] bin = new byte[bufLen]; //This is intermediate storage for the encryption.
				long rdlen = 0;              //This is the total number of bytes written.
				long totlen = fin.Length;    //This is the total length of the input file.
				int len;                     //This is the number of bytes to be written at a time.
 
				RijndaelManaged rijn = new RijndaelManaged();
				
				decStream = new CryptoStream(fin, rijn.CreateDecryptor(rijnKey, rijnIV), CryptoStreamMode.Read);

				fireMessage("Rijndael decrypting...");
 
				//odkoduj testowy fragment
				byte[]test = new byte[testHeader.Length];
				decStream.Read(test,0,testHeader.Length);
				if (BitConverter.ToString(test) != testHeaderString)
				{
					decStream.Clear();
					decStream = null;
					fireMessage("error while decrypting file "+inName);
					return false;
				}

				//create output file
				fout = new FileStream(outName, FileMode.Create, FileAccess.Write);

				//Read from the encrypted file and write dercypted data
				while(true)
				{
					len = decStream.Read(bin,0,bufLen);
					if (len == 0)
						break;
					fout.Write(bin,0,len);
					rdlen += len;
					fireMessage(inName,(int)totlen,(int)rdlen);
				}
				fireMessage(string.Format("{0} - {1} bytes processed", inName ,rdlen));

				return true;
			}
			finally
			{
				if (decStream != null)
					decStream.Close();
				if (fout != null)
					fout.Close();
				if (fin != null)
					fin.Close();                   
			}
		}

		public bool DecryptData(String inName, System.IO.MemoryStream outStream, byte[] rijnKey, byte[] rijnIV)
		{    
			//Create the file streams to handle the input and output files.
			FileStream fin = null;
			//System.IO.Stream fout = null;
			CryptoStream decStream = null;
			
			try
			{
				fin = new FileStream(inName, FileMode.Open, FileAccess.Read);
       
				//Create variables to help with read and write.
				byte[] bin = new byte[bufLen]; //This is intermediate storage for the encryption.
				long rdlen = 0;              //This is the total number of bytes written.
				long totlen = fin.Length;    //This is the total length of the input file.
				int len;                     //This is the number of bytes to be written at a time.
 
				RijndaelManaged rijn = new RijndaelManaged();
				
				decStream = new CryptoStream(fin, rijn.CreateDecryptor(rijnKey, rijnIV), CryptoStreamMode.Read);

				fireMessage("Rijndael decrypting...");
 
				//odkoduj testowy fragment
				byte[]test = new byte[testHeader.Length];
				decStream.Read(test,0,testHeader.Length);
				if (BitConverter.ToString(test) != testHeaderString)
				{
					decStream.Clear();
					decStream = null;
					fireMessage("error while decrypting file "+inName);
					return false;
				}

				//create output file
				//fout = new FileStream(outName, FileMode.Create, FileAccess.Write);

				//Read from the encrypted file and write dercypted data
				while(true)
				{
					len = decStream.Read(bin,0,bufLen);
					if (len == 0)
						break;
					outStream.Write(bin,0,len);
					rdlen += len;
					fireMessage(inName,(int)totlen,(int)rdlen);
				}
				fireMessage(string.Format("{0} - {1} bytes processed", inName ,rdlen));

				return true;
			}
			finally
			{
				if (decStream != null)
					decStream.Close();
				//if (fout != null)
					//fout.Close();
				if (fin != null)
					fin.Close();                   
			}
		}

		#endregion
	}
}
