using System.Windows.Forms;

public class InputBox : Form
{
    private Button button1;
    private TextBox textBox1;
    private Label label1;

    public DialogResult Show(string title, string promptText, ref int value)
    {
        InitializeComponent();
        this.Text = title;
        Visible = false;
        Text = promptText;
        textBox1.Text = value.ToString();

        button1.DialogResult = DialogResult.OK;

        DialogResult dialogResult = ShowDialog();
        if (int.TryParse(textBox1.Text, out value))
            dialogResult = DialogResult.Cancel;
        else
            value = 0;
        return dialogResult;
    }

    private void InitializeComponent()
    {
        this.button1 = new System.Windows.Forms.Button();
        this.textBox1 = new System.Windows.Forms.TextBox();
        this.label1 = new System.Windows.Forms.Label();
        this.SuspendLayout();
        //
        // button1
        //
        this.button1.Location = new System.Drawing.Point(162, 37);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(77, 21);
        this.button1.TabIndex = 0;
        this.button1.Text = "OK";
        this.button1.UseVisualStyleBackColor = true;
        //
        // textBox1
        //
        this.textBox1.Location = new System.Drawing.Point(33, 38);
        this.textBox1.Name = "textBox1";
        this.textBox1.Size = new System.Drawing.Size(100, 23);
        this.textBox1.TabIndex = 1;
        //
        // label1
        //
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(30, 22);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(96, 15);
        this.label1.TabIndex = 2;
        this.label1.Text = "Enter No of Trays";
        //
        // InputBox
        //
        this.ClientSize = new System.Drawing.Size(281, 98);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.textBox1);
        this.Controls.Add(this.button1);
        this.Name = "InputBox";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.TopMost = true;
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}