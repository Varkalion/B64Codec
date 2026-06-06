using System;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace B64Codec
{
    public class MainForm : Form
    {
        private TextBox txtInput;
        private TextBox txtOutput;
        private Button btnEncode;
        private Button btnDecode;
        private Button btnCopy;
        private Timer resetColorTimer;

        public MainForm()
        {
            // Main Window Settings
            this.Text = "B64Codec - Base64 Converter";
            this.Size = new Size(500, 420);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 10);

            // Input Textbox & Label
            txtInput = new TextBox { Multiline = true, Size = new Size(460, 100), Location = new Point(12, 30), ScrollBars = ScrollBars.Vertical };
            Label lblInput = new Label { Text = "Input Text / Base64:", Location = new Point(12, 9), Size = new Size(150, 20) };
            txtInput.KeyDown += Txt_KeyDown;

            // Encode Button
            btnEncode = new Button { Text = "Encode to Base64 ➔", Location = new Point(12, 145), Size = new Size(220, 35), BackColor = Color.LightBlue };
            btnEncode.Click += BtnEncode_Click;

            // Decode Button
            btnDecode = new Button { Text = "➔ Decode Base64", Location = new Point(252, 145), Size = new Size(220, 35), BackColor = Color.LightGreen };
            btnDecode.Click += BtnDecode_Click;

            // Output Textbox & Label
            txtOutput = new TextBox { Multiline = true, Size = new Size(460, 130), Location = new Point(12, 210), ScrollBars = ScrollBars.Vertical, ReadOnly = true };
            Label lblOutput = new Label { Text = "Output Result:", Location = new Point(12, 187), Size = new Size(150, 20) };
            txtOutput.KeyDown += Txt_KeyDown;

            // Copy Button
            btnCopy = new Button { Text = "📋 Copy Output", Location = new Point(12, 345), Size = new Size(460, 30), BackColor = Color.LightGray };
            btnCopy.Click += BtnCopy_Click;

            // Color Reset Timer for Copy Button
            resetColorTimer = new Timer();
            resetColorTimer.Interval = 1000; // 1 second
            resetColorTimer.Tick += (s, e) => {
                btnCopy.BackColor = Color.LightGray;
                btnCopy.Text = "📋 Copy Output";
                resetColorTimer.Stop();
            };

            // Add Controls to Form
            this.Controls.AddRange(new Control[] { lblInput, txtInput, btnEncode, btnDecode, lblOutput, txtOutput, btnCopy });
        }

        // Handle Visual Feedback on Copy Click
        private void BtnCopy_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtOutput.Text))
            {
                Clipboard.SetText(txtOutput.Text);
                
                btnCopy.BackColor = Color.LimeGreen;
                btnCopy.Text = "✓ Copied to Clipboard!";
                resetColorTimer.Start();
            }
        }

        // Enable Ctrl+A Shortcut for ReadOnly Textbox
        private void Txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                ((TextBox)sender).SelectAll();
                e.SuppressKeyPress = true; // Prevents Windows beep sound
            }
        }

        // Encoding Logic
        private void BtnEncode_Click(object sender, EventArgs e)
        {
            try {
                if (string.IsNullOrEmpty(txtInput.Text)) return;
                txtOutput.Text = Convert.ToBase64String(Encoding.UTF8.GetBytes(txtInput.Text));
            } catch (Exception ex) {
                MessageBox.Show("Encoding Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Decoding Logic
        private void BtnDecode_Click(object sender, EventArgs e)
        {
            try {
                if (string.IsNullOrEmpty(txtInput.Text)) return;
                txtOutput.Text = Encoding.UTF8.GetString(Convert.FromBase64String(txtInput.Text.Trim()));
            } catch {
                MessageBox.Show("Invalid Base64 string format!", "Decoding Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
