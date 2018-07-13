using System;
using System.IO;
using System.Text;



namespace XenLib
{
	public class FileWriter
	{
		static StreamWriter writer;

		public static void WriteToFile(string filePath, string content, bool append=false)
		{
			if (File.Exists(filePath) == false)
			{
				throw new System.ArgumentException("File does not exists");
			}

			writer = new StreamWriter(filePath, append);
			writer.Write(content);
			writer.Close();
		}

		public static void AppendStringToFile(string filePath, string content)
		{
			if (File.Exists(filePath) == false)
			{
				throw new System.ArgumentException("File does not exists");
			}

			writer = new StreamWriter(filePath, true);
			writer.WriteLine(content);
			writer.Close();
		}

	}

	public class FileReader
	{

		public static string FileToString(string filePath)
		{
			if (File.Exists(filePath) == false)
			{
				throw new System.ArgumentException("File does not exists");
			}

			string fileContent = "";

			//Stream reader to read the file.
			StreamReader fileReader = new StreamReader(filePath, Encoding.UTF8);
			string line;
				
			try
			{
				do{
					line = fileReader.ReadLine();
					fileContent += line + "\n";
				}while(line != null);
					
				fileReader.Close();
			}
			catch (Exception e)
			{
				throw new System.Exception (e.Message);
			}

			return fileContent;
		}


		public static string[] SplitFileLinesToArr(string filePath)
		{
			string fileContent = FileToString(filePath);
			fileContent = fileContent.TrimEnd('\n', '\r');

			return fileContent.Split('\n');
		}



	}





}
