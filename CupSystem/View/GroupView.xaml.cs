using CupSystem.ViewModel;
using JsonFileDatabase.Model;
using System.Windows;

namespace CupSystem.View
{
    /// <summary>
    /// Interaction logic for GroupView.xaml
    /// </summary>
    public partial class GroupView : Window
    {

        public GroupView(Group g, List<Player> p)
        {
            InitializeComponent();

            (DataContext as GroupViewModel)!.SetGroup(g);
            (DataContext as GroupViewModel)!.SetPlayers(p);
        }
    }
}
