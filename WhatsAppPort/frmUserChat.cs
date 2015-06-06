using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WhatsAppApi.Account;
using WhatsAppApi.Helper;
using WhatsAppApi.Parser;
using WhatsAppApi.Response;
using System.IO;

namespace WhatsAppPort
{
    public partial class frmUserChat : Form
    {
        //public delegate void StringDelegate(string value);
        //public delegate void ProtocolDelegate(ProtocolTreeNode node);

        //public event StringDelegate MessageSentEvent;
        //public event Action MessageAckEvent;
        //public event ProtocolDelegate MessageRecievedEvent;
        private User user;
        private bool isTyping;

        public frmUserChat(User user)
        {
            InitializeComponent();
            this.user = user;
            this.isTyping = false;
            WhatsEventHandler.MessageRecievedEvent += WhatsEventHandlerOnMessageRecievedEvent;
            WhatsEventHandler.IsTypingEvent += WhatsEventHandlerOnIsTypingEvent;
        }

        private void WhatsEventHandlerOnIsTypingEvent(string @from, bool value)
        {
            if (!this.user.WhatsUser.GetFullJid().Equals(from))
                return;

            this.lblIsTyping.Visible = value;
        }
        private void WhatsEventHandlerOnMessageRecievedEvent(FMessage mess)
        {
            var tmpMes = mess.data;
            this.AddNewText(this.user.UserName, tmpMes);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (this.txtBxSentText.Text.Length == 0)
                return;
            
            WhatSocket.Instance.SendMessage(this.user.WhatsUser.GetFullJid(), txtBxSentText.Text);
            this.AddNewText(this.user.UserName, txtBxSentText.Text);
            txtBxSentText.Clear();
        }

        private void AddNewText(string from, string text)
        {
            this.txtBxChat.AppendText(string.Format("{0}: {1}{2}", from, text, Environment.NewLine));
        }

        private void txtBxSentText_TextChanged(object sender, EventArgs e)
        {
            if (!this.isTyping)
            {
                this.isTyping = true;
                WhatSocket.Instance.SendComposing(this.user.WhatsUser.GetFullJid());
                this.timerTyping.Start();
            }
        }

        private void timerTyping_Tick(object sender, EventArgs e)
        {
            if (this.isTyping)
            {
                this.isTyping = false;
                return;
            }
            WhatSocket.Instance.SendPaused(this.user.WhatsUser.GetFullJid());
            this.timerTyping.Stop();
        }

        private void frmUserChat_Load(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }


        private void btnsendfile_Click(object sender, EventArgs e)
        {
           
    OpenFileDialog dialog = new OpenFileDialog();
    //dialog.Filter = "Text files | *.txt"; // file types, that will be allowed to upload
    dialog.Multiselect = false; // allow/deny user to upload more than one file at a time
    if (dialog.ShowDialog() == DialogResult.OK) // if user clicked OK
    {
        String path = dialog.FileName; // get name of file
        string type = path.Substring(path.Length - 3);
        type = type.ToUpper();
        if (type.Contains("JPG") || type.Contains( "GIF" )|| type.Contains("PNG"))
           
        {
        System.Drawing.Image image = System.Drawing.Image.FromFile(path);
        //yasodha
        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
        byte[] imginbytes = ms.ToArray();
        WhatsAppApi.WhatsApp wa = new WhatsAppApi.WhatsApp();
        //string to = "919177556688";
         WhatsAppApi.WhatsApp.ImageType imp=new  WhatsAppApi.WhatsApp.ImageType();
         WhatSocket.Instance.SendMessageImage(this.user.WhatsUser.GetFullJid(), imginbytes, imp);
        }
        else
        {
            
            byte[] videoinbytes = null;
            FileStream fs = new FileStream(path,
                                           FileMode.Open,
                                           FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(path).Length;
            videoinbytes = br.ReadBytes((int)numBytes);
            WhatsAppApi.WhatsApp.VideoType vid = new WhatsAppApi.WhatsApp.VideoType();
            WhatSocket.Instance.SendMessageVideo(this.user.WhatsUser.GetFullJid(), videoinbytes, vid);
        }
    
}
        }
    }
}
