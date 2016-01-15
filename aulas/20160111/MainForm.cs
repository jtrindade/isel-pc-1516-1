using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            PrepareForOldStyle();
        }

        // GUI control thread blocked.
        private void butDoWrong_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = true;
            txtResult.Text = "working...";
            prgFeedback.Value = 0;
            Refresh();
            for (int i = 0; i < 5; ++i) {
                Thread.Sleep(2000);
                prgFeedback.Value = (i + 1) * 20;
            }
            txtResult.Text = "DONE";
        }

        // Illegal access from secondary thread.
        private void butDoItIllegal_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            txtResult.Text = "working...";
            prgFeedback.Value = 0;
            ThreadPool.QueueUserWorkItem((_) =>
            {
                for (int i = 0; i < 5; ++i)
                {
                    Thread.Sleep(2000);
                    prgFeedback.Value = (i + 1) * 20;
                }
                txtResult.Text = "DONE";
            });
        }

        // Updates UI in the right thread.
        private void butDoItRaw_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = true;
            txtResult.Text = "working...";
            prgFeedback.Value = 0;
            Control mainCtx = txtResult;
            ThreadPool.QueueUserWorkItem((_) =>
            {
                for (int i = 0; i < 5; ++i)
                {
                    Thread.Sleep(2000);
                    int percentage = (i + 1) * 20;
                    mainCtx.BeginInvoke((Action)(() => {
                        prgFeedback.Value = percentage;
                    }));
                }
                mainCtx.BeginInvoke((Action)(() => {
                    txtResult.Text = "DONE";
                }));
            });
        }

        private void PrepareForOldStyle()
        {
            bgwWorker.DoWork += (src, args) =>
            {
                for (int i = 0; i < 5; ++i)
                {
                    Thread.Sleep(2000);
                    bgwWorker.ReportProgress((i + 1) * 20);
                }
                args.Result = "DONE";
            };
            bgwWorker.ProgressChanged += (src, args) =>
            {
                prgFeedback.Value = args.ProgressPercentage;
            };
            bgwWorker.RunWorkerCompleted += (src, args) =>
            {
                txtResult.Text = (string)args.Result;
            };
        }

        // Uses a BackgroundWorker
        private void butDoOldStyle_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = true;
            txtResult.Text = "working...";
            prgFeedback.Value = 0;
            bgwWorker.RunWorkerAsync();
        }

        // Using TPL
        private async void butDoItNow_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = true;
            txtResult.Text = "working...";
            prgFeedback.Value = 0;
            // SynchronizationContext.SetSynchronizationContext(null);
            txtResult.Text = await WorkAsync(new Progress<int>((val) => { prgFeedback.Value = val; }));
        }

        private Task<string> WorkAsync(IProgress<int> prog) {
            return Task.Run(() => {
                for (int i = 0; i < 5; ++i)
                {
                    Thread.Sleep(2000);
                    prog.Report((i + 1) * 20);
                }
                return "DONE";
            });
        }

        // Back to the original code (corrected).
        private async void _butDoItNow_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = true;
            txtResult.Text = "working...";
            prgFeedback.Value = 0;
            for (int i = 0; i < 5; ++i)
            {
                await Task.Delay(2000);
                prgFeedback.Value = (i + 1) * 20;
            }
            txtResult.Text = "DONE";
        }
    }
}
