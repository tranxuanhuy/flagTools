using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace testForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            progressBar1.Value = 100;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", "proxy.txt");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", "links.txt");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 30;
            SelenFirefox.BuildCredenticalXmlFileForFirefox();
            progressBar1.Value = 100;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 30;
            SelenFirefox.ChangeAnnotation(textBox1.Text);
            progressBar1.Value = 100;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            backgroundWorker3.RunWorkerAsync();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                Application.Exit();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Process.Start("winword.exe", "sample.xml");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            backgroundWorker2.RunWorkerAsync();
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 30));
            label3.Invoke((System.Windows.Forms.MethodInvoker)(() => label3.Text = "initializing"));
            SelenFirefox.FlagSocksEscortModuleWithPR(label3, progressBar2);
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 100));
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 30));
            SelenFirefox.FlagProxyModuleWithPR();
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 100));
        }

        private void button10_Click(object sender, EventArgs e)
        {
            backgroundWorker4.RunWorkerAsync();
        }

        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 30));
            SelenFirefox.FlagProxySystemNoPR(label4);
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 100));
        }

        private void button11_Click(object sender, EventArgs e)
        {
            SelenFirefox.GetCityOfSock();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", "sockWithCity.txt");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            backgroundWorker5.RunWorkerAsync();
        }

        private void backgroundWorker5_DoWork(object sender, DoWorkEventArgs e)
        {
            SelenFirefox.changeStatusInputIP(label3, progressBar2);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            SelenFirefox.Flag1Link();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", "socks.txt");
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", "survivalLinks.txt");
        }

        private void button17_Click(object sender, EventArgs e)
        {
            SelenFirefox.CheckLinkDie(label3);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            SelenFirefox.GetCityOfProxy();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", "proxyWithCity.txt");
        }

        private void button21_Click(object sender, EventArgs e)
        {
            backgroundWorker6.RunWorkerAsync();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", "proxyWithCityHttpRequest.txt");
        }

        private void button23_Click(object sender, EventArgs e)
        {
            SelenFirefox.GetDns();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", "dnslist.txt");
        }

        private void backgroundWorker6_DoWork(object sender, DoWorkEventArgs e)
        {
            SelenFirefox.GetCityOfProxyHttpRequest(label3);
        }

        private void button24_Click(object sender, EventArgs e)
        {
            backgroundWorker7.RunWorkerAsync();
        }

        private void backgroundWorker7_DoWork(object sender, DoWorkEventArgs e)
        {
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 30));
            SelenFirefox.FlagSockModuleNoPR(label4);
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 100));
        }

        private void backgroundWorker8_DoWork(object sender, DoWorkEventArgs e)
        {
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 30));
            SelenFirefox.CheckLinkDie(label3);
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 100));
        }

        private void button25_Click(object sender, EventArgs e)
        {
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 30));
            SelenFirefox.testModule(null, label3, radioButton1, radioButton2, groupBox2);
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 100));
        }

        private void button26_Click(object sender, EventArgs e)
        {
            SelenFirefox.FlagProxyModuleNoPR2(textBox2.Text);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button27_Click(object sender, EventArgs e)
        {
            backgroundWorker9.RunWorkerAsync();
        }

        private void backgroundWorker9_DoWork(object sender, DoWorkEventArgs e)
        {
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 30));
            SelenFirefox.FlagProxyFFNoPR(label4);
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 100));
        }

        private void button28_Click(object sender, EventArgs e)
        {
            backgroundWorker10.RunWorkerAsync();
        }

        private void button29_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 30;
            SelenFirefox.ClickConfirmationLinkInGmailTesting();
            progressBar1.Value = 100;
        }

        private void button30_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 30;
            SelenFirefox.GetZipcodeOfProxyByHttpRequest();
            progressBar1.Value = 100;
        }

        private void backgroundWorker10_DoWork(object sender, DoWorkEventArgs e)
        {
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 30));
            SelenFirefox.PostingProxyModuleNoPR(label4, textBox3.Text, textBox4.Text, textBox5.Text);
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 100));
        }

        private void button33_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", "adsLoginput.txt");
        }

        private void button31_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", "adsLive.txt");
        }

        private void button32_Click(object sender, EventArgs e)
        {
            SelenFirefox.CheckAdsLive(label3);
        }

        private void button34_Click(object sender, EventArgs e)
        {
            backgroundWorker11.RunWorkerAsync();
        }

        private void backgroundWorker11_DoWork(object sender, DoWorkEventArgs e)
        {
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 30));
            SelenFirefox.fakePRSoftware(label4);
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 100));
        }

        private void button35_Click(object sender, EventArgs e)
        {
            backgroundWorker12.RunWorkerAsync();
        }

        private void backgroundWorker12_DoWork(object sender, DoWorkEventArgs e)
        {
            SelenFirefox.flagHttprequest(label3);
        }

        private void button36_Click(object sender, EventArgs e)
        {
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 30));
            SelenFirefox.GetRandomAds(null);
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 100));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button37_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", "sockWithCityHttpRequest.txt");
        }

        private void backgroundWorker13_DoWork(object sender, DoWorkEventArgs e)
        {
            SelenFirefox.GetCityOfSockHttpRequest(label3);
        }

        private void button38_Click(object sender, EventArgs e)
        {
            backgroundWorker13.RunWorkerAsync();
        }

        private void button39_Click(object sender, EventArgs e)
        {
            backgroundWorker14.RunWorkerAsync();
        }

        private void backgroundWorker14_DoWork(object sender, DoWorkEventArgs e)
        {
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 30));
            SelenFirefox.NearestSocksCity(null, label3);
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 100));
        }

        private void button40_Click(object sender, EventArgs e)
        {
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 30));
            SelenFirefox.configFakePRSoftware(label4);
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 100));
        }

        private void button41_Click(object sender, EventArgs e)
        {
            backgroundWorker15.RunWorkerAsync();
        }

        private void backgroundWorker15_DoWork(object sender, DoWorkEventArgs e)
        {
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 30));
            SelenFirefox.sameState5sock(null, label3);
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 100));
        }

        private void button42_Click(object sender, EventArgs e)
        {
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 30));
            SelenFirefox.Nearest5SocksCityStep2(label3);
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 100));
        }

        private void button43_Click(object sender, EventArgs e)
        {
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 30));
            SelenFirefox.getlinkCL(textBox6.Text);
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 100));
        }

        private void button44_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", "sockWithCityHttpRequest.txt");
        }

        private void button45_Click(object sender, EventArgs e)
        {
            SelenFirefox.checkAnonymousSock(label3);
        }

        private void button46_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", "proxyWithCityHttpRequest.txt");
        }

        private void button47_Click(object sender, EventArgs e)
        {
            SelenFirefox.checkAnonymousProxy(label3);
        }

        private void button48_Click(object sender, EventArgs e)
        {
            SelenFirefox.forwardMail(label4, textBox3.Text, textBox4.Text, textBox5.Text);
        }

        private void button49_Click(object sender, EventArgs e)
        {
            SelenFirefox.createGmail(textBox6.Text);
        }

        private void button50_Click(object sender, EventArgs e)
        {
            textBox6.Text = SelenFirefox.getTimeNowVietnam() + "";
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button51_Click(object sender, EventArgs e)
        {
            SelenFirefox.getTrueAds(label3);
        }

        private void button52_Click(object sender, EventArgs e)
        {
            SelenFirefox.PostTutorAndCreatePvasAndForwardmail(label4, textBox3.Text, textBox4.Text, textBox5.Text, radioButton1, radioButton4, groupBox2);
        }

        private void button53_Click(object sender, EventArgs e)
        {
            SelenFirefox.PostTutorOnly(label4, textBox3.Text, textBox4.Text, textBox5.Text);
        }

        private void button54_Click(object sender, EventArgs e)
        {
            SelenFirefox.SeoProxyFFNoPR(label4);
        }

        private void button55_Click(object sender, EventArgs e)
        {
            SelenFirefox.SeoProxySystemNoPR(label4);
        }

        private void button56_Click(object sender, EventArgs e)
        {
            SelenFirefox.PostCanada(label4, textBox3.Text, textBox4.Text, textBox5.Text);
        }

        private void button57_Click(object sender, EventArgs e)
        {
            SelenFirefox.titleToLink(textBox6.Text);
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 30));
            progressBar1.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar1.Value = 100));
        }

        private void button58_Click(object sender, EventArgs e)
        {
            backgroundWorker16.RunWorkerAsync();
        }

        private void backgroundWorker16_DoWork(object sender, DoWorkEventArgs e)
        {
            SelenFirefox.PostTutorOutlookMulti(label4, textBox3.Text, textBox4.Text, textBox5.Text, radioButton1, radioButton2);
        }

        private void backgroundWorker17_DoWork(object sender, DoWorkEventArgs e)
        {
            SelenFirefox.PostCanadaOutlookMulti(label4, textBox3.Text, textBox4.Text, textBox5.Text, radioButton1, radioButton2);
        }

        private void button59_Click(object sender, EventArgs e)
        {
            backgroundWorker17.RunWorkerAsync();
        }

        private void button60_Click(object sender, EventArgs e)
        {
            SelenFirefox.sameState5sock(null, label3);
        }

        private void button61_Click(object sender, EventArgs e)
        {
            SelenFirefox.ChangePassPVAMulti(label4, textBox3.Text, textBox4.Text, textBox5.Text, radioButton1, radioButton2);
        }

        private void button62_Click(object sender, EventArgs e)
        {
            backgroundWorker18.RunWorkerAsync();
        }

        private void backgroundWorker18_DoWork(object sender, DoWorkEventArgs e)
        {
            SelenFirefox.IEPostTutorOutlookMulti(label4, textBox3.Text, textBox4.Text, textBox5.Text, radioButton1, radioButton2);
        }

        private void button63_Click(object sender, EventArgs e)
        {
            backgroundWorker19.RunWorkerAsync();
        }

        private void backgroundWorker19_DoWork(object sender, DoWorkEventArgs e)
        {
            SelenFirefox.PostandCreatePva(label4, textBox3.Text, textBox4.Text, textBox5.Text, radioButton1, radioButton4, groupBox2);
        }

        private void button64_Click(object sender, EventArgs e)
        {
            SelenFirefox.CreateOutlookAlias(label4, textBox3.Text, textBox4.Text, textBox5.Text, radioButton1, radioButton2);
        }

        private void button65_Click(object sender, EventArgs e)
        {
            backgroundWorker20.RunWorkerAsync();
        }

        private void backgroundWorker20_DoWork(object sender, DoWorkEventArgs e)
        {
            SelenFirefox.PostMultiChrome(label4, textBox3.Text, textBox4.Text, textBox5.Text, radioButton1, radioButton4, groupBox2);
        }
    }
}
