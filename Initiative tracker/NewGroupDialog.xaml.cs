using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Initiative_tracker {
    /// <summary>
    /// Interaction logic for NewGroupDialog.xaml
    /// </summary>
    public partial class NewGroupDialog : Window {
        public delegate void givelist(List<string> names);
        event givelist listgiver;
        List<string> names;
        public NewGroupDialog(givelist method) {
            listgiver += method;
            names = new List<string>();
            InitializeComponent();
            Keyboard.Focus(namebox);
        }

        void addClick(object sender, RoutedEventArgs args) {
            names.Add(namebox.Text);
            namebox.Text = "";
            Keyboard.Focus(namebox);
        }

        void doneClick(object sender, RoutedEventArgs args) {
            listgiver.Invoke(names);
            this.Close();
        }
        
    }
}
