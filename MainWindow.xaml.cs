//Akshay, Tyler, Anil, Shubham

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace System_Dependencies
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                List<Program> programs = new List<Program>();
                string[] readDisplay = File.ReadAllLines(ofd.FileName);
                foreach (string r in readDisplay)
                {
                    lbxDisplay.Items.Add(r);
                    string[] tokens = r.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    switch (tokens[0])
                    {
                        case "DEPEND":
                            {
                                Program TheOneThatDependsOnTheRest = null;
                                //loop 
                                for (int i = 1; i < tokens.Length; i++)
                                {

                                    //searches tokens to see if that token already exists 
                                    //
                                    Program _prog = programs.Find(q => q.Name == tokens[i]);
                                    if (_prog == null)
                                    {
                                        _prog = new Program();
                                        _prog.Name = tokens[i];
                                        programs.Add(_prog);
                                    }
                                    if (TheOneThatDependsOnTheRest != null)
                                    {
                                        _prog.child.Add(TheOneThatDependsOnTheRest);
                                        TheOneThatDependsOnTheRest.parent.Add(_prog);
                                    }
                                    if (i == 1)
                                    {
                                        TheOneThatDependsOnTheRest = _prog;
                                    }
                                }
                                break;
                            }
                        case "INSTALL":
                            {
                                //searches to find
                                Program _prog = programs.Find(q => q.Name == tokens[1]);
                                if (_prog == null)
                                {
                                    //example for foo if program cannot find foo, it will create a foo and then install 
                                    _prog = new Program();
                                    _prog.Name = tokens[1];
                                    _prog.Installed = true;
                                    programs.Add(_prog);
                                    lbxDisplay.Items.Add("\tInstalling " + _prog.Name);
                                }
                                else
                                {
                                    if (_prog.Installed)
                                    {
                                        lbxDisplay.Items.Add("\t" + tokens[1] + " is already installed");
                                    }
                                    else
                                    {
                                        foreach (Program p in _prog.parent)
                                        {
                                            if (!p.Installed)
                                            {
                                                lbxDisplay.Items.Add("\tInstalling " + p.Name);
                                                p.Installed = true;
                                            }
                                        }
                                        _prog.Installed = true;
                                        lbxDisplay.Items.Add("\tInstalling " + _prog.Name);
                                    }
                                }
                            }
                            break;
                        case "REMOVE":
                            {
                                //searches to find
                                Program _prog = programs.Find(q => q.Name == tokens[1]);
                                if (_prog == null)
                                {
                                    //example for foo if program cannot find foo, it will create a foo and then install 
                                    _prog = new Program();
                                    _prog.Name = tokens[1];
                                    _prog.Installed = false;
                                    programs.Add(_prog);
                                    lbxDisplay.Items.Add("\tRemoving " + _prog.Name);
                                }
                                else if (_prog.Installed)
                                {
                                    if (_prog.child.Count(q => q.Installed == true) == 0)
                                    {
                                        lbxDisplay.Items.Add("\tRemoving " + _prog.Name);
                                        _prog.Installed = false;
                                    }
                                    else
                                    {
                                        foreach (Program p in _prog.child)
                                        {
                                            if (p.Installed)
                                            {
                                                lbxDisplay.Items.Add("\t" + _prog.Name+ " is still needed");
                                                p.Installed = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                                else if (!_prog.Installed)
                                {
                                    lbxDisplay.Items.Add("\tNot Installed" + _prog.Name);
                                }
                            }
                            break;
                        case "LIST":
                            {
                                foreach (Program p in programs)
                                {
                                    if (p.Installed)
                                    {
                                        lbxDisplay.Items.Add("\t"+p.Name);
                                    }
                                }
                                break;
                            }
                            
                            case "END":
                            break;
                    }
                }

            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

/*foreach (Program p in programs)
                            {
                                lbxDisplay.Items.Add(p.Name);
                                lbxDisplay.Items.Add("  depends on");
                                foreach (Program q in p.ThingsIdependOn)
                                    lbxDisplay.Items.Add("  " + q.Name);
                                lbxDisplay.Items.Add("  depending");
                                foreach (Program q in p.ThingsThatDependsOnMe)
                                    lbxDisplay.Items.Add("  " + q.Name);
                            
                            // break;*/
