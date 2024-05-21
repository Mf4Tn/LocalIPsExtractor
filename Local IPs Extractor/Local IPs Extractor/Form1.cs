using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

namespace Local_IPs_Extractor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string local_ip = "";
        public List<string> ips = new List<string>();
        private void button1_Click(object sender, EventArgs e)
        {
            string sub_ip = local_ip.Split('.')[0] + '.' + local_ip.Split('.')[1] + '.';
            int d = 0;
            for(int i = 0; i < 255; i++)
            {
                for(int j = 0; j < 255; j++)
                {
                    string ip = sub_ip + i.ToString() + '.' + j.ToString();
                    ips.Add(ip);
                    Task.Run(() =>
                    {
                        try
                        {
                            d++; // threading counter
                            TaskCompletionSource<bool> res = new TaskCompletionSource<bool>();
                            if(d > 50)
                            {
                                res.SetResult(true);
                                d = 0;
                            }
                            Ping ping = new Ping();
                            PingReply reply = new Ping().Send(ip);
                            //MessageBox.Show(ip);
                            textBox2.Invoke(new MethodInvoker(delegate
                            {
                                textBox2.Text = "Checking : "+ip;
                            }));
                            if (reply.Status == IPStatus.Success)
                            {
                                listView1.Invoke(new MethodInvoker(delegate
                                {
                                    //MessageBox.Show("valid ip" + ip);
                                    listView1.Items.Add(ip);
                                    
                                }));

                            }
                            else
                            {
                            }
                            
                        }
                        catch
                        {

                        }
                    });
                    

                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] addr = ipEntry.AddressList;
            foreach(IPAddress ip in addr)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) { textBox1.Text = "My Local IP: " + ip.ToString(); local_ip = ip.ToString(); }
                
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
