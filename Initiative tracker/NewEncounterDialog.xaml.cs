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
    /// Interaction logic for NewEncounterDialog.xaml
    /// </summary>
    public partial class NewEncounterDialog : Window {
        public delegate void givelist(List<player> players);
        event givelist listgiver;
        List<string> names;
        List<string> oldnames;
        List<player> players;
        public NewEncounterDialog(List<string> names, givelist method) {
            this.players = new List<player>();
            this.names = new List<string>(names);
            oldnames = new List<string>(names);
            listgiver += method;
            InitializeComponent();
            refreshView();
        }
        void refreshView() {
            if (names.Count!=0) {
                string name = names[0];
                names.RemoveAt(0);
                namebox.Text = name;
                Keyboard.Focus(initBox);
            } else {
                namebox.Text = "";
                Keyboard.Focus(namebox);
            }
            initBox.Text = "";
        }

        void doneClick(object sender, RoutedEventArgs args) {
            listgiver.Invoke(players);
            names = oldnames;
            this.Close();
        }

        void addClick(object sender, RoutedEventArgs args) {
            try {
                string name = namebox.Text;
                int initiative = Convert.ToInt32(initBox.Text);
                players.Add(new player(name, initiative));
                refreshView();
            }catch {

            }
        }
    }
}
