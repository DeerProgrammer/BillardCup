using CupSystem.ViewModel;
using JsonFileDatabase.Model;
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

namespace CupSystem.View
{
    /// <summary>
    /// Interaction logic for PlayoffView.xaml
    /// </summary>
    public partial class PlayoffView : Window
    {
        public PlayoffView(Playoff p, string c)
        {
            InitializeComponent();

            (DataContext as PlayoffViewModel)!.SetPlayoff(p);
            (DataContext as PlayoffViewModel)!.SetCupName(c);
        }
    }
}
