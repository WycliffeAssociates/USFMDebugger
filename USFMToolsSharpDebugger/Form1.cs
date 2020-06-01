using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using USFMToolsSharp;
using USFMToolsSharp.Models.Markers;

namespace USFMToolsSharpDebugger
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void txtInput_TextChanged(object sender, EventArgs e)
        {
            USFMParser parser = new USFMParser();
            var output = parser.ParseFromString(txtInput.Text);
            trOutput.Nodes.Clear();
            foreach(var marker in output.Contents)
            {
                trOutput.Nodes.Add(CreateTreeNode(marker));
            }
        }

        private TreeNode CreateTreeNode(Marker marker)
        {
            TreeNode output = new TreeNode();
            output.Text = marker.GetType().Name;
            output.Tag = marker;
            foreach(var node in marker.Contents)
            {
                output.Nodes.Add(CreateTreeNode(node));
            }
            return output;
        }

        private string CreateDetail(Marker marker)
        {
            switch (marker)
            {
                case TextBlock textBlock:
                    return $"Text: {textBlock.Text}";
                case FMarker fMarker:
                    return $"Caller: {fMarker.FootNoteCaller}";
                case VMarker vMarker:
                    return $"Verse Number: {vMarker.VerseNumber}";
            }
            return "";
        }

        private void trOutput_AfterSelect(object sender, TreeViewEventArgs e)
        {
            lblDetails.Text = CreateDetail((Marker)e.Node.Tag);
        }
    }
}
