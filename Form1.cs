using System.Net.Sockets;
using System.Text;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly TextBox _textBox;
        private readonly Button _button;
        private readonly TextBox _resultTextBox;

        public Form1()
        {
            InitializeComponent();

            _textBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 10),
                Width = 200
            };

            _button = new Button
            {
                Text = "Get Streets",
                Location = new System.Drawing.Point(220, 10),
                Width = 100
            };
            _button.Click += Button_Click;

            _resultTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 40),
                Width = 310,
                Height = 200,
                Multiline = true
            };

            Controls.Add(_textBox);
            Controls.Add(_button);
            Controls.Add(_resultTextBox);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            string request = _textBox.Text;
            Task.Run(() => SendRequest(request));
        }

        private void SendRequest(string request)
        {
            try
            {
                TcpClient client = new TcpClient("127.0.0.1", 8888);
                NetworkStream stream = client.GetStream();
                byte[] buffer = Encoding.UTF8.GetBytes(request);
                stream.Write(buffer, 0, buffer.Length);

                buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                _resultTextBox.Invoke((MethodInvoker)(() => _resultTextBox.Text = response));

                client.Close();
            }
            catch (Exception ex)
            {
                _resultTextBox.Invoke((MethodInvoker)(() => _resultTextBox.Text = $"Error: {ex.Message}"));
            }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new ClientForm());
        }
    }
}
