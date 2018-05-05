﻿using System;
using System.Windows.Forms;
namespace ConsoleControlSample
{
    /// <summary>
    /// The sample form.
    /// </summary>
    public partial class FormConsoleControlSample : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormConsoleControlSample"/> class.
        /// </summary>
        public FormConsoleControlSample()
        {
            InitializeComponent();
            this.consoleTabControl.SelectedIndexChanged += new EventHandler(consoleTabControl_SelectedIndexChanged);
        }

        void consoleTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUIState();
        }

        /// <summary>
        /// Handles the Load event of the FormConsoleControlSample control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void FormConsoleControlSample_Load(object sender, EventArgs e)
        {
            //  Update the UI state.
            UpdateUIState();
        }



        private int _index = 0;
        void toolStripButtonNewTab_Click(object sender, System.EventArgs e)
        {
            var tabControl = this.consoleTabControl;
            tabControl.AddConsoleTabPage(string.Format("{0}", _index++), null);
        }
        /// <summary>
        /// Handles the Click event of the toolStripButtonNewProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>

        private void toolStripButtonNewProcess_Click(object sender, EventArgs e)
        {
            //  Create the new process form.
            FormNewProcess formNewProcess = new FormNewProcess();

            //  If the form is shown OK, start the process.
            if (formNewProcess.ShowDialog() == DialogResult.OK)
            {
                var tabControl = this.consoleTabControl;
                //  Start the proces.
                tabControl.GetSelectedConsoleTabPage().GetConsoleControl().StartProcess(formNewProcess.FileName, formNewProcess.Arguments);

                //  Update the UI state.
                UpdateUIState();
            }
        }

        /// <summary>
        /// Handles the Click event of the toolStripButtonStopProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void toolStripButtonStopProcess_Click(object sender, EventArgs e)
        {
            getCurrentConsoleControl().StopProcess();

            //  Update the UI state.
            UpdateUIState();
        }

        /// <summary>
        /// Updates the state of the UI.
        /// </summary>
        private void UpdateUIState()
        {
            var consoleControl = getCurrentConsoleControl();
            if (consoleControl != null)
            {
                //  Update the state.
                if (consoleControl.IsProcessRunning)
                    toolStripStatusLabelConsoleState.Text =string.Format("Running:{0} {1}",
                                                            System.IO.Path.GetFileName(
                                                                consoleControl.ProcessInterface.ProcessFileName),
                                                                 consoleControl.ProcessInterface.ProcessArguments);
                else
                    toolStripStatusLabelConsoleState.Text = "Not Running";

                //  Update toolbar buttons.
                toolStripButtonRunCMD.Enabled = !consoleControl.IsProcessRunning;
                toolStripButtonNewProcess.Enabled = !consoleControl.IsProcessRunning;
                toolStripButtonStopProcess.Enabled = consoleControl.IsProcessRunning;
                toolStripButtonShowDiagnostics.Checked = consoleControl.ShowDiagnostics;
                toolStripButtonInputEnabled.Checked = consoleControl.IsInputEnabled;
                toolStripButtonSendKeyboardCommandsToProcess.Checked = consoleControl.SendKeyboardCommandsToProcess;
            }
        }

        /// <summary>
        /// Handles the Click event of the toolStripButtonShowDiagnostics control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void toolStripButtonShowDiagnostics_Click(object sender, EventArgs e)
        {
            var consoleControl = getCurrentConsoleControl();
            consoleControl.ShowDiagnostics = !consoleControl.ShowDiagnostics;
            UpdateUIState();
        }

        /// <summary>
        /// Handles the Tick event of the timerUpdateUI control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void timerUpdateUI_Tick(object sender, EventArgs e)
        {
            UpdateUIState();
        }

        /// <summary>
        /// Handles the Click event of the toolStripButtonInputEnabled control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void toolStripButtonInputEnabled_Click(object sender, EventArgs e)
        {
            var consoleControl = getCurrentConsoleControl();
            consoleControl.IsInputEnabled = !consoleControl.IsInputEnabled;
            UpdateUIState();
        }

        /// <summary>
        /// Handles the Click event of the toolStripButtonRunCMD control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void toolStripButtonRunCMD_Click(object sender, EventArgs e)
        {
            getCurrentConsoleControl().StartProcess("cmd", null);
            UpdateUIState();
        }

        /// <summary>
        /// Handles the Click event of the toolStripButtonSendKeyboardCommandsToProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void toolStripButtonSendKeyboardCommandsToProcess_Click(object sender, EventArgs e)
        {
            var consoleControl = getCurrentConsoleControl();
            consoleControl.SendKeyboardCommandsToProcess = !consoleControl.SendKeyboardCommandsToProcess;
            UpdateUIState();
        }

        /// <summary>
        /// Handles the Click event of the toolStripButtonClearOutput control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void toolStripButtonClearOutput_Click(object sender, EventArgs e)
        {
            getCurrentConsoleControl().ClearOutput();
        }

        private ConsoleControl.ConsoleControl getCurrentConsoleControl()
        {
            var tab = this.consoleTabControl.SelectedTab as ConsoleControlEx.ConsoleTabPage;
            if (tab != null)
            {
                return tab.GetConsoleControl();
            }
            return null;
        }
    }
}