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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Initiative_tracker {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        List<character> characterlist;
        List<character> orgCharList;
        List<player> playerList;
        int turn;
        int round;
        public MainWindow() {
            InitializeComponent();
            turn = 0;
            round = 0;
            advanceTurn.IsEnabled = false;
            newEncounter.IsEnabled = false;
        }

        void sortList() {
            SinglyLinkedNode first = null;
            foreach(character character in characterlist) {
                if (first == null) {
                    first = new SinglyLinkedNode(character, null);
                }else {
                    if (character.initiative > first.character.initiative) {
                        first = new SinglyLinkedNode(character, first);
                    }else {
                        first.insert(character);
                    }
                }
            }
            characterlist = new List<character>();
            first.getList(characterlist);
            InitiativeList.ItemsSource = characterlist;
            refreshView();
        }

        void AdvanceTurn(object sender, RoutedEventArgs args) {
            character last = characterlist[0];
            last.progress();
            characterlist.RemoveAt(0);
            characterlist.Add(last);
            turn++;
            round = (turn / characterlist.Count)+1;
            refreshView();
        }

        void NewGroup(object sender, RoutedEventArgs args) {
            NewGroupDialog ngd = new NewGroupDialog(getNames);
            ngd.ShowDialog();

            
        }

        void getNames(List<player> players) {
            playerList = players;
            newEncounter.IsEnabled = true;
            advanceTurn.IsEnabled = false;
        }
        void addEffect(object sender, RoutedEventArgs args) {
            Button button = sender as Button;
            character target = button.DataContext as character;

            target.createEffect(refreshView);
        }

        void changeHealth(object sender, RoutedEventArgs args) {
            Button button = sender as Button;
            character target = button.DataContext as character;
            ChangeHealthDialog chd = new ChangeHealthDialog(target, refreshView);
            chd.ShowDialog();
        }

        private void NewEncounter(object sender, RoutedEventArgs e) {
            NewEncounterDialog ned = new NewEncounterDialog(playerList, getInitList);
            ned.ShowDialog();
        }

        void getInitList(List<character> characters) {
            turn = 0;
            characterlist = characters;
            orgCharList = new List<character>(characters);
            advanceTurn.IsEnabled = true;
            sortList();
        }
        void refreshView() {
            /*foreach(character character in characterlist) {
                if (character.health <= 0) {
                    characterlist.Remove(character);
                }
            }*/
            InitiativeList.Items.Refresh();
            TurnaRound.Text = "Round: " + round;
        }
    }

    public class SinglyLinkedNode {
        public character character { get; set; }
        public SinglyLinkedNode node { get; set; }
        public SinglyLinkedNode(character _char, SinglyLinkedNode _node) {
            character = _char;
            node = _node;
        }
        character getNext() {
            if (node != null) {
                return node.character;
            }
            return null;
        }

        public void insert(character _char) {
            if (node != null) {
                if (_char.initiative > node.character.initiative) {
                    SinglyLinkedNode newnode = new SinglyLinkedNode(_char, node);
                    node = newnode;
                } else {
                    node.insert(_char);
                }
            }else {
                node = new SinglyLinkedNode(_char, node);
            }
        }
        public void getList(List<character> characterlist) {
            characterlist.Add(character);
            if (node != null) {
                node.getList(characterlist);
            }
        }
    }

    public class character {
        public delegate void refresh();
        event refresh refresher;
        public string name { get; set; }
        public int initiative { get; set; }
        public int health { get; set; }

        public string status {get; set;}

        private List<Status> statusList;
        public character(string _name, int _initiative, int _health) {
            name = _name;
            initiative = _initiative;
            health = _health;
            status = "";
            statusList = new List<Status>();
        }
        public void createEffect(refresh method) {
            refresher += method;
            NewEffect ne = new NewEffect(this.addEffect);
            ne.ShowDialog();
        }

        public void addEffect(string name, int duration) {
            statusList.Add(new Status(name, duration));

            status = "";
            foreach (Status stat in statusList) {
                if (stat.duration > 0) {
                    status += ", " + stat.name + ": " + stat.duration + " turns";
                }
            }
            try {
                status = status.Substring(2);
            } catch {
                status = "";
            }

            refresher.Invoke();
        }
        public void progress() {
            status = "";
            foreach (Status stat in statusList) {
                stat.progress();
                if (stat.duration > 0) {
                    status += ", " + stat.name + ": " + stat.duration + " turns";
                }
            }
            try {
                status = status.Substring(2);
            }catch {
                status = "";
            }
            
        }
        public override string ToString() {
            return "Name: " + name + " Health: " + health + " Initiative: " + initiative;
        }
    }
    public class Status {
        public string name { get; set; }
        public int duration { get; set; }
        public Status(string _name, int _duration) {
            name = _name;
            duration = _duration;
        }
        public void progress() {
            duration = duration - 1;
        }
    }
}
