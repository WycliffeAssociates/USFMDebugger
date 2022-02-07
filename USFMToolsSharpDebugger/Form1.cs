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
            USFMParser parser = new USFMParser(ignoreUnknownMarkers:true);
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
            return marker switch
            {
                TextBlock textBlock => $"Text: {textBlock.Text}",
                FMarker fMarker => $"Caller: {fMarker.FootNoteCaller}",
                VMarker vMarker => $"Verse Number: {vMarker.VerseNumber}",
                CMarker cMarker => $"Chapter Number: {cMarker.Number}",
                QMarker qMarker => $"Poetry Level: {qMarker.Depth}\n Isblock: {qMarker.IsPoetryBlock}",
                SMarker sMarker => $"Section level: {sMarker.Weight}",
                _ => "",
            };
        }

        private void trOutput_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var marker = (Marker)e.Node.Tag;
            lblDetails.Text = CreateDetail(marker);
            txtInput.Select(marker.Position, marker.Identifier.Length);
        }
    }
}
