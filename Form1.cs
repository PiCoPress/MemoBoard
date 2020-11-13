using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemoBoard
{
    public partial class MemoBoard : Form
    {
        public void DialogErr(String content)
        {
            MessageBox.Show(content, "Error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static String PATH = Application.LocalUserAppDataPath+"\\";
        bool init = false;
        const int SPT = 30;
        Label beforeLabel = null;
        public static Dictionary<String, Label> li = Program.li;


        Color defaultBackColor = Color.SkyBlue;
        Color clickedBackColor = Color.Ivory;


        public MemoBoard()
        {
            try
            {
                InitializeComponent();
                if (!Directory.Exists(PATH) || (Directory.GetFiles(PATH).Length == 0))
                {
                    Directory.CreateDirectory(PATH);
                    init = true;
                    TextBox tmp = new TextBox();
                    tmp.Text = "My Memo was empty.";
                    tmp.ReadOnly = true;
                    tmp.Multiline = true;
                    tmp.Width = 500;
                    tmp.Height = 500;
                    tmp.ForeColor = Color.FromArgb(100, 100, 100);
                    tabler.Controls.Add(tmp, 0, 0);
                }
                else
                {
                    int a = 0;
                    int b = 0;
                    int col = tabler.ColumnCount;
                    int row = tabler.RowCount;
                    if (row == b) tabler.RowCount++;
                    String[] arr = Directory.GetFiles(PATH);

                    foreach (String str in arr)
                    {
                        Label tb = new Label();
                        tb.BackColor = defaultBackColor;
                        tb.Width = 300;
                        tb.BorderStyle = BorderStyle.FixedSingle;
                        tb.Height = 300;
                        //tb.Enabled = false;
                        int leng = arr.Length;
                        tb.DoubleClick += Tb_Click;
                        tb.Click += OnceCl;
                        String val = File.ReadAllText(str);
                        tb.Text = SPT > val.Length ? val : val.Substring(0, SPT);
                        tb.Tag = int.Parse(str.Split('\\')[str.Split('\\').Length - 1]);
                        tabler.Controls.Add(tb, 0, 0);
                        li.Add(tb.Tag.ToString(), tb);
                    }
                    void Tb_Click(object sender1, EventArgs e1)
                    {
                        try
                        {
                            Label casted = (Label)sender1;
                            String fff = casted.Tag.ToString();
                            TextArea fi = new TextArea(File.ReadAllText(PATH + fff), fff);
                            casted.BackColor = defaultBackColor;
                            fi.Show();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Critical Error : File not found.\n" + ex.Message, "Error occured.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Close();
                        }
                    }
                    void OnceCl(object sender1, EventArgs ev)
                    {
                        Label casted = (Label)sender1;
                        if (beforeLabel == null)
                        {
                            casted.BackColor = clickedBackColor;
                            beforeLabel = casted;
                        }
                        else
                        {
                            beforeLabel.BackColor = defaultBackColor;
                            casted.BackColor = clickedBackColor;
                            beforeLabel = casted;
                        }
                    }
                }
            }catch (Exception err)
            {
                DialogErr("Program cannot be run.");
                Console.WriteLine(err);
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(init)
            {
                tabler.Controls.Clear();
                init = false;
            }
            Label tb = new Label();
            tb.Text = "";
            tb.BackColor = defaultBackColor;
            tb.BorderStyle = BorderStyle.FixedSingle;
            tb.Width = 300;
            tb.Height = 300;
            int leng = Directory.GetFiles(PATH).Length;
            tb.DoubleClick += Tb_Click;
            tb.Click += once;
            tb.Tag = leng + 1;
            TextArea ta = null;
            try
            {
                li.Add(tb.Tag.ToString(), tb);
                ta = new TextArea("", tb.Tag);
            }
            catch(Exception ex)
            {
                int ii = 0;
                while(li.ContainsKey(ii.ToString())) ++ii;
                li.Add(ii.ToString(), tb);
                ta = new TextArea("", ii);
            }
            ta.Show();
            void once(object rr, EventArgs ar)
            {
                Label casted = (Label)rr;
                if (beforeLabel == null)
                {
                    casted.BackColor = clickedBackColor;
                    beforeLabel = casted;
                }
                else
                {
                    beforeLabel.BackColor = defaultBackColor;
                    casted.BackColor = clickedBackColor;
                    beforeLabel = casted;
                }
            }
            void Tb_Click(object sender1, EventArgs e1)
            {
                try
                {
                    Label casted = (Label)sender1;
                    String fff = casted.Tag.ToString();
                    casted.BackColor = defaultBackColor;
                    TextArea fi = new TextArea(File.ReadAllText(PATH + ((Label)sender1).Tag), ((Label)sender1).Tag);
                    fi.Show();
                }catch (Exception ex)
                {
                    MessageBox.Show("Critical Error : File not found.\n" + ex.Message, "Error occured.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
        }
        
        public void Send(String tt, String str)
        {
            li[tt].Text = SPT > str.Length ? str : str.Substring(0, SPT);
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(this.button1, "New Memo");
        }

        private void MemoBoard_ResizeEnd(object sender, EventArgs e)
        {
            Control tl = (Control)sender;
            //tl.Width = this.Width - 110;
            Console.WriteLine(this.Height + ", " + this.Width);
        }
    }
}
