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
    /// Interaction logic for NewEffect.xaml
    /// </summary>
    public partial class NewEffect : Window {
        public delegate void giveeffect(string name, int duration);
        event giveeffect effectgiver;
        public NewEffect(giveeffect method) {
            effectgiver += method;
            InitializeComponent();
            Keyboard.Focus(namebox);
        }

        void addClick(object sender, RoutedEventArgs args) {
            try {
                string name = namebox.Text;
                int duration = Convert.ToInt32(DuraBox.Text);
                effectgiver.Invoke(name, duration);
                this.Close();
            } catch {

            }
        }
        
        void cancelClick(object sender, RoutedEventArgs args) {
            this.Close();
        }
    }
}
