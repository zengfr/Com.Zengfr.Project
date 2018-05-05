using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WordNet.Common;
using WordNet.App.Properties;

namespace WordNet
{
	public partial class MainForm : Form
	{
		#region Constructors
		public MainForm()
		{
			InitializeComponent();
		}
		#endregion Constructors

		#region Methods

		#region DisplayResults
		private void DisplayResults( string orgWord, Dictionary<string, List<Definition>> results )
		{
			mWebBrowser.Navigate( "about:blank" );
			HtmlDocument resultDoc = mWebBrowser.Document;
			StringBuilder sb = new StringBuilder();

			if( results.Count > 0 )
			{
				#region Prep for sorting, build and rendering
				List<string> words = new List<string>();
				Dictionary<string, List<Definition>> defSets = new Dictionary<string, List<Definition>>();
				#endregion

				#region Sort results by part of speech
				foreach( string key in results.Keys )
				{
					foreach( Definition def in results[ key ] )
					{
						string pos = def.DisplayPartOfSpeech;
						if( !defSets.ContainsKey( pos ) )
							defSets.Add( pos, new List<Definition>() );

						defSets[ pos ].Add( def );
						foreach( string word in def.Words )
						{
							string linkedWord = string.Format( Resources.LinkedWordFormat, word.ToLower() );
							if( !words.Contains( linkedWord ) )
								words.Add( linkedWord );
						}
					}
				}
				#endregion

				#region Build markup for browser control
				foreach( string key in defSets.Keys )
				{
					StringBuilder defText = new StringBuilder( "<ul>" );
					foreach( Definition def in defSets[ key ] )
					{
						string formattedDefinition = FormatDefinition( def.DefinitionText );
						if( !string.IsNullOrEmpty( formattedDefinition ) )
						{
							defText.AppendLine( string.Format( "<li>{0}</li>", formattedDefinition ) );
						}
					}
					defText.AppendLine( "</ul>" );
					sb.AppendFormat( Resources.PartOfSpechFormat, string.Format( "{0} <sup>({1})</sup>", orgWord, key ), defText.ToString() );
				}

				sb.Append( string.Join( ", ", words.ToArray() ) );
				#endregion
			}
			else
			{
				#region Build no results markup
				sb.AppendFormat( "<h1>No match was found for \"{0}\".</h1><br />Try your search on <a href=\"http://wordnet.princeton.edu/perl/webwn?s={0}\" target=\"_blank\">WordNet Online</a>", orgWord );
				#endregion
			}

			#region Write markup to browser doc and refresh
			resultDoc.Write( string.Format( Resources.ResultPageFormat, sb.ToString() ) );
			mWebBrowser.Refresh();
			#endregion
		}
		#endregion DisplayResults

		#region FormatDefinition
		private string FormatDefinition( string text )
		{
			string retVal = string.Empty;
			if( !string.IsNullOrEmpty( text ) )
			{
				int exStart = text.IndexOf( '"' );
				if( exStart > -1 )
				{
					retVal += "<strong>";
					retVal += text.Insert( exStart, "</strong><br /><i>" );
					retVal += "</i>";
				}
				else
				{
					retVal = string.Format( "<strong>{0}</strong>", text );
				}
			}
			return retVal;
		}
		#endregion FormatDefinition

		#region DefineWord
		private void DefineWord( string word )
		{
			Dictionary<string, List<Definition>> definitions = DictionaryHelper.GetDefinition( word );
			DisplayResults( word, definitions );
		}
		#endregion DefineWord

		#endregion Methods

		#region Event Handlers

		#region mGoButton_Click
		private void mGoButton_Click( object sender, EventArgs e )
		{
			try
			{
				if( !string.IsNullOrEmpty( mWordTextBox.Text ) )
				{
					this.Cursor = Cursors.WaitCursor;

					DefineWord( mWordTextBox.Text );

					this.Cursor = Cursors.Default;
				}
				else
				{
					MessageBox.Show( this, "Please enter a word.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
				}
			}
			catch( Exception ex )
			{
				MessageBox.Show( this, ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error );
			}
		}
		#endregion mGoButton_Click

		#region mWebBrowser_Navigating
		private void mWebBrowser_Navigating( object sender, WebBrowserNavigatingEventArgs e )
		{
			string url = e.Url.ToString();

			if( url.StartsWith( "define" ) )
			{
				e.Cancel = true;

				string[] segments = url.Split( ':' );
				if( segments.Length > 1 )
				{
					string word = segments[ 1 ];
					DefineWord( word );
				}
			}
		}
		#endregion mWebBrowser_Navigating

		#endregion Event Handlers
	}
}