using DataModel;
using LinqToDB;
using LinqToDB.Tools.Mapper;
using Microsoft.VisualBasic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;

namespace Osterhase_selber
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Person> liste = null;
        const double links = 16.209652;
        const double unten = 47.786898;
        const double rechts = 16.281017;
        const double oben = 47.846533;

        List<SolidColorBrush> colors = new List<SolidColorBrush>();

        List<Tuple<Person, Ellipse>> Startpunkte = new List<Tuple<Person, Ellipse>>();

        private Random r = new Random();

        int helfer;

        OsterStadtDb db;

        public MainWindow()
        {
            InitializeComponent();
            db = new OsterStadtDb();
        }

        private void VisualizeHelfer_Click(object sender, RoutedEventArgs e)
        {
            string input = Interaction.InputBox("Bitte Nummer eingeben:", "Nummer eingeben", "");

            if (int.TryParse(input, out int number))
            {
                helfer = number;
                colors.Clear();
                for (int i = 0; i < number; i++)
                {
                    colors.Add(new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(1, 255))));
                }
                liste = kMeans(500, number);
                Map.Children.Clear();
                foreach (Person x in liste)
                {
                    Ellipse el = new Ellipse();
                    el.Stroke = Brushes.Black;
                    el.Fill = colors[x.ClusterId];
                    el.Width = 12;
                    el.Height = 12;
                    el.StrokeThickness = 1.5;


                    Canvas.SetBottom(el, Map.ActualHeight * ((double)x.Latitude - unten) / (oben - unten));
                    Canvas.SetLeft(el, Map.ActualWidth * ((double)x.Longitude - links) / (rechts - links));

                    el.Tag = x;
                    el.MouseLeftButtonDown += OnMouseLeftButtonDownEllipse;

                    Map.Children.Add(el);
                }
            }
            else
            {
                MessageBox.Show("Ungültige Nummer");
            }

        }

        private void OnMouseLeftButtonDownEllipse(object sender, MouseButtonEventArgs e)
        {
            Person p = (Person)((Ellipse)sender).Tag;
            var curr = Startpunkte.Where(x => x.Item1.ClusterId == p.ClusterId).FirstOrDefault();
            if (curr != null)
            {
                Startpunkte.Remove(curr);
                curr.Item2.Stroke = Brushes.Black;
            }

            Startpunkte.Add(Tuple.Create(p, (Ellipse)sender));
            ((Ellipse)sender).Stroke = Brushes.White;

        }

        private void VisualizeWeg_Click(object sender, RoutedEventArgs e)
        {
            if (liste == null)
            {
                Map.Children.Clear();
                VisualizeHelfer_Click(null, null);
            }

            if (Startpunkte.Count < helfer)
            {
                MessageBox.Show("Wähle " + helfer + " Startpunkte");
                return;
            }
            var lines = Map.Children.Cast<object>().Where(x => x.GetType() == typeof(Line)).Cast<Line>().ToList();

            foreach (var l in lines)
            {
                Map.Children.Remove(l);
            }
            foreach (var x in Startpunkte)
            {

                List<Person> weg;
                if (liste.Where(o => o.ClusterId == x.Item1.ClusterId).ToList().Count > 24)
                {
                    weg = TSP_Prims(x.Item1);
                }
                else
                {
                    weg = HeldKarp(x.Item1);

                }
                for (int i = 0; i < weg.Count - 1; i++)
                {
                    Line line = new Line();
                    line.Stroke = colors[weg[0].ClusterId];
                    line.StrokeThickness = 3;
                    line.X1 = Map.ActualWidth * ((double)weg[i].Longitude - links) / (rechts - links);
                    line.Y1 = Map.ActualHeight * ((double)weg[i].Latitude - oben) / (unten - oben);
                    line.X2 = Map.ActualWidth * ((double)weg[i + 1].Longitude - links) / (rechts - links);
                    line.Y2 = Map.ActualHeight * ((double)weg[i + 1].Latitude - oben) / (unten - oben);

                    Map.Children.Add(line);
                }
            }


        }
        private void VisualizePoints_Click(object sender, RoutedEventArgs e)
        {
            Map.Children.Clear();
            if (liste != null)
                liste.Clear();
            else
                foreach (Person x in db.GetTable<Person>())
                {
                    Ellipse el = new Ellipse();
                    el.Stroke = Brushes.Black;
                    el.Fill = Brushes.Black;
                    el.Width = 12;
                    el.Height = 12;

                    Canvas.SetBottom(el, Map.ActualHeight * ((double)x.Latitude - unten) / (oben - unten));
                    Canvas.SetLeft(el, Map.ActualWidth * ((double)x.Longitude - links) / (rechts - links));

                    el.Tag = x;
                    el.MouseLeftButtonDown += OnMouseLeftButtonDownEllipse;


                    Map.Children.Add(el);
                }
        }
        private void CreatePerson_Click(object sender, RoutedEventArgs e)
        {
            PersonDialog d = new PersonDialog();
            if (d.ShowDialog() == true)
            {
                db.Insert(d.p);
            }
        }

        private void Map_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var ellipses = Map.Children.Cast<object>().Where(x => x.GetType() == typeof(Ellipse)).Cast<Ellipse>();

            foreach (var el in ellipses)
            {
                Canvas.SetBottom(el, Map.ActualHeight * Canvas.GetBottom(el) / e.PreviousSize.Height);
                Canvas.SetLeft(el, Map.ActualWidth * Canvas.GetLeft(el) / e.PreviousSize.Width);
            }

            var lines = Map.Children.Cast<object>().Where(x => x.GetType() == typeof(Line)).Cast<Line>();

            foreach (var l in lines)
            {
                l.X1 = Map.ActualWidth * l.X1 / e.PreviousSize.Width;
                l.Y1 = Map.ActualHeight * l.Y1 / e.PreviousSize.Height;

                l.X2 = Map.ActualWidth * l.X2 / e.PreviousSize.Width;
                l.Y2 = Map.ActualHeight * l.Y2 / e.PreviousSize.Height;
            }

        }


        // Aufteilungs Algorithmus (K-Means)

        public double calcDistanceCentroid(Person p1, Centroid p2)
        {
            return Math.Sqrt(Math.Pow(Math.Abs((double)(p2.Longitude - p1.Longitude)), 2)
                + Math.Pow(Math.Abs((double)(p2.Latitude - p1.Latitude)), 2));
        }

        public double calcDistancePersons(Person p1, Person p2)
        {
            return Math.Sqrt(Math.Pow(Math.Abs((double)(p2.Longitude - p1.Longitude)), 2)
                + Math.Pow(Math.Abs((double)(p2.Latitude - p1.Latitude)), 2));
        }




        public List<Person> kMeans(int maxIter, int helfer)
        {
            List<Person> erg = db.GetTable<Person>().ToList();

            List<Centroid> centroids = db.GetTable<Person>()
                .OrderBy(x => new Random().Next()).Take(helfer)
                .Select(p => new Centroid { Longitude = p.Longitude, Latitude = p.Latitude }).ToList();

            for (int i = 0; i < centroids.Count; i++)
            {
                centroids[i].ClusterId = i;
            }

            for (int i = 0; i < maxIter; i++)
            {
                bool changed = false;

                foreach (var p in erg)
                {
                    int bestCluster = 0;
                    double bestDistance = double.MaxValue;

                    foreach (var x in centroids)
                    {
                        double dist = calcDistanceCentroid(p, x);
                        if (dist < bestDistance)
                        {
                            bestDistance = dist;
                            bestCluster = x.ClusterId;
                        }
                    }
                    if (p.ClusterId != bestCluster)
                    {
                        p.ClusterId = bestCluster;
                        changed = true;
                    }
                }
                foreach (var x in centroids)
                {
                    var ClusterPoints = erg.Where(p => p.ClusterId == x.ClusterId);

                    if (ClusterPoints.Any())
                    {
                        x.Latitude = ClusterPoints.Average(p => p.Latitude);
                        x.Longitude = ClusterPoints.Average(p => p.Longitude);
                    }
                }
                if (!changed)
                {
                    break;
                }
            }



            return erg; // TwoOpt(erg);
        }

        public class Centroid
        {
            public decimal? Longitude;
            public decimal? Latitude;
            public int ClusterId;
        }

        // Travelling Salesman

        private List<Person> HeldKarp(Person p)
        {
            List<List<double>> distances = new List<List<double>>();
            List<Person> clusterPs = liste.Where(o => o.ClusterId == p.ClusterId && o != p).ToList();
            clusterPs.Insert(0, p);
            for (int i = 0; i < clusterPs.Count; i++)
            {
                distances.Add(new List<double>());
                for (int j = 0; j < clusterPs.Count; j++)
                {
                    distances[i].Add(calcDistancePersons(clusterPs[i], clusterPs[j]));
                }

            }


            int n = distances.Count;
            int subsetCount = 1 << n;
            const double INFINITY = double.MaxValue / 4;

            List<List<double>> dp = new List<List<double>>(subsetCount);
            List<List<int>> parents = new List<List<int>>(subsetCount);

            for (int i = 0; i < subsetCount; i++)
            {
                dp.Add(Enumerable.Repeat(INFINITY, n).ToList());
                parents.Add(Enumerable.Repeat(-1, n).ToList());
            }

            dp[1][0] = 0;

            for (int mask = 1; mask < subsetCount; mask++)
            {
                if ((mask & 1) == 0)
                    continue;

                for (int j = 1; j < n; j++)
                {
                    if ((mask & (1 << j)) == 0)
                        continue;

                    int previousMask = mask ^ (1 << j);
                    for (int k = 0; k < n; k++)
                    {
                        if ((previousMask & (1 << k)) == 0)
                            continue;

                        double cost = dp[previousMask][k] + distances[k][j];
                        if (cost < dp[mask][j])
                        {
                            dp[mask][j] = cost;
                            parents[mask][j] = k;
                        }
                    }
                }
            }

            int fullMask = subsetCount - 1;
            double minCost = INFINITY;
            int lastCity = 0;
            for (int j = 1; j < n; j++)
            {
                double cost = dp[fullMask][j]; // + distances[j][0] für echten TSP; jetzt: Shortest Hamiltonian Path
                if (cost < minCost)
                {
                    minCost = cost;
                    lastCity = j;
                }
            }

            List<int> tour = new List<int>();
            int currentMask = fullMask;
            int currentCity = lastCity;

            while (currentCity != 0)
            {
                tour.Add(currentCity);
                int prevCity = parents[currentMask][currentCity];
                currentMask ^= (1 << currentCity);
                currentCity = prevCity;
            }

            tour.Add(0);
            tour.Reverse();

            List<Person> erg = new List<Person>();

            foreach (var x in tour)
            {
                erg.Add(clusterPs[x]);
            }

            return erg;
        }


        // Alter K-Nearest Neighbor, liefert NICHT die optimale Lösung, aber funktioniert.
        List<Person> TSP_Prims(Person start)
        {
            List<Person> erg = new List<Person>();

            Person? p = start;

            while (p != null)
            {
                erg.Add(p);
                p.Visited = true;
                p = liste.Where(x => x.ClusterId == start.ClusterId && !x.Visited).OrderBy(x => calcDistancePersons(x, p)).FirstOrDefault();
            }

            // reset
            foreach (var y in liste)
            {
                y.Visited = false;
            }

            return TwoOpt(erg);
        }


        public List<Person> TwoOpt(List<Person> route)
        {
            bool improved = true;

            while (improved)
            {
                improved = false;

                for (int i = 1; i < route.Count - 2; i++)
                {
                    for (int j = i + 1; j < route.Count; j++)
                    {
                        if (j - i == 1)
                            continue;

                        double oldDistance =
                            calcDistancePersons(route[i - 1], route[i]) +
                            calcDistancePersons(route[j - 1], route[j % route.Count]);

                        double newDistance =
                            calcDistancePersons(route[i - 1], route[j - 1]) +
                            calcDistancePersons(route[i], route[j % route.Count]);

                        if (newDistance < oldDistance)
                        {
                            route.Reverse(i, j - i);
                            improved = true;
                        }
                    }
                }
            }

            return route;
        }
    }
}