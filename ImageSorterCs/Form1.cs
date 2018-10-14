using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageSorterCs
{
    public partial class ImageSorter : Form
    {
        string mnemonics = "123456789qwertyuiop";
        string root;
        public int fileIndex = 0;
        List<string> dirs;
        public ImageSorter()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        
        }

        private void buttonDemo()
        {

            System.Windows.Forms.Button button;
            button = new folderButton("c:\\test\\", pictureBox1,"1",this);
            flowLayoutPanel1.Controls.Add(button);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            root = textBox1.Text;
            dirs = new List<string>(System.IO.Directory.EnumerateDirectories(root));
            int index = 0;
            foreach (string dir in dirs)
            {
                System.Windows.Forms.Button button;
                button = new folderButton(dir, pictureBox1, mnemonics[index].ToString(),this);
                flowLayoutPanel1.Controls.Add(button);
                index++;
            }
            actionButton nextButton = new actionButton("+", actionButton.NEXT,this);
            actionButton prevButton = new actionButton("0", actionButton.PREVIOUS,this);
            flowLayoutPanel1.Controls.Add(nextButton);
            flowLayoutPanel1.Controls.Add(prevButton);
            fileIndex = 0;
            loadImage();
        }

        public void loadImage()
        {
            List<string> files = System.IO.Directory.EnumerateFiles(root).ToList<String>();
            fileIndex = (fileIndex < files.Count ? (fileIndex>0?fileIndex:files.Count-1) : 0);
            string newImage = files[fileIndex];
            pictureBox1.ImageLocation = newImage;

        }
    }

    class folderButton : Button
    {
        public PictureBox PicBox;
        public string Path;
        public string FolderName;
        private ImageSorter parentForm;
        public folderButton(string folderPath, PictureBox picBox, string i, ImageSorter f)
        {
            this.Location = new System.Drawing.Point(3, 3);
            this.Name = "folderButton";
            this.Size = new System.Drawing.Size(150, 22);
            this.TabIndex = 1;
            this.UseVisualStyleBackColor = true;
            this.Click += new System.EventHandler(this.moveFile);
            this.Path = folderPath;
            string[] folderParts = folderPath.Split('\\');
            FolderName = folderParts[folderParts.Length - 1];
            this.Text = "&" + i + ") " + FolderName;
            this.PicBox = picBox;
            this.parentForm = f;
        }
        public void moveFile(object sender, EventArgs e)
        {
            System.IO.Directory.Move(PicBox.ImageLocation,Path+"\\"+(System.IO.Path.GetFileName(PicBox.ImageLocation)));
            parentForm.loadImage();
        }
    }

    class actionButton : Button
    {
        public int Mode = 0;
        public static int NEXT = 1;
        public static int PREVIOUS = 2;
        private ImageSorter formRef;

        public actionButton(string mnemonic, int mode, ImageSorter f)
        {
            this.Location = new System.Drawing.Point(3, 3);
            this.Name = "folderButton";
            this.Size = new System.Drawing.Size(150, 22);
            this.TabIndex = 1;
            this.UseVisualStyleBackColor = true;
            this.Click += new System.EventHandler(this.doAction);
            this.Text = "&"+mnemonic+")"+(mode==1?">":(mode==2?"<":"wut"));
            Mode = mode;
            formRef = f;
        }
        public void doAction(object sender, EventArgs e)
        {
            if (Mode == actionButton.NEXT)
            {
                formRef.fileIndex++;
                formRef.loadImage();
            }
            else if (Mode == actionButton.PREVIOUS)
            {
                formRef.fileIndex--;
                formRef.loadImage();
            }
        }
    }

}
