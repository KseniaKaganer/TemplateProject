using UnityEngine;
using System.IO;
using BitMiracle.LibTiff.Classic;


namespace XenLib
{
	
	public class TextureManager
	{

		public static Texture2D GetTextureFromCamera(Camera cam, TextureFormat tf=TextureFormat.RGBA32)
		{
			Texture2D tex = new Texture2D(cam.targetTexture.width, cam.targetTexture.height, tf, false);
			Rect rect = new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height);

			RenderTexture currentRT = RenderTexture.active;
			RenderTexture.active = cam.targetTexture;
			cam.Render();

			tex.ReadPixels(rect, 0, 0);
			tex.Apply();

			return tex;
		}


		public static Texture2D TextureRGBAFloatToTextureRFloat(Texture2D textureRGBA)
		{
			// Transfer Texture2D RGBAFloat to RFloat
			Texture2D textureR = new Texture2D(textureRGBA.width, textureRGBA.height, TextureFormat.RFloat, false);

			textureR.SetPixels(textureRGBA.GetPixels());
			textureR.Apply();

			return textureR;
		}


		public static ushort[] TextureRFloatToUshortArr(Texture2D textureR)
		{
			int tWidth = textureR.width;
			int tHeight = textureR.height;

			ushort[] imgUshortArr = new ushort[tWidth * tHeight];

			for (int x = 0; x < tWidth; ++x)
			{
				for (int y = 0; y < tHeight; ++y)
				{
					imgUshortArr[y * tWidth + x] = (ushort)(textureR.GetPixel(x, y).r * 100);
				}
			}

			return imgUshortArr;
		}


		public static bool Save16BitGrayScaleToTiff(ushort[] arrImageBuffer, uint uiImageWidth, uint uiImageHeight, string sFilePathName)
		{
			bool bSaveResult = false;

			try
			{
				using (Tiff output = Tiff.Open(sFilePathName, "w"))
				{
					// Set Tiff Image Fields
					output.SetField(TiffTag.IMAGEWIDTH, uiImageWidth);
					output.SetField(TiffTag.IMAGELENGTH, uiImageHeight);
					output.SetField(TiffTag.SAMPLESPERPIXEL, 1);
					output.SetField(TiffTag.BITSPERSAMPLE, 16);
					output.SetField(TiffTag.ORIENTATION, Orientation.BOTLEFT);//Orientation.TOPLEFT);
					output.SetField(TiffTag.ROWSPERSTRIP, uiImageHeight);
					output.SetField(TiffTag.XRESOLUTION, 88.0);  // Placeholder, with some DPI (Pixel per ResolutionUnit)
					output.SetField(TiffTag.YRESOLUTION, 88.0);  // Placeholder, with some DPI (Pixel per ResolutionUnit)
					output.SetField(TiffTag.RESOLUTIONUNIT, ResUnit.CENTIMETER);
					output.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);
					output.SetField(TiffTag.PHOTOMETRIC, Photometric.MINISBLACK);
					output.SetField(TiffTag.COMPRESSION, Compression.NONE);
					output.SetField(TiffTag.FILLORDER, FillOrder.MSB2LSB);

					byte[] row_buffer = new byte[uiImageWidth * sizeof(ushort)];

					for (int iRowIndex = 0; iRowIndex < uiImageHeight; ++iRowIndex)
					{
						// Copy current row data to BYTE array
						System.Buffer.BlockCopy(arrImageBuffer, (int)(iRowIndex * uiImageWidth * sizeof(ushort)), row_buffer, 0, row_buffer.Length);
						output.WriteScanline(row_buffer, iRowIndex);
					}

					bSaveResult = true; // All done :)
					output.Close();
				}
			}
			catch (System.Exception)
			{
				bSaveResult = false;
			}

			return bSaveResult;
		}

		public static void SaveTextureToFile(Texture2D tex, string imageFormat, string filePathName)
		{

			switch (imageFormat)
			{
			case "png":
				filePathName += ".png";
				File.WriteAllBytes(filePathName, tex.EncodeToPNG());
				break;
			case "jpg":
				filePathName += ".jpg";
				File.WriteAllBytes(filePathName, tex.EncodeToJPG(70));
				break;
			case "tif":
				// Texture2D textureR = TextureRGBAFloatToTextureRFloat(tex);
				ushort[] imgUshortArr = TextureRFloatToUshortArr(tex);
				//Destroy(textureR);
				filePathName += ".tif";
				Save16BitGrayScaleToTiff(imgUshortArr, (uint)tex.width, (uint)tex.height, filePathName);
				break;
			default:
				break;
			}

		}


	}



}