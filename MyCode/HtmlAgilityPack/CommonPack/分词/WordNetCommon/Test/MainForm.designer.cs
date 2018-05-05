namespace WordNet
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( MainForm ) );
			this.label1 = new System.Windows.Forms.Label();
			this.mWordTextBox = new System.Windows.Forms.TextBox();
			this.mGoButton = new System.Windows.Forms.Button();
			this.mWebBrowser = new System.Windows.Forms.WebBrowser();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 12, 9 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 132, 13 );
			this.label1.TabIndex = 0;
			this.label1.Text = "Enter a word to search for:";
			// 
			// mWordTextBox
			// 
			this.mWordTextBox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.mWordTextBox.Location = new System.Drawing.Point( 12, 25 );
			this.mWordTextBox.Name = "mWordTextBox";
			this.mWordTextBox.Size = new System.Drawing.Size( 395, 20 );
			this.mWordTextBox.TabIndex = 1;
			// 
			// mGoButton
			// 
			this.mGoButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.mGoButton.Location = new System.Drawing.Point( 413, 23 );
			this.mGoButton.Name = "mGoButton";
			this.mGoButton.Size = new System.Drawing.Size( 34, 23 );
			this.mGoButton.TabIndex = 2;
			this.mGoButton.Text = "Go";
			this.mGoButton.UseVisualStyleBackColor = true;
			this.mGoButton.Click += new System.EventHandler( this.mGoButton_Click );
			// 
			// mWebBrowser
			// 
			this.mWebBrowser.AllowWebBrowserDrop = false;
			this.mWebBrowser.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.mWebBrowser.IsWebBrowserContextMenuEnabled = false;
			this.mWebBrowser.Location = new System.Drawing.Point( 12, 51 );
			this.mWebBrowser.MinimumSize = new System.Drawing.Size( 20, 20 );
			this.mWebBrowser.Name = "mWebBrowser";
			this.mWebBrowser.ScriptErrorsSuppressed = true;
			this.mWebBrowser.Size = new System.Drawing.Size( 435, 260 );
			this.mWebBrowser.TabIndex = 3;
			this.mWebBrowser.WebBrowserShortcutsEnabled = false;
			this.mWebBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler( this.mWebBrowser_Navigating );
			// 
			// MainForm
			// 
			this.AcceptButton = this.mGoButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 459, 323 );
			this.Controls.Add( this.mWebBrowser );
			this.Controls.Add( this.mGoButton );
			this.Controls.Add( this.mWordTextBox );
			this.Controls.Add( this.label1 );
			this.Icon = ( ( System.Drawing.Icon )( resources.GetObject( "$this.Icon" ) ) );
			this.Name = "MainForm";
			this.Text = "Sharp WordNet";
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox mWordTextBox;
		private System.Windows.Forms.Button mGoButton;
		private System.Windows.Forms.WebBrowser mWebBrowser;
	}
}

