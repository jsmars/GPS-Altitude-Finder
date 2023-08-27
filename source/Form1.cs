using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataSearcher
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			if (folderBrowser.ShowDialog() == DialogResult.OK)
				txtPath.Text = folderBrowser.SelectedPath;
		}

		private void btnSearch_Click(object sender, EventArgs e)
		{
			if (txtPath.Text.Length == 0)
			{
				Log("No valid path chosen");
				return;
			}
			var files = Directory.GetFiles(txtPath.Text, txtFilter.Text);

			var data = new Dictionary<string, List<string>>();
			var dataNumeric = new Dictionary<string, List<double>>();
			var total = new List<double>();
			int lineCount = 0;

			foreach (var file in files)
			{
				var filename = Path.GetFileName(file);
				var list = data[filename] = new List<string>();
				var listNumeric = dataNumeric[filename] = new List<double>();

				var lines = File.ReadAllLines(file);
				lineCount += lines.Length;

				foreach (var line in lines)
				{
					var index = line.IndexOf(txtInput.Text);
					if (index >= 0)
					{ 
						var start = index + txtInput.Text.Length;
						var end = line.IndexOf(txtRight.Text, start);
						var dataStr = end < 0 ? line.Substring(start) : line.Substring(start, end - start);
						list.Add(dataStr);
						if (double.TryParse(dataStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var val))
						{
							listNumeric.Add(val);
							total.Add(val);
						}
					}
				}
			}

			//foreach (var list in dataNumeric)
			//	list.Value.Sort();
			//total.Sort();

			Log($"Files searched: {files.Length}");
			Log($"Lines searched: {lineCount}");

			if (total.Count == 0)
			{
				Log("No numeric data found in lines");
				return;
			}

			Log($"Min: {total.Min()} Max: {total.Max()} Average: {total.Average()}");

			double max = 0;
			string maxFile = "";
			var listDiff = new Dictionary<string, double>();
			var minusFiles = new List<string>();
			foreach (var list in dataNumeric)
			{
				var diff = list.Value.Max() - list.Value.Min();
				listDiff[list.Key] = diff;
				if (diff > max)
				{
					maxFile = list.Key;
					max = diff;
				}
				if (list.Value.Any(x => x < 0))
					minusFiles.Add(list.Key);
			}

			var sortedDict = from entry in listDiff orderby entry.Value descending select entry;

			Log($"Max diff: {max} for {maxFile} ({(max * 10).ToString("0.0")} meters)");

			foreach (var item in sortedDict)
			{
				var list = dataNumeric[item.Key];
				Log($"{item.Key} Diff: {(item.Value * 10).ToString("0.00")} meters.    Min: {list.Min()} Max: {list.Max()} Average: {list.Average()} Diff: {list.Max() - list.Min()} {(minusFiles.Contains(item.Key) ? "Contains NEGATIVE altitude! Recording started in air. Check manually!" : "")}");
			}

			//foreach (var list in dataNumeric)
			//	Log($"{list.Key} Min: {list.Value.Min()} Max: {list.Value.Max()} Average: {list.Value.Average()} Diff: {list.Value.Max() - list.Value.Min()}");
		}

		void Log(string text)
		{
			txtOutput.Text += text + "\r\n";
		}

		private void btnHelp_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Info: " +
				"\nThis application was mainly made to search through a folder of GPS files created for DJI FPV and calculating the max altitude of the clips, but could in theory be used for other stuff as well.It will search though all files and show you the ranges of altitude values in the files.If I've understood things correctly, the altitude data in the DJI FPV is based on barometer data and might not be completely accurate, and also it will show the value divided by 10, and this is compensated for. In some cases the altitude is displayed as a negative value as well. The displayed height is the difference between the min and max altitude values multiplied by 10, so if you start or land in the video, and the barometer is working correctly, it should be fairly accurate, but check manually if you are unsure." +
				"\n\nInstructions:" +
				"\nBrowse to your directory and press the search button." +
				"\n\nHope this is helpful! " +
				"\n\nCheck out this project on github for any fixes etc." +
				"\nCreated by Jonathan Smårs. jsmars.com") ;
		}

		private void btnGithub_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("https://github.com/jsmars/GPS-Altitude-Finder/");
		}
	}
}
