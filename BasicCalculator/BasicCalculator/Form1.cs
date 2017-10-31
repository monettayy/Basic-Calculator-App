#region Needed Namespaces
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
#endregion

namespace BasicCalculator
{
    public partial class Form1 : Form
    {
        #region Fields || Properties || Ctors
        List<Button> NumberButtons = new List<Button>();
        List<Button> OtherButtons = new List<Button>();
        List<string> num = new List<string>();
        Label lblEquation = new Label();
        Label lblResult = new Label();

        Font FontForAll = new Font("Lucida Sans Typewriter", 20);
        Font FontForResult = new Font("Lucida Sans Typewriter", 25);
        Panel Panel1 = new Panel();
        Panel Panel2 = new Panel();
        bool isLastOp = true;
        int lastindex;

        public Form1()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void Form1_Load(object sender, EventArgs e)
        {
            ShowScreen();
            GenerateNumberButton();
            GenerateOtherButton();
        }

        private void ButtonHover(object sender, EventArgs e)
        {
            BackToBasic(NumberButtons);
            BackToBasic(OtherButtons);
            Button b = (Button)sender;
            b.BackColor = Color.DarkTurquoise;
        }

        private void ButtonRemoveHover(object sender, EventArgs e)
        {
            BackToBasic(NumberButtons);
            BackToBasic(OtherButtons);
        }

        private void lblEquation_TextChanged(object sender, EventArgs e)
        {
            if (lblEquation.Text.EndsWith("+") || lblEquation.Text.EndsWith("-") || lblEquation.Text.EndsWith("*") || lblEquation.Text.EndsWith("/") || lblEquation.Text.EndsWith("%"))
                lastindex = lblEquation.Text.Length - 1;

            if (lblEquation.Text.EndsWith("."))
            {
                string before = lblEquation.Text.Length > 1 ? lblEquation.Text.Substring(lblEquation.Text.Length - 2, 1) : "";
                if (before == "" || before == "+" || before == "-" || before == "*" || before == "/" || before == "%")
                    lblEquation.Text = lblEquation.Text.Insert(lblEquation.Text.Length - 1, "0");
            }
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            ButtonHover(sender, e);
            Button b = (Button)sender;
            b.Font = new Font("Lucida Sans Typewriter", 20, FontStyle.Bold);
            try
            {
                if (b.Text == "C")
                {
                    lblEquation.Text = String.Empty;
                    lblResult.Text = String.Empty;
                    lastindex = 0;
                    isLastOp = true;
                }
                else if (b.Text == "CE")
                    lblEquation.Text = lblEquation.Text.Substring(0, lblEquation.Text.Length - 1);
                else if (b.Tag == "operation")
                {
                    if (isLastOp == true)
                        lblEquation.Text = lblEquation.Text.Substring(0, lblEquation.Text.Length - 1);

                    lblEquation.Text += b.Text;
                    isLastOp = true;
                }
                else if (b.Tag == "equals")
                {
                    lblResult.Text = Evaluate(lblEquation.Text).ToString();
                    lblEquation.Text = String.Empty;
                    lastindex = 0;
                    isLastOp = true;
                }
                else if (b.Text == ".")
                {
                    string temp = "";
                    if (lblEquation.Text.Length > 0 || lblEquation.Text.Length > lastindex + 1)
                        temp = lblEquation.Text.Substring(lastindex);

                    if (temp.Length == 0 || !temp.Contains("."))
                        lblEquation.Text += b.Text;
                    else
                        lblEquation.Text = lblEquation.Text;
                }
                else
                {
                    lblEquation.Text += b.Text;
                    isLastOp = false;
                }
            }
            catch { }
        }

        private void Keypress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.Equals(e.KeyChar, '.') || Char.Equals(e.KeyChar, '+') || Char.Equals(e.KeyChar, '-') || Char.Equals(e.KeyChar, '*') || Char.Equals(e.KeyChar, '/') || Char.Equals(e.KeyChar, '%') || Char.Equals(e.KeyChar, '='))
            {
                string key = e.KeyChar.ToString();

                Button b;
                if (Char.IsDigit(e.KeyChar))
                    b = key == "0" ? NumberButtons[9] : NumberButtons[Convert.ToInt32(key) - 1];
                else
                {
                    switch (key)
                    {
                        case ".":
                            b = NumberButtons[10]; break;
                        case "+":
                            b = OtherButtons[2]; break;
                        case "-":
                            b = OtherButtons[3]; break;
                        case "*":
                            b = OtherButtons[4]; break;
                        case "/":
                            b = OtherButtons[5]; break;
                        case "%":
                            b = OtherButtons[6]; break;
                        default: //case "=":
                            b = OtherButtons[7]; break;
                    }
                }

                ButtonClicked(b, new EventArgs());
            }
        }
        #endregion

        #region Methods
        private void ShowScreen()
        {
            this.Size = new Size(400, 430);
            this.Text = "Basic Calculator";

            Panel1.Size = new Size(210, 280);
            Panel1.Location = new Point(13, 100);
            Panel1.BackColor = Color.PaleTurquoise;
            Controls.Add(Panel1);

            Panel2.Size = new Size(140, 280);
            Panel2.Location = new Point(230, 100);
            Panel2.BackColor = Panel1.BackColor;
            Controls.Add(Panel2);

            lblEquation.Font = FontForAll;
            lblEquation.TextAlign = ContentAlignment.MiddleRight;
            lblEquation.AutoSize = false;
            lblEquation.Location = new Point(13, 13);
            lblEquation.Size = new Size(358, 40);
            lblEquation.BackColor = Color.DarkTurquoise;
            lblEquation.TextChanged += lblEquation_TextChanged;
            Controls.Add(lblEquation);

            lblResult.Font = FontForResult;
            lblResult.TextAlign = ContentAlignment.MiddleRight;
            lblResult.AutoSize = false;
            lblResult.Location = new Point(13, 53);
            lblResult.Size = new Size(358, 40);
            lblResult.BackColor = Color.DarkTurquoise;
            Controls.Add(lblResult);

        }

        private void GenerateNumberButton()
        {
            int x = 0;
            int y = 0;
            int size = 70;
            string name;

            SuspendLayout();
            for (int i = 1; i <= 11; i++)
            {
                Button b = new Button();

                name = (i).ToString();
                b.Text = name;

                if (i == 10)
                {
                    name = "0";
                    b.Text = "0";
                }
                else if (i == 11)
                {
                    x += size;
                    name = "Dot";
                    b.Text = ".";
                }
                b.Font = FontForAll;
                b.Name = "btn" + name;
                b.TextAlign = ContentAlignment.MiddleCenter;
                b.Size = new Size(i == 10 ? size + 70 : size, size);
                b.Location = new Point(x, y);
                b.KeyPress += Keypress;
                b.Click += ButtonClicked;
                b.MouseHover += ButtonHover;
                b.MouseLeave += ButtonRemoveHover;
                NumberButtons.Add(b);
                Panel1.Controls.Add(NumberButtons[i - 1]);

                x += size;
                if (i % 3 == 0)
                {
                    y += size;
                    x = 0;
                }
            }
            ResumeLayout();
        }

        private void GenerateOtherButton()
        {
            int x = 0;
            int y = 0;
            int size = 70;
            string name = "Other";
            string[] names = new string[] { "C", "CE", "+", "-", "*", "/", "%", "=" };

            SuspendLayout();
            for (int i = 1; i <= 8; i++)
            {
                Button b = new Button();
                b.Font = FontForAll;
                name = "Other" + (i).ToString();
                b.Text = names[i - 1];
                b.Tag = i == 8 ? "equals" : "operation";
                b.Name += "btn" + name;
                b.TextAlign = ContentAlignment.MiddleCenter;
                b.Size = new Size(size, size);
                b.Location = new Point(x, y);
                b.Click += ButtonClicked;
                b.KeyPress += Keypress;
                b.MouseHover += ButtonHover;
                b.MouseLeave += ButtonRemoveHover;
                OtherButtons.Add(b);
                Panel2.Controls.Add(OtherButtons[i - 1]);

                x += size;
                if (i % 2 == 0)
                {
                    y += size;
                    x = 0;
                }
            }
            ResumeLayout();
        }

        private void BackToBasic(List<Button> Buttons)
        {
            foreach (var b in Buttons)
            {
                b.BackColor = Color.PaleTurquoise;
                b.Font = FontForAll;
            }
        }

        private double Evaluate(string expression)
        {
            try
            {
                #region Using DataTable
                /*
                DataTable table = new DataTable();
                table.Columns.Add("expression", typeof(string), expression);
                DataRow row = table.NewRow();
                table.Rows.Add(row);
                return double.Parse((string)row["expression"]);
                */
                #endregion

                List<string> ex = ChangeExpression(expression);
                double result = 0;
                
                for (int i = 0; i < ex.Count; )
                    if (ex[i] == "*" || ex[i] == "/" || ex[i] == "%")
                    {
                        double x = Convert.ToDouble(ex[i - 1]);
                        double y = Convert.ToDouble(ex[i + 1]);

                        result = ex[i] == "*" ? Mul(x, y) : ex[i] == "/" ? Div(x, y) : Dul(x, y);
                        expression = expression.Replace((x.ToString() + ex[i] + y.ToString()), result.ToString());
                        ex = ChangeExpression(expression);
                        i = 0;
                    }
                    else
                    i++;

                for (int i = 0; i < ex.Count; )
                    if (ex[i] == "+" || ex[i] == "-")
                    {
                        double x = Convert.ToDouble(ex[i - 1]);
                        double y = Convert.ToDouble(ex[i + 1]);

                        result = ex[i] == "+" ? Add(x, y) : Sub(x, y);
                        expression = expression.Replace((x.ToString() + ex[i] + y.ToString()), result.ToString());
                        ex = ChangeExpression(expression);
                        i = 0;
                    }
                    else
                        i++;

                return result;
            }
            catch { return 0; }
        }

        private List<string> ChangeExpression(string expression)
        {
            char[] arr = expression.ToArray();
            List<string> ex = new List<string>();
            int lastIndexIndex = 0;
            
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == '+' || arr[i] == '-' || arr[i] == '*' || arr[i] == '/' || arr[i] == '%')
                {
                    ex.Add(expression.Substring(lastIndexIndex, i - lastIndexIndex));
                    ex.Add(arr[i].ToString());
                    lastIndexIndex = i + 1;
                }
            }
            ex.Add(expression.Length == lastIndexIndex ? "0" : expression.Substring(lastIndexIndex, expression.Length - lastIndexIndex));

            return ex;
        }

        #region Mini Operations
        private double Add(double x, double y) { return x + y; }
        private double Sub(double x, double y) { return x - y; }
        private double Mul(double x, double y) { return x * y; }
        private double Div(double x, double y) { return x / y; } //if y = 0 cannot be
        private double Dul(double x, double y) { return x % y; } // ||  = undefined
        #endregion
        #endregion
    }
}
